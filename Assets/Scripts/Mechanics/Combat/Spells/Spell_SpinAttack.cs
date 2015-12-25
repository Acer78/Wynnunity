using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spell_SpinAttack : Spell {

	public Health health;
	public float dmgMult = 1.5f;
	public float knockback = 7;
	public List<Health> updatedHits = new List<Health>();
	public bool doneCasting = true;
	public Vector2 knockMultipliers = Vector2.one;
	public ParticleSystem FX;
	public Vector2 StunEffect = Vector2.one;

	void Start() {
		health = transform.root.GetComponent<Health> ();
		GetComponent<Collider> ().enabled = false;
	}

	void Update() {
		if (health.health == 0) {
			updatedHits.Clear();
		}
		for (int i = 0; i < updatedHits.Count; i++) {
			if(updatedHits[i] == null)
				updatedHits.RemoveAt(i);
		}
	}
	public override IEnumerator CastSpell(Item_Weapon Weapon) {
		doneCasting = false;
		updatedHits.Clear ();
		GetComponent<Collider> ().enabled = true;
		yield return new WaitForFixedUpdate ();
		Item_Weapon adjustedWeapon = new Item_Weapon (Weapon.name, (int) Mathf.Clamp (Mathf.RoundToInt(Weapon.dmgMin * dmgMult), 1, Mathf.Infinity), (int) Mathf.Clamp (Mathf.RoundToInt(Weapon.dmgMax * dmgMult), 1, Mathf.Infinity), Weapon.range, knockback, 0);
		List<Health> hitObjs = new List<Health> ();
		FX.Play ();
			foreach (Health hit in updatedHits) {
			if (hit != health && hit.health != 0 && !hitObjs.Contains (hit)) {
				if (hit && !transform.root.GetComponent<Enemy> () && hit.transform.root.GetComponent<Enemy> () ||
				    hit && transform.root.GetComponent<Enemy> () && !hit.transform.root.GetComponent<Enemy> ()) {
					adjustedWeapon.DealDamage (hit, transform, knockMultipliers, Weapon.IDs, true);
					hit.transform.root.GetComponent<Effects>().Apply("Stun", (new float[2] {StunEffect.x, StunEffect.y}));
					hitObjs.Add (hit);
				}
			}
		}
		yield return new WaitForEndOfFrame ();
		doneCasting = true;
		GetComponent<Collider> ().enabled = false;
	}

	void OnTriggerStay (Collider col) {
		if (col.GetComponent<Health> ()) {
			if (col.GetComponent<Health> () != health && col.GetComponent<Health> ().health != 0) {
				if (!updatedHits.Contains (col.GetComponent<Health> ())) {
					updatedHits.Add (col.GetComponent<Health> ());
				}
			} else if (col.GetComponent<Health> () != health) {
				if (updatedHits.Contains (col.GetComponent<Health> ())) {
					updatedHits.Remove (col.GetComponent<Health> ());
				}
			}
		}
	}

	void OnTriggerExit (Collider col) {
		if (col.GetComponent<Health> ()) {
			if(updatedHits.Contains (col.GetComponent<Health>()))
			{
				updatedHits.Remove (col.GetComponent<Health>());
			}
		}
	}
}