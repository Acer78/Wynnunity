using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spell_Charge : Spell {

	public Health health;
	public float dmgMult = 1.5f;
	public float knockback = 6.75f;
	public float ChargeSpeed = 10f;
	public Vector2 ChargeMults = Vector2.one;
	public Vector2 ChargeMultsMin = Vector2.one;
	public Vector2 ChargeMultsMax = Vector2.one;
	public List<Health> updatedHits = new List<Health>();
	public bool doneCasting = true;
	public Vector2 knockMultipliers = Vector2.one;
	public ColInfo hitbox;
	public ParticleSystem FX;

	void Start() {
		health = transform.root.GetComponent<Health> ();
		if (!hitbox)
			hitbox = transform.root.GetComponent<ColInfo> ();
		if (!FX)
			FX = GetComponentInChildren<ParticleSystem> ();
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
		ChargeMults.x = Random.Range (ChargeMultsMin.x, ChargeMultsMax.x);
		ChargeMults.y = Random.Range (ChargeMultsMin.y, ChargeMultsMax.y);
		Item_Weapon adjustedWeapon = new Item_Weapon (Weapon.name, (int) Mathf.Clamp (Mathf.RoundToInt(Weapon.dmgMin * dmgMult), 1, Mathf.Infinity), (int) Mathf.Clamp (Mathf.RoundToInt(Weapon.dmgMax * dmgMult), 1, Mathf.Infinity), Weapon.range, knockback, 0);
		List<Health> hitObjs = new List<Health> ();
		hitbox.GetComponent<Rigidbody> ().AddRelativeForce (Vector3.up*ChargeMults.y * ChargeSpeed, ForceMode.VelocityChange);
		yield return new WaitForSeconds (0.075f);
		FX.Play ();
		if(transform.root.GetComponent<Enemy> ())
			transform.root.GetComponent<Enemy> ().disableWalk = true;
		hitbox.GetComponent<Rigidbody> ().AddRelativeForce (Vector3.forward*ChargeMults.x * ChargeSpeed, ForceMode.VelocityChange);
		yield return new WaitForSeconds (0.05f);
		if (transform.root.GetComponent<Enemy> ())
			transform.root.GetComponent<Enemy> ().disableWalk = true;
		while (!hitbox.CollidingWithSolids) {
			if (transform.root.GetComponent<Enemy> ())
			transform.root.GetComponent<Enemy> ().disableWalk = true;
			foreach (Health hit in updatedHits) {
				if (hit != health && hit.health != 0 && !hitObjs.Contains (hit)) {
					if(hit && transform.root.GetComponent<Enemy>() && !hit.transform.root.GetComponent<Enemy>() ||
					   hit && !transform.root.GetComponent<Enemy>() && hit.transform.root.GetComponent<Enemy>())
					{
						adjustedWeapon.DealDamage (hit, transform, knockMultipliers, Weapon.IDs);
						hitObjs.Add (hit);
					}
				}
			}
			yield return new WaitForEndOfFrame();
		}
		FX.Stop ();
		health.invulnerable = false;
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