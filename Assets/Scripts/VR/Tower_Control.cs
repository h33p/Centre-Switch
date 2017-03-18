using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;

namespace VRPlayer {
	public class Tower_Control : NetworkBehaviour {

		public LayerMask mask;
		RaycastHit hit;
		public string tag;

		Transform lastTower;


		public override void OnStartLocalPlayer () {
			Camera.main.gameObject.SetActive (false);
			UIStatic.singleton.canvas.worldCamera = VRCamera.camRoot.cam;
		}

		void Update () {
			if (!isLocalPlayer)
				return;
			
			if (CrossPlatformInputManager.GetButtonDown ("Fire2")) {
				if (Physics.Raycast (VRCamera.camRoot.cam.transform.position, VRCamera.camRoot.cam.transform.forward, out hit, Mathf.Infinity, mask))
					if (hit.collider.tag == tag)
						CmdSwitchToTower (hit.transform.GetComponent<NetworkIdentity> ());
			} if (CrossPlatformInputManager.GetButtonDown ("Cancel"))
				CmdSwitchToInit ();
			
		}

		[Command]
		void CmdSwitchToInit () {
			if (lastTower != null && lastTower != VRStartPoint.start)
				lastTower.GetComponent<Tower_Visuals> ().enabled = false;
			VRCamera.SetToTransform (VRStartPoint.start);
			lastTower = VRStartPoint.start;
		}

		[Command]
		void CmdSwitchToTower (NetworkIdentity tower) {
			if (lastTower != null && lastTower != VRStartPoint.start)
				lastTower.GetComponent<Tower_Visuals> ().enabled = false;
			tower.GetComponent<Tower_Visuals> ().enabled = true;
			VRCamera.SetToTransform (tower.GetComponent<Tower_Visuals> ().cameraView);
			lastTower = tower.transform;
		}
	}
}