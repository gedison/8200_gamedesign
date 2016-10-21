public class DijkstraTileTraverser : TileTraverser{
    private int[] weights;
    private int width, height;
    private bool isCartesian = false;

    private enum Direction {TOP_LEFT, TOP, TOP_RIGHT, LEFT, RIGHT, BOTTOM_LEFT, BOTTOM, BOTTOM_RIGHT};
    private Direction[] omniDirectional = {Direction.TOP_LEFT, Direction.TOP, Direction.TOP_RIGHT, Direction.LEFT, Direction.RIGHT, Direction.BOTTOM_LEFT, Direction.BOTTOM, Direction.BOTTOM_RIGHT};
    private Direction[] cartesian = {Direction.TOP, Direction.LEFT, Direction.RIGHT, Direction.BOTTOM};

    public DijkstraTileTraverser(int[] weights, int width, int height, bool isCartesian) {
        this.isCartesian = isCartesian;
        setDimensions(weights, width, height);
    }

    public void setDimensions(int[] weights, int width, int height) {
        this.weights = weights;
        this.width = width;
        this.height = height;
    }

    public void setOmnidirectional() {
        isCartesian = false;
    }

    public void setCartesian() {
        isCartesian = true;
    }

    public Node[] getTileTrafersal(int start) {
        Node[] nodes = new Node[width * height];
        for (int i = 0; i < (width * height); i++) nodes[i] = new Node(i, weights[i]);
        nodes[start].setDistanceFromStart(0);

        int minIndex = getMinimumNodeIndex(nodes);
        while (minIndex != -1) {
            Direction[] directions = (isCartesian) ? cartesian : omniDirectional; 
            foreach(Direction direction in directions) {
                int index = getIndexOneUnitFromStartInDirectionX(minIndex, direction);
                if (index >= 0) nodes[index].updateDistanceFromStart(nodes[minIndex]);
            }
            nodes[minIndex].setHasBeenVisited();
            minIndex = getMinimumNodeIndex(nodes);
        }return nodes;
    }

    private int getMinimumNodeIndex(Node[] nodes) {
        int minIndex = -1;
        int minDistance = 1000000;
        for (int i = 0; i < nodes.Length; i++) {
            Node currentNode = nodes[i];
            if ((!currentNode.getHasBeenVisited()) && (currentNode.getDistanceFromStart() < minDistance)) {
                minDistance = currentNode.getDistanceFromStart();
                minIndex = i;
            }
        }
        return minIndex;
    }

    private int getIndexOneUnitFromStartInDirectionX(int startIndex, Direction direction) {
        int ret = -1;
        switch (direction) {
            case Direction.TOP_LEFT:
                ret = startIndex - (width + 1);
                if (startIndex % width == 0) ret = -1;
                break;
            case Direction.TOP: ret = startIndex - width; break;
            case Direction.TOP_RIGHT:
                ret = startIndex - (width - 1);
                if ((startIndex + 1) % width == 0) ret = -1;
                break;
            case Direction.LEFT:
                ret = startIndex - 1;
                if (startIndex % width == 0) ret = -1;
                break;
            case Direction.RIGHT:
                ret = startIndex + 1;
                if ((startIndex + 1) % width == 0) ret = -1;
                break;
            case Direction.BOTTOM_LEFT:
                ret = startIndex + (width - 1);
                if ((startIndex % width == 0) || (startIndex >= ((width * height) - width))) ret = -1;
                break;
            case Direction.BOTTOM:
                ret = startIndex + width;
                if (ret >= (width * height)) ret = -1;
                break;
            case Direction.BOTTOM_RIGHT:
                ret = startIndex + (width + 1);
                if (((startIndex + 1) % width == 0) || (startIndex >= ((width * height) - width))) ret = -1;
                break;
            default: break;
        }return ret;
    }
}
