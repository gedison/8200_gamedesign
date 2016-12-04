using UnityEngine;
using UnityEngine.UI;

public class EnemyScreenSpaceUI : MonoBehaviour {

    public float healthPanelOffset = 0.45f;

    public Canvas canvas;
    public GameObject healthPrefab;

    private GameObject healthPanel;
    private Health enemyHealth;
    private Text enemyName;
    private Slider healthSlider;

    void Start() {
        enemyHealth = GetComponent<Health>();
        healthPanel = Instantiate(healthPrefab) as GameObject;
        healthPanel.transform.SetParent(canvas.transform, false);
        enemyName = healthPanel.GetComponentInChildren<Text>();
        enemyName.text = name;
        healthSlider = healthPanel.GetComponentInChildren<Slider>();
    }

    void LateUpdate() {
        if (enemyHealth == null) enemyHealth = GetComponent<Health>();
        healthSlider.value = enemyHealth.getCurrentHealth()/(float)enemyHealth.totalHealth;

        //Converts the tranform postion of the game object to screen coordinates
        Vector3 worldPos = new Vector3(transform.position.x, transform.position.y + healthPanelOffset, transform.position.z);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        healthPanel.transform.position = new Vector3(screenPos.x, screenPos.y, screenPos.z);
    }

    public void destroyObject() {
        Destroy(healthPanel);
    }
}
