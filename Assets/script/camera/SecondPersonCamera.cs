using UnityEngine;
using System.Collections;

public class SecondPersonCamera : MonoBehaviour {

    public GameObject player;
    public float mouseSensitivity = .1f;

    private Vector3 lastMousePosition;
    private Vector3 cameraOffsetFromPlayer = new Vector3(2.5f, 3.8f, 0f); 
    private float cameraSpeed;
    private bool isPlayerMoving = false;

    private float yAxisRotationOffset = 0f;
    private float maxDistanceAwayFromPlayer = 5.5f;


    void Start() {
        cameraSpeed = player.GetComponent<CharacterMovementController>().animationSpeed-2;
        cameraOffsetFromPlayer.y += player.transform.position.y;
        transform.position = player.transform.position + cameraOffsetFromPlayer;
    }

    private bool isCameraWithinDistanceOfPlayer(float x, float y, float x2, float y2, float maxDistance) {
        float distance = Mathf.Sqrt(Mathf.Pow(x2 - x, 2) + Mathf.Pow(y2 - y, 2));
        if (distance < maxDistance) return true;
        else return false;
    }

    void Update() {
        //If mouse button is down, record last position
        if (Input.GetMouseButtonDown(2) || Input.GetMouseButtonDown(1)) {
            lastMousePosition = Input.mousePosition;
        }

        //Pan the Camera
        if (Input.GetMouseButton(1)) {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            delta = delta * mouseSensitivity;

            //Mutiple the delta by the camera rotation around the y-axis so the pan direction is equivalent to the mouse movement
            delta = Quaternion.Euler(0, 0, -yAxisRotationOffset) * delta;

            //If the pan is within the max distance the camera can be from the player allow it
            if(player!=null && isCameraWithinDistanceOfPlayer(transform.position.x+delta.y, transform.position.z-delta.x, player.transform.position.x, player.transform.position.z, maxDistanceAwayFromPlayer))
                transform.Translate(delta.y, 0, -delta.x, Space.World);

            //Update the mouse position
            lastMousePosition = Input.mousePosition;
        }

        //Rotate the Camera around the player
        if (Input.GetMouseButton(2)) {
            Vector3 delta = Input.mousePosition - lastMousePosition;

            transform.RotateAround(player.transform.position, new Vector3(0, 1), delta.x * .5f);
            //Update the rotation around the y-axis
            yAxisRotationOffset += delta.x * .5f;

            //Update the camera offset
            cameraOffsetFromPlayer += (transform.position - (player.transform.position + cameraOffsetFromPlayer));
            lastMousePosition = Input.mousePosition;
        }
    }

    void LateUpdate() {
        if (player != null && player.GetComponent<CharacterMovementController>().isCharacterMoving()) isPlayerMoving = true;
        else isPlayerMoving = false;

        //If the character is moving and the camera is more than maxDistance - 1 units away move the camera with the character
        if (isPlayerMoving && !isCameraWithinDistanceOfPlayer(transform.position.x, transform.position.z, player.transform.position.x, player.transform.position.z, maxDistanceAwayFromPlayer-1)) {
            transform.position = Vector3.Lerp(this.transform.position, player.transform.position + cameraOffsetFromPlayer, cameraSpeed * Time.deltaTime);
        }
    }
}
