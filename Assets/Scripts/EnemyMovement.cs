using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyMovement : MonoBehaviour {
	private float walkingSpeed = 0.8f;
	private float runningSpeed = 5.0f;
	private float fireWalkingSpeed = 2.0f;
	private float fireRunningSpeed = 5.0f;

	private Dictionary<int, float> dicSpeed;
	private Animator animator;
	private WayPoints wayPointsScript;

	// Use this for initialization
	void Start () {
		animator = GetComponentInChildren<Animator> ();
		wayPointsScript = GetComponent<WayPoints> ();

		dicSpeed = new Dictionary<int, float>();
		dicSpeed.Add (Animator.StringToHash ("action.walking"), walkingSpeed);
		dicSpeed.Add (Animator.StringToHash ("action.fire_walking"), fireWalkingSpeed);
		dicSpeed.Add (Animator.StringToHash ("action.run"), runningSpeed);
		dicSpeed.Add (Animator.StringToHash ("action.fire_running"), fireRunningSpeed);
	}

	// Update is called once per frame
	void Update () {
		AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo (0);
		if (info.nameHash == Animator.StringToHash ("action.walking") && wayPointsScript != null) {
			// 순찰코스가 존재하는 경우 다음번 코스에로 회전
			Transform[] points = wayPointsScript.points;
			int curIndex = wayPointsScript.curIndex;
			if (curIndex < points.Length) {
				Vector3 target = new Vector3 (points [curIndex].position.x, transform.position.y, points [curIndex].position.z);
				transform.LookAt (target);
			}
		}
		Vector3 moveDirection = transform.forward;
		float speed = (dicSpeed.ContainsKey (info.nameHash)) ? dicSpeed [info.nameHash] : 0.0f;
		moveDirection *= speed;
		Vector3 motion = moveDirection * Time.deltaTime;
		transform.position += motion;
	}
}
