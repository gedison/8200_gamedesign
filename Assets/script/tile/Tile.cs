using UnityEngine;

public abstract class Tile : MonoBehaviour {

    
    public bool tileIsOccupied = false;
    public Color withinRangeTileColor = new Color(1, 1, .45f);
    public Color outsideOfRangeTileColor = new Color(1, 0, 0);
    private Color defaultTileColor = new Color(1, 1, 1);
    public enum TileState {SELECTED_WITHIN_RANGE, SELECTED_OUTSIDE_RANGE, NOT_SELECTED};
    private TileState currentState = TileState.NOT_SELECTED, lastState = TileState.NOT_SELECTED;
    private int tileID;

    public void setTileID(int tileID) {
        this.tileID = tileID;
    }

    public int getTileID() {
        return tileID;
    }

    void OnMouseEnter() {
        WorldController.instance.onTileHover(tileID);
    }

    void OnMouseOver() {
        if(currentState == TileState.NOT_SELECTED) WorldController.instance.onTileHover(tileID);
    }

    void OnMouseDown() {
        WorldController.instance.onTileSelect(tileID);
    }

    void Update() {
        if (currentState != lastState) {
            switch (currentState) {
                case TileState.NOT_SELECTED:
                    GetComponent<Renderer>().material.shader = Shader.Find("Standard");
                    GetComponent<Renderer>().material.SetColor("_Color", defaultTileColor);
                    break;
                case TileState.SELECTED_OUTSIDE_RANGE:
                    //GetComponent<Renderer>().material.shader = Shader.Find("Self-Illumin/Diffuse");
                    GetComponent<Renderer>().material.SetColor("_Color", outsideOfRangeTileColor);
                    break;
                case TileState.SELECTED_WITHIN_RANGE:
                    //GetComponent<Renderer>().material.shader = Shader.Find("Self-Illumin/Diffuse");
                    GetComponent<Renderer>().material.SetColor("_Color", withinRangeTileColor);
                    break;
                default:
                    GetComponent<Renderer>().material.shader = Shader.Find("Standard");
                    GetComponent<Renderer>().material.SetColor("_Color", defaultTileColor);
                    break;
            }lastState = currentState;
        }
    }

    public void setCurrentState(TileState newState) {
        currentState = newState;
    }

    public void switchTileIsOccupied() {
        tileIsOccupied = !tileIsOccupied;
    }

    public abstract int getMovementModifier();
    public abstract string toString();
}
