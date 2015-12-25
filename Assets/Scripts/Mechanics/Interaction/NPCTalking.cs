using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class NPCTalking : NPC {

	public List<string> text;
	public List<float> delays;
	public bool talking;
	private bool isQuestNPC;
	private QuestNPC npcComponent;

	void Awake() {
		isQuestNPC = (GetComponent<QuestNPC> () != null);
		if (isQuestNPC)
			npcComponent = GetComponent<QuestNPC> ();
	}

	public IEnumerator Talk() {
		talking = true;
		for (int i = 0; i < text.Count; i++) {
			Chat.Write("<color=green>" + NPCname + ": </color>" + "<color=lime>" + text[i] + "</color>");
			yield return new WaitForSeconds(delays[i]);
		}
		talking = false;
	}

}
