using UnityEngine;
using System.Collections;

public class IkArmController : MonoBehaviour {
	public Transform target;
	public Transform elbowTarget;
	
	private InverseKinematics inverseKinematicsScript;
	
	// Use this for initialization
	void Start () {
		inverseKinematicsScript = GetComponent<InverseKinematics>();
	}
	
	// Update is called once per frame
	void Update () {
		inverseKinematicsScript.target = target.position;
		inverseKinematicsScript.elbowTarget = elbowTarget.position;
		inverseKinematicsScript.CalculateIK();
	}
}
