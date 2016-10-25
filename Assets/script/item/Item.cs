using UnityEngine;
using System.Collections;

public abstract class Item : MonoBehaviour {
    public string itemName;
    public string itemDescription;
    public string classRestriction = "";

    public bool isItemRestrictedByProfession() {
        if (classRestriction.Equals("")) return false;
        else return true;
    }

    public abstract string toString();
}
