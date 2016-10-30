using UnityEngine;
using System.Collections;

public class CharacterPosition : MonoBehaviour {

    private int currentInstanceID = 0;

    void Update() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit)) {
            int temp = hit.transform.GetInstanceID();
            if (temp != currentInstanceID) {
                if (currentInstanceID == 0) {
                    currentInstanceID = temp;
                    WorldController.instance.switchTileIsOccupied(currentInstanceID);
                } else {
                    WorldController.instance.switchTileIsOccupied(currentInstanceID);
                    currentInstanceID = temp;
                    WorldController.instance.switchTileIsOccupied(currentInstanceID);
                }              
            }
        }
    }

    public int getCurrentInstanceID() {
        return currentInstanceID;
    }
}
