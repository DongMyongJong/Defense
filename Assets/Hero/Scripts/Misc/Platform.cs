using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {
	public float maxHeight = 5.0f;
	public float minHeight = 1.0f;
	public float speed = 2.0f;
	private bool rising;
	private float velocity;
	
	// Use this for initialization
	void Start () {
		rising = true;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 position = transform.position;
		if (rising) {
			transform.position = new Vector3(position.x, Mathf.SmoothDamp(transform.position.y, maxHeight, ref velocity, 1.0f / speed), position.z);
			if (Mathf.Abs(transform.position.y - maxHeight) < 0.1f) {
				rising = false;
			}
		} else {
			transform.position = new Vector3(position.x, Mathf.SmoothDamp(transform.position.y, minHeight, ref velocity, 1.0f / speed), position.z);
			if (Mathf.Abs(transform.position.y - minHeight) < 0.1f) {
				rising = true;
			}
		}
	}
}
