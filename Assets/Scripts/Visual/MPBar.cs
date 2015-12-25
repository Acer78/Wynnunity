using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MPBar : MonoBehaviour {
	
	public Text text;
	private Mana player;

	void Start () {
		player = Player.Mana;
	}

	void Update () {
		transform.localScale = Vector3.up + Vector3.forward + Vector3.right*Mathf.Ceil(((float) player.MP /(float) player.maxMP)*18)/18;
		text.text = (float) Mathf.CeilToInt(player.MP) + " MP";
	}
}