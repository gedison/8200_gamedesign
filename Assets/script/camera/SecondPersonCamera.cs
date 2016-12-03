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

    private float yOffset = 0f;


    void Start() {
        cameraSpeed = player.GetComponent<CharacterMovementController>().movementSpeed - 1;
        offset.y += player.transform.position.y;
        transform.position = player.transform.position + offset;
    }

    private bool isCameraWithinDistanceOfPlayer(float x, float y, float x2, float y2, float maxDistance) {
        float distance = Mathf.Sqrt(Mathf.Pow(x2 - x, 2) + Mathf.Pow(y2 - y, 2));
        if (distance < maxDistance) return true;
        else return false;
    }

    void Update() {
        if (Input.GetMouseButtonDown(2) || Input.GetMouseButtonDown(1)) {
            lastPosition = Input.mousePosition;
        }

        
        if (Input.GetMouseButton(1)) {
            Vector3 delta = Input.mousePosition - lastPosition;
            delta = delta * mouseSensitivity;
            delta = Quaternion.Euler(0, 0, -yOffset) * delta;

            if(isCameraWithinDistanceOfPlayer(transform.position.x+delta.y, transform.position.z-delta.x, player.transform.position.x, player.transform.position.z, 5.0f))
                transform.Translate(delta.y, 0, -delta.x, Space.World);

            lastPosition = Input.mousePosition;
        }
        
        
        if (Input.GetMouseButton(2)) {
            Vector3 delta = Input.mousePosition - lastPosition;
            if (Vector3.Distance(transform.position, (player.transform.position + offset)) < 6) {
                transform.RotateAround(player.transform.position, new Vector3(0, 1), delta.x * .5f);
                yOffset += delta.x * .5f;
            }

            offset += (transform.position - (player.transform.position+offset));
            lastPosition = Input.mousePosition;
        }
    }

    void LateUpdate() {
        if(player!=null && player.GetComponent<CharacterMovementController>().isCharacterMoving())isMoving = true;

        if (player != null &&  isMoving) {
            if (!isCameraWithinDistanceOfPlayer(transform.position.x , transform.position.z, player.transform.position.x, player.transform.position.z, 3.0f)) {
                transform.position = Vector3.Lerp(this.transform.position, player.transform.position + offset, cameraSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, (player.transform.position + offset)) < 3.0f) isMoving = false;
            }
        }
        
            
    }
}
