using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public static Walk Walk;
	public static Punch Punch;
	public static Health Health;
	public static Mana Mana;
	public static MouseLook TurnBody;
	public static MouseLook TurnHead;
	public static LayerMask EnemyLOS;
	public static LayerMask SpawnLOS;
	public static float CamRevertTime;
	public static float KillAltitude = -256;
	public static GameObject EnemyGC2;
	public static Inventory Inventory;
	public static Effects Effects;

	public Walk walk;
	public Punch punch;
	public Health health;
	public Mana mana;
	public MouseLook TB;
	public MouseLook TH;
	public LayerMask enemyLOS;
	public LayerMask spawnLOS;
	public float killAltitude = -256;
	private float nextReCheck;
	public GameObject enemyGC2;

	// Use this for initialization
	
	void Awake () {
		if (!Effects && GetComponent<Effects> ())
			Effects = GetComponent<Effects> ();
		else if (!Effects)
			Effects = (Effects) gameObject.AddComponent<Effects> ();
		EnemyGC2 = enemyGC2;
		DontDestroyOnLoad (gameObject);
		PlayerManager.Players.Add (this);
		Walk = GetComponent<Walk> ();
		Punch = GetComponent<Punch> ();
		Health = GetComponent<Health> ();
		Mana = GetComponent<Mana> ();
		TurnBody = TB;
		TurnHead = TH;
		EnemyLOS = enemyLOS;
		SpawnLOS = spawnLOS;

		walk = Walk;
		health = Health;
		punch = Punch;
		mana = Mana;
	}

	void Update() {
		KillAltitude = killAltitude;
		if (Camera.main.transform.parent.localRotation.z != 0 && Time.time > CamRevertTime) {
			Vector3 rotAngle = Camera.main.transform.parent.localEulerAngles;
			rotAngle.z = 0;
			Camera.main.transform.parent.localEulerAngles = rotAngle;
		}
	}

	public static void RecalcArmour() {
		int def = 0;
		foreach (ArmourSlot armour in Inventory.ArmourSlots)
			if (armour.slot.item is Item_Armour)
				def += ((Item_Armour)armour.slot.item).def;
		Health.dmgTaken = ((float) Health.maxHealth /(float) (Health.maxHealth + def));
	}
}
