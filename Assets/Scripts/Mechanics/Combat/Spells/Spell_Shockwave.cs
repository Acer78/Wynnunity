using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spell_Shockwave : Spell {

	public Health health;
	public float dmgMult = 3.5f;
	public float knockback = 21.666f;
	public float ExpandSpeed = 15f;
	public float ExpandTime = 0.75f;
	public float PowerLoss = 1f;
	public List<Health> updatedHits = new List<Health>();
	public bool doneCasting = true;
	public Vector2 knockMultipliers = Vector2.one;
	public ParticleSystem FX;
	public Transform col;
	private Transform defParent;
	private Vector3 defPos;
	public float waveThickness;

	void Start() {
		health = transform.root.GetComponent<Health> ();
		defParent = transform.parent;
		defPos = transform.localPosition;
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
		transform.SetParent (null);
		doneCasting = false;
		Item_Weapon adjustedWeapon = new Item_Weapon (Weapon.name, (int) Mathf.Clamp (Mathf.RoundToInt(Weapon.dmgMin * dmgMult), 1, Mathf.Infinity), (int) Mathf.Clamp (Mathf.RoundToInt(Weapon.dmgMax * dmgMult), 1, Mathf.Infinity), Weapon.range, knockback, 0);
		List<Health> hitObjs = new List<Health> ();
		float endTime = Time.time + ExpandTime;
		Vector3 size = col.localScale;
		float dm = dmgMult;
		FX.Play ();
		while (Time.time < endTime) {
			foreach (Health hit in updatedHits) {
				if (hit != health && hit.health != 0 && !hitObjs.Contains (hit)) {
					if(hit && defParent && !defParent.root.GetComponent<Enemy>() && hit.transform.root.GetComponent<Enemy>() ||
					   hit && defParent && defParent.root.GetComponent<Enemy>() && !hit.transform.root.GetComponent<Enemy>()) {
						if(Vector3.Distance (transform.position, hit.transform.position) > size.x/2 - waveThickness)
						{
							adjustedWeapon.DealDamage (hit, transform, knockMultipliers, Weapon.IDs, true);
							hitObjs.Add (hit);
						}
					}
				}
			}

			dmgMult -= PowerLoss*2*Time.deltaTime;
			adjustedWeapon.dmgMax = Mathf.RoundToInt (Weapon.dmgMax*dmgMult);
			adjustedWeapon.dmgMin = Mathf.RoundToInt(Weapon.dmgMin*dmgMult);
			size.x += ExpandSpeed*Time.deltaTime;
			size.z += ExpandSpeed*Time.deltaTime;
			col.localScale = size;
			yield return new WaitForEndOfFrame();
		}
		dmgMult = dm;
		size.x = 0;
		size.z = 0;
		col.localScale = size;
		transform.SetParent (defParent);
		transform.localPosition = defPos;
		doneCasting = true;
		updatedHits.Clear ();
	}

	void OnTriggerEnter (Collider col) {
		if (col.GetComponent<Health> ()) {
			if(col.GetComponent<Health>() != health && col.GetComponent<Health>().health != 0)
			{
				if(!updatedHits.Contains (col.GetComponent<Health>()))
				{
					updatedHits.Add (col.GetComponent<Health>());
				}
			}
			else if(col.GetComponent<Health>() != health) {
				if(updatedHits.Contains (col.GetComponent<Health>()))
				{
					updatedHits.Remove (col.GetComponent<Health>());
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