using UnityEngine;
using System.Collections;

public class SpinningTurret : MonoBehaviour {
	public bool on = true;
	public float maxSpeed = 1000.0f;
	public float acceleration = 0.8f;
	public float speed;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (on) {
			speed = Mathf.Lerp(speed, maxSpeed, Time.deltaTime * acceleration);
		} else {
			speed = Mathf.Lerp(speed, 0, Time.deltaTime * acceleration);
		}
		Vector3 angles = transform.localRotation.eulerAngles;
		transform.localRotation = Quaternion.Euler(angles.x, angles.y + speed * Time.deltaTime, angles.z);
	}
}
