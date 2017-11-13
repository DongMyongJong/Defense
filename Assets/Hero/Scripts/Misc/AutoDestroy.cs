using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour {
	public float life;
	private float destroyTime;
	
	// Use this for initialization
	void Start () {
		destroyTime = Time.time + life;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time > destroyTime){
			Destroy(gameObject);
		}
	}
}
