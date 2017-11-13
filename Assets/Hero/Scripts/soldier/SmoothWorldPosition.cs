//Character Controller clibs stairs' steps in a very snappy manner.
//The purpose of this script is to smooth this snappy changes of position.
//This script must be in a game object in between the hierarchy of the character controller and the soldier.

using UnityEngine;
using System.Collections;

public class SmoothWorldPosition : MonoBehaviour {
	public float horizontalSmooth = 3.0f;
	public float verticalUpSmooth = 10.0f;
	public float verticalDownSmooth = 1.0f;//50.0;
	private Vector3 worldPosition;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void LateUpdate() {
		horizontalSmooth = Mathf.Max(horizontalSmooth, 0.0f);
		verticalUpSmooth = Mathf.Max(verticalUpSmooth, 0.0f);
		verticalDownSmooth = Mathf.Max(verticalDownSmooth, 0.0f);
		if (horizontalSmooth == 0) {
			worldPosition.x = transform.parent.position.x;
			worldPosition.z = transform.parent.position.z;
		} else {
			worldPosition.x = Mathf.Lerp(worldPosition.x, transform.parent.position.x, Time.deltaTime * 50.0f / horizontalSmooth);
			worldPosition.z = Mathf.Lerp(worldPosition.z, transform.parent.position.z, Time.deltaTime * 50.0f / verticalUpSmooth);
		}
		if (worldPosition.y < transform.parent.position.y) {
			if (verticalUpSmooth == 0) {
				worldPosition.y = transform.parent.position.y;
			} else {
				worldPosition.y = Mathf.Lerp(worldPosition.y, transform.parent.position.y, Time.deltaTime * 50.0f / verticalUpSmooth);
			}
		} else {
			if (verticalDownSmooth == 0) {
				worldPosition.y = transform.parent.position.y;
			} else {
				worldPosition.y = Mathf.Lerp(worldPosition.y, transform.parent.position.y, Time.deltaTime * 50.0f / verticalDownSmooth);
			}
		}
		transform.position = worldPosition;
		Quaternion rot = Quaternion.FromToRotation (transform.forward, transform.parent.forward);
		transform.Rotate (Vector3.up * rot.eulerAngles.y);
	}
}
