using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterUIController : MonoBehaviour {

    public GameObject player;
    public Slider healthBar;
    public UICircle2 apBar;

    private CharacterController myCharacterController;
    private Health myCharacterHealth;

    // Use this for initialization
    void Start () {
        myCharacterController = player.GetComponent<CharacterController>();

    }

    void Update() {
        if (myCharacterHealth == null && player != null) {
            myCharacterHealth = player.GetComponent<Health>();
        }

        if (player != null) {
            if (myCharacterHealth != null) healthBar.value = ((float)myCharacterHealth.getCurrentHealth() / myCharacterHealth.totalHealth);
            float test = ((float)myCharacterController.getCurrentActionPoints() / myCharacterController.totalActionPoints);
            int apPercent = (int)(test * 100);
            apBar.newPercent = (apPercent);
        }
    }
}
