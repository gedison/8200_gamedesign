using UnityEngine;

public class Health : MonoBehaviour {
    public int totalHealth = 20;
    private int currentHealth;

	void Start () {
        currentHealth = totalHealth;
    }

    public int getCurrentHealth() {
        return currentHealth;
    }

    public void incrementCurrentHealthByX(int x) {
        currentHealth = ((currentHealth + x) > totalHealth) ? totalHealth : (currentHealth + x);
    }

    public void decrementCurrentHealthByX(int x) {
        currentHealth = ((currentHealth - x) < 0) ? 0 : (currentHealth - x);
    }


    void Update() {
        //On death update the game board and delete the component's game object and any associated UI
        if (currentHealth == 0) {
            int currentInstanceID = GetComponent<CharacterPosition>().getTileID();
            WorldController.instance.switchTileIsOccupied(currentInstanceID);
            WorldController.instance.updateTraversalMap(true);

            if(GetComponent<EnemyScreenSpaceUI>()!=null)GetComponent<EnemyScreenSpaceUI>().destroyObject();
            if (GetComponent<ScreenSpaceDamageUI>() != null) GetComponent<ScreenSpaceDamageUI>().destroyObject();
            Destroy(this.gameObject);
        }
    }
}
