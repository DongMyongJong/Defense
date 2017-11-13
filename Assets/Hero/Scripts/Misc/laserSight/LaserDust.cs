using UnityEngine;
using System.Collections;

public class LaserDust : MonoBehaviour {
	private float startTime ;
	private float life = 0.5f;
	private float lifeVariation = 1.0f;
	private float endTime;
	private float length;
	private float scale;
	private float maxAlpha;
	
	// Use this for initialization
	void Start () {
		startTime = Time.time;
		life = life + lifeVariation * Random.value;
		endTime = Time.time + life;
		length = Random.Range(6.0f, 8.0f);
		scale = Random.Range(0.11f, 0.14f);
		Color laserColor = new Color(0.0f, 0.0f, 0.0f);
		renderer.material.SetColor("_TintColor", laserColor);
		maxAlpha = Random.Range(0.1f, 0.3f);
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time > endTime){
			Destroy(gameObject);
		}
		float age = Time.time - startTime;
		float progress = age / life;
		float curveProgress = -4.0f * Mathf.Pow(progress, 2.0f) + progress * 4.0f;
		float parentAlpha = 1.0f;
		if(transform.parent != null){
			parentAlpha = transform.parent.GetComponent<LaserLine>().GetCurveProgress();
		}
		Color laserColor = new Color(curveProgress * maxAlpha * parentAlpha, 0.0f, 0.0f);
		renderer.material.SetColor("_TintColor", laserColor);
		transform.LookAt(Camera.main.transform.position);
		transform.localScale = Vector3.one * (scale + curveProgress * scale * 0.2f);
	}
}
