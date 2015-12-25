using UnityEngine;
using System.Collections;

public class AdvancedLight : MonoBehaviour {

	public bool inverse;

	public void ApplyOptions() {
		if (Options.AdvancedShadows && !inverse || !Options.AdvancedShadows && inverse)
			GetComponent<Light> ().enabled = true;
		else
			GetComponent<Light> ().enabled = false;
	}
}
