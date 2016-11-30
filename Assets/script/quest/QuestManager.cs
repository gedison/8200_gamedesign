using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* This class will keep a list of references to all quests in play as well as any
 * completed quests. It will check for any completed quests whenever told to.
 */

public class QuestManager : MonoBehaviour {
	public GameObject qObjects;
	private bool instantiated = false;
	// Keep our list of quests
	private List<QuestTemplate> quests;
	private List<QuestTemplate> available;
	private List<QuestTemplate> active;
	private List<QuestTemplate> completed;

	void Start () {
		// Get all of our quests
		quests = new List<QuestTemplate>();
		available = new List<QuestTemplate>();
		active = new List<QuestTemplate>();
		completed = new List<QuestTemplate>();
		instantiated = true;
	}

	public void SetQuests(GameObject q) {
		if (!instantiated) {
			quests = new List<QuestTemplate>();
			available = new List<QuestTemplate>();
			active = new List<QuestTemplate>();
			completed = new List<QuestTemplate>();
			instantiated = true;
		}
		QuestTemplate[] acquired = q.GetComponents<QuestTemplate> ();
		Debug.Log ("Set Quest: " + q.name);
		Debug.Log ("Length: " + acquired.GetLength(0));
		foreach (QuestTemplate qt in acquired) {
			if (qt == null)
				break;
			Debug.Log (qt.getDescription ());
			Debug.Log (qt.GetType ());
			quests.Add (qt);
			available.Add (qt);
		}
	}

	public void Update () {
		if (!instantiated) {
			quests = new List<QuestTemplate>();
			available = new List<QuestTemplate>();
			active = new List<QuestTemplate>();
			completed = new List<QuestTemplate>();
			instantiated = true;
		}
		ArrayList removable = new ArrayList();
		// Update the state of active quests
		foreach (QuestTemplate q in active) {
			q.updateState ();
		}

		// Check if any new quests are started
		foreach(QuestTemplate q in available) {
			Debug.Log ("At least one.");
			if (q.isStarted () && !q.isCompleted ()) {
				Debug.Log ("YAY!!!");
				active.Add (q);
				removable.Add (q);
				//available.Remove (q);
			} else if (q.isCompleted ()) {
				completed.Add (q);
				removable.Add (q);
				//available.Remove (q);

				// Add the next in the chain, if it exists
				QuestTemplate n = q.getNextQuest ();
				if (n != null && available.Contains(n)) {
					removable.Add (n);
					//available.Remove (n);
					active.Add (n);
				}
			}
		}
		foreach (QuestTemplate q in removable) {
			available.Remove (q);
		}
		removable.Clear ();
		// Check if quests are complete
		foreach (QuestTemplate q in active) {
			if (q.isCompleted ()) {
				Debug.Log ("Quest done!");
				removable.Add (q);
				completed.Add (q);

				// Add the next in the chain, if it exists
				QuestTemplate n = q.getNextQuest ();
				if (n != null && available.Contains(n)) {
					available.Remove (n);
					active.Add (n);
				}
			}
		}
		foreach (QuestTemplate q in removable) {
			active.Remove (q);
		}
		removable.Clear ();


	}

	public string getDescriptions () {
		if (!instantiated) {
			quests = new List<QuestTemplate>();
			available = new List<QuestTemplate>();
			active = new List<QuestTemplate>();
			completed = new List<QuestTemplate>();
			instantiated = true;
		}
		string des = "";
		Debug.Log ("Size: " + active.Count);
		foreach (QuestTemplate q in active) {
			des += q.getDescription ();
			des += "\n\n";
		}

		return des;
	}

	// Get the number of active quests
	int getActiveCount () {
		return active.Count;
	}

	// Get the number of completed quests
	int getCompletedCount () {
		return completed.Count;
	}
}
