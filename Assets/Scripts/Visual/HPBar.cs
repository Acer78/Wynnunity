using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HPBar : MonoBehaviour {

	public Text text;
	private Health player;
	// Use this for initialization
	void Start () {
		player = Player.Health;
	}
	
	// Update is called once per frame
	void Update () {
		transform.localScale = Vector3.up + Vector3.forward + Vector3.right*Mathf.Ceil(((float) player.health /(float) player.maxHealth)*20)/20;
		text.text = (float) Mathf.CeilToInt(player.health) + " HP";
	}
}
