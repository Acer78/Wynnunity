using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Item_Armour : Item {
	
	public enum ArmourType
	{
		HELMET,
		CHESTPLATE,
		LEGGINGS,
		BOOTS
	}
	public ArmourType Type = ArmourType.HELMET;
	public int lvlMin;
	public int def;
	
	public Item_Armour (string itemname = "", int lvlmin = 1, ArmourType type = ArmourType.HELMET, int itemdef = 1)
	{
		name = itemname;
		lvlMin = lvlmin;
		Type = type;
		def = itemdef;
	}
}
