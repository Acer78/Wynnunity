using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spell : MonoBehaviour {

	public int ManaConsumed = 6;
	public int SpamMana = 0;
	public float SpamResetTime = 3f;
	public float NextSpamResetTime;

	public virtual IEnumerator CastSpell(Item_Weapon Weapon) {
		yield return null;
	}
}
