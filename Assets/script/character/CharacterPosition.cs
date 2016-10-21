using UnityEngine;
using System.Collections;

public class CharacterPosition : MonoBehaviour {

    private int currentInstanceID = 0;

    void Update() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit))currentInstanceID = hit.transform.GetInstanceID();
    }

    public int getCurrentInstanceID() {
        return currentInstanceID;
    }
}
