using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour {

    public string type;

    void Start() {
        Debug.Log("START");
        Collider myCollider = this.GetComponent<Collider>();
        myCollider.isTrigger = true;
    }


    void OnTriggerEnter(Collider other) {
        Debug.Log("TRIGGERED");
        switch (type) {
            case "cure": other.gameObject.AddComponent<Cure>(); break;
        }

        Destroy(gameObject);
    }

    void Update() {
        Vector3 rotationAxis = new Vector3(0.0f, 1.0f, 0.75f);
        float rotationAmount = 60.0f;
        transform.Rotate(rotationAxis * rotationAmount * Time.deltaTime);
    }

}
