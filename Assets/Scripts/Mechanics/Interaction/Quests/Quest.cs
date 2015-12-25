using UnityEngine;
using System.Collections;

public class QStage {
	public int npcID = 0;
	public enum InteractTypes {
		TALK
	}
	public InteractTypes InteractType = InteractTypes.TALK; 

	public QStage(int npc = 0, InteractTypes action = InteractTypes.TALK) {
		npcID = npc;
		InteractType = action;
	}
}

public class Quest {
	
	public int currentStage = 0;
	public QStage[] stages = new QStage[1] {new QStage(0, QStage.InteractTypes.TALK)};

	public void NextStage() {
		if (currentStage < stages.Length - 1) {
			Chat.Write ("<color=grey><i>Stage Completed!</i></color>");
			currentStage ++;
		} else if (currentStage == stages.Length - 1) {
			Chat.Write ("<color=grey><b>Quest Completed!</b></color>");
			currentStage ++;
		}
	}

}
