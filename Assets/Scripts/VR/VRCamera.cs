using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRPlayer {
	public class VRCamera : MonoBehaviour {

		public static VRCamera camRoot;
		public Camera cam;

		void Awake () {
			camRoot = this;
		}

		public static void SetToTransform (Transform trans) {
			camRoot.transform.position = trans.position;
			camRoot.transform.rotation = trans.rotation;

		}
	}

}