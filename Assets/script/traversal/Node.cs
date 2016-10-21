public class Node {
    private int id, distanceFromStart, traversalCost;
    private Node previousNode = null;
    private bool hasBeenVisited = false;

    public Node(int id, int traversalCost) {
        this.id = id;
        this.traversalCost = traversalCost;
        distanceFromStart = 1000000;
    }

    public void setHasBeenVisited() {
        hasBeenVisited = true;
    }

    public bool getHasBeenVisited() {
        return hasBeenVisited;
    }

    public void updateDistanceFromStart(Node testNode) {
        int newDistance = testNode.getDistanceFromStart() + traversalCost;
        if (newDistance < distanceFromStart) {
            previousNode = testNode;
            distanceFromStart = newDistance;
        }
    }

    public int getDistanceFromStart() {
        return distanceFromStart;
    }

    public void setDistanceFromStart(int newDistance) {
        distanceFromStart = newDistance;
    }

    public int getID() {
        return id;
    }

    public Node getPreviousNode() {
        return previousNode;
    }
}
