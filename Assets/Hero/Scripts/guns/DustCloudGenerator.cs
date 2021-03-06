﻿using UnityEngine;
using System.Collections;

public class DustCloudGenerator : MonoBehaviour {
	public GameObject dustCloudPrefab;
	public float rate = 8.0f;
	public Material[] materials;
	public bool on = true;
	public float life = 0.3f;
	
	private float nextdustCloudTime;
	private float destroyTime;
	public Vector3 velocity;
	
	// Use this for initialization
	void Start () {
		destroyTime = Time.time + life;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > destroyTime){
			Destroy(gameObject);
		}
		if(Time.time > nextdustCloudTime){
			nextdustCloudTime = Time.time + (1.0f / rate);
			GameObject newDustCloud = Instantiate(dustCloudPrefab, transform.position, transform.rotation) as GameObject;
			int materialId  = Mathf.RoundToInt(Random.Range(0, materials.Length-1));
			newDustCloud.renderer.material = materials[materialId];
			newDustCloud.GetComponent<DustCloud>().velocity = velocity;
		}
	}
}
