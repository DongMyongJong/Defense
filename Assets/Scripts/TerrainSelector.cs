using UnityEngine;
using System.Collections;

public class TerrainSelector : MonoBehaviour {

	private int terrainIndex;

	// Use this for initialization
	void Start () {
		string name = GetComponent<dfTextureSprite> ().Texture.name.Trim();
		terrainIndex = int.Parse(name.Substring(name.Length - 1));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnDoubleTapGesture() {
		Debug.Log ("terrainIndex = " + terrainIndex);
		Application.LoadLevel ("battlefield");
	}
}
