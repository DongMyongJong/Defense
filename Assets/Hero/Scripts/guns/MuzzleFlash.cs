using UnityEngine;
using System.Collections;

public class MuzzleFlash : MonoBehaviour {
	public float minLife = 0.01f;
	public float maxLife = 0.02f;
	
	private float destroyTime;
	private float angle;
	
	// Use this for initialization
	void Start () {
		destroyTime = Time.time + Random.Range(minLife, maxLife);
		angle = 90.0f * Mathf.Round(Random.Range(0.0f, 3.0f));
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > destroyTime){
			Destroy(gameObject);
		}
		transform.LookAt(Camera.main.transform.position);
		Vector3 angles = transform.localRotation.eulerAngles;
		transform.localRotation = Quaternion.Euler(angles.x, angles.y, angles.z + angle);
	}
}
