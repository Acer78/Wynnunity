using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnTrigger : MonoBehaviour {

	public List<GameObject> MobsInTrigger = new List<GameObject>();
	public SpawnArea SA;
	private float nextCheckTime;
	public bool playerPresent;
	private Collider Collider;
	
	void Start () {
		Collider = GetComponent<Collider> ();
		Collider.enabled = false;
		if (!SA) {
			SA = transform.parent.GetComponent<SpawnArea>();
		}
	}

	void Update () {
		for (int i = 0; i < MobsInTrigger.Count; i++) {
			if(MobsInTrigger[i] == null) {
				MobsInTrigger.RemoveAt (i);
			}
		}

		if (Time.time >= nextCheckTime) {
			nextCheckTime = Time.time + 0.25f;
			playerPresent = false;
			StartCoroutine (checkForPlayer());
		}
	}

	IEnumerator checkForPlayer() {
		Collider.enabled = true;
		yield return new WaitForFixedUpdate ();
		Collider.enabled = false;
		if (!playerPresent) {
			SA.activated = false;
		}
	}

	void OnTriggerStay (Collider col) {
		if (col.transform.root.GetComponent<MobsSpawnArea>() && col.transform.root.GetComponent<MobsSpawnArea>().MSA == SA && !MobsInTrigger.Contains(col.transform.root.gameObject)) {
			MobsInTrigger.Add (col.transform.root.gameObject);
		}

		if (col.transform.root.tag == "Player" && col.transform.root.GetComponent<Health> ().health != 0) {
			if(!SA.activated)
				SA.ResetNextSpawn();
			SA.activated = true;
			playerPresent = true;
		} else if (col.transform.root.tag == "Player" && col.transform.root.GetComponent<Health> ().health == 0) {
			SA.activated = false;
			playerPresent = false;
		}
	}

	void OnTriggerExit (Collider col) {
		if(MobsInTrigger.Contains (col.transform.root.gameObject)) {
			MobsInTrigger.Remove (col.transform.root.gameObject);
		}

		if (col.transform.root.tag == "Player") {
			SA.activated = false;
			playerPresent = false;
		}
	}
}