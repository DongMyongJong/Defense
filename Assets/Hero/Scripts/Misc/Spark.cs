using UnityEngine;
using System.Collections;

public class Spark : MonoBehaviour {
	private float life = 0.06f;
	private float destroyTime;
	private float angle;
	public Material[] material;
	public GameObject flyingSparkPrefab;	
	private int flyingSparkAmount = 3;
	private int flyingSparkAmountVariation = 2;
	
	// Use this for initialization
	void Start () {
		int materialID = Mathf.FloorToInt(Random.value * material.Length);
		renderer.material = material[materialID];
		destroyTime = Time.time + life;
		angle = Random.value * 360.0f; 
		transform.LookAt(Camera.main.transform.position);
		Vector3 angles = transform.localRotation.eulerAngles;
		transform.localRotation = Quaternion.Euler(angles.x, angles.y, angles.z + angle);
		transform.localScale *= 0.5f + Random.value;
		flyingSparkAmount = flyingSparkAmount + Mathf.RoundToInt(Random.Range(-flyingSparkAmountVariation * 0.5f, flyingSparkAmountVariation * 0.5f));
		for(int i = 0; i < flyingSparkAmount; i++){
			Instantiate(flyingSparkPrefab, transform.position, transform.rotation);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > destroyTime){
			Destroy(gameObject);
		}
		transform.LookAt(Camera.main.transform.position);
		Vector3 angles = transform.localRotation.eulerAngles;
		transform.localRotation = Quaternion.Euler(angles.x, angles.y, angles.z + angle);
		transform.localScale *= 1 + 10 *(Time.deltaTime);
	}
}
