using UnityEngine;
using System.Collections;

public class SecondPersonCamera : MonoBehaviour {

    public GameObject player;
    public float mouseSensitivity = .1f;
    private Vector3 lastPosition;
    private Vector3 offset = new Vector3(2.5f, 3.8f, 0f); 
    private float cameraSpeed;
    private bool isMoving = false;

    private float rotX = 60, rotY = -90, rotZ = 0;


    void Start() {
        cameraSpeed = player.GetComponent<CharacterMovementController>().movementSpeed - 1;
        offset.y += player.transform.position.y;
        transform.position = player.transform.position + offset;
    }

    void Update() {
        if (Input.GetMouseButtonDown(2) || Input.GetMouseButtonDown(1)) {
            lastPosition = Input.mousePosition;
        }

        /*
        if (Input.GetMouseButton(2)) {
            Vector3 delta = Input.mousePosition - lastPosition;
            Debug.Log("Delta X: " + delta.x * mouseSensitivity + " " + delta.y * mouseSensitivity + " " + delta.z * mouseSensitivity);
            transform.Translate(delta.y * mouseSensitivity, 0, -delta.x * mouseSensitivity, Space.World);
            lastPosition = Input.mousePosition;
        }
        */
        
        if (Input.GetMouseButton(1)) {
            Vector3 delta = Input.mousePosition - lastPosition;
            Debug.Log("Delta X: " + delta.x * mouseSensitivity + " " + delta.y * mouseSensitivity + " " + delta.z * mouseSensitivity);
            if (Vector3.Distance(transform.position, (player.transform.position + offset)) < 6) transform.RotateAround(player.transform.position, new Vector3(0, 1), -delta.x * .5f);

            offset += (transform.position - (player.transform.position+offset));
            lastPosition = Input.mousePosition;
        }


    }

    void LateUpdate() {
        if(player!=null && player.GetComponent<CharacterMovementController>().isCharacterMoving())isMoving = true;

        if (player != null &&  isMoving) {
            //transform.rotation = Quaternion.Euler(rotX, rotY, rotZ);
            transform.position = Vector3.Lerp(this.transform.position, player.transform.position + offset, cameraSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, (player.transform.position+offset)) < .5) isMoving = false;
        }
        
            
    }
}
