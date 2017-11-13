using UnityEngine;
using System.Collections;

public class FlyingSpark : MonoBehaviour {
	public float life = 1.5f;
	public float lifeVariation = 0.5f;
	
	private float startTime;
	private float destroyTime;
	private Vector3 velocity;
	private float gravity = 9.8f;
	private float scale = 1.0f;
	
	// Use this for initialization
	void Start () {
		startTime = Time.time;
		life += Random.Range(-lifeVariation * 0.5f, lifeVariation * 0.5f); //Vary life.
		destroyTime = Time.time + life;
		velocity = Random.insideUnitSphere * 4.0f;
		velocity.y += Random.value * 3.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time > destroyTime){
			Destroy(gameObject);
		}
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast(transform.position, velocity, out hit, velocity.magnitude * Time.deltaTime)){
			velocity = Vector3.Reflect(velocity, hit.normal);
		}
		//Velocity.
		velocity.y -= gravity * Time.deltaTime;
		velocity = Vector3.Lerp(velocity, Vector3.zero, Time.deltaTime);
		transform.position += velocity * Time.deltaTime;
		//Rotation.
		transform.LookAt(transform.position + velocity);
		//Scale.
		scale = Mathf.Lerp(0.2f, 0.05f, (Time.time - startTime) / life);
		Vector3 localScale = Vector3.one * scale;
		transform.localScale = new Vector3(localScale.x, localScale.y, (0.2f + velocity.magnitude * 0.6f) * scale);
	}
}
