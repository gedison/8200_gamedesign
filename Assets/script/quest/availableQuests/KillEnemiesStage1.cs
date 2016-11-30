using UnityEngine;
using System.Collections;

public class KillEnemiesStage1 : QuestTemplate {
	private bool started = false;
	private bool completed = false;
	private string description = "There are two enemies within the dungeon. They must be cleared" +
		" out, in order to make the area safe. Find and kill the two enemies.";

	public GameObject targets;

	private int toKill;

	// Check if the criteria for starting have been met
	override
	public bool canStart () {
		return !started && !completed;
	}

	// Initialize the quest, do any setup it needs
	override
	public void start () {
		// Begin the quest
		started = true;
		completed = false;
		//foreach 
	}

	// Check if the quest is started
	override
	public bool isStarted () {
		return started;
	}

	// Check if the criteria for the quest is completed
	override
	public bool isCompleted () {
		return completed;
	}

	// End the quest, do any cleanup; returns true if there is a subsequent quest
	override
	public bool complete () {
		completed = true;
		return false;
	}

	// Check if the quest is completed and update it accordingly
	override
	public void updateState () {
		bool done = false;
		int count = 0;
		foreach (Transform enemy in targets.transform) {
			count += 1;
		}
		if (count == 0)
			done = true;

		if (done)
			complete ();
	}

	// If available, acquire the next quest
	override
	public QuestTemplate getNextQuest () {
		return null;
	}

	// Returns the quest description
	override
	public string getDescription () {
		return description;
	}
}
