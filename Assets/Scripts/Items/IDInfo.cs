using UnityEngine;
using System.Collections;

public class IDInfo : MonoBehaviour {

	public Item_Weapon weapon;
	public Health Health;
	public Mana Mana;

	public float LS;
	public float MS;

	public float totalLS;
	public float totalMS;

	// Use this for initialization
	void Start () {
		if (!Health)
			Health = transform.root.GetComponent<Health> ();
		if (!Mana && transform.root.GetComponent<Mana>())
			Mana = transform.root.GetComponent<Mana> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (weapon == null) {
			weapon = GetComponent<WeaponInfo> ().weapon;
		}

		totalLS = LS;
		totalMS = LS;
	}

	public void Steal() {
		if(Health && Health.health != 0)
			Health.health += totalLS;
		if (Mana && Health.health != 0) {
			Mana.MP += totalMS;
		}
	}
}
