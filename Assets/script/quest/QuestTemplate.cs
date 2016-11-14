using UnityEngine;
using System.Collections;

/* This class will act as an abstract base class for any quest. Each quest must
 * fulfill the given methods for the manager to properly interact with it.
 */
public abstract class QuestTemplate : MonoBehaviour {

	// Check if the criteria for starting have been met
	public abstract bool canStart ();

	// Initialize the quest, do any setup it needs
	public abstract void start ();

	// Check if the quest is started
	public abstract bool isStarted ();

	// Check if the criteria for the quest is completed
	public abstract bool isCompleted ();

	// End the quest, do any cleanup; returns true if there is a subsequent quest
	public abstract bool complete ();

	// Update the state of the quest
	public abstract void updateState ();

	// If available, acquire the next quest
	public abstract QuestTemplate getNextQuest ();

	// Returns a text description of the quest
	public abstract string getDescription();
}
