using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {
	public bool firing = false;
	public float accuracy;
	private Transform bulletCaseGenerator;
	private Transform bulletTraceGenerator;
	private Transform muzzleFlashGenerator;
	private BulletCaseGenerator bulletCaseGeneratorScript;
	private BulletTraceGenerator bulletTraceGeneratorScript;
	private MuzzleFlashGenerator muzzleFlashGeneratorScript;
	
	// Use this for initialization
	void Start () {
		bulletCaseGenerator = transform.Find("bulletCaseGenerator");
		bulletTraceGenerator = transform.Find("bulletTraceGenerator");
		muzzleFlashGenerator = transform.Find("muzzleFlashGenerator");
		bulletCaseGeneratorScript = bulletCaseGenerator.GetComponent<BulletCaseGenerator>();
		bulletTraceGeneratorScript = bulletTraceGenerator.GetComponent<BulletTraceGenerator>();
		muzzleFlashGeneratorScript = muzzleFlashGenerator.GetComponent<MuzzleFlashGenerator>();
		firing = false;
		//accuracy = 0.9;
	}
	
	// Update is called once per frame
	void Update () {
		bulletCaseGeneratorScript.on = firing;
		bulletTraceGeneratorScript.on = firing;
		muzzleFlashGeneratorScript.on = firing;
		bulletTraceGeneratorScript.accuracy = accuracy;
	}
}
