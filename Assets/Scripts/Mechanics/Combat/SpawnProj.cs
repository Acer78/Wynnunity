using UnityEngine;
using System.Collections;

public class SpawnProj : MonoBehaviour {

	public GameObject Spawn;
	public float gravity = 12;
	private Rigidbody rb;
	public Vector3 firedVel;
	private ParticleSystem FX;
	public float lifeTime = 5f;
	private float lifeTimeLeft;
	public Spell_SmokeBomb smokeBombCast;

	void Start() {
		lifeTimeLeft = lifeTime;
		if (GetComponentInChildren<ParticleSystem> ())
			FX = GetComponentInChildren<ParticleSystem> ();
		rb = GetComponent<Rigidbody> ();
		rb.AddForce (firedVel, ForceMode.VelocityChange);
	}

	void FixedUpdate() {
		lifeTimeLeft -= Time.fixedDeltaTime;
		if (lifeTimeLeft <= 0)
			Destroy (gameObject);
		rb.AddForce (Vector3.down * gravity, ForceMode.Acceleration);
	}

	void OnCollisionEnter () {
		GameObject spawnedGO = (GameObject) Instantiate (Spawn, transform.position, transform.rotation);
		if (smokeBombCast) {
			DoTArea smoke = spawnedGO.GetComponent<DoTArea>();
			smoke.damageInfo = smokeBombCast.adjustedWeapon;
			smoke.health = smokeBombCast.health;
			smoke.lifeTime = smokeBombCast.lifetime;
		}
		Despawn ();
	}

	void Despawn () {
		if (FX) {
			FX.loop = false;
			FX.emissionRate = 0;
			FX.transform.SetParent(null);
			Destroy (FX.gameObject, FX.startLifetime);
		}
		Destroy (gameObject);
	}
}
