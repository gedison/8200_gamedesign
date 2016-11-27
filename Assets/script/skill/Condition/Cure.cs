using UnityEngine;
using System.Collections;

public class Cure : MonoBehaviour, Condition{

    private int turnsActive = 1;

    public string getName() {
        return "Cure Wounds";
    }

    public void removeCondition() {
        Destroy(this);
    }

    public void decrementConditionCount() {
        turnsActive--;
        if (turnsActive <= 0) removeCondition();
    }

    public void doConditionActionOnSelf() {
        if (turnsActive > 0) {
            if (GetComponent<Health>() != null) {
                Health playerHealth = GetComponent<Health>();
                playerHealth.incrementCurrentHealthByX(playerHealth.totalHealth - playerHealth.getCurrentHealth());
                decrementConditionCount();
            }
        }
    }


}
