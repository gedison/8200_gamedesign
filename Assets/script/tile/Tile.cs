using UnityEngine;

public abstract class Tile : MonoBehaviour {

    public Texture tileTexture;

    public Color withinRangeTileColor = new Color(1, 1, .45f);
    public Color outsideOfRangeTileColor = new Color(1, 0, 0);
    private Color defaultTileColor = new Color(1, 1, 1);
   
    public enum TileState {SELECTED_WITHIN_RANGE, SELECTED_OUTSIDE_RANGE, NOT_SELECTED};
    private TileState currentState = TileState.NOT_SELECTED, lastState = TileState.NOT_SELECTED;

    void Start() {
        if (GetComponent<Renderer>().material.name.Contains("Default")) {
            if (tileTexture != null) GetComponent<Renderer>().material.mainTexture = tileTexture;
            else GetComponent<Renderer>().material.mainTexture = Resources.Load("default", typeof(Texture2D)) as Texture2D;
        }
    }

    void OnMouseEnter() {
        WorldController.instance.setCurrentPath(this.GetComponent<Transform>());
    }

    void Update() {
        if (currentState != lastState) {
            switch (currentState) {
                case TileState.NOT_SELECTED:
                    GetComponent<Renderer>().material.shader = Shader.Find("Standard");
                    GetComponent<Renderer>().material.SetColor("_Color", defaultTileColor);
                    break;
                case TileState.SELECTED_OUTSIDE_RANGE:
                    GetComponent<Renderer>().material.shader = Shader.Find("Self-Illumin/Diffuse");
                    GetComponent<Renderer>().material.SetColor("_Color", outsideOfRangeTileColor);
                    break;
                case TileState.SELECTED_WITHIN_RANGE:
                    GetComponent<Renderer>().material.shader = Shader.Find("Self-Illumin/Diffuse");
                    GetComponent<Renderer>().material.SetColor("_Color", withinRangeTileColor);
                    break;
                default:
                    GetComponent<Renderer>().material.shader = Shader.Find("Standard");
                    GetComponent<Renderer>().material.SetColor("_Color", defaultTileColor);
                    break;
            }lastState = currentState;
        }
    }

    public void setCurrentState(Tile.TileState newState) {
        currentState = newState;
    }

    public abstract int getMovementModifier();
    public abstract string toString();
}
