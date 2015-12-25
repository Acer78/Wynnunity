using UnityEngine;
using System.Collections;

public class Item {

	public string name;
	public int maxStack = 64;
	public GameObject model;
	public Sprite invIcon;

	public Item()
	{
		name = "";
		maxStack = 64;
		model = Resources.Load ("Data/Items/ItemIcons/Placeholder") as GameObject;
		invIcon = Resources.Load ("Data/Items/ItemIcons/InvIcons/PlaceholderIcon") as Sprite;
	}

	public Item(string itemName)
	{
		name = itemName;
		maxStack = 64;
		model = Resources.Load ("Data/Items/ItemIcons/Placeholder") as GameObject;
		invIcon = Resources.Load ("Data/Items/ItemIcons/InvIcons/PlaceholderIcon") as Sprite;
	}
}
