using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LocalPlayer : NetworkBehaviour {

	public static LocalPlayer localPlayer;
	public bool vr;

	public override void OnStartLocalPlayer () {
		localPlayer = this;
	}
}
