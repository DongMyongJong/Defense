using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyActionConditioner {

	private Animator _animator;
	private bool toStanding;
	private bool toSquat;
	private bool toLaying;
	private bool toRun;
	private bool toFire;
	private bool toIdle;
	private bool toWalking;
	private bool toButt;
	private bool onBelt;
	
	private void refresh() {
		_animator.SetBool("toStanding", toStanding);
		_animator.SetBool("toSquat", toSquat);
		_animator.SetBool("toLaying", toLaying);
		_animator.SetBool("toRun", toRun);
		_animator.SetBool("toFire", toFire);
		_animator.SetBool("toIdle", toIdle);
		_animator.SetBool("toWalking", toWalking);
		_animator.SetBool("toButt", toButt);
		_animator.SetBool ("onBelt", onBelt);
	}
	public void setAllWithValue(bool value) {
		toStanding = value;
		toSquat = value;
		toLaying = value;
		toRun = value;
		toFire = value;
		toIdle = value;
		toWalking = value;
		toButt = value;
		onBelt = value;
		refresh ();
	}
	public EnemyActionConditioner(Animator animator) {
		_animator = animator;
		toStanding = false;
		toSquat = false;
		toLaying = false;
		toRun = false;
		toFire = false;
		toIdle = false;
		toWalking = false;
		toButt = false;
		onBelt = false;
		refresh ();
	}
	public bool OnBelt
	{
		get
		{
			return onBelt;
		}
		set{
			onBelt = value;
			_animator.SetBool("onBelt", onBelt);
		}
	}
	public bool ToStanding
	{
		get
		{
			return toStanding;
		}
		set
		{
			toStanding = value;
			if (toStanding) {
				toSquat = toLaying = toRun = toFire = toIdle = toWalking = toButt = false;
			}
			refresh ();
		}
	}
	public bool ToSquat
	{
		get
		{
			return toSquat;
		}
		set
		{
			toSquat = value;
			if (toSquat) {
				toStanding = toLaying = toRun = toFire = toIdle = toWalking = toButt = false;
			}
			refresh ();
		}
	}
	public bool ToLaying
	{
		get
		{
			return toLaying;
		}
		set
		{
			toLaying = value;
			if (toLaying) {
				toStanding = toSquat = toRun = toFire = toIdle = toWalking = toButt = false;
			}
			refresh ();
		}
	}
	public bool ToRun
	{
		get
		{
			return toRun;
		}
		set
		{
			toRun = value;
			if (toRun) {
				toStanding = toSquat = toLaying = toFire = toIdle = toWalking = toButt = false;
			}
			refresh ();
		}
	}
	public bool ToFire
	{
		get
		{
			return toFire;
		}
		set
		{
			toFire = value;
			if (toFire) {
				toStanding = toSquat = toLaying = toRun = toIdle = toWalking = toButt = false;
			}
			refresh ();
		}
		
	}
	public bool ToIdle
	{
		get
		{
			return toIdle;
		}
		set
		{
			toIdle = value;
			if (toIdle) {
				toStanding = toSquat = toLaying = toRun = toFire = toWalking = toButt = false;
			}
			refresh ();
		}
		
	}
	public bool ToWalking
	{
		get
		{
			return toWalking;
		}
		set
		{
			toWalking = value;
			if (toWalking) {
				toStanding = toSquat = toLaying = toRun = toIdle = toFire = toButt = false;
			}
			refresh ();
		}
		
	}
	public bool ToButt
	{
		get
		{
			return toButt;
		}
		set
		{
			toButt = value;
			if (toButt) {
				toStanding = toSquat = toLaying = toRun = toIdle = toFire = toWalking = false;
			}
			refresh ();
		}
		
	}
	public void changeWithStringExclusive(string condition, bool val) {
		if (condition == "toStanding") {
			ToStanding = val;
		} else if (condition == "toSquat") {
			ToSquat = val;
		} else if (condition == "toLaying") {
			ToLaying = val;
		} else if (condition == "toRun") {
			ToRun = val;
		} else if (condition == "toFire") {
			ToFire = val;
		} else if (condition == "toIdle") {
			ToIdle = val;
		} else if (condition == "toWalking") {
			ToWalking = val;
		} else if (condition == "toButt") {
			ToButt = val;
		} else {
			Debug.Log(string.Format("Condition=\"{0}\" is not found", condition));
		}
	}

	public void changeWithStringInclusive(List<KeyValuePair<string, bool>> conditions) {
		toStanding = false;
		toSquat = false;
		toLaying = false;
		toRun = false;
		toFire = false;
		toIdle = false;
		toWalking = false;
		toButt = false;
		onBelt = false;
		foreach (KeyValuePair<string, bool> kv in conditions) {
			string condition = kv.Key;
			bool val = kv.Value;
			if (condition == "toStanding") {
				toStanding = val;
			} else if (condition == "toSquat") {
				toSquat = val;
			} else if (condition == "toLaying") {
				toLaying = val;
			} else if (condition == "toRun") {
				toRun = val;
			} else if (condition == "toFire") {
				toFire = val;
			} else if (condition == "toIdle") {
				toIdle = val;
			} else if (condition == "toWalking") {
				toWalking = val;
			} else if (condition == "toButt") {
				toButt = val;
			} else if (condition == "onBelt") {
				onBelt = val;
			} else {
				Debug.Log(string.Format("Condition=\"{0}\" is not found", condition));
			}
		}
		refresh ();
	}
}
