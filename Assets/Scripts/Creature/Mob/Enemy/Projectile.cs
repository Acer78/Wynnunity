using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Projectile : MonoBehaviour {

	public Item_Weapon stats;
	private Rigidbody rb;
	public float FiredVel;
	public IDInfo IDs;
	public Vector2 knockMult;
	public float lifeTime = 10;
	private float destroyTime;
	private ParticleSystem FX;

	public bool isEnemy;
	public bool teamDamage = true;

	// Use this for initialization
	void Start () {
		FX = GetComponentInChildren<ParticleSystem> ();
		destroyTime = Time.time + lifeTime;
		rb = GetComponent<Rigidbody> ();
		rb.AddRelativeForce (Vector3.forward * FiredVel, ForceMode.VelocityChange);
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time >= destroyTime)
			Despawn ();
	}

	void OnTriggerEnter (Collider col) {
		if (col.gameObject.GetComponent<Health> () && col.gameObject.GetComponent<Health> ().health != 0) {
			if(teamDamage || !isEnemy && col.GetComponent<Enemy>() || isEnemy && !col.GetComponent<Enemy>())
				stats.DealDamage(col.gameObject.GetComponent<Health>(), transform, knockMult, IDs);
		}
		Despawn ();
	}

	void ChangeEnemyCol(Color hc, List<MeshRenderer> rends) {
		foreach (MeshRenderer rend in rends) {
			rend.material.color = hc;
			rend.transform.root.GetComponent<Enemy>().defColourTime = Time.time + 0.5f;
		}
	}

	void Despawn() {
		if (FX) {
			FX.loop = false;
			FX.emissionRate = 0;
			FX.transform.SetParent (null);
			Destroy (FX.gameObject, FX.startLifetime);
		}
		Destroy (gameObject);
	}
}