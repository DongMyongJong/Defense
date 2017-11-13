using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAnimation : MonoBehaviour {
	
	public AnimationCurve hitBlendCurve;
	public Transform head;

	private Animator animator;

	private const float bulletDistance = 20.0f; // 총알의 유효사거리
	private const float buttDistance = 2.0f; // 총탁으로 타격하는 거리, 이 거리안에 있으면 달려가서 총탁으로 타격

	private const float starePeriod = 5.0f; // 주변을 주시하는 시간

	private float headAngle = 0.0f; // 현재 머리가 돌아간 각
	private const float headPeriod = 2.5f; // 머리가 한번 두리번거리는 시간
	private const float headRotationAngle = 120.0f; // 머리가 돌아가는 각

	private float eyeDistance = 200.0f; // 보임거리
	private float eyeHorzAngle = 120.0f; // 수평방향의 시각
	private float eyeVertAngle = 120.0f; // 수직방향의 시각

	private bool foundHero = false;
	private bool dead = false;

	private Health healthScript;

	private EnemyActionConditioner actionConditioner;
	private EnemyActionPlayer actionPlayer;
	private float bornHealth;
	private Transform target;

	// Use this for initialization
	void Start () {
		healthScript = GetComponent<Health> ();
		bornHealth = healthScript.health;
		animator = GetComponentInChildren<Animator> ();

		actionConditioner = new EnemyActionConditioner(animator);
		actionPlayer = new EnemyActionPlayer (animator, actionConditioner);

		WayPoints wayPointsScript = GetComponent<WayPoints> ();
		if (wayPointsScript != null) {
			actionPlayer.StartActionName = "walking";
		} else {
			actionPlayer.StartActionName = "idle";
		}
	}

	private bool hitted() {
		return (Time.time < healthScript.GetLastHitTime () + starePeriod && healthScript.health < bornHealth);
	}

	private bool findHero(Vector3 angles, float dxRot, float dyRot) {
		Quaternion rot = Quaternion.Euler(angles.x + dxRot, angles.y + dyRot, angles.z);
		Vector3 direction = rot * head.forward;
		Ray ray = new Ray(head.position, direction);
		RaycastHit hit = new RaycastHit();
		bool result = false;
		if (Physics.Raycast(ray, out hit, eyeDistance)){
			target = hit.transform.root;
			if (target.tag.Equals("hero")) { // 두리번거리다가 주인공을 찾은 경우
				result = true;
			}
		}
		return result;
	}
	
	void LateUpdate() {
		if (dead) {
			return;
		}
		if (hitted () && !foundHero) {
			// 현재 총탄을 맞았으면 두리번거리면서 주인공 찾기
			headAngle += 360.0f / headPeriod * Time.deltaTime;
			float rotationY = headRotationAngle / 2.0f * Mathf.Sin (Mathf.Deg2Rad * headAngle);
			Vector3 angles = head.rotation.eulerAngles;
			head.rotation = Quaternion.Euler (angles.x, angles.y + rotationY, angles.z);
		}
		if (!foundHero) {
			Vector3 angles = Quaternion.Euler(head.forward).eulerAngles;
			// 주인공 탐색
			for (float y = 0.0f; y < eyeHorzAngle / 2.0f; y += 10.0f) {
				for (float x = 0.0f; x < eyeVertAngle / 2.0f; x += 10.0f) {
					foundHero = findHero(angles, x, y);
					if (foundHero) {
						return;
					}
					foundHero = findHero(angles, -x, -y);
					if (foundHero) {
						return;
					}
				}
			}
		}
		actionPlayer.refresh ();
	}
	// Update is called once per frame
	void Update () {
		if (dead || healthScript == null) {
			return;
		}
		if (hitted ()) {
			float health = healthScript.GetHealth();
			if (health > 0) {
				float timeAfterHit = Time.time - healthScript.GetLastHitTime();
				float hitBlend = hitBlendCurve.Evaluate(timeAfterHit);
				actionPlayer.playHitEffect(healthScript.GetHitDirection(), hitBlend);
				if (!foundHero) {
					actionConditioner.ToStanding = true;
				}
			} else {
				// 죽는 동작 구현, 모든 콜라이더 disable
				GetComponent<CharacterController>().enabled = false;
				TriggerChildrenCollider children = GetComponent<TriggerChildrenCollider>();
				foreach(Collider c in children.childrenColliderList) {
					c.enabled = false;
				}
				actionPlayer.playDieEffect(healthScript.GetHitDirection());
				dead = true;
			}
		} else {
			if (!foundHero) {
				actionConditioner.changeWithStringExclusive(string.Format("to{0}", StringUtils.pascalCasing(actionPlayer.StartActionName)), true);
			}
		}
		if (foundHero) {
			Quaternion rot = Quaternion.FromToRotation (transform.forward, target.position - transform.position);
			transform.Rotate (Vector3.up * rot.eulerAngles.y);
			float distance = Vector3.Distance(transform.position, target.position);
			int action = animator.GetCurrentAnimatorStateInfo (0).nameHash;
			if (distance > bulletDistance) { // 현재 거리가 총알의 유효사거리보다 멀 때
				if (action == Animator.StringToHash("action.walking") ||
				    action == Animator.StringToHash("action.idle")) {
					actionPlayer.registerSimpleTransition(action, "toRun", "action.run");
				} else if (action == Animator.StringToHash("action.fire_laying")) {
					actionPlayer.registerRunningFromFireLaying();
				} else if (action == Animator.StringToHash("action.fire_squat")) {
					actionPlayer.registerRunningFromFireSquat();
				} else if (action == Animator.StringToHash("action.fire_belt")) {
					actionPlayer.registerSimpleTransition(action, "toRun", "action.run");
				}
			} else if (distance < buttDistance) {
				if (action == Animator.StringToHash("action.walking") ||
				    action == Animator.StringToHash("action.run") ||
				    action == Animator.StringToHash("action.fire_walking") ||
				    action == Animator.StringToHash("action.fire_running")) {
					actionPlayer.registerSimpleTransition(action, "toButt", "action.butt_kick");
				}
			} else { // 사거리안에 들어왔을 때
				if (action == Animator.StringToHash("action.walking")) {
					// 0: 그냥 사격, 1: 무릎끓고 사격, 2: 누워서 사격
					int no = Random.Range(1, 100) % 13 % 3;
					switch(no) {
					case 0:
						actionPlayer.registerSimpleTransition(action, "toFire", "action.fire_walking");
						break;
					case 1:
						actionPlayer.registerFireSquatFromWalking();
						break;
					case 2:
						actionPlayer.registerFireLayingFromWalking();
						break;
					default:
						break;
					}
				} else if (action == Animator.StringToHash("action.run")) {
					// 0: 총을 총끈에 의지하여 달리면서 사격, 1: 총을 어깨 우에 들고 달리면서 사격
					// 2: 누워서 가격, 3: 끓어앉아 사격
					int no = Random.Range(1, 100) % 13 % 4;
					switch(no) {
					case 0:
						actionPlayer.registerFireOnBeltFromRunning();
						break;
					case 1:
						actionPlayer.registerFireFromRunning();
						break;
					case 2:
						actionPlayer.registerFireLayingFromRunning();
						break;
					case 3:
						actionPlayer.registerFireSquatFromRunning();
						break;
					default:
						break;
					}
				} else if (action == Animator.StringToHash("action.butt_kick")) {
					actionPlayer.registerSimpleTransition(action, "toRun", "action.run");
				}
			} 
		}
	}
}
