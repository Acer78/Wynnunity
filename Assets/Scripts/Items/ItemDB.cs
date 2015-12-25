using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;

public class ItemDB {

	public static List<Item> Others = new List<Item> ();
	private static string[] OthersLines;

	public static List<Item_Weapon> Weapons = new List<Item_Weapon>();
	private static string[] WeaponLines;

	public static List<Item_Armour> Armour = new List<Item_Armour>();
	private static string[] ArmourLines;

	public static List<Item_Consumable> Consumables = new List<Item_Consumable> ();
	private static string[] ConsumablesLines;

	public static GameObject droppedItem;

	public static void GetAllOthers() {
		Others.Clear ();
		OthersLines = (Resources.Load ("Data/Items/Other") as TextAsset).text.Split ('\n');
		for (int i = 0; i < OthersLines.Length;) {
			Item item = new Item ();
			item.name = OthersLines [i++];
			item.maxStack = int.Parse (OthersLines[i++]);
			item.model = (GameObject)Resources.Load (OthersLines [i++]);
			item.invIcon = Resources.Load<Sprite> (OthersLines [i++]);
			Others.Add (item);
			i++;
		}
	}

	public static void GetAllWeapons(bool logItems = true, bool clearLog = true) {
		droppedItem = Resources.Load<GameObject> ("Data/Items/DroppedItem");
		Weapons.Clear ();
		if(clearLog && Application.isEditor)
			System.Type.GetType("UnityEditorInternal.LogEntries,UnityEditor.dll").GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public).Invoke (null,null);
		WeaponLines = (Resources.Load ("Data/Items/Weapons") as TextAsset).text.Split ('\n');
		for (int i = 9; i < WeaponLines.Length;) {
			Item_Weapon weapon = new Item_Weapon();
			weapon.name = WeaponLines[i++];
			weapon.maxStack = 1;
			string weaponType = WeaponLines[i++];
			if(weaponType == "Dagger")
				weapon.Type = Item_Weapon.WeaponType.DAGGER;
			else if (weaponType == "Spear")
				weapon.Type = Item_Weapon.WeaponType.SPEAR;
			else if (weaponType == "Wand")
				weapon.Type = Item_Weapon.WeaponType.WAND;
			else if (weaponType == "Spoon")
				weapon.Type = Item_Weapon.WeaponType.SPOON;
			else
				weapon.Type = Item_Weapon.WeaponType.OTHER;

			weapon.lvlMin = int.Parse (WeaponLines[i++]);
			weapon.dmgMin = int.Parse(WeaponLines[i++]);
			weapon.dmgMax = int.Parse (WeaponLines[i++]);
			weapon.range = float.Parse (WeaponLines[i++]);
			weapon.knockback = float.Parse (WeaponLines[i++]);
			weapon.hitCooldown = float.Parse (WeaponLines[i++]);
			weapon.model = (GameObject) Resources.Load (WeaponLines[i++]);
			weapon.invIcon = Resources.Load<Sprite> (WeaponLines[i++]);
			Weapons.Add (weapon);
			i++;
			if(logItems)
				Debug.Log (weapon.name + " {MinDmg: " + weapon.dmgMin +
				           ", MaxDmg: " + weapon.dmgMax +
				           ", range: " + weapon.range +
				           ", knockback: " + weapon.knockback +
				           ", hitCooldown: " + weapon.hitCooldown + "}");
		}
		GetAllArmour ();
		GetAllOthers ();
		GetAllConsumables ();
	}

	public static void GetAllArmour() {
		Armour.Clear ();
		ArmourLines = (Resources.Load ("Data/Items/Armour") as TextAsset).text.Split ('\n');
		for (int i = 0; i < ArmourLines.Length;) {
			Item_Armour armour = new Item_Armour();
			armour.name = ArmourLines[i++];
			armour.maxStack = 1;
			string armourType = ArmourLines[i++];
			if(armourType == "Helmet")
				armour.Type = Item_Armour.ArmourType.HELMET;
			else if (armourType == "Chestplate")
				armour.Type = Item_Armour.ArmourType.CHESTPLATE;
			else if (armourType == "Leggings")
				armour.Type = Item_Armour.ArmourType.LEGGINGS;
			else if (armourType == "Boots")
				armour.Type = Item_Armour.ArmourType.BOOTS;

			armour.lvlMin = int.Parse (ArmourLines[i++]);
			armour.def = int.Parse(ArmourLines[i++]);
			armour.model = (GameObject) Resources.Load (ArmourLines[i++]);
			armour.invIcon = Resources.Load<Sprite> (ArmourLines[i++]);
			Armour.Add (armour);
			i++;
		}
	}

	public static void GetAllConsumables() {
		Consumables.Clear ();
		ConsumablesLines = (Resources.Load ("Data/Items/Consumables") as TextAsset).text.Split ('\n');
		for (int i = 0; i < ConsumablesLines.Length;) {
			Item_Consumable consumable = new Item_Consumable ("", new List<effectInfo>());
			consumable.name = ConsumablesLines [i++];
			consumable.description = ConsumablesLines [i++];
			consumable.maxStack = int.Parse (ConsumablesLines [i++]);
			int effectCount = int.Parse (ConsumablesLines[i++]);
			for(int ii = 0; ii < effectCount; ii++) {
				consumable.effects.Add (new effectInfo());
				consumable.effects[ii].type = ConsumablesLines[i++];
				string[] parameters = ConsumablesLines[i++].Split (',');
				consumable.effects[ii].stats = new float[parameters.Length];
				for(int morei = 0; morei < parameters.Length; morei++) {
					consumable.effects[ii].stats[morei] = float.Parse (parameters[morei]);
				}
			}
			consumable.model = (GameObject)Resources.Load (ConsumablesLines [i++]);
			consumable.invIcon = Resources.Load<Sprite> (ConsumablesLines [i++]);
			Consumables.Add (consumable);
			i++;
		}
	}

	public static Item FindItem(Vector2 ID) {
		if (ID.y == 0)
			return Others [(int)ID.x];
		else if (ID.y == 1)
			return Weapons [(int)ID.x];
		else if (ID.y == 2)
			return Armour [(int)ID.x];
		else if (ID.y == 3)
			return Consumables [(int)ID.x];
		else
			return null;
	}

#if UNITY_EDITOR
	[MenuItem("Assets/Log All Item IDs")]
#endif
	public static void LogAllIDs() {
		GetAllWeapons (false, true);

		Debug.Log ("===NORMAL ITEMS START HERE===");
		foreach (Item item in Others) {
			Debug.Log (Others.IndexOf(item) + " = " + item.name + " (other/0)");
		}
		Debug.Log ("===WEAPONS START HERE===");
		foreach (Item_Weapon weapon in Weapons) {
			Debug.Log (Weapons.IndexOf (weapon) + " = " + weapon.name + " (weapon/1)");
		}
		Debug.Log ("===ARMOUR STARTS HERE===");
		foreach (Item_Armour armour in Armour) {
			Debug.Log (Armour.IndexOf(armour) + " = " + armour.name + " (armour/2)");
		}
		Debug.Log ("===CONSUMABLES START HERE===");
		foreach (Item_Consumable consumable in Consumables) {
			Debug.Log (Consumables.IndexOf(consumable) + " = " + consumable.name + " (consumable/3)");
		}

	}
}
