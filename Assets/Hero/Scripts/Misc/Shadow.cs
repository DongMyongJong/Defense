using UnityEngine;
using System.Collections;

public class Shadow : MonoBehaviour {
	public string[] ignoreRootName;
	public float distanceTolerance = 0.4f;
	public float maxOpacity = 1.0f;
	
	private float opacity = 1.0f;
	private Transform castingPoint;
	private float buffer = 0.02f;

	// Use this for initialization
	void Start () {
		renderer.enabled = true;
		castingPoint = transform.Find("castingPoint");
		castingPoint.parent = transform.parent;
		transform.parent = transform.root;
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void LateUpdate() {
		//Opacity distance.
		float distanceShadow = Vector3.Distance(transform.position, castingPoint.position);
		opacity = Mathf.Lerp(maxOpacity, 0.0f,  distanceShadow * (1 / distanceTolerance));
		//Shadow position.
		transform.position = castingPoint.position;
		RaycastHit[] shadowHits = Physics.RaycastAll(transform.position + Vector3.up * 0.5f, -Vector3.up);
		float maxShadowYPosition = -999999.0f;
		for (int i = 0; i < shadowHits.Length; i++){
			RaycastHit shadowHit = shadowHits[i];
			string name = shadowHit.transform.root.name;
			bool takeIt = true;
			for (int n = 0; n < ignoreRootName.Length; n++) {
				string ignoreName = ignoreRootName[n];
				if (name == ignoreName) {
					takeIt = false;
				}
			}
			if (takeIt) {
				if (shadowHit.point.y + buffer > maxShadowYPosition) {
					maxShadowYPosition = shadowHit.point.y + buffer;
					transform.position = new Vector3(transform.position.x, maxShadowYPosition, transform.position.z);
					transform.LookAt(transform.position + shadowHit.normal);
				}
			}
		}
		if (shadowHits.Length == 0) {
			opacity = 0.0f;
		}
		Color c = renderer.material.color;
		renderer.material.color = new Color(c.r, c.g, c.b, opacity);
	}
}
