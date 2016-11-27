using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {

    public float rpm = 15.0f;
	
	void Update () {
        transform.Rotate((float)3.0 * rpm * Time.deltaTime, (float)6.0 * rpm * Time.deltaTime, 0 );
    }
}
