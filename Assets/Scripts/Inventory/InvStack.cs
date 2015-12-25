using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InvStack : MonoBehaviour {

	public Item item;
	public int amount = 1;
	private GameObject icon;
	public Inventory inv;
	public Text amtText;

	// Use this for initialization
	void Awake () {
		inv = Player.Inventory;
		GetComponent<Button>().onClick.AddListener (() => inv.slotClicked(this));
		item = ItemDB.Weapons [0];
		icon = new GameObject ("invIcon");
		icon.transform.SetParent (transform);
		icon.transform.localPosition = Vector3.zero;
		icon.transform.localRotation = Quaternion.Euler (Vector3.zero);
		icon.transform.localScale = Vector3.one*0.4f;
		UpdateIcon ();
		amtText = ((GameObject)Instantiate (inv.amtPrefab)).GetComponent<Text>();
		amtText.transform.SetParent (transform);
		amtText.transform.localPosition = inv.amtPrefab.transform.localPosition;
		amtText.transform.localRotation = inv.amtPrefab.transform.localRotation;
		amtText.transform.localScale = inv.amtPrefab.transform.localScale;
	}

	public void UpdateIcon() {
		if (item != null && item.invIcon != null) {
			Image InvIcon;
			if(icon.GetComponent<Image>()) {
				InvIcon = icon.GetComponent<Image>();
			} else {
				InvIcon = icon.AddComponent<Image>();
			}
			InvIcon.sprite = item.invIcon;
		}
	}

	// Update is called once per frame
	void Update () {
		if (amount != 1 && amount != 0)
			amtText.text = amount.ToString ();
		else
			amtText.text = "";
		if (item == null || item == ItemDB.Weapons [0])
			amount = 0;
	}
}
