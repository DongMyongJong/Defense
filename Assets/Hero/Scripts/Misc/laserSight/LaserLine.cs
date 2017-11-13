using UnityEngine;
using System.Collections;

public class LaserLine : MonoBehaviour {
	public GameObject laserDustPrefab;

	private float startTime;
	private float life = 1.0f;
	private float lifeVariation = 1.0f;
	private float endTime;
	private float length;
	private float laserDustRate = 12.0f;
	private float nextLaserDustTime;
	private Color laserColor;
	private float curveProgress;
	
	// Use this for initialization
	void Start () {
		startTime = Time.time;
		life = life + lifeVariation * Random.value;
		endTime = Time.time + life;
		length = Random.Range(1.0f ,3.0f);
		laserColor = new Color(0.0f, 0.0f, 0.0f);
		for (int i = 0; i < transform.childCount; i++){
			Transform child = transform.GetChild(i);
			child.renderer.material.SetColor("_TintColor", laserColor);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time > endTime){
			Destroy(gameObject);
		}
		float age = Time.time - startTime;
		float progress = age / life;
		curveProgress = -4.0f * Mathf.Pow(progress, 2.0f) + progress * 4.0f;
		laserColor = new Color(curveProgress * 0.10f, 0.0f, 0.0f);
		for (int i = 0; i < transform.childCount; i++){
			Transform child = transform.GetChild(i);
			if(child.name == "visual"){
				child.renderer.material.SetColor("_TintColor", laserColor);
				Vector3 localScale = Vector3.one * 0.1f;
				localScale.y = length + 2.0f * curveProgress + Random.value * 1.0f;
			}
		}
		transform.localScale = Vector3.one;
		if(Time.time > nextLaserDustTime){
			nextLaserDustTime = Time.time + (1.0f / laserDustRate);
			GameObject newLaserDust = Instantiate(laserDustPrefab, transform.position, Quaternion.identity) as GameObject;
			newLaserDust.transform.parent = transform;
			float getPosition = (transform.localScale.y * 0.5f) / newLaserDust.transform.localScale.y;
			Vector3 localPosition = newLaserDust.transform.localPosition;
			newLaserDust.transform.localPosition = new Vector3(localPosition.x, Random.Range(-getPosition * 0.5f, getPosition * 0.5f), localPosition.z);
	
		}
	}
	
	public float GetCurveProgress(){
		return curveProgress; //Red is the only color used on the laser color. Black is transparent because of particle additive material.
	}
}
