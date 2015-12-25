using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Tag : MonoBehaviour {

	private Text text;
	public Enemy enemy;
	private string[] HPBar;
	public int HPPlace = 0;
	private int lasthp = 0;

	// Use this for initialization
	void Start () {
		if(!text)
			text = GetComponent<Text> ();
		HPBar = new string[] {
			"[:::::" + "<color=maroon>" + Mathf.CeilToInt (enemy.HP.health) + "</color>" + ":::::</color>",
			"[:::::" + "<color=maroon>" + Mathf.CeilToInt (enemy.HP.health) + "</color>" + "::::</color>:",
			"[:::::" + "<color=maroon>" + Mathf.CeilToInt (enemy.HP.health) + "</color>" + ":::</color>::",
			"[:::::" + "<color=maroon>" + Mathf.CeilToInt (enemy.HP.health) + "</color>" + "::</color>:::",
			"[:::::" + "<color=maroon>" + Mathf.CeilToInt (enemy.HP.health) + "</color>" + ":</color>::::",
			"[:::::</color>" + "<color=maroon>" + Mathf.CeilToInt (enemy.HP.health) + "</color>" + ":::::",
			"[::::</color>:" + "<color=maroon>" + Mathf.CeilToInt (enemy.HP.health) + "</color>" + ":::::",
			"[:::</color>::" + "<color=maroon>" + Mathf.CeilToInt (enemy.HP.health) + "</color>" + ":::::",
			"[::</color>:::" + "<color=maroon>" + Mathf.CeilToInt (enemy.HP.health) + "</color>" + ":::::",
			"[:</color>::::" + "<color=maroon>" + Mathf.CeilToInt (enemy.HP.health) + "</color>" + ":::::",
			"[</color>:::::" + "<color=maroon>" + Mathf.CeilToInt (enemy.HP.health) + "</color>" + ":::::"
		};
	}
	
	// Update is called once per frame
	void Update () {

		if (Mathf.CeilToInt (enemy.HP.health) != lasthp) {
			HPBar = new string[] {
				"[:::::" + "<color=maroon>" + Mathf.CeilToInt (enemy.HP.health) + "</color>" + ":::::</color>",
				"[:::::" + "<color=maroon>" + Mathf.CeilToInt (enemy.HP.health) + "</color>" + "::::</color>:",
				"[:::::" + "<color=maroon>" + Mathf.CeilToInt (enemy.HP.health) + "</color>" + ":::</color>::",
				"[:::::" + "<color=maroon>" + Mathf.CeilToInt (enemy.HP.health) + "</color>" + "::</color>:::",
				"[:::::" + "<color=maroon>" + Mathf.CeilToInt (enemy.HP.health) + "</color>" + ":</color>::::",
				"[:::::</color>" + "<color=maroon>" + Mathf.CeilToInt (enemy.HP.health) + "</color>" + ":::::",
				"[::::</color>:" + "<color=maroon>" + Mathf.CeilToInt (enemy.HP.health) + "</color>" + ":::::",
				"[:::</color>::" + "<color=maroon>" + Mathf.CeilToInt (enemy.HP.health) + "</color>" + ":::::",
				"[::</color>:::" + "<color=maroon>" + Mathf.CeilToInt (enemy.HP.health) + "</color>" + ":::::",
				"[:</color>::::" + "<color=maroon>" + Mathf.CeilToInt (enemy.HP.health) + "</color>" + ":::::",
				"[</color>:::::" + "<color=maroon>" + Mathf.CeilToInt (enemy.HP.health) + "</color>" + ":::::"
			};
		}
		lasthp = Mathf.CeilToInt (enemy.HP.health);
		if (enemy.HP.health == 0)
			//Vector3 stayPos = 
			transform.parent.SetParent (null);
		if (!enemy)
			Destroy (transform.parent.gameObject, 0.1f);
		HPPlace = 10 - Mathf.RoundToInt (((float) enemy.HP.health / (float) enemy.HP.maxHealth) * 10);
		text.text = "<color=red>" + enemy.visible_name + "</color><color=orange> [Lv. " + enemy.level + "]</color>" +
			"\n<color=red>" + HPBar [Mathf.Clamp(HPPlace, 0, 10)] + "<color=red>]</color>";
	}
}
