using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ArmourSlot : MonoBehaviour {
	
	public InvStack slot;
	public Item_Armour.ArmourType Type = Item_Armour.ArmourType.HELMET;

	void Start() {
		slot = GetComponent<InvStack> ();
	}
}
