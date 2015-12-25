using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Health : MonoBehaviour {

	public bool player = false;
	public Player PlayerInfo;
	public float health = 10;
	public int maxHealth = 10;
	public int regen = 5;
	public float regenDelay = 10;
	private float nextRegen;
	public bool invulnerable = false;
	public bool dead = false;
	public Transform respScreen;
	public Transform spawnPos;
	public float dmgTaken = 1f;
	public float knockTaken = 1f;
	public bool fallOnDeath = true;
	public ParticleSystem deathFX;

	public float ChanceForDrop = 0.1f;
	public int minDrops = -1;
	public int maxDrops = -1;
	public List<Vector3> Drops = new List<Vector3>();

	public int minXP = 10;
	public int maxXP = 15;

	// Use this for initialization
	void Start () {
		nextRegen = regenDelay + Time.time;
		if (!PlayerInfo)
			PlayerInfo = GetComponent<Player> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y <= Player.KillAltitude)
			health = 0;
		health = Mathf.Clamp (health, 0, maxHealth);
		if (health == 0 && !dead && !player) {
			dead = true;
			StartCoroutine (Die ());

		} else if (health == 0 && !dead) {
			dead = true;
			respScreen.gameObject.SetActive(true);
			PlayerInfo.walk.enabled = false;
			PlayerInfo.punch.enabled = false;
			PlayerInfo.TB.enabled = false;
			PlayerInfo.TH.enabled = false;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			GetComponent<Rigidbody> ().isKinematic = true;
		} else if (!dead) {

			if(Time.time >= nextRegen && health < maxHealth && regen > 0 || Time.time >= nextRegen && regen < 0 ) {
				nextRegen = Time.time + regenDelay;
				health += regen;
			}

		}
	}

	public IEnumerator Die () {
		//Loot Drop
		if (minDrops == -1 || maxDrops == -1) {
			if (Random.Range (0f, 1f) <= ChanceForDrop) {
				if (Drops.Count > 0) {
					Vector3 drop = RandomWeighted (Drops);
					Vector2 dropID = new Vector2 (drop.x, drop.z);
					DropItem (ItemDB.FindItem (dropID));
				}
			}
		} else {
			int dropAmt = Random.Range (minDrops, maxDrops + 1);
			for(int i = 0; i < dropAmt; i++) {
				if (Drops.Count > 0) {
					Vector3 drop = RandomWeighted (Drops);
					Vector2 dropID = new Vector2 (drop.x, drop.z);
					DropItem (ItemDB.FindItem (dropID));
				}
			}
		}

		//XP Drop
		if (GetComponent<Enemy>() && (GetComponent<Enemy>().level == 0 || Random.Range (0f, 100f) <= 100 - Mathf.Abs (XP.Level - GetComponent<Enemy> ().level) * 3)) {
			XP.Bar.GainXP (Random.Range (minXP, maxXP + 1));
		}

		foreach (ParticleSystem PS in GetComponentsInChildren<ParticleSystem>()) {
			PS.Stop ();
		}

		if (fallOnDeath) {
			for (float i = 0; i < 90; i += 180*Time.deltaTime) {
				transform.Rotate (Vector3.forward * 180 * Time.deltaTime, Space.Self);
				yield return new WaitForEndOfFrame ();
			}
			yield return new WaitForSeconds (0.75f);
		} else if(deathFX) {
			foreach (Collider col in GetComponentsInChildren<Collider>()) {
				col.enabled = false;
			}
			transform.GetChild (0).gameObject.SetActive (false);
			deathFX.Play ();
			yield return new WaitForSeconds(deathFX.duration + deathFX.startLifetime);
		}
		Destroy (gameObject);
	}

	public void Respawn () {

		health = maxHealth;
		PlayerInfo.mana.MP = PlayerInfo.mana.maxMP;
		transform.position = spawnPos.position;
		transform.rotation = spawnPos.rotation;
		GetComponent<Rigidbody> ().isKinematic = false;
		GetComponent<Rigidbody> ().velocity = Vector3.zero;
		nextRegen = Time.time + regenDelay;
		PlayerInfo.walk.enabled = true;
		PlayerInfo.punch.enabled = true;
		PlayerInfo.TB.enabled = true;
		PlayerInfo.TH.enabled = true;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		dead = false;
		invulnerable = false;
	}

	public void DropItem (Item item, int amount = 1) {
		for (int i = 0; i < amount; i++) {
			DroppedItem droppedItem = ((GameObject) Instantiate(ItemDB.droppedItem, transform.position, transform.rotation)).GetComponent<DroppedItem>();
			droppedItem.item = item;
		}
	}

	Vector3 RandomWeighted (List<Vector3> outcomes) {
		float weightTotal = 0;
		for (int i = 0; i < outcomes.Count; i++) {
			weightTotal += outcomes[i].y;
		}
		int result = 0;
		float total = 0;
		float randVal = Random.Range( 0, weightTotal);
		for ( result = 0; result < outcomes.Count; result++ ) {
			total += outcomes[result].y;
			if ( total >= randVal ) break;
		}
		return outcomes[result];
	}

	public void SimulateDeaths(int specificID = -1, int specificIDType = -1) {
		int droppedTotal = 0;
		int SpecificDrops = 0;
		Vector2 specificIDV2 = new Vector2 (specificID, specificIDType);

		for (int i = 0; i < 1000; i++) {
			if (Random.Range (0f, 1f) <= ChanceForDrop) {
				if (Drops.Count > 0) {
					Vector3 drop = RandomWeighted (Drops);
					Vector2 dropID = new Vector2 (drop.x, drop.z);
					droppedTotal++;
					if(specificIDV2 != -Vector2.one && dropID == specificIDV2)
						SpecificDrops++;
				}
			}
		}
		Debug.Log ("Total: " + droppedTotal + ", Specific: " + SpecificDrops);
	}
}
