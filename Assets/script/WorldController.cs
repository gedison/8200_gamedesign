using UnityEngine;

public class WorldController : MonoBehaviour {

    public static WorldController instance = null;
    public GameObject tiles;

    void Awake() {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
        StartGame();
    }

    private void StartGame() {


        

        foreach (Transform tileRow in tiles.transform) {
            foreach(Transform tile in tileRow.transform) {
                //Debug.Log(tile.name+" "+tile.GetInstanceID()+" "+tile.GetComponent<Tile>().getMovementModifier());
                
        
            }     
        }
    }

    void Update() {
     
    }
}

