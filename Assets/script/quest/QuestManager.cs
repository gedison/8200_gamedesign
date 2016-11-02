using UnityEngine;
using System.Collections;

/* This class will keep a list of references to all quests in play as well as any
 * completed quests. It will check for any completed quests whenever told to.
 */

public class QuestManager : MonoBehaviour {
	// Keep our list of quests
	private QuestTemplate[] quests;
	private QuestTemplate[] active;
	private QuestTemplate[] completed;

	void Start () {
		// Get all of our quests
	}

	void Update () {
		// Check if quests are complete
		foreach (QuestTemplate q in quests) {
			if (q.isCompleted ()) {
			}
		}
	}
}
