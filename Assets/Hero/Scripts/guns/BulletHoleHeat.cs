using UnityEngine;
using System.Collections;

public class BulletHoleHeat : MonoBehaviour {
	public float life = 1.0f;
	public Color startColor;
	public Color endColor;
	
	private float startTime;
	private float destroyTime;
	
	// Use this for initialization
	void Start () {
		life = Random.value * life;
		startTime = Time.time;
		destroyTime = Time.time + life;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time > destroyTime){
			Destroy(gameObject);
		}
		float age = Time.time - startTime;
		float progress = age / life;
		Color heatColor = renderer.material.GetColor("_TintColor");
		heatColor = Color.Lerp(startColor, endColor, progress);
		heatColor.a = Mathf.Lerp(1.0f, 0.0f, progress);
		renderer.material.SetColor("_TintColor", heatColor);
	}
}
