using UnityEngine;
using System.Collections;

public class WeaponInfo : MonoBehaviour {

	public Item_Weapon weapon;

	// Use this for initialization
	void Start () {
		if (GetComponent<EnemyAttack> ())
			weapon = GetComponent<EnemyAttack> ().Weapon;
	}
	
	// Update is called once per frame
	void Update () {
		if (weapon != null) {
			weapon.IDs = GetComponent<IDInfo>();
		}
	}
}
