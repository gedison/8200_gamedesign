using UnityEngine;

public class CharacterPosition : MonoBehaviour {

    private int currentTileID = 0;

    /* Uses a downward raycast to determine what tile the object currently occupies
     * If the occupied tile has changed, tell the previous tile it is no longer occupied, and the new tile that it has
     * a new inhabitant
     */
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

    void OnMouseDown() {
        WorldController.instance.onTileHover(currentTileID);
        WorldController.instance.onTileSelect(currentTileID);
    }

    public int getTileID() {
        return currentTileID;
    }
}
