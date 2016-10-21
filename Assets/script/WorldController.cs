using UnityEngine;

public class WorldController : MonoBehaviour {

    public static WorldController instance = null;
    public GameObject tiles;
    public GameObject player;

    public int tileWidth, tileHeight;
    private Transform[] tileArray;
    private Node[] traversalMap;
    private Node savedNode = null;
    private TileTraverser tileTraverser;
    private int lastID;

    void Awake() {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
        StartGame();
    }

    private void StartGame() {
        int[] weights = new int[tileHeight * tileWidth];
        tileArray = new Transform[tileHeight * tileWidth];
       
        int height = 0, width = 0;
        foreach (Transform tileRow in tiles.transform) {
            width = 0;
            foreach(Transform tile in tileRow.transform) {
                tileArray[(height * tileWidth) + width] = tile;
                weights[(height * tileWidth) + width] = tile.GetComponent<Tile>().getMovementModifier();
                width++;
            }height++;  
        } tileTraverser = new DijkstraTileTraverser(weights, tileWidth, tileHeight, true);
    }

    private int getTileIndexFromID(int tileID) {
        for (int i = 0; i < tileArray.Length; i++) {
            if (tileID == tileArray[i].GetInstanceID()) return i;
        }return -1;
    }

    public void setCurrentPath(int tileID) {
        while(savedNode != null) {
            tileArray[savedNode.getID()].GetComponent<Tile>().setCurrentState(Tile.TileState.NOT_SELECTED);
            savedNode = savedNode.getPreviousNode();
        }

        int index = getTileIndexFromID(tileID);
        if (index != -1) {
            Node selectedNode = traversalMap[index];
            savedNode = traversalMap[index];
            while (selectedNode != null) {
                int distanceFromStart = selectedNode.getDistanceFromStart();
                if (distanceFromStart < 5) tileArray[selectedNode.getID()].GetComponent<Tile>().setCurrentState(Tile.TileState.SELECTED_WITHIN_RANGE);
                else tileArray[selectedNode.getID()].GetComponent<Tile>().setCurrentState(Tile.TileState.SELECTED_OUTSIDE_RANGE);
                selectedNode = selectedNode.getPreviousNode();
            }
        }
    }

    public void moveToTile() {
        /*
        while (savedNode != null) {
            Debug.Log(tileArray[savedNode.getID()].transform.position.x + " " + tileArray[savedNode.getID()].transform.position.z);
            savedNode = savedNode.getPreviousNode();
        }
        */

    }

    void Update() {
        int currentID = player.GetComponent<CharacterPosition>().getCurrentInstanceID();
        if (currentID != lastID) {
            traversalMap = tileTraverser.getTileTrafersal(getTileIndexFromID(currentID));
            lastID = currentID;
        }
    }
}

