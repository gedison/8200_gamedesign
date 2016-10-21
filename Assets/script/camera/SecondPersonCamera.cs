using UnityEngine;
using System.Collections;

public class SecondPersonCamera : MonoBehaviour {

    public GameObject player;
    private Vector3 offset = new Vector3(2.5f, 3.8f, 0f); 
    private float cameraSpeed;

    void Start() {
        cameraSpeed = player.GetComponent<CharacterMovementController>().movementSpeed - 1;
        offset.y += player.transform.position.y;
    }

    void LateUpdate() {
        //transform.rotation = Quaternion.identity;
        transform.position = Vector3.Lerp(this.transform.position, player.transform.position + offset, cameraSpeed * Time.deltaTime);
    }
}
