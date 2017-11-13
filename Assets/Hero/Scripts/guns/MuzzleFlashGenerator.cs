using UnityEngine;
using System.Collections;

public class MuzzleFlashGenerator : MonoBehaviour {
	public GameObject muzzleFlashPrefab;
	public Material[] materials;
	public float rate = 8.0f;
	public bool on = true;
	
	private float nextMuzzleFlashTime;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (on){
			if(Time.time > nextMuzzleFlashTime){
				nextMuzzleFlashTime = Time.time + (1.0f / rate);
				GameObject newMuzzleFlash = Instantiate(muzzleFlashPrefab, transform.position, transform.rotation) as GameObject;
				int materialId = Mathf.RoundToInt(Random.Range(0, materials.Length));
				newMuzzleFlash.renderer.material = materials[materialId];
				newMuzzleFlash.transform.parent = transform;
			}
		}
	}
}
