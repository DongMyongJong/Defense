using UnityEngine;
using System.Collections;

public class LaserSight : MonoBehaviour {
	public GameObject laserLinePrefab;
	public bool on;
	public bool disableRootCollider = true;
	
	private Transform laserPoint;
	private Transform laserPointOrigin;
	private float laserLineRate = 2.0f;
	private float nextLaserLineTime;
	private float positionBuffer = 2.0f;//Between the ends.

	// Use this for initialization
	void Start () {
		on = true;
		laserPoint = transform.Find("laserPoint");
		laserPointOrigin = transform.Find("laserPointOrigin");
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit = new RaycastHit();
		float maxLength = 20.0f;
		if (disableRootCollider){
			transform.root.collider.enabled = false;
		}
		if (Physics.Raycast(transform.position, transform.forward, out hit) && on){
			TriggerChildrenCollider triggerChildrenColliderScript = hit.transform.root.GetComponent<TriggerChildrenCollider>(); //Children collider property.
			bool reCheck = false; //Re-check if there's a hit for children collider.
			Collider mainColliderHit = hit.collider; //Parent collider. (must be re enabled)
			if (triggerChildrenColliderScript != null) { //Trigger children property. Enable children collider and disable root collider.
				hit.collider.enabled = false;
				Collider[] childrenColliderList = triggerChildrenColliderScript.childrenColliderList;
				for (int i = 0; i < childrenColliderList.Length; i++) {
					childrenColliderList[i].enabled = true;
				}
				reCheck = Physics.Raycast(transform.position, transform.forward, out hit); //Recheck collision for children collider.
			}	
			if (reCheck || triggerChildrenColliderScript == null) { 
				laserPoint.position = hit.point + hit.normal * 0.03f;
				laserPoint.GetComponent<LaserPoint>().on = true;
				maxLength = Mathf.Min(maxLength, Vector3.Distance(transform.position, hit.point));
			} else {
				laserPoint.GetComponent<LaserPoint>().on = false;
			}
			if(triggerChildrenColliderScript != null) {//Trigger children property. Disable children collider and enable root collider.
				mainColliderHit.enabled = true;
				Collider[] childrenColliderList = triggerChildrenColliderScript.childrenColliderList;
				for (int n = 0; n < childrenColliderList.Length; n++) {
					childrenColliderList[n].enabled = false;
				}
			}
		} else {
			laserPoint.GetComponent<LaserPoint>().on = false;
		}
		if (disableRootCollider) {
			transform.root.collider.enabled = true;
		}
		laserLineRate = maxLength * 0.5f;
		
		if (Time.time > nextLaserLineTime && on) {
			nextLaserLineTime = Time.time + (1/laserLineRate);
			GameObject newLaserLine = Instantiate(laserLinePrefab, transform.position, Quaternion.identity) as GameObject;
			newLaserLine.name = "laserLine";
			newLaserLine.transform.parent = transform;
			newLaserLine.transform.localRotation = Quaternion.identity;
			Vector3 angles = newLaserLine.transform.localRotation.eulerAngles;
			newLaserLine.transform.localRotation = Quaternion.Euler(angles.x + 90, angles.y, angles.z);
			Vector3 localPosition = newLaserLine.transform.localPosition;
			if (maxLength < positionBuffer * 2.0f) {
				newLaserLine.transform.localPosition = new Vector3(localPosition.x, localPosition.y, positionBuffer);
			} else {
				newLaserLine.transform.localPosition = new Vector3(localPosition.x, localPosition.y, Random.Range(positionBuffer, maxLength - positionBuffer));
			}
		}
		if (on) {
			laserPointOrigin.GetComponent<LaserPoint>().on = true;
		} else {
			laserPoint.GetComponent<LaserPoint>().on = false;
			laserPointOrigin.GetComponent<LaserPoint>().on = false;
		}
		//Delete laser lines further than ray cast hit.
		if (maxLength > positionBuffer * 2) {
			for (int m = 0; m < transform.childCount; m++) {
				Transform child = transform.GetChild(m);
				if (child.localPosition.z > maxLength && child.name == "laserLine") {
					Destroy(child.gameObject);
				}
			}
		}
	}
}
