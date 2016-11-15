using UnityEngine;
using System.Collections;

public class Dazed : MonoBehaviour, Condition{

    private int turnsActive = 2;

    public string getName() {
        return "Dazed";
    }

    public void removeCondition() {
        Destroy(this);
    }

    public void decrementConditionCount() {
        turnsActive--;
        if (turnsActive <= 0) removeCondition();
    }

    public void doConditionActionOnSelf(){
        if (turnsActive > 0) {
            if(GetComponent<CharacterController>()!=null)GetComponent<CharacterController>().setActionPointsToZero();
            decrementConditionCount();
        }
    }


}
