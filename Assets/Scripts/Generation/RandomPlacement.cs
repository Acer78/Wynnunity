using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomPlacement : MonoBehaviour {

	public GameObject prefab;
	public int spawnMin = 5;
	public int spawnMax = 8;
	public Vector3 spawnArea = Vector3.right + Vector3.forward;
	public Vector3 rotMin = Vector3.up*-180;
	public Vector3 rotMax = Vector3.up*180;
	public Vector3 scaleMin = Vector3.one;
	public Vector3 scaleMax = Vector3.one;

	// Use this for initialization
	void Start () {
		placePrefabs ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void placePrefabs () {
		//Destroy children
		List<GameObject> children = new List<GameObject>();
		foreach (Transform child in transform) {
			children.Add (child.gameObject);
		}
		children.ForEach (child => DestroyImmediate (child));

		//Instantiate and set prefabs up
		int placeAmt = Random.Range (spawnMin, spawnMax + 1);
		for (int i = 0; i < placeAmt; i++) {
			Vector3 spawnPos = new Vector3(
				Random.Range (-spawnArea.x, spawnArea.x),
				Random.Range (-spawnArea.y, spawnArea.y),
				Random.Range (-spawnArea.z, spawnArea.z));

			Quaternion rot = Quaternion.Euler (
				Random.Range (rotMin.x, rotMax.x),
				Random.Range (rotMin.y, rotMax.y),
				Random.Range (rotMin.z, rotMax.z));

			GameObject Inst = (GameObject)Instantiate (prefab, transform.position + spawnPos, rot);
			Inst.transform.localScale = new Vector3(
				Random.Range (scaleMin.x, scaleMax.x),
				Random.Range (scaleMin.y, scaleMax.y),
				Random.Range (scaleMin.z, scaleMax.z));
			Inst.transform.SetParent (transform);
		}

	}
}
