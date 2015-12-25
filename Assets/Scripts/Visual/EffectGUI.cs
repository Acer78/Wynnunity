using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EffectGUI : MonoBehaviour {

	public string type;
	public Effects effects;
	private float timeLeft;
	private float power;

	private Transform trueParent;
	
	// Use this for initialization
	void Start () {
		trueParent = transform.parent;
		if (!effects)
			effects = Player.Effects;
	}
	
	// Update is called once per frame
	void Update () {

		//Regen
		if (type == "Heal") {
			if (effects.Regen.x > 0) {
				if(transform.parent != trueParent) {
					transform.SetParent (trueParent);
					GetComponent<Image>().enabled = true;
					transform.GetChild (0).gameObject.SetActive(true);
					transform.GetChild (1).gameObject.SetActive(true);
					transform.GetChild (2).gameObject.SetActive(true);
				}
				int minutes = Mathf.FloorToInt (effects.Regen.x / 60);
				int seconds = Mathf.FloorToInt (effects.Regen.x % 60);
				float regenPerSec = effects.Regen.z == 0 ? effects.Regen.y : Mathf.Round((effects.Regen.y / effects.Regen.z)*100)/100;
				transform.GetChild (2).GetComponent<Text> ().text = minutes + ":" + seconds.ToString ("00") + "\n" + regenPerSec + " HP/s";
			} else if(transform.parent == trueParent){
				transform.SetParent (trueParent.parent);
				GetComponent<Image>().enabled = false;
				transform.GetChild (0).gameObject.SetActive(false);
				transform.GetChild (1).gameObject.SetActive(false);
				transform.GetChild (2).gameObject.SetActive(false);
			}
		}
		//Mana
		 else if (type == "Mana") {
			if (effects.ManaRegen.x > 0) {
				if(transform.parent != trueParent) {
					transform.SetParent (trueParent);
					GetComponent<Image>().enabled = true;
					transform.GetChild (0).gameObject.SetActive(true);
					transform.GetChild (1).gameObject.SetActive(true);
					transform.GetChild (2).gameObject.SetActive(true);
				}
				int minutes = Mathf.FloorToInt (effects.ManaRegen.x / 60);
				int seconds = Mathf.FloorToInt (effects.ManaRegen.x % 60);
				float regenPerSec = effects.ManaRegen.z == 0 ? effects.ManaRegen.y : Mathf.Round((effects.ManaRegen.y / effects.ManaRegen.z)*100)/100;
				transform.GetChild (2).GetComponent<Text> ().text = minutes + ":" + seconds.ToString ("00") + "\n" + regenPerSec + " MP/s";
			} else if(transform.parent == trueParent){
				transform.SetParent (trueParent.parent);
				GetComponent<Image>().enabled = false;
				transform.GetChild (0).gameObject.SetActive(false);
				transform.GetChild (1).gameObject.SetActive(false);
				transform.GetChild (2).gameObject.SetActive(false);
			}
		}

		//Speed
		else if (type == "Speed") {
			if (effects.Speed.x > 0) {
				if(transform.parent != trueParent) {
					transform.SetParent (trueParent);
					GetComponent<Image>().enabled = true;
					transform.GetChild (0).gameObject.SetActive(true);
					transform.GetChild (1).gameObject.SetActive(true);
					transform.GetChild (2).gameObject.SetActive(true);
				}
				int minutes = Mathf.FloorToInt (effects.Speed.x / 60);
				int seconds = Mathf.FloorToInt (effects.Speed.x % 60);
				transform.GetChild (2).GetComponent<Text> ().text = minutes + ":" + seconds.ToString ("00") + "\n+" + effects.Speed.y*10 + "%";
			} else if(transform.parent == trueParent){
				transform.SetParent (trueParent.parent);
				GetComponent<Image>().enabled = false;
				transform.GetChild (0).gameObject.SetActive(false);
				transform.GetChild (1).gameObject.SetActive(false);
				transform.GetChild (2).gameObject.SetActive(false);
			}
		}

		//Slowness
		else if (type == "Slowness") {
			if (effects.Slowness.x > 0) {
				if(transform.parent != trueParent) {
					transform.SetParent (trueParent);
					GetComponent<Image>().enabled = true;
					transform.GetChild (0).gameObject.SetActive(true);
					transform.GetChild (1).gameObject.SetActive(true);
					transform.GetChild (2).gameObject.SetActive(true);
				}
				int minutes = Mathf.FloorToInt (effects.Slowness.x / 60);
				int seconds = Mathf.FloorToInt (effects.Slowness.x % 60);
				transform.GetChild (2).GetComponent<Text> ().text = minutes + ":" + seconds.ToString ("00") + "\n-" + effects.Slowness.y*10 + "%";
			} else if(transform.parent == trueParent){
				transform.SetParent (trueParent.parent);
				GetComponent<Image>().enabled = false;
				transform.GetChild (0).gameObject.SetActive(false);
				transform.GetChild (1).gameObject.SetActive(false);
				transform.GetChild (2).gameObject.SetActive(false);
			}
		}

		//Stun
		else if (type == "Stun") {
			if (effects.Stun.x > 0) {
				if(transform.parent != trueParent) {
					transform.SetParent (trueParent);
					GetComponent<Image>().enabled = true;
					transform.GetChild (0).gameObject.SetActive(true);
					transform.GetChild (1).gameObject.SetActive(true);
					transform.GetChild (2).gameObject.SetActive(true);
				}
				int minutes = Mathf.FloorToInt (effects.Stun.x / 60);
				int seconds = Mathf.FloorToInt (effects.Stun.x % 60);
				transform.GetChild (2).GetComponent<Text> ().text = minutes + ":" + seconds.ToString ("00");
			} else if(transform.parent == trueParent){
				transform.SetParent (trueParent.parent);
				GetComponent<Image>().enabled = false;
				transform.GetChild (0).gameObject.SetActive(false);
				transform.GetChild (1).gameObject.SetActive(false);
				transform.GetChild (2).gameObject.SetActive(false);
			}
		}
	}
}
