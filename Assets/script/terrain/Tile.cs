using UnityEngine;

public abstract class Tile : MonoBehaviour {

    public Texture tileTexture;

    void Start() {

      
        if (GetComponent<Renderer>().material.name.Contains("Default")) {
            if (tileTexture != null) GetComponent<Renderer>().material.mainTexture = tileTexture;
            else GetComponent<Renderer>().material.mainTexture = Resources.Load("default", typeof(Texture2D)) as Texture2D;
        }
       
    }

    public abstract int getMovementModifier();
    public abstract string toString();
}
