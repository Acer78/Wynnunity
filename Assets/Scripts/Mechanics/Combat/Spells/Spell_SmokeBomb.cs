using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spell_SmokeBomb : Spell {

	public Health health;
	public float dmgMult = 0.6f;
	public float lifetime = 6.75f;
	public float hitCooldown = 0.75f;
	public bool doneCasting = true;
	public GameObject bomb;
	public SpawnProj spawnBomb;
	public float spawnVel;
	public List<Transform> spawnPoses;
	public Item_Weapon adjustedWeapon;

	void Start() {
		if (!health) {
			health = transform.root.GetComponent<Health>();
		}
	}

	void Update() {

	}

	public override IEnumerator CastSpell(Item_Weapon Weapon) {
		doneCasting = false;
		adjustedWeapon = new Item_Weapon (Weapon.name, (int) Mathf.Clamp (Mathf.RoundToInt(Weapon.dmgMin * dmgMult), 1, Mathf.Infinity), (int) Mathf.Clamp (Mathf.RoundToInt(Weapon.dmgMax * dmgMult), 1, Mathf.Infinity), Weapon.range, 0, 1);
		foreach (Transform spawnPos in spawnPoses) {
			GameObject smokeBomb = (GameObject)Instantiate (bomb, spawnPos.position, spawnPos.rotation);
			spawnBomb = smokeBomb.GetComponent<SpawnProj> ();
			spawnBomb.firedVel = spawnVel * spawnPos.forward;
			spawnBomb.smokeBombCast = this;
		}
		doneCasting = true;
		yield return null;
	}
}