using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class XPBar : MonoBehaviour {
	
	public Text text;
	private float nextCheckTime;

	void Awake() {
		XP.Bar = this;
	}

	public void GainXP(int points) {
		XP.Experience += points;
		XP.CheckLevel (XP.Experience);
		UpdateBar ();
	}

	public void UpdateBar () {
		text.text = XP.Level.ToString ();
		if (XP.Level == 1) {
			transform.localScale = Vector3.up + Vector3.forward + Vector3.right * Mathf.Ceil (((float)XP.Experience / (float)XP.LevelupXP [XP.Level - 1]) * 100) / 100;
		} else if (XP.Level == XP.LevelCap) {
			transform.localScale = Vector3.up + Vector3.forward;
		} else {
			transform.localScale = Vector3.up + Vector3.forward + Vector3.right*Mathf.Ceil(((float) (XP.Experience - XP.LevelupXP[XP.Level - 2])/(float) (XP.LevelupXP[XP.Level - 1] - XP.LevelupXP[XP.Level - 2]))*100)/100;
		}
	}

	void Update() {
		if (Time.time > nextCheckTime) {
			nextCheckTime += 0.5f;
			UpdateBar ();
		}
	}
}
