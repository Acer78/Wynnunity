using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public bool ClearLog = false;
	public bool LogItems = true;
	public static Quest currentQuest = new Quest();

	public static void ReloadDB (bool logitems, bool clearlog) {
		ItemDB.GetAllWeapons (logitems, clearlog);
	}

	public void ReloadDB () {
		ItemDB.GetAllWeapons (LogItems, ClearLog);
	}

	void Awake () {
		DontDestroyOnLoad (gameObject);
		ReloadDB ();
	}

	void Update () {
		if (Input.GetKey (KeyCode.LeftControl) && Input.GetKeyDown (KeyCode.F1)) {
			Debug.Log ("[Ctrl+F1] Reloading database...");
			ReloadDB();
		}
	}
}
