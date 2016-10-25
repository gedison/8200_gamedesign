using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

    public Text selectedText;
    public int totalHealth = 20;
    private int currentHealth;
    private bool isSelected = false;

	void Start () {
        currentHealth = totalHealth;
        selectedText = GameObject.Find("Selected Object").GetComponent<Text>(); 
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

    void OnMouseDown() {
        selectedText.GetComponent<TextSelect>().gameObjectWithHealth = this.gameObject;
    }



    void Update() {
        if (currentHealth == 0) {
            selectedText.GetComponent<TextSelect>().gameObjectWithHealth = null;
            Destroy(this.gameObject);
        }
    }


}
