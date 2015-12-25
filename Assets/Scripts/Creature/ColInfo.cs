using UnityEngine;
using System.Collections;

public class ColInfo : MonoBehaviour {

	public bool CollidingWithSolids = false;

	void OnCollisionExit() {
		CollidingWithSolids = false;
	}
	void OnCollisionStay() {
		CollidingWithSolids = true;
	}
}
