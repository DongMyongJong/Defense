using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public dfSprite dialog;
	public GameObject lookPanel;
	public GameObject movePanel;
	public dfSprite crouchButton;
	public dfSprite jumpButton;
	public dfSprite shootButton;
	public dfSprite radarPanel;
	public delegate void delFnYes();
	public delegate void delFnCancel();

	private const float design_width = 1280;
	private const float design_height = 720;
	private const float min_button_size = 96;
	private const float max_button_size = 128;

	private delFnYes fnYes;
	private delFnCancel fnCancel;

	// Use this for initialization
	void Start () {
//		Screen.lockCursor = true;
//		Screen.showCursor = false;
		if (dialog != null) {
			dialog.IsVisible = false;
		}
		fnYes = null;
		fnCancel = null;

		layoutControls ();
	}

	private void layoutControls()
	{
		float ratio = Mathf.Clamp(Mathf.Min (Screen.width / design_width, Screen.height / design_height), min_button_size, max_button_size) / max_button_size;
		crouchButton.Size = crouchButton.Size * ratio;
		jumpButton.Size = jumpButton.Size * ratio;
		shootButton.Size = shootButton.Size * ratio;
		foreach (dfSprite child in radarPanel.GetComponentsInChildren<dfSprite>()) {
			child.Size = child.Size * ratio;
		}
		foreach (dfSprite child in lookPanel.GetComponentsInChildren<dfSprite> ()) {
			child.Size = child.Size * ratio;
		}
		foreach (dfSprite child in movePanel.GetComponentsInChildren<dfSprite>()) {
			child.Size = child.Size * ratio;
		}
		radarPanel.Position = new Vector3 (crouchButton.Position.x, radarPanel.Position.y, radarPanel.Position.z);
		dfPanel panel = movePanel.GetComponent<dfPanel> ();
		panel.Size = panel.Size * ratio;
		panel = lookPanel.GetComponent<dfPanel> ();
		panel.Size = panel.Size * ratio;
	}
	
	// Update is called once per frame
	void Update () {
//		Screen.lockCursor = true;
	}

	public void OnDialogOK() {
		dialog.IsVisible = false;
		if (fnYes != null) {
			fnYes ();
		}
	}

	public void OnDialogCancel() {
		dialog.IsVisible = false;
		if (fnCancel != null) {
			fnCancel ();
		}
	}

	public void OnClickExit() {
		if (dialog.IsVisible == true) {
			return;
		}
		showQuestionMessage ("프로그람을 정말 끝내겠습니까", delegate() {
			Application.Quit ();
		}, null);
	}

	private void showQuestionMessage(string message, delFnYes yes, delFnCancel cancel) {
		if (dialog == null) {
			return;
		}
		dfLabel label = dialog.GetComponentInChildren<dfLabel> ();
		label.Text = message;
		fnYes = yes;
		fnCancel = cancel;
		dialog.IsVisible = true;
	}
}
