using UnityEngine;
using System.Collections;

public class SecondPersonCamera : MonoBehaviour {

    public GameObject player;
    private Vector3 offset = new Vector3(2.5f, 3.8f, 0f); 
    private float cameraSpeed;

    void Start() {
        cameraSpeed = player.GetComponent<CharacterMovementController>().movement - 1;
        offset.y += player.GetComponent<CharacterMovementController>().playerHeight;
    }

    void LateUpdate() {
        //transform.rotation = Quaternion.identity;
        transform.position = Vector3.Lerp(this.transform.position, player.transform.position + offset, cameraSpeed * Time.deltaTime);
    }
}
