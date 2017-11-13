using UnityEngine;
using System.Collections;

public class ReceptionController : MonoBehaviour {

	public dfSlider backSlider;
	public dfSlider effectSlider;
	public dfPanel settingAndShopPanel;

	// Use this for initialization
	void Start () {
		// 설정창문 처음에 숨기기
		settingAndShopPanel.IsVisible = false;
		// 배경음과 효과음의 볼륨설정
		float backVolume = (PlayerPrefs.HasKey("back_volume"))?PlayerPrefs.GetFloat("back_volume"):0;
		float effectVolume = (PlayerPrefs.HasKey("effect_volume"))?PlayerPrefs.GetFloat("effect_volume"):0;
		backSlider.Value = backVolume * 100.0f;
		effectSlider.Value = effectVolume * 100.0f;
	}
	
	// Update is called once per frame
	void Update () {
		// 배경음, 효과음 볼륨 갱신
		AudioSource[] arrAudio = Camera.main.GetComponents<AudioSource>();
		foreach(AudioSource audio in arrAudio) {
			audio.volume = backSlider.Value / 100.0f;
		}
	}

	public void OnNewStart() {
		showAndSelectTab(2);
	}
	
	public void OnLoadSaved() {
		showAndSelectTab(3);
	}

	public void OnSettingClick() {
		showAndSelectTab(0);
	}

	public void OnShopClick() {
		showAndSelectTab(1);
	}

	public void OnExitClick() {
		Application.Quit();
	}

	public void hideSettingAndShopPanel() {
		if (settingAndShopPanel.IsVisible) {
			settingAndShopPanel.IsVisible = false;
		}
		// 배경음, 효과음 보관
		PlayerPrefs.SetFloat("back_volume", backSlider.Value / 100.0f);
		PlayerPrefs.SetFloat("effect_volume", effectSlider.Value / 100.0f);
	}

	public void OnItemBuy(string name) {
	}

	private void showAndSelectTab(int index) {
		if (!settingAndShopPanel.IsVisible) {
			settingAndShopPanel.IsVisible = true;
		}
		dfTabstrip tabStrip = settingAndShopPanel.GetComponentInChildren<dfTabstrip>();
		if (tabStrip.SelectedIndex != index) {
			tabStrip.SelectedIndex = index;
		}
		settingAndShopPanel.GetComponentInChildren<dfTabContainer>().SelectedIndex = index;
	}
}
