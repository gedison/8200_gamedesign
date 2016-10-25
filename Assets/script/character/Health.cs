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
	

}
