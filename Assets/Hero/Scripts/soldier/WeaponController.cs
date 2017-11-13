using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SoldierMovement))]
[RequireComponent(typeof(SoldierMovement))]

public class WeaponController : MonoBehaviour {
	public string gunLocaion = "soldierCharacter/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/gun";
	public float aimSpeed = 5.0f;
	
	private bool firing = false;
	private Transform gun;
	private Transform crosshair;
	private float accuracyLoss;
	private float accuracyLossTarget;
	private float shootingAimLoss;
	private float vibratingAimLoss; //shootingAimLoss with firing vibration.
	private bool isSprinting;
	//External scripts.
	private Gun gunScript;
	private Crosshair crosshairScript;
	private SoldierMovement soldierMovementScript;
	private CrouchController crouchControllerScript;
	private Health healthScript;

	public bool triggered = false;
	
	// Use this for initialization
	void Start () {
		gun = transform.Find(gunLocaion);
		crosshair = transform.Find("crosshair");
		gunScript = gun.GetComponent<Gun>();
		//External scripts.
		crosshairScript = crosshair.GetComponent<Crosshair>();
		soldierMovementScript = GetComponent<SoldierMovement>();
		crouchControllerScript = GetComponent<CrouchController>();
		healthScript = GetComponent<Health>();
	}
	
	// Update is called once per frame
	void Update () {
		float health = 100.0f;
		if(healthScript != null) {
			health = healthScript.GetHealth();
		}
		//Input.
		bool isGrounded = soldierMovementScript.isGrounded;
		if (triggered && !isSprinting && isGrounded && health > 0) {
			firing = true;
			gunScript.firing = firing;
		} else {
			firing = false;
			gunScript.firing = firing;
		}
		//Accuracy.
		float aimCrouchMultiplier	= 1 + crouchControllerScript.globalCrouchBlend * 10.0f;
		float turnSpeed = soldierMovementScript.turnSpeed;
		float forwardSpeed = soldierMovementScript.forwardSpeed;
		float strafeSpeed = soldierMovementScript.strafeSpeed;
		accuracyLossTarget = 1.0f;
		if (forwardSpeed > soldierMovementScript.forwardSpeedMultiplier * 1.2f) {
			isSprinting = true;
			accuracyLossTarget += 1.0f;
		} else {
			isSprinting = false;
		}
		
		if (gunScript.firing) {
			shootingAimLoss = Mathf.Lerp(shootingAimLoss, 2.0f, Time.deltaTime * 2.0f);
			crosshairScript.yOffset += Random.Range(0.0f, 0.5f) * Time.deltaTime;
			crosshairScript.xOffset += Random.Range(-0.05f, shootingAimLoss * 0.1f) * Time.deltaTime;
		} else {
			shootingAimLoss = Mathf.Lerp(shootingAimLoss, 0.0f, Time.deltaTime * 20.0f);
		}
		vibratingAimLoss = shootingAimLoss + Mathf.Sin(Time.time * 80.0f) * shootingAimLoss * 0.5f;
		accuracyLossTarget += vibratingAimLoss;
		accuracyLossTarget += Mathf.Pow(Mathf.Abs(forwardSpeed * 2.0f + strafeSpeed * 2.0f), 0.1f);
		accuracyLossTarget += Mathf.Pow(Mathf.Pow(Mathf.Abs(turnSpeed), 2.3f) / Mathf.Pow(10.0f, 4.0f), 0.35f);
		accuracyLossTarget += (1- crouchControllerScript.globalCrouchBlend) * 0.5f;
		if (accuracyLoss > accuracyLossTarget) {
			accuracyLoss = Mathf.Lerp(accuracyLoss, accuracyLossTarget, Time.deltaTime * aimSpeed * aimCrouchMultiplier * 0.5f);//Gain aim.
		} else {
			accuracyLoss = Mathf.Lerp(accuracyLoss, accuracyLossTarget, Time.deltaTime * aimSpeed);//Lose aim.
		}
		crosshairScript.accuracyLoss = accuracyLoss;
		accuracyLoss = Mathf.Max(accuracyLoss, 1.0f);
		float accuracy = 1 / accuracyLoss;
		gunScript.accuracy = accuracy;
	}
	
	public bool isFiring() {
		return firing;
	}
}
