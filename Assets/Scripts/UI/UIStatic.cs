using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStatic : MonoBehaviour {

	public static UIStatic singleton;

	public GameObject classSwitchUI;
	public GameObject playerUI;
	public GameObject ui;

	public Image healthImg;
	public Text healthText;

	public Canvas canvas;

	public Image abilityPlayImage;
	public Image abilityUsedImage;
	public Text abilityUsedText;
	public RectTransform payloadProress;
	public Text ammoText;

	public bool vr = false;
	public bool enableUI = false;
	public HealthSystem playerHealth;
	public Shooting shootingStars;
	public Classes.ClassChoose classSwitch;

	void Start () {
		singleton = this;
	}
	
	public void ChangeClass (int newClass) {
		if (!LocalPlayer.localPlayer.vr)
			LocalPlayer.localPlayer.GetComponent<Classes.ClassChoose> ().SwitchClass ((Classes.Class) newClass);
		
	}

	void Update () {

		if (!enableUI && ui != null) {
			ui.SetActive (false);
			return;
		} else if (enableUI) {
			ui.SetActive (true);
		}

		if (vr && playerUI != null) {
			playerUI.SetActive (false);
			return;
		}

		if (playerHealth != null) {
			healthImg.fillAmount = playerHealth.health * 0.01f;
			healthText.text = playerHealth.health.ToString () + "%";
		}

		if (shootingStars != null) {
			ammoText.text = ""+shootingStars.weapons [shootingStars.weaponNum].ammo;
		}

		if (classSwitch != null) {
			switch (classSwitch.playerClass) {
			case Classes.Class.Assault:
				abilityUsedImage.enabled = true;
				break;
			case Classes.Class.Medic:
				abilityUsedImage.enabled = true;
				break;
			case Classes.Class.Scout:
				abilityUsedImage.enabled = false;
				abilityPlayImage.fillAmount = 0;
				abilityUsedText.text = "";
				break;
			case Classes.Class.Shield:
				abilityUsedImage.enabled = true;
				if (classSwitch.shieldAbilities.enable) {
					abilityPlayImage.fillAmount = 1 - (Time.time - classSwitch.shieldAbilities.startTime) / classSwitch.shieldAbilities.duration;
					abilityUsedText.text = "" + (int)(classSwitch.shieldAbilities.duration - (Time.time - classSwitch.shieldAbilities.startTime));
				} else {
					Debug.Log ("dd");
					if (Time.time - classSwitch.shieldAbilities.endTime < classSwitch.shieldAbilities.cooldownTime) {
						abilityPlayImage.fillAmount = (Time.time - classSwitch.shieldAbilities.endTime) / classSwitch.shieldAbilities.duration;
						abilityUsedText.text = "" + (int)(classSwitch.shieldAbilities.cooldownTime - (Time.time - classSwitch.shieldAbilities.endTime));
					} else {
						abilityPlayImage.fillAmount = 0;
						abilityUsedText.text = "";
					}
				}
				break;
			}
		}
	}
}
