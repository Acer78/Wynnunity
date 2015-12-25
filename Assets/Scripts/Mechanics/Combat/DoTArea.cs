using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoTArea : MonoBehaviour {

	public Item_Weapon damageInfo;
	public List<Health> updatedHits = new List<Health>();
	public Health health;
	public float nextHitTime;
	public ParticleSystemRenderer FX;
	private Collider Collider;
	public float lifeTime = -1;
	private float destroyTime;
	public Vector2 slowdown = new Vector2(1f, 5);
	
	public int Hurt = -1; //Use only if health is null. -1 = Default Settings (Hurt both), 0 = Hurt Player, 1 = Hurt Enemies.

	void Start () {
		if(lifeTime != -1)
			destroyTime = Time.time + lifeTime;
		if (!Collider)
			Collider = GetComponent<Collider> ();
		Collider.enabled = false;
		if (!FX)
			FX = GetComponentInChildren<ParticleSystemRenderer> ();
		if (damageInfo == null)
			damageInfo = new Item_Weapon ("DoT Area", 600, 670, 0, 0, 0.5f);
	}

	void Update () {
		if (Time.time >= destroyTime && lifeTime != -1) {
			Despawn ();
		}
		if (updatedHits.Count > 0) {
			foreach(Health hitObj in updatedHits) {
				if(hitObj && hitObj.GetComponent<Enemy>()) {
					if(Hurt == -1 && !health || Hurt == 1 && !health || health && !health.GetComponent<Enemy>())
						hitObj.GetComponent<Enemy>().effects.Apply ("Slowness", (new float[2] {slowdown.x, slowdown.y}));
				}
			}
		}
		if (Time.time >= nextHitTime) {
			nextHitTime = Time.time + damageInfo.hitCooldown;
			if(Options.ThreeDSmoke)
				FX.renderMode = ParticleSystemRenderMode.Mesh;
			else
				FX.renderMode = ParticleSystemRenderMode.Billboard;
			StartCoroutine (Damage ());
		}
	}

	IEnumerator Damage() {
		Collider.enabled = true;
		updatedHits.Clear ();
		yield return new WaitForFixedUpdate ();
		if(updatedHits.Count > 0) {
			foreach(Health hitObj in updatedHits) {
				if(hitObj != health) {
					if(!health && Hurt == -1 || !health && Hurt == 0 && !hitObj.transform.root.GetComponent<Enemy>() ||
					   !health && Hurt == 1 && hitObj.transform.root.GetComponent<Enemy>() ||
					   health && health.GetComponent<Enemy>() && !hitObj.transform.root.GetComponent<Enemy>() ||
					   health && !health.GetComponent<Enemy>() && hitObj.transform.root.GetComponent<Enemy>()) {
						damageInfo.DealDamage(hitObj, transform, Vector2.zero, damageInfo.IDs);
					}
				}
			}
		}
		Collider.enabled = false;
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

	void Despawn () {
		if (GetComponentInChildren<ParticleSystem>()) {
			ParticleSystem SmokeFX = GetComponentInChildren<ParticleSystem>();
			SmokeFX.loop = false;
			SmokeFX.transform.SetParent (null);
			Destroy (SmokeFX.gameObject, SmokeFX.duration);
		}
		Destroy (gameObject);
	}

}
