using UnityEngine;
using System.Collections;

public class CharacterPosition : MonoBehaviour {

    private int currentTileID = 0;


    void Update() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit)) {
            GameObject tile = hit.transform.gameObject;
            int temp = tile.GetComponent<Tile>().getTileID();
            if (temp != currentTileID) {
                if (currentTileID == 0) {
                    currentTileID = temp;
                    WorldController.instance.switchTileIsOccupied(currentTileID);
                } else {

                    WorldController.instance.switchTileIsOccupied(currentTileID);
                    currentTileID = temp;
                    WorldController.instance.switchTileIsOccupied(currentTileID);
                }              
            }
        }
    }

    public int getTileID() {
        return currentTileID;
    }
}
