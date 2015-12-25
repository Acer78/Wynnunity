using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Chat : MonoBehaviour {
	
	public static string log = "";
	private static List<string> cutLog = new List<string>();
	private static Text txtfield;
	private static float nextRemoveTime = 4;
	private static int maxChars = 1250;

	// Use this for initialization
	void Start () {
		txtfield = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (log.Length >= maxChars || Time.time >= nextRemoveTime && cutLog.Count > 0) {
			nextRemoveTime = Time.time + 4;
			cutLog.RemoveAt (0);
			log = "";
			foreach (string line in cutLog) {
				log += "\n" + line;
			}
			txtfield.text = log;
		}
	}

	public static void Write (string line) {
		log += "\n" + line;
		txtfield.text = log;
		string[] cutLogArray = log.Split ('\n');
		cutLog.Clear ();
		cutLog.AddRange (cutLogArray);
		nextRemoveTime = Time.time + 4;
	}

	public void ChangeWidth (float width) {
		transform.parent.GetComponent<RectTransform> ().SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, width);
	}
}
