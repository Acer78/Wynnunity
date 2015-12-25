using UnityEngine;
using System.Collections;

public class Mana : MonoBehaviour {

	public Player player;
	public float maxMP = 18;
	public float MP = 18;
	public float DefRegen = 1;
	public float Regen = 1;
	public float RegenTime = 1f;
	public float SprintingMPLoss = 0.5f;
	public float JumpSprintMPLoss = 0.5f;
	public float nextRegenTime;
	public bool jumpsprint = false;
	public int StopSprintThreshold = 6;

	void Start() {
		if (!player)
			player = Player.Health.GetComponent<Player> ();
	}

	void Update() {
		if (Time.time >= nextRegenTime && player.health.health != 0) {
			MP += Regen;
			nextRegenTime = Time.time + RegenTime;
		}

		if (player.walk.speed == player.walk.sprintSpeed) {
			Regen = 0;
			MP -= SprintingMPLoss * Time.deltaTime;
			if (Input.GetKey (KeyCode.Space)) {
				jumpsprint = true;
			}
			if (jumpsprint) {
				MP -= JumpSprintMPLoss * Time.deltaTime;
			}
		} else {
			Regen = DefRegen;
		}

		if(Input.GetKeyUp (KeyCode.Space)) {
			jumpsprint = false;
		}

		MP = Mathf.Clamp (MP, 0, maxMP);


	}
}
