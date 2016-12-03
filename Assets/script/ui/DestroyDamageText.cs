using UnityEngine;
using System.Collections;

public class DestroyDamageText : MonoBehaviour {

    public float offset = 1.0f;
    private float heightIncreasePerSecond = 2f;
    private float height = 0;

    void Start () {
        Invoke("DestroyObject", 2.0f);
	}
	
    void DestroyObject() {
        Destroy(gameObject);
    }

    void LateUpdate() {
        height += heightIncreasePerSecond * Time.deltaTime;
        Vector3 worldPos = new Vector3(transform.position.x, transform.position.y + offset + height, transform.position.z);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        transform.position = new Vector3(screenPos.x, screenPos.y, screenPos.z);
    }
}
