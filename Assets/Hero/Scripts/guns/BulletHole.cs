using UnityEngine;
using System.Collections;

public class BulletHole : MonoBehaviour {
	public Material[] materials;
	public float life = 4.0f;
	public float size = 1.0f;
	
	private float destroyTime;
	private float startTime;
	
	// Use this for initialization
	void Start () {
		startTime = Time.time;
		destroyTime = Time.time + life;
		int chooseId = Mathf.RoundToInt(Random.Range(0, materials.Length));
		renderer.material = materials[chooseId];
		Transform parent = transform.parent;
		transform.parent = null;
		Vector3 angles = transform.localRotation.eulerAngles;
		transform.localRotation = Quaternion.Euler(angles.x, angles.y, Random.value * 360);
		transform.localScale = Vector3.one * (0.5f + Random.value * 0.5f) * size;
		transform.parent = parent;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > destroyTime){
			Destroy(gameObject);
		}
		//var age = Time.time - startTime;
		if (Time.time > destroyTime - 1.0){
			float fadeProgress = destroyTime - Time.time;
			Color c = renderer.material.color;
			renderer.material.color = new Color(c.r, c.g, c.b, fadeProgress);
		}
	}
}
