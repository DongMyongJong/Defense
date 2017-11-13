using UnityEngine;
using System.Collections;

public class InverseKinematics : MonoBehaviour {
	public Vector3 target;
	public Vector3 elbowTarget;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void CalculateIK(){
		transform.LookAt(target, transform.position - elbowTarget);
		Transform upperArm = transform.Find("upperArm");
		Transform elbow = transform.Find("upperArm/elbow");
		Transform hand = transform.Find("upperArm/elbow/hand");
		float upperArmLength = Vector3.Distance(upperArm.position, elbow.position);
		float forearmLength = Vector3.Distance(elbow.position, hand.position);
		float armLength = upperArmLength + forearmLength;
		float hypotenuse = upperArmLength;
		float targetDistance = Vector3.Distance(upperArm.position, target);
		targetDistance = Mathf.Min(targetDistance, armLength - 0.0001f); //Do not allow target distance be further away than the arm's length.
		float adjacent = targetDistance * (upperArmLength / armLength);
		float ikAngle = Mathf.Acos(adjacent / hypotenuse) * Mathf.Rad2Deg;
		upperArm.LookAt(target, transform.root.up);
		Vector3 angles = upperArm.localRotation.eulerAngles;
		upperArm.localRotation = Quaternion.Euler(angles.x + ikAngle, angles.y, angles.z);
		elbow.LookAt(target, transform.root.up);
	}
}
