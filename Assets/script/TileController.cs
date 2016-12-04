using UnityEngine;

public class TileController{

    private int tileWidth;
    private Transform[] tileArray;
    private TileTraverser tileTraverser = null;

    private Node savedNode = null;
    private int[] tilesEffectedByPlayerSkill;

    public TileController(GameObject tiles, int tileWidth, int tileHeight) {
        this.tileWidth = tileWidth;
        tileArray = new Transform[tileHeight * tileWidth];

        int height = 0, width = 0;
        foreach (Transform tileRow in tiles.transform) {
            width = 0;
            foreach (Transform tile in tileRow.transform) {
                tile.GetComponent<Tile>().setTileID((height * tileWidth) + width);
                tileArray[(height * tileWidth) + width] = tile;
                width++;
            }
            height++;
        }

        tileTraverser = new DijkstraTileTraverser(tileArray, tileWidth, tileHeight, true);
    }

    private int getIDFromXY(int tileX, int tileY) {
        return (tileY * tileWidth) + tileX;
    }

    public Transform getTileFromID(int tileID) {
        return tileArray[tileID];
    }

    public Transform getTileFromXY(int tileX, int tileY) {
        return tileArray[getIDFromXY(tileX, tileY)];
    }

    public void switchIsTileOccupied(int tileID) {
        GameObject tile = tileArray[tileID].gameObject;
        tile.GetComponent<Tile>().switchTileIsOccupied();
    }

    public void switchIsTileOccupied(int tileX, int tileY) {
        switchIsTileOccupied(getIDFromXY(tileX, tileY));
    }

    public int getDistanceBetweenTwoTiles(int a, int b) {
        int aX, aY, bX, bY;
        aX = a % tileWidth;
        aY = a / tileWidth;
        bX = b % tileWidth;
        bY = b / tileWidth;

        return (int)Mathf.Sqrt(Mathf.Pow((aX - bX), 2) + Mathf.Pow((aY - bY), 2));
    }

    public void updateCharactersTraversalMap(GameObject character) {
        if (character != null) {
            int characterPosition = character.GetComponent<CharacterPosition>().getTileID();
            character.GetComponent<CharacterMovementController>().setTraversalMap(tileTraverser.getTileTrafersal(characterPosition));
        }
    }

    public void setTileState(int tileID, Tile.TileState newTileState) {
        getTileFromID(tileID).GetComponent<Tile>().setCurrentState(newTileState);
    }

    public void setTileState(int tileX, int tileY, Tile.TileState newTileState) {
        getTileFromXY(tileX, tileY).GetComponent<Tile>().setCurrentState(newTileState);
    }

    private void resetLastPath() {
        while (savedNode != null) {
            getTileFromID(savedNode.getID()).GetComponent<Tile>().setCurrentState(Tile.TileState.NOT_SELECTED);
            savedNode = savedNode.getPreviousNode();
        }

        if (tilesEffectedByPlayerSkill != null) {
            for (int i = 0; i < tilesEffectedByPlayerSkill.Length; i++) {
                getTileFromID(tilesEffectedByPlayerSkill[i]).GetComponent<Tile>().setCurrentState(Tile.TileState.NOT_SELECTED);
            }
        }
    }

    public bool highlightTraversalPathFromCharacterToTile(GameObject character, int tileIndex) {
        if (character == null || tileIndex < 0) return false;
        resetLastPath();

        CharacterController myCharacterController = character.GetComponent<CharacterController>();
        CharacterMovementController myCharacterMovementController = character.GetComponent<CharacterMovementController>();
        Node[] traversalMap = myCharacterMovementController.getTraversalMap();
        if (traversalMap != null) {
            Node selectedNode = traversalMap[tileIndex];
            savedNode = traversalMap[tileIndex];
            while (selectedNode != null) {
                int distanceFromStart = selectedNode.getDistanceFromStart();
                if (distanceFromStart < myCharacterController.getMovementSpeed())
                    getTileFromID(selectedNode.getID()).GetComponent<Tile>().setCurrentState(Tile.TileState.SELECTED_WITHIN_RANGE);
                else
                    getTileFromID(selectedNode.getID()).GetComponent<Tile>().setCurrentState(Tile.TileState.SELECTED_OUTSIDE_RANGE);
                selectedNode = selectedNode.getPreviousNode();
            } return true;
        }
        return false;      
    }

    public bool highlightAttackFromCharacter(GameObject character, int tileIndex) {
        if (character == null || tileIndex < 0) return false;
        resetLastPath();

        CharacterController myCharacterController = character.GetComponent<CharacterController>();
        tilesEffectedByPlayerSkill = myCharacterController.getTilesEffectedByCurrentSkill(tileIndex);
        for (int i = 0; i < tilesEffectedByPlayerSkill.Length; i++) {
            getTileFromID(tilesEffectedByPlayerSkill[i]).GetComponent<Tile>().setCurrentState(Tile.TileState.SELECTED_WITHIN_RANGE);
        }

        return true;
    }

    public int[] getLastAttack() {
        return tilesEffectedByPlayerSkill;
    }

    public Node getLastTraversalPath() {
        return savedNode;
    }
}
