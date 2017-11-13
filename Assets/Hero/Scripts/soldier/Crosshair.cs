using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {
	public Vector2 crosshairCenter = new Vector2(0.5f, 0.6f);
	public float offseting = 2.0f;
	public Texture textureUp;
	public Texture textureDown;
	public Texture textureRight;
	public Texture textureLeft;
	public float accuracyLoss;
	public float xOffset;
	public float yOffset;

	public string lookAtJoystickId;
	public string moveJoystickId;
	
	private Vector3 position;
	private float xOffsetSpeed;
	private float yOffsetSpeed;
	private Color crosshairColor = Color.white;
	private float crosshairAlpha = 1.0f;
	
	//External Scripts.
	private Health healthScript;
	
	// Use this for initialization
	void Start () {
		healthScript = transform.root.GetComponent<Health>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void LateUpdate() {
		float health = 100.0f;
		if (healthScript != null) {
			health = healthScript.GetHealth();
		}
		if (health > 0) {
			Vector2 lookAtPos = dfTouchJoystick.GetJoystickPosition(lookAtJoystickId);
			xOffsetSpeed += lookAtPos.x * Time.deltaTime * 0.2f;
			yOffsetSpeed += lookAtPos.y * Time.deltaTime * 0.2f;
			Vector2 movePos = dfTouchJoystick.GetJoystickPosition(moveJoystickId);
			xOffsetSpeed += movePos.x * Time.deltaTime * 0.2f;
			yOffsetSpeed += movePos.y * Time.deltaTime * 0.2f;
		}
		xOffsetSpeed = Mathf.Lerp(xOffsetSpeed, 0, Time.deltaTime * 20.0f);
		yOffsetSpeed = Mathf.Lerp(yOffsetSpeed, 0, Time.deltaTime * 20.0f);
		xOffset += xOffsetSpeed;
		yOffset += yOffsetSpeed;
		xOffset = Mathf.Lerp(xOffset, 0, Time.deltaTime * Mathf.Pow(Mathf.Abs(xOffset), offseting) * offseting * 100);
		yOffset = Mathf.Lerp(yOffset, 0, Time.deltaTime * Mathf.Pow(Mathf.Abs(yOffset), offseting) * offseting * 100);
		transform.position = new Vector3(crosshairCenter.x + xOffset, crosshairCenter.y + yOffset, 0);
	}
	
	void OnGUI() {
		float health = 100.0f;
		if (healthScript != null) {
			health = healthScript.GetHealth();
		}
		float alphaWave = 0.1f;
		if (health > 0) {
			crosshairAlpha = Mathf.Sin(Time.time) * alphaWave + (1-alphaWave);
		} else {
			crosshairAlpha = Mathf.Lerp(crosshairAlpha, 0, Time.deltaTime);
		}
		crosshairColor.a = crosshairAlpha;
		GUI.color = crosshairColor;
		float scale = Screen.width * 0.03f;
		float xPosition = Screen.width * crosshairCenter.x + xOffset * Screen.width - scale * 0.5f;
		float yPosition = Screen.height * (1-crosshairCenter.y) - yOffset * Screen.height - scale * 0.5f;
		float screenAccuracyDisplay = (accuracyLoss * Screen.width) / 40.0f;
		GUI.DrawTexture(new Rect(xPosition, yPosition + screenAccuracyDisplay, scale, scale), textureUp);
		GUI.DrawTexture(new Rect(xPosition, yPosition - screenAccuracyDisplay, scale, scale), textureDown);
		GUI.DrawTexture(new Rect(xPosition - screenAccuracyDisplay, yPosition, scale, scale), textureRight);
		GUI.DrawTexture(new Rect(xPosition + screenAccuracyDisplay, yPosition, scale, scale), textureLeft);
	}
}
