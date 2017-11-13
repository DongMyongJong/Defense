using UnityEngine;
using System.Collections;

public class BulletCase : MonoBehaviour {
	public float life = 0.5f;

	private float destroyTime;
	public Vector3 velocity;
	private float gravity = 9.8f;
	private float turnSpeed;
	private float turnAngle;
	
	// Use this for initialization
	void Start () {
		destroyTime = Time.time + life;
		turnAngle = Random.value * 360.0f;
		turnSpeed = Random.Range(-360.0f, 360.0f);
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time > destroyTime){
			Destroy(gameObject);
		}
		transform.LookAt(Camera.main.transform.position);
		turnAngle += turnSpeed * Time.deltaTime;
		Vector3 angles = transform.localRotation.eulerAngles;
		transform.localRotation = Quaternion.Euler(angles.x, angles.y, angles.z + turnAngle);
		velocity.y -= gravity * Time.deltaTime;
		transform.position += velocity * Time.deltaTime;
		transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * life * 2.0f);
	}
}
