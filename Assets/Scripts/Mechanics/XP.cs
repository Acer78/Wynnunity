using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;

public class XP {
	public static int Experience;
	public static int Level = 1;
	public static int LevelCap = 75;
	public static XPBar Bar;
	public static List<int> LevelupXP = new List<int>();
#if UNITY_EDITOR
	[MenuItem("Assets/Recalc Levelup XP")]
#endif
	public static void RecalcLevelupXPAndLog() {
		LevelupXP.Clear ();
		for(int i = 0; i < LevelCap - 1; i++) {
			LevelupXP.Add (Mathf.RoundToInt(Mathf.Pow (i + 2, 3f)*10));
			Debug.Log (Mathf.RoundToInt(Mathf.Pow (i + 2, 3f)*10));
		}
	}

	public static void RecalcLevelupXP() {
		LevelupXP.Clear ();
		for(int i = 0; i < LevelCap - 1; i++) {
			LevelupXP.Add (Mathf.RoundToInt(Mathf.Pow (i + 2, 3f)*10));
		}
	}
	
	public static int CheckLevel(int XP) {
		int level = 1;
		while (LevelupXP[level - 1] <= XP) {
			if(level < LevelupXP.Count)
				level++;
			else {
				Level = level + 1;
				return level + 1;
			}
		}
		Level = level;
		return level;
	}
}
