using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct Transition {

	public int action;
	public List<KeyValuePair<string, bool>> conditions;

	public Transition(string strAction) {
		action = Animator.StringToHash (strAction);
		conditions = new List<KeyValuePair<string, bool>> ();
	}
	public Transition(int myAction) {
		action = myAction;
		conditions = new List<KeyValuePair<string, bool>> ();
	}
}
