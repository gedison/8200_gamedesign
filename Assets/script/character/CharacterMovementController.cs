using UnityEngine;
using System.Collections.Generic;

public class CharacterMovementController : MonoBehaviour {

    //Speed that the character is animated to move
    public float animationSpeed = 3.0f;

    //Boolean value to indicate that the characters movement grid needs to be updated
    private bool traversalMapNeedsToBeUpdated = true;
    private Node[] traversalMap;

    private List<Node> path;
    private Transform currentTarget;

    //Is the player currently being animated
    private bool isMoving = false;

    void Update () {
        if (currentTarget != null && !doesCurrentLocationEqualCurrentTarget()) {
            Vector3 target = currentTarget.position;
            target.y = transform.position.y;

            float step = animationSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target, step);
        }
	}

    public void setUpdateTraversalMapToTrue() {
        traversalMapNeedsToBeUpdated = true;
    }

    public bool doesTraversalMapNeedToBeUpdated() {
        return traversalMapNeedsToBeUpdated;
    }

    public void setTraversalMap(Node[] traversalMap) {
        traversalMapNeedsToBeUpdated = false;
        this.traversalMap = traversalMap;
    }

    public Node[] getTraversalMap() {
        return traversalMap;
    }

    //Starts the players movement along the path of Nodes
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
        //Check if the character is at their goal
        if(transform.position.x == currentTarget.transform.position.x && transform.position.z == currentTarget.transform.position.z) {
            //Change the tile state to visited
            currentTarget.GetComponent<Tile>().setCurrentState(Tile.TileState.NOT_SELECTED);
            //If the players path is not empty
            if (path.Count > 0) {
                //Set the next movement goal
                currentTarget = WorldController.instance.getTileFromArrayIndex(path[0].getID());
                path.RemoveAt(0);
            } else {
                //Else end the players movement
                traversalMapNeedsToBeUpdated = true;
                currentTarget = null;
                isMoving = false;
            }
            return true;
        }else return false;
    }
}
