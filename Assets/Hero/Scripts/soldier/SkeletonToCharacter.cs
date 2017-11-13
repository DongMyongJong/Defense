using UnityEngine;
using System.Collections;

public class SkeletonToCharacter : MonoBehaviour {
	public string rootLocation = "soldierCharacter/Bip01";
	public string pelvisLocation = "soldierCharacter/Bip01/Bip01 Pelvis";
	public string spineRootLocation = "soldierCharacter/Bip01/Bip01 Pelvis/Bip01 Spine"; 
	public string stomachLocation = "soldierCharacter/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1";
	public string torsoLocation = "soldierCharacter/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2";
	public string neckLocation = "soldierCharacter/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck"; 
	public string headLocation = "soldierCharacter/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 Head";
	public string rightThighLocation = "soldierCharacter/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 R Thigh";
	public string rightCalfLocation = "soldierCharacter/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 R Thigh/Bip01 R Calf";    
	public string rightFootLocation = "soldierCharacter/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 R Thigh/Bip01 R Calf/Bip01 R Foot";    
	public string rightToeLocation = "soldierCharacter/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 R Thigh/Bip01 R Calf/Bip01 R Foot/Bip01 R Toe0";     
	public string leftThighLocation = "soldierCharacter/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 L Thigh";     
	public string leftCalfLocation = "soldierCharacter/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 L Thigh/Bip01 L Calf";   
	public string leftFootLocation = "soldierCharacter/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 L Thigh/Bip01 L Calf/Bip01 L Foot";    
	public string leftToeLocation = "soldierCharacter/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 L Thigh/Bip01 L Calf/Bip01 L Foot/Bip01 L Toe0";    
	public string rightClavicleLocation = "soldierCharacter/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 R Clavicle";   
	public string rightUpperArmLocation = "soldierCharacter/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm";    
	public string rightForearmLocation = "soldierCharacter/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm";     
	public string rightHandLocation = "soldierCharacter/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand";    
	public string leftClavicleLocation = "soldierCharacter/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 L Clavicle";   
	public string leftUpperArmLocation = "soldierCharacter/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 L Clavicle/Bip01 L UpperArm";    
	public string leftForearmLocation = "soldierCharacter/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm";     
	public string leftHandLocation = "soldierCharacter/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand";  
	
	private Transform root;
	private Transform pelvis;
	private Transform spineRoot;
	private Transform stomach;
	private Transform torso;
	private Transform neck;
	private Transform head;
	private Transform rightThigh;
	private Transform rightCalf;
	private Transform rightFoot;
	private Transform rightToe;
	private Transform leftThigh;
	private Transform leftCalf;
	private Transform leftFoot;
	private Transform leftToe;
	private Transform rightClavicle;
	private Transform rightUpperArm;
	private Transform rightForearm;
	private Transform rightHand;
	private Transform leftClavicle;
	private Transform leftUpperArm;
	private Transform leftForearm;
	private Transform leftHand;
	
	private Transform rootSource;
	private Transform pelvisSource;
	private Transform spineRootSource;
	private Transform stomachSource;
	private Transform torsoSource;
	private Transform neckSource;
	private Transform headSource;
	private Transform rightThighSource;
	private Transform rightCalfSource;
	private Transform rightFootSource;
	private Transform rightToeSource;
	private Transform leftThighSource;
	private Transform leftCalfSource;
	private Transform leftFootSource;
	private Transform leftToeSource;
	private Transform rightClavicleSource;
	private Transform rightUpperArmSource;
	private Transform rightForearmSource;
	private Transform rightHandSource;
	private Transform leftClavicleSource;
	private Transform leftUpperArmSource;
	private Transform leftForearmSource;
	private Transform leftHandSource;
	
	// Use this for initialization
	void Start () {
		//Source.
		rootSource = transform.Find("smoothWorldPosition/soldierSkeleton/Bip01");
		pelvisSource = transform.Find("smoothWorldPosition/soldierSkeleton/Bip01/Bip01 Pelvis");
		spineRootSource  = transform.Find("smoothWorldPosition/soldierSkeleton/Bip01/Bip01 Pelvis/Bip01 Spine"); 
		stomachSource  = transform.Find("smoothWorldPosition/soldierSkeleton/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1");
		torsoSource  = transform.Find("smoothWorldPosition/soldierSkeleton/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2");
		neckSource  = transform.Find("smoothWorldPosition/soldierSkeleton/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck"); 
		headSource = transform.Find("smoothWorldPosition/soldierSkeleton/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 Head");
		rightThighSource = transform.Find("smoothWorldPosition/soldierSkeleton/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 R Thigh");
		rightCalfSource = transform.Find("smoothWorldPosition/soldierSkeleton/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 R Thigh/Bip01 R Calf");    
		rightFootSource = transform.Find("smoothWorldPosition/soldierSkeleton/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 R Thigh/Bip01 R Calf/Bip01 R Foot");    
		rightToeSource = transform.Find("smoothWorldPosition/soldierSkeleton/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 R Thigh/Bip01 R Calf/Bip01 R Foot/Bip01 R Toe0");     
		leftThighSource = transform.Find("smoothWorldPosition/soldierSkeleton/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 L Thigh");     
		leftCalfSource = transform.Find("smoothWorldPosition/soldierSkeleton/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 L Thigh/Bip01 L Calf");   
		leftFootSource = transform.Find("smoothWorldPosition/soldierSkeleton/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 L Thigh/Bip01 L Calf/Bip01 L Foot");    
		leftToeSource = transform.Find("smoothWorldPosition/soldierSkeleton/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 L Thigh/Bip01 L Calf/Bip01 L Foot/Bip01 L Toe0");    
		rightClavicleSource = transform.Find("smoothWorldPosition/soldierSkeleton/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 R Clavicle");   
		rightUpperArmSource = transform.Find("smoothWorldPosition/soldierSkeleton/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm");    
		rightForearmSource = transform.Find("smoothWorldPosition/soldierSkeleton/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm");     
		rightHandSource = transform.Find("smoothWorldPosition/soldierSkeleton/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand");    
		leftClavicleSource = transform.Find("smoothWorldPosition/soldierSkeleton/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 L Clavicle");   
		leftUpperArmSource = transform.Find("smoothWorldPosition/soldierSkeleton/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 L Clavicle/Bip01 L UpperArm");    
		leftForearmSource = transform.Find("smoothWorldPosition/soldierSkeleton/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm");     
		leftHandSource = transform.Find("smoothWorldPosition/soldierSkeleton/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand");    	
		//Target.
		root = transform.Find(rootLocation);
	 	pelvis = transform.Find(pelvisLocation);
	 	spineRoot = transform.Find(spineRootLocation);
	 	stomach = transform.Find(stomachLocation);
	 	torso = transform.Find(torsoLocation);
	 	neck = transform.Find(neckLocation);
	 	head = transform.Find(headLocation);
	  	rightThigh = transform.Find(rightThighLocation);
	  	rightCalf = transform.Find(rightCalfLocation);
	  	rightFoot = transform.Find(rightFootLocation);
	  	rightToe = transform.Find(rightToeLocation);
	 	leftThigh = transform.Find(leftThighLocation);
	 	leftCalf = transform.Find(leftCalfLocation);
	 	leftFoot = transform.Find(leftFootLocation);
	 	leftToe = transform.Find(leftToeLocation);
	 	rightClavicle = transform.Find(rightClavicleLocation);
	 	rightUpperArm = transform.Find(rightUpperArmLocation);
	 	rightForearm = transform.Find(rightForearmLocation );
	 	rightHand  = transform.Find(rightHandLocation);
	 	leftClavicle = transform.Find(leftClavicleLocation);
	 	leftUpperArm = transform.Find(leftUpperArmLocation );
	 	leftForearm = transform.Find(leftForearmLocation);
	 	leftHand = transform.Find(leftHandLocation);
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void LateUpdate() {
		//Check if something was deleted. Assume if Bip01 was deleted everything is deleted. (For LOD)
		if (root == null) {
			root = transform.Find(rootLocation);
			spineRoot = transform.Find(spineRootLocation);
			stomach = transform.Find(stomachLocation);
			torso = transform.Find(torsoLocation);
			neck = transform.Find(neckLocation);
			head = transform.Find(headLocation);
			rightThigh = transform.Find( rightThighLocation);
			rightCalf = transform.Find( rightCalfLocation);
			rightFoot = transform.Find( rightFootLocation);
			rightToe = transform.Find( rightToeLocation);
			leftThigh = transform.Find(leftThighLocation);
			leftCalf = transform.Find(leftCalfLocation);
			leftFoot = transform.Find(leftFootLocation);
			leftToe = transform.Find(leftToeLocation);
			rightClavicle = transform.Find(rightClavicleLocation);
			rightUpperArm = transform.Find(rightUpperArmLocation);
			rightForearm = transform.Find(rightForearmLocation);
			rightHand = transform.Find(rightHandLocation);
			leftClavicle = transform.Find(leftClavicleLocation);
			leftUpperArm = transform.Find(leftUpperArmLocation);
			leftForearm = transform.Find(leftForearmLocation);
			leftHand = transform.Find(leftHandLocation);
		}
	
		//Match Rotation.
		if (root != null) {
			root.rotation = rootSource.rotation;
			root.position = rootSource.position;
		}
		if (spineRoot != null) {
			spineRoot.rotation = spineRootSource.rotation;
		}
		if (stomach != null) {
			stomach.rotation = stomachSource.rotation;
		}
		if (torso != null) {
			torso.rotation = torsoSource.rotation;
		}
		if (neck != null) {
			neck.rotation = neckSource.rotation;
		}
		if (head != null) {
			head.rotation = headSource.rotation;
		}
		if (rightThigh != null) {
			rightThigh.rotation =  rightThighSource.rotation;
		}
		if (rightCalf != null) {
			rightCalf.rotation =  rightCalfSource.rotation;
		}
		if (rightFoot != null) {
			rightFoot.rotation =  rightFootSource.rotation;
		}
		if (rightToe != null) {
			rightToe.rotation =  rightToeSource.rotation;
		}
		if (leftThigh != null) {
			leftThigh.rotation = leftThighSource.rotation;
		}
		if(leftCalf != null){
			leftCalf.rotation = leftCalfSource.rotation;
		}
		if (leftFoot != null) {
			leftFoot.rotation = leftFootSource.rotation;
		}
		if (leftToe != null) {
			leftToe.rotation = leftToeSource.rotation;
		}
		if (rightClavicle != null) {
			rightClavicle.rotation = rightClavicleSource.rotation;
		}
		if (rightUpperArm != null) {
			rightUpperArm.rotation = rightUpperArmSource.rotation;
		}
		if (rightForearm != null) {
			rightForearm.rotation = rightForearmSource.rotation;
		}
		if (rightHand != null) {
			rightHand.rotation = rightHandSource.rotation;
		}
		if (leftClavicle != null) {
			leftClavicle.rotation = leftClavicleSource.rotation;
		}
		if (leftUpperArm != null) {
			leftUpperArm.rotation = leftUpperArmSource.rotation;
		}
		if (leftForearm != null) {
			leftForearm.rotation = leftForearmSource.rotation;
		}
		if (leftHand != null) {
			leftHand.rotation = leftHandSource.rotation;
		}
	}
}
