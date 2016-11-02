﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* This class will keep a list of references to all quests in play as well as any
 * completed quests. It will check for any completed quests whenever told to.
 */

public class QuestManager : MonoBehaviour {
	public GameObject qObjects;
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
	}

	void Update () {
		// Check if any new quests are started
		foreach(QuestTemplate q in available) {
			if (q.isStarted () && !q.isCompleted ()) {
				active.Add (q);
				available.Remove (q);
			} else if (q.isCompleted ()) {
				completed.Add (q);
				available.Remove (q);
			}
		}
		// Check if quests are complete
		foreach (QuestTemplate q in active) {
			if (q.isCompleted ()) {
				active.Remove (q);
				completed.Add (q);
			}
		}
	}
}