using UnityEngine;
using System.Collections;

public class SoldierAim : MonoBehaviour {
	public Transform soldierCamera;
	public Vector3 headRotationFix = new Vector3(180,90,90);
	public Transform crosshair;
	
	private Vector3 target;
	private Vector3 targetTarget; //The target for Mathf.Lerp to the target.
	private Transform rightUpperArm;
	private Transform rightClavicle;
	private Transform leftUpperArm;
	private Transform leftClavicle;
	private Transform leftForearm;
	private Transform leftFinger;
	private Transform spine1;
	private Transform spine2;
	private Transform head;
	private Transform neck;
	private float cameraYRotation;
	private float cameraPitch;
	private float torsoOffsetAngle;
	private float torsoOffsetPitch;
	private float leanHead = 0.0f;
	private SoldierAnimation soldierAnimationScript;
	private InverseKinematics leftArmIkScript;
	private Transform ikArm;
	private	Transform ikUpperArm;
	private	Transform ikForearm;
	private float transition;
	private float transitionTarget; // 0 means normal animation, 1 means complete aim.
	private float transition2;
	private float transition2Target; // 0 means normal animation, 1 means complete aim. For head and torso.
	
	// Use this for initialization
	void Start () {
		spine1 =  transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1");
		spine2 =  spine1.Find("Bip01 Spine2");
		neck = spine2.Find("Bip01 Neck");
		rightClavicle = neck.Find("Bip01 R Clavicle");
		rightUpperArm = rightClavicle.Find("Bip01 R UpperArm");
		leftClavicle = neck.Find("Bip01 L Clavicle");
		leftUpperArm = leftClavicle.Find("Bip01 L UpperArm");
		leftForearm = leftUpperArm.Find("Bip01 L Forearm");
		leftFinger = leftForearm.Find("Bip01 L Hand/Bip01 L Finger2/Bip01 L Finger21");
		head = neck.Find("Bip01 Head");
		soldierAnimationScript = GetComponent<SoldierAnimation>();
		ikArm = leftUpperArm.Find("ikArm");
		ikArm.parent = transform.root;
		ikArm.position = leftUpperArm.position;
		ikUpperArm = ikArm.Find("upperArm");
		ikForearm = ikUpperArm.Find("elbow");
		leftArmIkScript = ikArm.GetComponent<InverseKinematics>();
		//Debug.Log(ikArm + " - " + ikUpperArm + " - " + ikForearm);
		float upperArmLength = Vector3.Distance(leftUpperArm.position, leftForearm.position);
		float forearmLength = Vector3.Distance(leftForearm.position, leftFinger.position);	
		ikForearm.parent = null;
		ikUpperArm.localScale = new Vector3(upperArmLength, upperArmLength, upperArmLength);
		ikForearm.localScale = new Vector3(forearmLength, forearmLength, forearmLength);
		ikForearm.parent = ikUpperArm;
		ikForearm.localPosition = Vector3.zero;
		ikForearm.position += ikUpperArm.forward * upperArmLength;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void LateUpdate() {
		//Target.
		Ray targetRay = soldierCamera.camera.ViewportPointToRay(crosshair.position);
		float targetRayDistance = 300.0f;
		RaycastHit targetHit = new RaycastHit();
		targetTarget = targetRay.origin + targetRay.direction * targetRayDistance;
		if(Physics.Raycast(targetRay, out targetHit, targetRayDistance)){
			float targetHitDistance = Vector3.Distance(targetHit.point, soldierCamera.position);
			float soldierDistance = Vector3.Distance(transform.position, soldierCamera.position);
			if (targetHitDistance > soldierDistance * 1.0f){
				Ray targetRayUpperArm = new Ray(rightUpperArm.position, targetHit.point - rightUpperArm.position);
				targetTarget = rightUpperArm.position + (targetHit.point - rightUpperArm.position).normalized * targetRayDistance;
			}
		}
		target = Vector3.Lerp(target,targetTarget,Time.deltaTime * 10.0f);
		Transform aimAid = new GameObject("aidAim").transform; //This will be used to aid the body parts aim in the rigth direction.
		//Transition 1(arms).
		transitionTarget = 1.0f;
		transitionTarget *= 1 - soldierAnimationScript.landingBlend; //Landing.
		transitionTarget *= 1 - soldierAnimationScript.fallingBlend;//Falling.
		transitionTarget *= 1 - soldierAnimationScript.sprintBlend;// Sprinting.
		transitionTarget *= 1 - soldierAnimationScript.crouchSprintBlend; //Crouch sprinting.
		transitionTarget *= 1 - soldierAnimationScript.hitBlend; //Getting hit.
		transitionTarget *= 1 - soldierAnimationScript.dieBlend; //Dying.
		//Transition 2(spine and head)
		transition2Target = 1.0f;
		transition2Target *= 1 - soldierAnimationScript.hitBlend; //Getting hit.
		transition2Target *= 1 - soldierAnimationScript.dieBlend; //Dying.	
		//Store pre-rotations.
		Quaternion rightUpperArmLocalRotation = rightUpperArm.localRotation;
		Quaternion leftUpperArmLocalRotation = leftUpperArm.localRotation;
		Quaternion leftForearmLocalRotation  = leftForearm.localRotation;
		Quaternion headLocalRotation = head.localRotation;
		Quaternion spine1LocalRotation  = spine1.localRotation;
		Quaternion spine2LocalRotation = spine2.localRotation;
		//Aiming.
		float characterYRotation = transform.rotation.eulerAngles.y;//Torso.
		float spineYRotation = spine1.rotation.eulerAngles.y + 60;
		if (soldierCamera != null){
			cameraYRotation = soldierCamera.rotation.eulerAngles.y;
			cameraPitch = soldierCamera.localRotation.eulerAngles.x;
		}
		float deltaTorsoAngle = Mathf.DeltaAngle(spineYRotation, cameraYRotation);
		torsoOffsetAngle = Mathf.Lerp(torsoOffsetAngle, deltaTorsoAngle, Time.deltaTime * 15.0f);
		float deltaTorsoPitch = Mathf.DeltaAngle(0, cameraPitch);
		torsoOffsetPitch = Mathf.Lerp(torsoOffsetPitch, deltaTorsoPitch, Time.deltaTime * 15.0f);
		Vector3 angles1 = spine1.localRotation.eulerAngles;
		spine1.localRotation = Quaternion.Euler(angles1.x - torsoOffsetAngle * 0.5f, angles1.y, angles1.z - torsoOffsetPitch * 0.5f);
		Vector3 angles2 = spine2.localRotation.eulerAngles;
		spine2.localRotation = Quaternion.Euler(angles2.x - torsoOffsetAngle * 0.5f, angles2.y, angles2.z - torsoOffsetPitch * 0.5f);
		aimAid.position = rightUpperArm.position;//Right arm.
		Transform gunAim = rightUpperArm.Find("Bip01 R Forearm/Bip01 R Hand/gun/gunAim");
		aimAid.LookAt(gunAim);
		rightUpperArm.parent = aimAid;
		aimAid.LookAt(target);
		rightUpperArm.parent = rightClavicle;
		aimAid.position = leftUpperArm.position;//Upper arm.
		aimAid.LookAt(leftForearm);
		leftUpperArm.parent = aimAid;
		Transform gunGrab = rightUpperArm.Find("Bip01 R Forearm/Bip01 R Hand/gun/gunGrab");
		aimAid.LookAt(gunGrab);
		leftUpperArm.parent = leftClavicle;
		//IK. (Left arm).
		ikArm.position = leftUpperArm.position;
		Transform leftElbowTarget = spine2.Find("leftElbowTarget");
		leftArmIkScript.elbowTarget = leftElbowTarget.position;
		leftArmIkScript.target = gunGrab.position;
		leftArmIkScript.CalculateIK();
		Quaternion upperArmRotation = ikUpperArm.Find("upperArmRotation").rotation;
		Quaternion forearmRotation = ikForearm.Find("forearmRotation").rotation;
		leftUpperArm.rotation = upperArmRotation;
		leftForearm.rotation = forearmRotation;
		Destroy(aimAid.gameObject);
		if (soldierCamera != null){ //Head.
			head.LookAt(target);
			head.Rotate(headRotationFix);
			//Lean head so it doesn't intersects with the gun.
			float minHeadLeanAngle = 0.0f;
			float maxHeadLeanAngle = 120.0f;
			float fullLeanAngle = 20.0f;
			float leanHeadTarget =  0.0f;
			if (head.localRotation.eulerAngles.x > minHeadLeanAngle && head.localRotation.eulerAngles.x < maxHeadLeanAngle){
				leanHeadTarget = head.localRotation.eulerAngles.x - minHeadLeanAngle; //Angle for leaning the head.
				leanHeadTarget /= maxHeadLeanAngle;
				leanHeadTarget *= fullLeanAngle;
			}
			leanHead = Mathf.Lerp(leanHead, leanHeadTarget, Time.deltaTime * 5.0f);
			head.Rotate(0,leanHead,0);
		}
		//Transition 1.
		transition = Mathf.Lerp(transition, transitionTarget, Time.deltaTime * 5.0f);
		if(transition < 1.0){ //Only Lerp if transitionTarget is between 0 and 1.
			rightUpperArm.localRotation = Quaternion.Lerp(rightUpperArmLocalRotation, rightUpperArm.localRotation, transition);
			leftUpperArm.localRotation = Quaternion.Lerp(leftUpperArmLocalRotation, leftUpperArm.localRotation, transition);
			leftForearm.localRotation = Quaternion.Lerp(leftForearmLocalRotation, leftForearm.localRotation, transition);
			
		}
		//Transition 2.
		transition2 = Mathf.Lerp(transition2, transition2Target, Time.deltaTime * 5.0f);
		if(transition2 < 1.0){ //Only Lerp if transitionTarget is between 0 and 1.
			head.localRotation = Quaternion.Lerp(headLocalRotation, head.localRotation, transition2);
			spine1.localRotation = Quaternion.Lerp(spine1LocalRotation, spine1.localRotation, transition2);
			spine2.localRotation = Quaternion.Lerp(spine2LocalRotation, spine2.localRotation, transition2);
		}
	}
}
