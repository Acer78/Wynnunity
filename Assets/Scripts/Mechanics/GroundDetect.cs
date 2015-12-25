using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroundDetect : MonoBehaviour {


	public float dist = 0.01f;
	public List<Transform> checks;
	public int failedCount = 0;

	public bool chckdist() {
		failedCount = 0;
		foreach (Transform check in checks) {
			if(Physics.Raycast(check.position, -check.transform.up, dist, Player.SpawnLOS))
			{
				return true;
			}
			else
			{
				failedCount ++;
			}
		}
		if (failedCount >= checks.Count) {
			return false;
		} else {
			return true;
		}
	}

	public bool checkSlope(float maxAngle) {
		RaycastHit hit;
		if (Physics.Raycast (checks [0].position, -checks [0].transform.up, out hit, Mathf.Infinity, Player.SpawnLOS)) {
			if (hit.collider.tag == "AlwaysClimbable" || Mathf.Rad2Deg * Mathf.Acos (Mathf.Clamp (hit.normal.y, -1f, 1f)) <= maxAngle) {
				return true;
			} else {
				return false;
			}
		} else {
			return true;
		}
	}

	public bool compareHeights() {
		RaycastHit hit;
		RaycastHit hit2;
		if (Physics.Raycast (checks [0].position, -checks [0].transform.up, out hit, Mathf.Infinity, Player.SpawnLOS)) {
			if (Physics.Raycast (checks [1].position, -checks [1].transform.up, out hit2, Mathf.Infinity, Player.SpawnLOS)) {
				if (hit.point.y >= hit2.point.y) {
					return true;
				}
				else {
					return false;
				}
			} else {
				return false;
			}
		} else {
			return false;
		}
	}
}
