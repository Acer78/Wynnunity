using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SpellComboManager : MonoBehaviour {

	public Vector2 ClickCombo;
	public List<SpellCast> Spells = new List<SpellCast>();
	public float retryTime;
	public int comboPlace;
	public Player player;
	public Text SpellText;
	private float minButtonCheckTime;
	public bool RMB = false;
	public bool LMB = false;
	public List<bool> CombosDone = new List<bool>();

	public List<SpellCast> DaggerSpells;
	public List<SpellCast> SpearSpells;
	public List<SpellCast> WandSpells;
	public List<SpellCast> SpoonSpells;

	void Start() {
		DaggerSpells = new List<SpellCast>(transform.GetChild (0).GetComponents<SpellCast> ());
		SpearSpells = new List<SpellCast>(transform.GetChild (1).GetComponents<SpellCast> ());
		WandSpells = new List<SpellCast>(transform.GetChild (2).GetComponents<SpellCast> ());
		SpoonSpells = new List<SpellCast>(transform.GetChild (3).GetComponents<SpellCast> ());
		if (!player)
			player = transform.root.GetComponent<Player> ();
	}

	public List<SpellCast> GetAvailableSpells(Item_Weapon.WeaponType weaponType) {
		if (weaponType == Item_Weapon.WeaponType.DAGGER)
			return DaggerSpells;
		else if (weaponType == Item_Weapon.WeaponType.SPEAR)
			return SpearSpells;
		else if (weaponType == Item_Weapon.WeaponType.WAND)
			return WandSpells;
		else if (weaponType == Item_Weapon.WeaponType.SPOON)
			return SpoonSpells;
		else
			return null;
	}

	void Update () {
		if (Input.GetMouseButtonDown (0) && (Spells.Count > 0 || comboPlace == 0) && player.health.health != 0) {
			LMB = true;
		} else {
			LMB = false;
		}
		
		if (Input.GetMouseButton (1) && (Spells.Count > 0 || comboPlace == 0) && Time.time >= minButtonCheckTime && player.health.health != 0) {
			RMB = true;
		} else {
			RMB = false;
		}
		
		if (!(Player.Punch.heldItem is Item_Weapon) || Player.Punch.heldItem is Item_Weapon && (((Item_Weapon) Player.Punch.heldItem).Type == Item_Weapon.WeaponType.OTHER || ((Item_Weapon) Player.Punch.heldItem).lvlMin > XP.Level) || Time.time > retryTime && comboPlace != 0) {
			comboPlace = 0;
			if(!SpellText.text.Contains ("!"))
				SpellText.text = "";
		}
		
		if (gameObject.activeInHierarchy && comboPlace == 0 || gameObject.activeInHierarchy && Spells.Count > 0 && comboPlace != 0) {

			CombosDone.Clear ();
			for(int i = 0; i < Spells.Count; i++)
			{
				CombosDone.Add (false);
			}

			if (LMB && Time.time < retryTime && comboPlace == 2) {
				minButtonCheckTime = Time.time + 0.2f;
				ClickCombo.y = 0;
				SpellText.text += "-Left";
				comboPlace = 3;
			}
			if (RMB && Time.time < retryTime && comboPlace == 2) {
				minButtonCheckTime = Time.time + 0.2f;
				ClickCombo.y = 1;
				SpellText.text += "-Right";
				comboPlace = 3;
			}
			if(comboPlace == 3)
			{
				for(int i = 0; i < Spells.Count; i++) {
					if(Spells[i].combo == ClickCombo) {
						CombosDone[i] = true;
					} else {
						CombosDone[i] = false;
					}
				}
			}

			if(comboPlace == 3){
				bool failedCombos = true;
				foreach(bool combo in CombosDone) {
					if(combo == true) {
						Spells[CombosDone.IndexOf(combo)].StartCoroutine (Spells[CombosDone.IndexOf(combo)].Cast());
						failedCombos = false;
					}
				}
				comboPlace = 0;
				if(failedCombos) {
					SpellText.text = "";
					retryTime = Time.time + 0.25f;
				}
			}

			if (LMB && Time.time < retryTime && comboPlace == 1) {
				retryTime = Time.time + 1;
				minButtonCheckTime = Time.time + 0.2f;
				comboPlace = 2;
				ClickCombo.x = 0;
				SpellText.text += "-Left";
			}
			if (RMB && Time.time < retryTime && comboPlace == 1) {
				retryTime = Time.time + 1;
				minButtonCheckTime = Time.time + 0.2f;
				comboPlace = 2;
				ClickCombo.x = 1;
				SpellText.text += "-Right";
			}

			if (Player.Punch.heldItem is Item_Weapon && ((Item_Weapon) Player.Punch.heldItem).Type != Item_Weapon.WeaponType.OTHER && ((Item_Weapon)Player.Punch.heldItem).lvlMin <= XP.Level && RMB && Time.time > retryTime && comboPlace == 0) {
				Spells = GetAvailableSpells (((Item_Weapon)Player.Punch.heldItem).Type);
				List<SpellCast> SpellsTemp = new List<SpellCast>(Spells);
				foreach(SpellCast spell in SpellsTemp) {
					if(!spell.enabled)
						Spells.Remove (spell);
				}
				retryTime = Time.time + 1;
				minButtonCheckTime = Time.time + 0.2f;
				comboPlace = 1;
				SpellText.text = "Right";
			}
		}
	}
}
