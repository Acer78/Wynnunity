using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnArea : MonoBehaviour {
	
	public GameObject Mob;
	public float MinSpawnDelay = 4f;
	public float MaxSpawnDelay = 10f;
	public int SpawnAmountMin = 1;
	public int SpawnAmountMax = 3;
	public int MaxMobsInTrigger = 12;
	public float MobHalfHeight = 1f;
	private float nextSpawnTime;

	public SpawnTrigger Trigger;
	public Vector3 Area;

	public bool activated = false;
	public bool limitSpawnAmount;

	public bool StrictSpawn;
	public Vector3 StrictRotation;

	private int SpawnAmount;

	void Start () {
		Area = transform.TransformDirection (transform.localScale / 2);
		if (!Trigger) {
			Trigger = transform.GetChild (0).GetComponent<SpawnTrigger>();
		}
	}

	void Update () {
		if (activated) {
			if(Time.time > nextSpawnTime && Trigger.MobsInTrigger.Count < MaxMobsInTrigger) {
				nextSpawnTime = Time.time + Random.Range (MinSpawnDelay, MaxSpawnDelay);
				SpawnGroup();
			} else if (Time.time > nextSpawnTime)
				ResetNextSpawn();
		}
	}

	public void SpawnGroup () {
		SpawnAmount = Random.Range (SpawnAmountMin, SpawnAmountMax + 1);
		if (limitSpawnAmount)
			SpawnAmount = Mathf.Clamp (SpawnAmount, 0, MaxMobsInTrigger - Trigger.MobsInTrigger.Count + 1);
		for (int i = 0; i < SpawnAmount; i++) {
			SpawnMob();
		}
	}

	public void SpawnGroup (int minAmount, int maxAmount, bool limitAmount) {

		SpawnAmount = Random.Range (minAmount, maxAmount + 1);
		if (limitAmount)
			SpawnAmount = Mathf.Clamp (SpawnAmount, 0, MaxMobsInTrigger - Trigger.MobsInTrigger.Count + 1);
		for (int i = 0; i < SpawnAmount; i++) {
			SpawnMob();
		}
	}

	void SpawnMob () {
		Vector3 randPos = new Vector3 (Random.Range (transform.position.x - Area.x, transform.position.x + Area.x), Random.Range (transform.position.y - Area.y, transform.position.y + Area.y), Random.Range (transform.position.z - Area.z, transform.position.z + Area.z));
		Vector3 randRot = new Vector3 (0, Random.Range (-180, 180), 0);
		if (StrictSpawn) {
			randPos = transform.position;
			randPos.y = transform.position.y;
		}
		randPos.y += MobHalfHeight;
		if (!Physics.CheckSphere (randPos, MobHalfHeight*0.75f, Player.EnemyLOS)) {
			if (Physics.CheckSphere (randPos - MobHalfHeight*Vector3.up, 0.1f, Player.SpawnLOS)) {
				GameObject InstMob = (GameObject) Instantiate (Mob, randPos, Quaternion.Euler(randRot));
				InstMob.name = Mob.name;
				InstMob.AddComponent <MobsSpawnArea>();
				InstMob.GetComponent <MobsSpawnArea>().MSA = this;
			} else if(randPos.y != transform.position.y - Area.y){
				randPos.y = transform.position.y - Area.y - MobHalfHeight;
				SpawnMob (randPos);
			} else {
				RaycastHit hit;
				randPos.y = transform.position.y + Area.y;
				if(Physics.Raycast (randPos, Vector3.down, out hit, Area.y*2f, Player.SpawnLOS)) {
					randPos = hit.point;
					SpawnMob (randPos);
				}
			}
		} else {
			SpawnMob(randPos);
		}
	}

	void SpawnMob (Vector3 pos) {
		Vector3 randRot = new Vector3 (0, Random.Range (-180, 180), 0);
		pos.y += MobHalfHeight;
		if (!Physics.CheckSphere (pos, MobHalfHeight*0.75f, Player.SpawnLOS)){
			if (Physics.CheckSphere (pos - MobHalfHeight*Vector3.up, 0.1f, Player.SpawnLOS)) {
				GameObject InstMob = (GameObject) Instantiate (Mob, pos, Quaternion.Euler(randRot));
				InstMob.name = Mob.name;
				InstMob.AddComponent <MobsSpawnArea>();
				InstMob.GetComponent <MobsSpawnArea>().MSA = this;
			} else {
				RaycastHit hit;
				pos.y = transform.position.y + Area.y;
				if(Physics.Raycast (pos, Vector3.down, out hit, Area.y*2f, Player.SpawnLOS)) {
					pos = hit.point;
					SpawnMob (pos);
				} else {
					SpawnMob ();
				}
			}
		} else {
			SpawnMob();
		}
	}

	public void ResetNextSpawn(){
		nextSpawnTime = Time.time + Random.Range (MinSpawnDelay, MaxSpawnDelay);
	}
}
