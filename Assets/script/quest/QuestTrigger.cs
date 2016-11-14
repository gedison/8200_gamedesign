using UnityEngine;
using System.Collections;

public class QuestTrigger : TriggerTemplate {
	private bool isTriggerable;
	public QuestTemplate quest;

	// Activates the trigger; does nothing if it is not triggerable
	override
	public void activate () {
		if (isTriggerable && quest.canStart ()) {
			quest.start ();
		}
	}

	// Queries whether or not the trigger can activate
	override
	public bool triggerable () {
		return isTriggerable && quest.canStart();
	}

	// Set whether or not the trigger can actually activate
	override
	public void canBeTriggered (bool val) {
		isTriggerable = val;
	}
}
