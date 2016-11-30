using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JournalDisplay : MonoBehaviour {
	private string text = "";
	private bool instantiated = false;

	// This is our display object
	public GameObject child;
	public Scrollbar bar;

	void Update () {
		if (!instantiated) {
			text = "Did this work?";
			GetComponent <ScrollRect> ().verticalScrollbar = bar;
			instantiated = true;
		}
		// Get a string containing the different quest descriptions
		text = WorldController.instance.qManager.getDescriptions ();

		Debug.Log ("Log: " + text);

		child.GetComponent<Text> ().text = text;
	}
}
