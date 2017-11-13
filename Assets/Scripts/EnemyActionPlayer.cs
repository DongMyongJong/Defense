using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyActionPlayer {
	private Animator _animator;
	private EnemyActionConditioner _conditioner;
	private Queue<Transition> _queue;
	private string startActionName;
	
	public string StartActionName {
		get
		{
			return startActionName;
		}
		set
		{
			startActionName = value;
			play (startActionName, 0);
		}
	}

	public EnemyActionPlayer(Animator animator, EnemyActionConditioner conditioner) {
		_animator = animator;
		_conditioner = conditioner;
		_queue = new Queue<Transition> ();
	}

	public void play(string animationName, int layer) {
		_animator.Play (animationName, layer);
	}

	public void playHitEffect(Vector3 hitDirection, float weight) {
		_animator.SetLayerWeight(1, weight);
		int action = _animator.GetCurrentAnimatorStateInfo (0).nameHash;
		if (action == Animator.StringToHash ("action.combat_mode_laying") || 
		    action == Animator.StringToHash ("action.fire_laying")) {
			_animator.Play ("die_laying", 1);
		} else if (action == Animator.StringToHash ("action.combat_mode_squat") || 
		   action == Animator.StringToHash ("action.fire_squat")) {
			_animator.Play ("die_squat", 1);
		} else {
			// 히트점의 방향에 의하여 동작 구현
			// 아래의 그림에서 
			// *: 히트점이고 
			// x축: 왼쪽에서 오른쪽으로
			// y축: 머리웃방향
			// z축: 눈의 시선방향
			//        z
			//       /|\
			//        |
			//        |
			// -------*--------> x
			//        |
			//        |
			//        |
			//
			if (hitDirection.z > 0) {
				if (hitDirection.x > 0) {
					if (hitDirection.x > hitDirection.z) {
						_animator.Play ("hit_from_right", 1);
					} else {
						_animator.Play ("hit_from_front", 1);
					}
				} else {
					if (Mathf.Abs (hitDirection.x) > hitDirection.z) {
						_animator.Play ("hit_from_left", 1);
					} else {
						_animator.Play ("hit_from_front", 1);
					}
				}
			} else {
				if (hitDirection.x > 0) {
					if (hitDirection.x > Mathf.Abs (hitDirection.z)) {
						_animator.Play ("hit_from_right", 1);
					} else {
						_animator.Play ("hit_from_back", 1);
					}
				} else {
					if (hitDirection.x > hitDirection.z) {
						_animator.Play ("hit_from_back", 1);
					} else {
						_animator.Play ("hit_from_left", 1);
					}
				}
			}
		}
	}

	public void playDieEffect(Vector3 hitDirection) {
		_animator.SetLayerWeight(1, 1.0f);
		int action = _animator.GetCurrentAnimatorStateInfo (0).nameHash;
		if (action == Animator.StringToHash ("action.combat_mode_laying") ||
		    action == Animator.StringToHash("action.fire_laying")) {
			_animator.Play ("die_laying", 1);
		} else if (action == Animator.StringToHash ("action.combat_mode_squat") ||
		    action == Animator.StringToHash("action.fire_squat")) {
			_animator.Play ("die_squat", 1);
		} else {
			if (hitDirection.z > 0) {
				int no = Random.Range (1, 100) % 13 % 3;
				switch (no) {
				case 0:
					_animator.Play ("die_on_belt", 1);
					break;
				case 1:
					_animator.Play ("die_on_back", 1);
					break;
				case 2:
					_animator.Play ("die_knocked_backward", 1);
					break;
				default:
					Debug.Log (string.Format ("Unrecognized no={0}", no));
					break;
				}
			} else {
				_animator.Play ("die_knocked_forward", 1);
			}
		}
	}

	public void refresh() {
		if (_queue.Count > 0) {
			Transition t = _queue.Peek ();
			if (t.action == _animator.GetCurrentAnimatorStateInfo (0).nameHash) {
				t = _queue.Dequeue ();
				_conditioner.changeWithStringInclusive (t.conditions);
			}
		}
	}

	private string getActionName(int action) {
		if (action == Animator.StringToHash ("action.idle")) {
			return "idle";
		} else if (action == Animator.StringToHash ("action.walking")) {
			return "walking";
		} else if (action == Animator.StringToHash ("action.run")) {
			return "run";
		} else if (action == Animator.StringToHash ("action.fire_running")) {
			return "fire_running";
		} else if (action == Animator.StringToHash ("action.butt_kick")) {
			return "butt_kick";
		} else if (action == Animator.StringToHash ("action.fire_walking")) {
			return "fire_walking";
		} else if (action == Animator.StringToHash ("action.fire_belt")) {
			return "fire_belt";
		} else if (action == Animator.StringToHash ("action.combat_mode_standing")) {
			return "combat_mode_standing";
		} else if (action == Animator.StringToHash ("action.combat_mode_squat")) {
			return "combat_mode_squat";
		} else if (action == Animator.StringToHash ("action.combat_mode_laying")) {
			return "combat_mode_laying";
		} else if (action == Animator.StringToHash ("action.fire_squat")) {
			return "fire_squat";
		} else if (action == Animator.StringToHash ("action.fire_laying")) {
			return "fire_laying";
		} else {
			return "";
		}
	}

	
	public void registerSimpleTransition(string fromActionName, string condition, string toActionName) {
		if (_queue.Count == 0) {
			Transition t = new Transition (fromActionName);
			t.conditions.Add (new KeyValuePair<string, bool> (condition, true));
			_queue.Enqueue (t);
			
			t = new Transition (toActionName);
			_queue.Enqueue (t);
		}
	}
	public void registerSimpleTransition(int fromAction, string condition, string toActionName) {
		if (_queue.Count == 0) {
			Transition t = new Transition (fromAction);
			t.conditions.Add (new KeyValuePair<string, bool> (condition, true));
			_queue.Enqueue (t);
			
			t = new Transition (toActionName);
			_queue.Enqueue (t);
		}
	}
	public void registerRunningFromFireLaying() {
		if (_queue.Count == 0) {
			Transition t = new Transition ("action.fire_laying");
			t.conditions.Add (new KeyValuePair<string, bool> ("toLaying", true));
			_queue.Enqueue (t);
			
			t = new Transition ("action.combat_mode_laying");
			t.conditions.Add (new KeyValuePair<string, bool> ("toStanding", true));
			_queue.Enqueue (t);
			
			t = new Transition ("action.combat_mode_standing");
			t.conditions.Add (new KeyValuePair<string, bool> ("toRun", true));
			_queue.Enqueue (t);
			
			t = new Transition ("action.run");
			_queue.Enqueue (t);
		}
	}
	public void registerRunningFromFireSquat() {
		if (_queue.Count == 0) {
			Transition t = new Transition ("action.fire_squat");
			t.conditions.Add (new KeyValuePair<string, bool> ("toSquat", true));
			_queue.Enqueue (t);
			
			t = new Transition ("action.combat_mode_squat");
			t.conditions.Add (new KeyValuePair<string, bool> ("toStanding", true));
			_queue.Enqueue (t);
			
			t = new Transition ("action.combat_mode_standing");
			t.conditions.Add (new KeyValuePair<string, bool> ("toRun", true));
			_queue.Enqueue (t);
			
			t = new Transition ("action.run");
			_queue.Enqueue (t);
		}
	}
	public void registerFireOnBeltFromRunning() {
		if (_queue.Count == 0) {
			Transition t = new Transition ("action.run");
			t.conditions.Add (new KeyValuePair<string, bool> ("toFire", true));
			t.conditions.Add (new KeyValuePair<string, bool> ("onBelt", true));
			_queue.Enqueue (t);
			
			t = new Transition ("action.fire_belt");
			_queue.Enqueue (t);
		}
	}
	public void registerFireFromRunning() {
		if (_queue.Count == 0) {
			Transition t = new Transition ("action.run");
			t.conditions.Add (new KeyValuePair<string, bool> ("toFire", true));
			t.conditions.Add (new KeyValuePair<string, bool> ("onBelt", false));
			_queue.Enqueue (t);
			
			t = new Transition ("action.fire_running");
			_queue.Enqueue (t);
		}
	}
	public void registerFireLayingFromRunning() {
		if (_queue.Count == 0) {
			Transition t = new Transition ("action.run");
			t.conditions.Add (new KeyValuePair<string, bool> ("toStanding", true));
			_queue.Enqueue (t);
			
			t = new Transition ("action.combat_mode_standing");
			t.conditions.Add (new KeyValuePair<string, bool> ("toLaying", true));
			_queue.Enqueue (t);
			
			t = new Transition ("action.combat_mode_laying");
			t.conditions.Add (new KeyValuePair<string, bool> ("toFire", true));
			_queue.Enqueue (t);
			
			t = new Transition ("action.fire_laying");
			_queue.Enqueue (t);
		}
	}
	public void registerFireSquatFromRunning() {
		if (_queue.Count == 0) {
			Transition t = new Transition ("action.run");
			t.conditions.Add (new KeyValuePair<string, bool> ("toStanding", true));
			_queue.Enqueue (t);
			
			t = new Transition ("action.combat_mode_standing");
			t.conditions.Add (new KeyValuePair<string, bool> ("toSquat", true));
			_queue.Enqueue (t);
			
			t = new Transition ("action.combat_mode_squat");
			t.conditions.Add (new KeyValuePair<string, bool> ("toFire", true));
			_queue.Enqueue (t);
			
			t = new Transition ("action.fire_squat");
			_queue.Enqueue (t);
		}
	}
	public void registerFireSquatFromWalking() {
		if (_queue.Count == 0) {
			Transition t = new Transition ("action.walking");
			t.conditions.Add (new KeyValuePair<string, bool> ("toStanding", true));
			_queue.Enqueue (t);
			
			t = new Transition ("action.combat_mode_standing");
			t.conditions.Add (new KeyValuePair<string, bool> ("toSquat", true));
			_queue.Enqueue (t);
			
			t = new Transition ("action.combat_mode_squat");
			t.conditions.Add (new KeyValuePair<string, bool> ("toFire", true));
			_queue.Enqueue (t);
			
			t = new Transition ("action.fire_squat");
			_queue.Enqueue (t);
		}
	}
	public void registerFireLayingFromWalking() {
		if (_queue.Count == 0) {
			Transition t = new Transition("action.walking");
			t.conditions.Add (new KeyValuePair<string, bool>("toStanding", true));
			_queue.Enqueue (t);
			
			t = new Transition("action.combat_mode_standing");
			t.conditions.Add (new KeyValuePair<string, bool>("toLaying", true));
			_queue.Enqueue (t);
			
			t = new Transition ("action.combat_mode_laying");
			t.conditions.Add (new KeyValuePair<string, bool>("toFire", true));
			_queue.Enqueue (t);
			
			t = new Transition ("action.fire_laying");
			_queue.Enqueue (t);
		}
	}
}
