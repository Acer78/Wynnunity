using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

	public Button[] Slots = new Button[40];
	private InvStack[] InvSlots = new InvStack[40];
	public GameObject InvPanel;
	public InvStack[] Hotbar;
	public int CurrentHotbarSlot = 0;
	private InvStack selectedOne;
	private InvStack selectedTwo;
	public Color selectedColour = new Color(0.75f, 0.75f, 0.75f);
	public List<ArmourSlot> ArmourSlots;
	public GameObject amtPrefab;
	private bool armourSwitching;
	private bool canPickUp = true;

	void Start () {
		XP.RecalcLevelupXP ();
		Player.Inventory = this;
		toggleInv (true);
		toggleInv (false);
		selectHotbarSlot (5, false);
	}

	void Update() {
		if (Input.mouseScrollDelta.y > 0) {
			selectHotbarSlot(1, true);
		} else if (Input.mouseScrollDelta.y < 0) {
			selectHotbarSlot(-1, true);
		}

		if (selectedOne && Input.GetKeyDown (KeyCode.Q)) {
			selectedOne.GetComponent<Image> ().color = Color.white;
			if(selectedTwo)
				selectedTwo.GetComponent<Image> ().color = Color.white;
			InvStack tempselected = selectedOne;
			selectedOne = null;
			selectedTwo = null;
			armourSwitching = false;
			DropItem (tempselected);
			selectHotbarSlot (CurrentHotbarSlot, false);
		}
	}
	
	void RecalcSlots () {
		ArmourSlots.Clear ();
		List<Button> SlotsTemp = new List<Button>(GetComponentsInChildren<Button> ());
		List<Button> SlotsTemp2 = new List<Button> (SlotsTemp);
		List<InvStack> InvSlotsTemp = new List<InvStack> ();
		foreach (Button button in SlotsTemp) {
			if(button.transform.parent.name == "Perm Items") {
				SlotsTemp2.Remove (button);
			} else if(button.GetComponent<ArmourSlot>()) {
				ArmourSlots.Add (button.GetComponent<ArmourSlot>());
			}
		}
		SlotsTemp2.CopyTo (Slots);

		foreach (Button Slot in Slots) {
			if(!Slot.GetComponent<InvStack>()) {
				Slot.gameObject.AddComponent<InvStack>();;
				InvSlotsTemp.Add (Slot.GetComponent<InvStack>());
			}
		}
		InvSlotsTemp.CopyTo (InvSlots);
		Hotbar = transform.GetChild (1).GetComponentsInChildren<InvStack> ();
	}

	public void selectHotbarSlot(int slot, bool relative) {
		Hotbar[CurrentHotbarSlot].GetComponent<Image>().color = Color.white;
		if (relative)
			slot += CurrentHotbarSlot;
		CurrentHotbarSlot = (int)Mathf.Repeat (slot, Hotbar.Length);
		Hotbar[CurrentHotbarSlot].GetComponent<Image>().color = Color.gray;
		Player.Punch.changeHeldItem(Hotbar[CurrentHotbarSlot].item);
		ItemPanel.weapon = Hotbar [CurrentHotbarSlot].item;
	}

	public void toggleInv(bool open) {
		InvPanel.SetActive (open);
		if (selectedOne) {
			selectedOne.GetComponent<Image> ().color = Color.white;
			if (selectedTwo)
				selectedTwo.GetComponent<Image> ().color = Color.white;
			selectedOne = null;
			selectedTwo = null;
			armourSwitching = false;
			selectHotbarSlot (CurrentHotbarSlot, false);
		}
		if(open)
			RecalcSlots ();
	}

	public void slotClicked(InvStack slot) {
		if (!selectedOne) {
			selectedOne = slot;
			ItemPanel.weapon = selectedOne.item;
			selectedOne.GetComponent<Image> ().color = selectedColour;
			if (selectedOne.GetComponent<ArmourSlot> ())
				armourSwitching = true;
		} else if (selectedOne && !selectedTwo) {
			selectedTwo = slot;
			bool skip = false;
			if (selectedTwo.GetComponent<ArmourSlot> () && !armourSwitching) {
				int tempint = System.Array.IndexOf(Slots, selectedOne.GetComponent<Button>());
				int tempint2 = System.Array.IndexOf(Slots, selectedTwo.GetComponent<Button>());
				selectedOne = Slots[tempint2].GetComponent<InvStack>();
				selectedTwo = Slots[tempint].GetComponent<InvStack>();
				armourSwitching = true;
			}
			else if (selectedTwo.GetComponent<ArmourSlot>() && armourSwitching) {
				skip = true;
			}

			if (selectedTwo.GetComponent<ArmourSlot>() && (selectedTwo.GetComponent<ArmourSlot>() && selectedTwo.item is Item_Armour && ((Item_Armour)selectedTwo.item).lvlMin > XP.Level || selectedOne.GetComponent<ArmourSlot>() || selectedOne.item is Item_Armour && ((Item_Armour)selectedOne.item).lvlMin > XP.Level) ||
			selectedOne.GetComponent<ArmourSlot>() && (selectedTwo.GetComponent<ArmourSlot>() || selectedTwo.item is Item_Armour && ((Item_Armour)selectedTwo.item).lvlMin > XP.Level || selectedOne.GetComponent<ArmourSlot>() && selectedOne.item is Item_Armour && ((Item_Armour)selectedOne.item).lvlMin > XP.Level)) {
				skip = true;
			}
			Item tempData_Item = selectedTwo.item;
			int tempData_amt = selectedTwo.amount;
			if(!skip && (!armourSwitching || selectedTwo.item == ItemDB.Weapons[0] || selectedTwo.item == null || armourSwitching && selectedTwo.item is Item_Armour && selectedOne.GetComponent<ArmourSlot>().Type == ((Item_Armour)selectedTwo.item).Type))
			{
				selectedTwo.item = selectedOne.item;
				selectedOne.item = tempData_Item;
				selectedTwo.amount = selectedOne.amount;
				selectedOne.amount = tempData_amt;
				selectedOne.UpdateIcon ();
				selectedTwo.UpdateIcon ();
			}
			selectedOne.GetComponent<Image> ().color = Color.white;
			selectedTwo.GetComponent<Image> ().color = Color.white;
			selectedOne = null;
			selectedTwo = null;
			armourSwitching = false;
			selectHotbarSlot (CurrentHotbarSlot, false);
			Player.RecalcArmour();
		}
	}

	public int NextAvailableSlot (Item item, int amount = 1) {
		if (canPickUp) {
			for (int i = InvSlots.Length - 1; i >= 0; i--) {
				if (InvSlots [i].item == ItemDB.Weapons [0] || InvSlots [i].item == null || InvSlots [i].item == item && InvSlots [i].amount + amount <= item.maxStack) {
					if (!InvSlots [i].GetComponent<ArmourSlot> ()) {
						return i;
					}
				}
			}
		}
		return -1;
	}

	public void CollectItem (Item item, int amount) {
		if (NextAvailableSlot (item, amount) != -1) {
			InvStack invSlot = InvSlots [NextAvailableSlot (item, amount)];
			invSlot.item = item;
			invSlot.amount = amount + invSlot.amount;
			invSlot.UpdateIcon ();
			selectHotbarSlot (CurrentHotbarSlot, false);
		}
		StartCoroutine (pickUpCooldown (0.05f));
	}

	IEnumerator pickUpCooldown(float t) {
		canPickUp = false;
		yield return new WaitForSeconds (t);
		canPickUp = true;
	}

	public void DropItem (InvStack slot, int amount = 1, bool justDelete = false) {
		amount = Mathf.Clamp (amount, 1, slot.amount);
		slot.amount -= amount;
		if (!justDelete) {
			for (int i = 0; i < amount; i++) {
				DroppedItem droppedItem = ((GameObject)Instantiate (ItemDB.droppedItem, Camera.main.transform.position, transform.rotation)).GetComponent<DroppedItem> ();
				droppedItem.item = slot.item;
				droppedItem.GetComponent<Rigidbody> ().AddForce ((transform.forward + transform.up) * 5f, ForceMode.VelocityChange);
				droppedItem.StartCoroutine (droppedItem.TriggerWait (1f));
			}
		}
		if (slot.amount <= 0) {
			slot.item = ItemDB.Weapons[0];
			slot.amount = 1;
			slot.UpdateIcon();
			selectHotbarSlot (CurrentHotbarSlot, false);
		}
	}
}