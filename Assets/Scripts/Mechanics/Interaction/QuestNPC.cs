using UnityEngine;
using System.Collections;

public class QuestNPC : NPC {
	
	public int ID = -1; //Use this in a quest to refer to this NPC.
	public NPCTalking[] talkComponents;
	public bool talking;
	public float waitTime = 0;

	void Awake() {
		talkComponents = GetComponents<NPCTalking> ();

	}

	public void Talk(Quest quest) {
		StartCoroutine (talk (quest));
	}

	private IEnumerator talk (Quest quest) {

		talking = true;
		waitTime = 0;
		bool nextStage = false;
		if (quest.currentStage < quest.stages.Length && quest.stages [quest.currentStage].npcID == ID) {
			nextStage = true;
			waitTime += 0.5f;
		}
		if (quest.currentStage < talkComponents.Length) {
			StartCoroutine (talkComponents [quest.currentStage].Talk ());
			foreach (float delay in talkComponents[quest.currentStage].delays) {
				waitTime += delay;
			}
		}
		yield return new WaitForSeconds (waitTime);
		if(nextStage)
			quest.NextStage ();
		talking = false;
	}

}
