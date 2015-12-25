using UnityEngine;
using System.Collections;

public class DroppedItem : MonoBehaviour {

	public Item item;
	private bool CanBeCollected = true;
	private float killTime;
	private float nextCheckTime;

	public Vector2 ID;

	// Use this for initialization
	void Start () {
		killTime = Time.time + 120;
		if (item == null) {
			item = ItemDB.FindItem(ID);
		}
		GameObject model = (GameObject)Instantiate (item.model, transform.position, item.model.transform.localRotation);
		model.transform.SetParent (transform);
		foreach (Transform modelPart in GetComponentsInChildren<Transform>()) {
			if(!modelPart.name.Contains ("Collider"))
				modelPart.gameObject.layer = 0;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time >= killTime) {
			Destroy(gameObject);
		}
		if (Time.time > nextCheckTime) {
			nextCheckTime = Time.time + 0.25f;
			foreach (Player checkPlayer in PlayerManager.Players) {
				float Distance = Vector3.Distance (checkPlayer.transform.position, transform.position);
				if (Distance > 128) {
					Destroy (gameObject);
				}
			}
		}
	}

	void OnTriggerEnter(Collider col) {
		if(CanBeCollected && col.tag == "Player" && Player.Inventory.NextAvailableSlot (item) != -1) {
				if (Player.Inventory.NextAvailableSlot (item, 1) != -1) {
					Player.Inventory.CollectItem (item, 1);
					Destroy (gameObject);
				}
		} else if(CanBeCollected && col.tag == "Player") {
			StartCoroutine(TriggerWait (0.05f, true));
		}
	}

	public IEnumerator TriggerWait(float time, bool restartCollider = false) {
		CanBeCollected = false;
		if (restartCollider) {
			GetComponent<Collider>().enabled = false;
		}
		yield return new WaitForSeconds(time);
		if (restartCollider) {
			GetComponent<Collider>().enabled = true;
		}
		CanBeCollected = true;
	}
}
