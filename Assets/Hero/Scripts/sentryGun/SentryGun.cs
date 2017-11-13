using UnityEngine;
using System.Collections;

public class SentryGun : MonoBehaviour {
	public string[] targetRootName;
	public GameObject explosionPrefab;
	public GameObject blackSmokePrefab;
	private float rotatorAngleScan = 35.0f;
	private float rotatorAcceleration = 5.0f;
	private bool firing = false;
	private BulletTraceGenerator bulletTraceGeneratorScript;
	private MuzzleFlashGenerator muzzleFlashGeneratorScript;
	private SpinningTurret spinningTurretScript;
	private Transform gunBase;
	private Transform rotator;
	private Transform pitch;
	private Transform barrel;
	private Transform bulletTraceGenerator;
	private Transform muzzleFlashGenerator;
	private Transform laserSight;
	private LaserSight laserSightScript;
	private bool exploded = false;
	//Ai
	private bool foundTarget;
	private float lastTargetfoundTime;
	private float targetBufferTime;
	private Transform targetTransform;
	private float sentryHealth;
	private Health healthScript;
	//Rotator.
	private float rotatorSpeed;
	private float rotatorMaxSpeed = 60.0f;
	private float rotatorAngleTarget;
	private float rotatorDirection = 1.0f;
	private float rotatorChangeDirectionTime;
	private float rotatorChangeDirectionDelay = 1.0f;
	//Pitch.
	private float pitchAngle;
	private float pitchVelocity;
	private float pitchTarget;
	private float deadPitchAngle = 30.0f;
	
	// Use this for initialization
	void Start () {
		gunBase = transform.Find("sentryGunBase");
		rotator = gunBase.Find("sentryGunRotator");
		pitch = rotator.Find("sentryGunPitch");
		barrel = pitch.Find("sentryGunBarrel");
		bulletTraceGenerator = barrel.Find("bulletTraceGenerator");
		muzzleFlashGenerator = barrel.Find("muzzleFlashGenerator");
		spinningTurretScript = barrel.GetComponent<SpinningTurret>();
		bulletTraceGeneratorScript = bulletTraceGenerator.GetComponent<BulletTraceGenerator>();
		muzzleFlashGeneratorScript = muzzleFlashGenerator.GetComponent<MuzzleFlashGenerator>();
		laserSight = pitch.Find("laserSight");
		laserSightScript = laserSight.GetComponent<LaserSight>();
		targetBufferTime = 2.0f;
		healthScript = GetComponent<Health>();
	}
	
	// Update is called once per frame
	void Update () {
		float sentryHealth = healthScript.health;
		float barrelSpeed = spinningTurretScript.speed;
		float maxBarrelSpeed  = spinningTurretScript.maxSpeed;
		if (barrelSpeed > maxBarrelSpeed * 0.8f) {
			bulletTraceGeneratorScript.on = firing;
			muzzleFlashGeneratorScript.on = firing;
		} else {
			bulletTraceGeneratorScript.on = false;
			muzzleFlashGeneratorScript.on = false;	
		}
		spinningTurretScript.on = firing;
		if (bulletTraceGeneratorScript.on) {
			rigidbody.AddForceAtPosition(bulletTraceGenerator.up * Time.deltaTime * Random.value * 1900.0f, bulletTraceGenerator.position) ;
		}
		if (sentryHealth <= 0 && exploded == false) {//Explosion.
			exploded = true;
			Instantiate(explosionPrefab, pitch.position, pitch.rotation);
			rigidbody.AddForce(Vector3.up * 400.0f);
			GameObject newBlackSmoke = Instantiate(blackSmokePrefab, pitch.position, pitch.rotation) as GameObject;
			newBlackSmoke.transform.parent = transform;
		}
		if (barrel.position.y < transform.position.y + 0.2f || sentryHealth == 0) {
			collider.enabled = false;
			gunBase.collider.enabled = true;
			gunBase.collider.isTrigger = false;
			rotator.collider.enabled = true;
			rotator.collider.isTrigger = false;
			pitch.collider.enabled = true;
			pitch.collider.isTrigger = false;
			barrel.collider.enabled = true;
			barrel.collider.isTrigger = false;
			firing = false; //Deactivate if laying on the ground.
			laserSightScript.on = false;
			rotatorSpeed = Mathf.Lerp(rotatorSpeed, 0.0f, Time.deltaTime * rotatorAcceleration);
		} else {
			sentryAI();
		}
		Vector3 angles = rotator.localRotation.eulerAngles;
		rotator.localRotation = Quaternion.Euler(angles.x, angles.y, angles.z + rotatorSpeed * Time.deltaTime);
		//Pitch Angle.
		if (sentryHealth > 0) { //Alive.
			pitchAngle = 4.0f;
		} else {
			pitchTarget = deadPitchAngle; //Dead.
			if (pitchAngle > pitchTarget + 1) {
				pitchVelocity -= 3.0f * Time.deltaTime;
			}
			if (pitchAngle < pitchTarget - 1) {
				pitchVelocity += 3.0f * Time.deltaTime;
			}
			pitchVelocity = Mathf.Lerp(pitchVelocity, 0.0f, Time.deltaTime * 5.0f);
			pitchAngle += pitchVelocity;
		}
		if (pitchAngle > deadPitchAngle) {
			pitchVelocity *= -1;
		}
		angles = pitch.localRotation.eulerAngles;
		pitch.localRotation = Quaternion.Euler(pitchAngle, angles.y, angles.z);
	}
	
	public void sentryAI(){
		//Check for targets.
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast(bulletTraceGenerator.position, bulletTraceGenerator.forward, out hit)) {
			bool isTarget = false;
			for (int i = 0; i < targetRootName.Length; i++) {
				if (hit.transform.root.name == targetRootName[i]) {
					isTarget = true; //Check if what's ahead correspond to the target list.
					break;
				}
			}
			if (isTarget) {
				foundTarget = true; //If what's ahead is a target.
				lastTargetfoundTime = Time.time;
				if (targetTransform == null) {
					targetTransform = hit.transform; //This is the target.
				}
			}
		}
		//If target has been destroyed.
		if (targetTransform == null) {
			foundTarget = false;
			firing = false;
		}
		//Target.
		if (foundTarget) {
			firing = true;
			Vector3 targetPositionNoHeight = targetTransform.position;
			targetPositionNoHeight.y = rotator.position.y;
			Vector3 targetDirection = targetPositionNoHeight - rotator.position;
			float angleToTarget = Vector3.Angle(targetDirection, -rotator.up);
			float targetSide = rotator.InverseTransformPoint(targetPositionNoHeight).x;
			if (targetSide > 0) {
				rotatorSpeed = Mathf.Lerp(rotatorSpeed, rotatorMaxSpeed, Time.deltaTime * rotatorAcceleration);
			} else {
				rotatorSpeed = Mathf.Lerp(rotatorSpeed, -rotatorMaxSpeed, Time.deltaTime * rotatorAcceleration);
			}
		}
		//No target.
		if (Time.time > lastTargetfoundTime + targetBufferTime) {
			foundTarget = false;
			targetTransform = null;
			firing = false;
			float getRotatorAngle = rotator.localRotation.eulerAngles.z;
			if (getRotatorAngle > 180.0f) {
				getRotatorAngle -= 360.0f;
			}
			if (rotatorDirection == 0) { //Choose new direction.
				rotatorChangeDirectionTime = Time.time + rotatorChangeDirectionDelay;
				if (getRotatorAngle > 0) {
					rotatorDirection = -1.0f;
				} else {
					rotatorDirection = 1.0f;
				}
			}
			if (Time.time < rotatorChangeDirectionTime) {
				rotatorSpeed = Mathf.Lerp(rotatorSpeed, 0, Time.deltaTime * rotatorAcceleration);//Hold still.
			}
			if (rotatorDirection > 0 && Time.time > rotatorChangeDirectionTime) {//Scan right.
				if (getRotatorAngle > rotatorAngleScan * 0.95f) {
					rotatorDirection = 0;
				}
				rotatorSpeed = Mathf.Lerp(rotatorSpeed,rotatorMaxSpeed, Time.deltaTime * rotatorAcceleration);
			}
			if (rotatorDirection < 0 && Time.time > rotatorChangeDirectionTime) {//Scan left.
				if (getRotatorAngle < -rotatorAngleScan * 0.95f) {
					rotatorDirection = 0;
				}
				rotatorSpeed = Mathf.Lerp(rotatorSpeed,-rotatorMaxSpeed, Time.deltaTime * rotatorAcceleration);			
			}
		} else {
			rotatorSpeed = Mathf.Lerp(rotatorSpeed, 0.0f, Time.deltaTime * rotatorAcceleration);
		}	
	}
}
