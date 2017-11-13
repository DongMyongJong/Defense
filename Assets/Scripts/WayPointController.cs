using UnityEngine;
using System.Collections;

public class WayPointController : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		Debug.Log ("OnTriggerEnter");
		WayPoints wayPointsScript = other.GetComponent<WayPoints> ();
		if (wayPointsScript != null) {
			int count = wayPointsScript.points.Length;
			int curIndex = wayPointsScript.curIndex;
			int prevIndex = wayPointsScript.prevIndex;
			if (curIndex == count - 1) {
				wayPointsScript.prevIndex = count - 1;
				wayPointsScript.curIndex = count - 2;
			} else if (curIndex == 0) {
				wayPointsScript.prevIndex = 0;
				wayPointsScript.curIndex = 1;
			} else {
				wayPointsScript.prevIndex = curIndex;
				if (prevIndex < curIndex) {
					wayPointsScript.curIndex++;
				} else {
					wayPointsScript.curIndex--;
				}
			}
		}
	}
}
