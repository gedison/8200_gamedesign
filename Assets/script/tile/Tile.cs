using UnityEngine;

public abstract class Tile : MonoBehaviour {

    public Texture tileTexture;
    public Color withinRangeTileColor = new Color(1, 1, .45f);
    public Color outsideOfRangeTileColor = new Color(1, 0, 0);
    private Color defaultTileColor = new Color(1, 1, 1);

    void Start() {
        if (GetComponent<Renderer>().material.name.Contains("Default")) {
            if (tileTexture != null) GetComponent<Renderer>().material.mainTexture = tileTexture;
            else GetComponent<Renderer>().material.mainTexture = Resources.Load("default", typeof(Texture2D)) as Texture2D;
        }
    }

    void OnMouseEnter() {
        GetComponent<Renderer>().material.shader = Shader.Find("Self-Illumin/Diffuse");

        bool isTileWithinRange = true;
        if (isTileWithinRange) {
            GetComponent<Renderer>().material.SetColor("_Color", withinRangeTileColor);
        } else {
            GetComponent<Renderer>().material.SetColor("_Color", outsideOfRangeTileColor);
        } 
    }

    void OnMouseExit() {
        GetComponent<Renderer>().material.shader = Shader.Find("Standard");
        GetComponent<Renderer>().material.SetColor("_Color", defaultTileColor);
    }

    void OnMouseClick() {
        //WorldController.instance;
    }

    public abstract int getMovementModifier();
    public abstract string toString();
}
