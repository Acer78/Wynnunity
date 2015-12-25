using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpell : MonoBehaviour {

	public List<Spell> Spells;
	public EnemyAttack enemy;
	public List<float> spellRanges;
	public float minSpellDelay = 10f;
	public float maxSpellDelay = 20f;
	public float chancePerSecond = 10f;
	public float tryCastTime;
	public Spell Spell;
	public ParticleSystem WarmupFX;
	public float DisableTurnBeforeEndCast = 0.25f;
	public List<float> DisableTurn;
	public bool onlyCastOnGround = true;
	public bool chooseByDistance;

	void Start() {
		tryCastTime = Time.time + minSpellDelay;
		if (!enemy)
			enemy = transform.root.GetComponent<EnemyAttack> ();
	}

	void Update() {
		if (enemy.enemy.HP.health == 0) {
			StopAllCoroutines ();
		}

		if (Time.time > tryCastTime && onlyCastOnGround && enemy.enemy.GroundCheck.chckdist() && enemy.enemy.player && enemy.hasLOS && Spells.Count != 0 && enemy.enemy.player.GetComponent<Health> ().health != 0 ||
		    Time.time > tryCastTime && !onlyCastOnGround && enemy.enemy.player && enemy.hasLOS && Spells.Count != 0 && enemy.enemy.player.GetComponent<Health> ().health != 0) {
			Spell = chooseSpell ();
			if (Spell != null) {
				float spellRange = spellRanges [Spells.IndexOf (Spell)];
				if (Vector3.Distance (enemy.transform.position, enemy.enemy.player.transform.position) < spellRange) {
					DisableTurnBeforeEndCast = DisableTurn [Spells.IndexOf (Spell)];
					if (Time.time > tryCastTime && Time.time < tryCastTime + maxSpellDelay) {
						if (Random.Range (0f, 100f) <= chancePerSecond * Time.deltaTime) {
							StartCoroutine (StartCasting (Spell, enemy.Weapon));
							tryCastTime = Time.time + minSpellDelay;
							Spell = null;
						}
					} else if (Time.time > tryCastTime + maxSpellDelay) {
						StartCoroutine (StartCasting (Spell, enemy.Weapon));
						tryCastTime = Time.time + minSpellDelay;
						Spell = null;
					}
				}
			}
		}
	}

	Spell chooseSpell() {
		if (!chooseByDistance) {
			return Spells [Random.Range (0, Spells.Count)];
		} else {
			Spell chosenSpell = null;
			float closest = -1f;
			foreach(Spell spell in Spells) {
				float spellRange = spellRanges [Spells.IndexOf (spell)];
				if (Vector3.Distance (enemy.transform.position, enemy.enemy.player.transform.position) < spellRange) {
					if(closest == -1 || spellRange < closest) {
						closest = spellRange;
						chosenSpell = spell;
					}
				}
			}
			if(chosenSpell != null) {
				return chosenSpell;
			} else
				return null;
		}
	}

	public IEnumerator StartCasting(Spell spell, Item_Weapon weapon) {
		WarmupFX.Play ();
		enemy.enemy.effects.Apply ("Stun", (new float[2]{WarmupFX.duration, 2}));
		yield return new WaitForSeconds (WarmupFX.duration - DisableTurnBeforeEndCast);
		enemy.enemy.disableTurnTime = Time.time + DisableTurnBeforeEndCast;
		yield return new WaitForSeconds (WarmupFX.duration - WarmupFX.time);
		Cast (spell, weapon);
	}

	public void Cast(Spell spell, Item_Weapon weapon) {
		spell.StartCoroutine (spell.CastSpell (weapon as Item_Weapon));
	}
}
