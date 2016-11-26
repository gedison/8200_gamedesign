using UnityEngine;
using System.Collections;

public class SecondPersonCamera : MonoBehaviour {

    public GameObject player;
    public float mouseSensitivity = .1f;
    private Vector3 lastPosition;
    private Vector3 offset = new Vector3(2.5f, 3.8f, 0f); 
    private float cameraSpeed;
    private bool isMoving = false;
    private bool panned = false;
    private float rotX = 60, rotY = -90, rotZ = 0;

    private float yOffset = 0f;


    void Start() {
        cameraSpeed = player.GetComponent<CharacterMovementController>().movementSpeed - 1;
        offset.y += player.transform.position.y;
        transform.position = player.transform.position + offset;
    }

    void Update() {
        if (Input.GetMouseButtonDown(2) || Input.GetMouseButtonDown(1)) {
            lastPosition = Input.mousePosition;
        }

        
        if (Input.GetMouseButton(1)) {
            Vector3 delta = Input.mousePosition - lastPosition;
            delta = delta * mouseSensitivity;
            delta = Quaternion.Euler(0, 0, -yOffset) * delta;
            transform.Translate(delta.y, 0, -delta.x, Space.World);
            lastPosition = Input.mousePosition;

            panned = true;
        }
        
        /*
        if (Input.GetMouseButton(2) && !panned) {
            Vector3 delta = Input.mousePosition - lastPosition;
            if (Vector3.Distance(transform.position, (player.transform.position + offset)) < 6) {
                transform.RotateAround(player.transform.position, new Vector3(0, 1), delta.x * .5f);
                yOffset += delta.x * .5f;
            }

            offset += (transform.position - (player.transform.position+offset));
            lastPosition = Input.mousePosition;
        }
        */
    }

    void LateUpdate() {
        if(player!=null && player.GetComponent<CharacterMovementController>().isCharacterMoving())isMoving = true;

        if (player != null &&  isMoving) {
            panned = false;
            //transform.rotation = Quaternion.Euler(rotX, rotY, rotZ);
            transform.position = Vector3.Lerp(this.transform.position, player.transform.position + offset, cameraSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, (player.transform.position+offset)) < .5) isMoving = false;
        }
        
            
    }
}
