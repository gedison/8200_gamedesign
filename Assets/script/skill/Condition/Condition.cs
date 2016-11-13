using UnityEngine;

public interface Condition  {
    string getName();
    void removeCondition();
    void decrementConditionCount();
    void doConditionActionOnSelf();
}
