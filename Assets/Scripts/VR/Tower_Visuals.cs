using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace VRPlayer {
	public class Tower_Visuals : NetworkBehaviour {

		[SyncVar]
		public bool enabled;

		public Transform xTransform;
		public Transform yTransform;
		public Transform zTransform;

		public Transform cameraView;

		void Update () {
			if (enabled && VRCamera.camRoot != null) {
				if (xTransform != null)
					xTransform.rotation = Quaternion.Euler (VRCamera.camRoot.cam.transform.rotation.eulerAngles.x, xTransform.rotation.eulerAngles.y, xTransform.rotation.eulerAngles.z);
				if (yTransform != null)
					yTransform.rotation = Quaternion.Euler (yTransform.rotation.eulerAngles.x, VRCamera.camRoot.cam.transform.rotation.eulerAngles.y, yTransform.rotation.eulerAngles.z);
				if (zTransform != null)
					zTransform.rotation = Quaternion.Euler (zTransform.rotation.eulerAngles.x, zTransform.rotation.eulerAngles.y, VRCamera.camRoot.cam.transform.rotation.eulerAngles.z);
			}
		}
	}
}