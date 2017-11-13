using UnityEngine;
using System.Collections;

public class SoldierCamera : MonoBehaviour {
	public float cameraTiltMultiplier = 1.0f;
	public string lookAtJoystickID;

	private Vector3 lastPosition;
	private float forwardSpeed;
	private float cameraTilt;
	private float verticalAim;
	private Vector3 localPosition;
	private Vector3 positionOffset;
	//External scripts.
	private Health healthScript;
	private CrouchController crouchControllerScript;
	
	//Soldier parts.
	private Transform spine2;
	
	// Use this for initialization
	void Start () {
		cameraTilt = 0.0f;
		verticalAim = 0.0f;
		localPosition = transform.localPosition;
		positionOffset = Vector3.zero;
		healthScript = transform.root.GetComponent<Health>();
		crouchControllerScript = transform.root.GetComponent<CrouchController>();
		spine2 = transform.root.Find("smoothWorldPosition/soldierSkeleton/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2");
	}
	
	// Update is called once per frame
	void Update () {
		float health = 100.0f;
		if (healthScript != null) {
			health = healthScript.GetHealth();
		}
		Vector2 joystickPos = dfTouchJoystick.GetJoystickPosition (lookAtJoystickID);
		//Camera tilt.
		float cameraTiltTarget;
		cameraTiltTarget = joystickPos.x; // Input.GetAxis("Mouse X");
		Vector3 velocity = transform.root.position - lastPosition;
		lastPosition = transform.root.position;
		forwardSpeed = transform.InverseTransformDirection(velocity).z;
		cameraTiltTarget *= -forwardSpeed * 60.0f * cameraTiltMultiplier ;
		cameraTiltTarget = Mathf.Clamp(cameraTiltTarget, -30.0f, 30.0f);
		cameraTilt = Mathf.Lerp(cameraTilt, cameraTiltTarget, Time.deltaTime * 3.0f);
		Vector3 angles = transform.localRotation.eulerAngles;
		if (health > 0) {
			transform.localRotation = Quaternion.Euler(angles.x, angles.y, cameraTilt);
		}
		//Vertical aim.
		verticalAim -= joystickPos.y * Time.deltaTime * 100.0f; // Input.GetAxis("Mouse Y") * Time.deltaTime * 100.0f;
		verticalAim = Mathf.Clamp(verticalAim, -40.0f, 50.0f);
		float crouchCameraYOffset = 0.0f; //Camera crouch;
		if (crouchControllerScript != null) {
			crouchCameraYOffset = -crouchControllerScript.globalCrouchBlend * 0.2f;
		}
		if (health <= 0) {
			verticalAim = 0.0f;
			crouchCameraYOffset = 0.0f;
		}
		if (health>0) {
			angles = transform.localRotation.eulerAngles;
			transform.localRotation = Quaternion.Euler(Mathf.LerpAngle(transform.localRotation.eulerAngles.x, verticalAim, Time.deltaTime * 5.0f), angles.y, angles.z);
		}
		//Local position.
		if (verticalAim > 0) {
			positionOffset.y = verticalAim * 0.03f;
		} else {
			positionOffset.y = verticalAim * 0.02f;
		}
	
		positionOffset.y += crouchCameraYOffset;
		if (health > 0) {
			transform.localPosition = Vector3.Lerp(transform.localPosition, localPosition + positionOffset, Time.deltaTime * 5.0f);
		}
		//Death Camera.
		if (health <= 0) {
			Vector3 spineRelativePos = spine2.position - transform.position;
			Quaternion lookSpineRotation  = Quaternion.LookRotation(spineRelativePos);
			transform.rotation = Quaternion.Slerp(transform.rotation, lookSpineRotation,Time.deltaTime * 3.0f);
			transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(2.0f, 3.0f, 0.0f), Time.deltaTime * 3.0f);
		}
	}
}
