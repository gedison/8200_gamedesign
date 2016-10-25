using UnityEngine;
using System.Collections.Generic;

public class CharacterMovementController : MonoBehaviour {

    public float animationSpeed = 3.0f;
    public float movementSpeed = 4;

    private List<Node> path;
    private Transform currentTarget;
    private float playerHeight;
    private bool isMoving = false;

    void Start() {
        playerHeight = transform.position.y;
    }

    void Update () {
        if (currentTarget != null && !doesCurrentLocationEqualCurrentTarget()) {      
            Vector3 target = currentTarget.position;
            target.y = playerHeight;

            float step = animationSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target, step);
        }
	}

    public void setPath(List<Node> path) {
        isMoving = true;

        this.path = path;
        currentTarget = WorldController.instance.getTileFromArrayIndex(path[0].getID());
        path.RemoveAt(0);
    }

    public bool isCharacterMoving() {
        return isMoving;
    }

    private bool doesCurrentLocationEqualCurrentTarget() {
        if(transform.position.x == currentTarget.transform.position.x && transform.position.z == currentTarget.transform.position.z) {
            currentTarget.GetComponent<Tile>().setCurrentState(Tile.TileState.NOT_SELECTED);
            if (path.Count > 0) {
                currentTarget = WorldController.instance.getTileFromArrayIndex(path[0].getID());
                path.RemoveAt(0);
            } else {
                currentTarget = null;
                isMoving = false;
            }
            return true;
        }else return false;
    }
}
