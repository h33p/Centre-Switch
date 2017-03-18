using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GreenByteSoftware.UNetController;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;

namespace Classes {

	public enum Class
	{
		Shield = 0, Scout = 1, Assault = 2, Medic = 3
	};

	public class ClassChoose : NetworkBehaviour {

		public GameObject shieldPrefab;
		private GameObject shieldObject;
		public TankAbilities shieldAbilities;

		public ControllerDataObject shieldParameters;
		public ControllerDataObject scoutParameters;
		public ControllerDataObject assaultParameters;
		public ControllerDataObject medicParameters;
		public Controller controller;

		public Class playerClass;

		public float cooldown = 30f;
		public float switchTime = -Mathf.Infinity;
		public int maxChanges = 5;
		public int changes = -1;

		public Sprite shield;
		public Sprite scout;
		public Sprite medic;
		public Sprite assault;

		public override void OnStartLocalPlayer () {
			UIStatic.singleton.classSwitch = this;
		}

		void Update () {
			if (!isLocalPlayer)
				return;
			
			if (CrossPlatformInputManager.GetButtonDown ("ClassSwitch"))
				UIStatic.singleton.classSwitchUI.SetActive(!UIStatic.singleton.classSwitchUI.activeSelf);
		}

		[ServerCallback]
		void Start () {
			RpcSwitchClass (playerClass);
		}

		public void SwitchClass (Class newClass) {
			CmdSwitchClass (newClass);
			UIStatic.singleton.classSwitchUI.SetActive (false);
		}

		[Command]
		void CmdSwitchClass(Class newClass) {
			if (changes < maxChanges && Time.time - switchTime >= cooldown) {
				switchTime = Time.time;
				changes++;
				RpcSwitchClass (newClass);
			}
		}

		[ClientRpc]
		public void RpcSwitchClass (Class newClass) {
			switch (playerClass) {
			case Class.Assault:

				break;
			case Class.Medic:

				break;
			case Class.Scout:

				break;
			case Class.Shield:
				shieldAbilities.enabled = false;
				Destroy (shieldObject);
				break;
			}

			switch (newClass) {
			case Class.Assault:
				controller.data = assaultParameters;
				UIStatic.singleton.abilityUsedImage.sprite = assault;
				break;
			case Class.Medic:
				controller.data = medicParameters;
				UIStatic.singleton.abilityUsedImage.sprite = medic;
				break;
			case Class.Scout:
				controller.data = scoutParameters;
				UIStatic.singleton.abilityUsedImage.sprite = scout;
				break;
			case Class.Shield:
				controller.data = shieldParameters;
				shieldObject = Instantiate (shieldPrefab, transform.position, transform.rotation, transform);
				shieldAbilities.enabled = true;
				shieldAbilities.shield = shieldObject;
				shieldObject.SetActive (false);
				UIStatic.singleton.abilityUsedImage.sprite = shield;
				break;
			}
			playerClass = newClass;
			Transform mt = NetworkManager.singleton.GetStartPosition ();
			transform.position = mt.position;
			transform.rotation = mt.rotation;
			controller.SetupThings ();
		}
	}
}