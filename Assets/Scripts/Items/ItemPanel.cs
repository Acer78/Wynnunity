using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemPanel : MonoBehaviour {

	public static Item weapon;
	private Text text;
	// Use this for initialization
	void Start () {
		weapon = Player.Punch.heldItem;
		text = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (weapon is Item_Weapon && weapon == ItemDB.Weapons [0] || weapon == null) {
			text.text = "<b>Item Statistics:</b>\n" +
				"<color=black>Empty Slot</color>";
		}
		else if (weapon is Item_Weapon) {
			Item_Weapon wpn = (Item_Weapon)weapon;
			text.text = "<b>Item Statistics:</b>\n" +
				"<color=black>" + wpn.name + "</color><color=grey> (weapon)</color>\n" +
				"<color=yellow>Lv. Min: " + wpn.lvlMin + "</color>\n" +
				"<color=purple>Dam: " + wpn.dmgMin + "-" + wpn.dmgMax + "</color>\n" +
				"<color=red>Range: " + wpn.range + "</color>\n" +
				"<color=navy>Knockback: " + wpn.knockback + "</color>\n" +
				"<color=grey>Cooldown: " + wpn.hitCooldown + "s</color>";
		} else if(weapon is Item_Armour) {
			Item_Armour armour = (Item_Armour)weapon;
			string armourType = "";
			if(armour.Type == Item_Armour.ArmourType.HELMET) {
				armourType = "Head";
			} else if(armour.Type == Item_Armour.ArmourType.CHESTPLATE) {
				armourType = "Body";
			} else if(armour.Type == Item_Armour.ArmourType.LEGGINGS) {
				armourType = "Leg";
			} else if(armour.Type == Item_Armour.ArmourType.BOOTS) {
				armourType = "Foot";
			}

			text.text = "<b>Item Statistics:</b>\n" +
				"<color=black>" + armour.name + "</color><color=#4e4e4e> (" + armourType + " armour)</color>\n" +
				"<color=yellow>Lv. Min: " + armour.lvlMin + "</color>\n" +
				"<color=purple>Def: " + armour.def + "</color>\n";
		} else if(weapon is Item_Consumable) {
			Item_Consumable consumable = (Item_Consumable)weapon;
			string effects = "";
			bool showEffects = false;
			foreach(effectInfo effect in consumable.effects) {
				if(effect.type != "Trip" && effect.type != "TripNoFOV" && effect.type != "Drunkness") {
					showEffects = true;
				}
			}

			foreach(effectInfo effect in consumable.effects) {
				if(effect.type != "Trip" && effect.type != "TripNoFOV")
					effects += ("\n<color=magenta><b>Effect Name:</b></color><color=purple> <i>" + effect.type + "</i></color><color=magenta>\nDuration: </color><color=purple>" + effect.stats[0] + "</color><color=magenta>\nPower:</color><color=purple> " + effect.stats[1] + "</color>");
			}

			if(showEffects) {
			text.text = "<b>Item Statistics:</b>\n" +
				"<color=black>" + consumable.name + "</color><color=#4e4e4e> (consumable)</color>\n" +
				"<color=blue>" + consumable.description + "</color>\n" +
				"<color=#990099><b>Effects:</b></color> " + effects;
			} else {
				text.text = "<b>Item Statistics:</b>\n" +
				"<color=black>" + consumable.name + "</color><color=#4e4e4e> (consumable)</color>\n" +
				"<color=blue>" + consumable.description + "</color>";
			}

		} else {
			text.text = "<b>Item Statistics:</b>\n" +
				"<color=black>" + weapon.name + "</color>\n";

		}
	}
}