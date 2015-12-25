using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class effectInfo {
	public string type;
	public float[] stats = new float[5];

	public effectInfo (string effecttype, float[] effectstats) {
		type = effecttype;
		stats = effectstats;
	}

	public effectInfo () {
		type = "Heal";
		stats = new float[3] {0,0,0};
	}
}

public class Item_Consumable : Item {

	public string description;
	public List<effectInfo> effects;

	public Item_Consumable (string itemname, List<effectInfo> Effects)
	{
		name = itemname;
		effects = Effects;
	}
	public Item_Consumable ()
	{
		name = "";
		effects = new List<effectInfo> (1);
	}
}