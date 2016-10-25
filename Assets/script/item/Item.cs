using UnityEngine;
using System.Collections;

public abstract class Item : MonoBehaviour {
    public string itemName;
    public string itemDescription;

    public abstract string toString();
}
