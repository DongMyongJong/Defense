using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]

public class CrouchController : MonoBehaviour {
	public float crouchSpeedMultiplier = 0.5f;
	public float crouchTogglingTime = 0.1f;
	public float globalCrouchBlend; //0 is standing up, 1 is crouching.
	
	private float globalCrouchBlendTarget;
	private float globalCrouchBlendVelocity;
	private bool disable;

	public bool triggered = false;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (triggered){
			if(!disable){
				if (globalCrouchBlend < 0.5f) {
					globalCrouchBlendTarget = 1.0f;
				} else {
					globalCrouchBlendTarget = 0.0f;
				}
			}
			disable = true;
		}
		else{
			disable = false;
		}
		globalCrouchBlend = Mathf.SmoothDamp(globalCrouchBlend, globalCrouchBlendTarget, ref globalCrouchBlendVelocity, crouchTogglingTime);
	}
}
