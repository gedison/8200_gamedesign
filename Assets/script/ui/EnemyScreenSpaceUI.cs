using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyScreenSpaceUI : MonoBehaviour {

    public Canvas canvas;
    public GameObject healthPrefab;
    public GameObject healthPanel;
    public float healthPanelOffset = 0.45f;

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
        healthSlider.value = enemyHealth.getCurrentHealth()/ (float)enemyHealth.totalHealth;
        Vector3 worldPos = new Vector3(transform.position.x, transform.position.y + healthPanelOffset, transform.position.z);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        healthPanel.transform.position = new Vector3(screenPos.x, screenPos.y, screenPos.z);
    }

    public void destroyObject() {
        Destroy(healthPanel);
    }

    

    
}
