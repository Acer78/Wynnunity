using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spell_Multihit : Spell {

	public Health health;
	public int hitAmount = 10;
	public float dmgPerHit = 0.25f;
	public float knockback = 1.25f;
	public float knockbackMax = 1.5f;
	public float finalKnockback = 15.25f;
	public float hitDelay = 0.075f;
	public float finalHitDelay = 0.25f;
	public List<Health> updatedHits = new List<Health>();
	public bool doneCasting = true;
	public Vector2 knockMultipliers = Vector2.one;
	public Vector2 knockMultipliersFinal = Vector2.one;
	public float DynamicFinalXKnock = 0.9f;
	public GameObject ExplosionFX;
	public ParticleSystem CastFX;
	private List<Health> hitting = new List<Health>();

	void Start() {
		GetComponent<Collider> ().enabled = false;
		health = transform.root.GetComponent<Health> ();
	}
	void Update() {
		if (health.health == 0) {
			updatedHits.Clear();
		}
		for (int i = 0; i < updatedHits.Count; i++) {
			if(updatedHits[i] == null || updatedHits[i].health == 0)
				updatedHits.RemoveAt(i);
		}
		if (!CastFX) {
			CastFX = GetComponentInChildren<ParticleSystem>();
		}
	}
	public override IEnumerator CastSpell(Item_Weapon Weapon) {
		doneCasting = false;
		updatedHits.Clear ();
		GetComponent<Collider> ().enabled = true;
		yield return new WaitForFixedUpdate ();
		CastFX.Play ();
		Item_Weapon adjustedWeapon = new Item_Weapon (Weapon.name, Mathf.Clamp (Weapon.dmgMin * dmgPerHit, 0, Mathf.Infinity), Mathf.Clamp (Weapon.dmgMax * dmgPerHit, 0, Mathf.Infinity), Weapon.range, knockback, 0);
		List<Health> Hits = new List<Health> (updatedHits);
		hitting.AddRange (Hits);
		for(int i = 0; i < hitAmount; i++)
		{
			foreach (Health hit in Hits) {
				if(hit != health && hit.health != 0)
				{
					if(hit && transform.root.GetComponent<Enemy>() && !hit.transform.root.GetComponent<Enemy>() ||
					   hit && !transform.root.GetComponent<Enemy>() && hit.transform.root.GetComponent<Enemy>()){
						Instantiate (ExplosionFX, hit.transform.position - Vector3.up*1.375f, transform.rotation);
						adjustedWeapon.knockback = Random.Range (knockback, knockbackMax);
						adjustedWeapon.DealDamage(hit, transform, knockMultipliers, Weapon.IDs);
					}
				}
			}
			if(Hits.Count > 0)
			{
				yield return new WaitForSeconds(hitDelay);
			}
			GameObject[] tempFX = GameObject.FindGameObjectsWithTag ("TempFX_MH");
			foreach(GameObject PS in tempFX) {
				Destroy (PS);
			}
		}
		if(Hits.Count > 0)
			yield return new WaitForSeconds (finalHitDelay);
		adjustedWeapon.dmgMin = Weapon.dmgMin;
		adjustedWeapon.dmgMax = Weapon.dmgMax;
		adjustedWeapon.knockback = finalKnockback;
		foreach (Health hit in Hits) {
			if(hit != health && hit.health != 0)
			{
				if(hit && hit.transform && transform.root.GetComponent<Enemy>() && !hit.transform.root.GetComponent<Enemy>() ||
				   hit && hit.transform && !transform.root.GetComponent<Enemy>() && hit.transform.root.GetComponent<Enemy>()){
					Instantiate (ExplosionFX, hit.transform.position - Vector3.up*1.375f, transform.rotation);
					Vector2 tempKMF = knockMultipliersFinal;
					if (DynamicFinalXKnock != 0) {
						knockMultipliersFinal.x = transform.InverseTransformDirection(hit.GetComponent<Rigidbody>().velocity).z*-DynamicFinalXKnock;
						knockMultipliersFinal.y *= adjustedWeapon.knockback;
						adjustedWeapon.knockback = 1;
					}
					adjustedWeapon.DealDamage(hit, transform, knockMultipliersFinal, Weapon.IDs);
					if (DynamicFinalXKnock != 0) {
						knockMultipliersFinal.x = tempKMF.x;
						knockMultipliersFinal.y = tempKMF.y;
						adjustedWeapon.knockback = finalKnockback;
					}
				}
			}
		}
		doneCasting = true;
		yield return new WaitForSeconds (hitDelay);
		GameObject[] tempFX2 = GameObject.FindGameObjectsWithTag ("TempFX_MH");
		foreach(GameObject PS in tempFX2) {
			Destroy (PS);
		}
		GetComponent<Collider> ().enabled = false;
		foreach (Health hit in Hits) {
			if(hitting.Contains (hit))
				hitting.Remove (hit);
		}
	}

	void OnTriggerStay (Collider col) {
			if (col.GetComponent<Health> ()) {
				if (col.GetComponent<Health> () != health && col.GetComponent<Health> ().health != 0) {
					if (!updatedHits.Contains (col.GetComponent<Health> ()) && !hitting.Contains (col.GetComponent<Health>())) {
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

	float getDisplacementX (Transform obj) {
		return transform.InverseTransformPoint (obj.position).x;
	}
}