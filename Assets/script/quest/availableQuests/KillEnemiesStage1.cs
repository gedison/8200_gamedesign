using UnityEngine;
using System.Collections;

public class KillEnemiesStage1 : QuestTemplate {
	private bool started = false;
	private bool completed = false;

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
		bool done = true;
		foreach (GameObject enemy in targets.transform) {
			if (enemy != null)
				done = false;
		}

		if (done)
			complete ();
	}

	// If available, acquire the next quest
	override
	public QuestTemplate getNextQuest () {
		return null;
	}

    override
    public string getDescription() {
        return "";
    }
}
