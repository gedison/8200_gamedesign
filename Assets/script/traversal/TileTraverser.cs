using UnityEngine;

interface TileTraverser{
    void setDimensions(Transform[] weights, int width, int height);
    Node[] getTileTrafersal(int start);
}
