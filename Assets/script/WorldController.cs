using UnityEngine;
using System.Collections;

public class WorldController : MonoBehaviour {

    public static WorldController instance = null;
    public GameObject tiles;

    public int tileWidth, tileHeight;
    private Transform[] tileArray;
    private Node[] traversalMap;
    private Node savedNode = null;
    private TileTraverser tileTraverser;

    void Awake() {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
        StartGame();
    }

    private void StartGame() {
        int[] weights = new int[tileHeight * tileWidth];
        tileArray = new Transform[tileHeight * tileWidth];

        int height = 0;
        foreach (Transform tileRow in tiles.transform) {
            int width = 0;
            foreach(Transform tile in tileRow.transform) {
                tileArray[(height * tileWidth) + width] = tile;
                weights[(height * tileWidth) + width] = tile.GetComponent<Tile>().getMovementModifier();
                width++;
            }height++;  
        }

        tileTraverser = new DijkstraTileTraverser(weights, tileWidth, tileHeight, true);
        traversalMap = tileTraverser.getTileTrafersal(7);
    }

    private int getSelectedTileIndex(Transform selectedTile) {
        for(int i=0; i<tileArray.Length; i++) {
            if (selectedTile.GetInstanceID() == tileArray[i].GetInstanceID()) return i;
        }return -1;
    }

    public void setCurrentPath(Transform selectedTile) {
        while(savedNode != null) {
            tileArray[savedNode.getID()].GetComponent<Tile>().setCurrentState(Tile.TileState.NOT_SELECTED);
            savedNode = savedNode.getPreviousNode();
        }

        int index = getSelectedTileIndex(selectedTile);
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

    void Update() {
     
    }
}

