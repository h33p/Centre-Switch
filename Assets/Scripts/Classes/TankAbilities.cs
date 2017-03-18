using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;

public class TankAbilities : NetworkBehaviour {
	public float startTime;
	public float endTime;
	public float duration;
	public float cooldownTime;
	public GameObject shield;
	public bool enable = false;

	void Update(){
		if (isLocalPlayer) {
			if (CrossPlatformInputManager.GetButtonDown ("Fire2")) {
				CmdShield (!shield.activeSelf);
			}
		} 

		if (isServer) {
			if (Time.time - startTime > duration) {
				RpcShield (false);
			}
		}
	}

	[Command]
	void CmdShield (bool enable) {
		if (enable && Time.time - endTime > cooldownTime) {
			RpcShield (true);
			startTime = Time.time;
		} else if (!enable) {
			RpcShield (false);
			endTime = Time.time - (cooldownTime * (1 - (Time.time - startTime) / duration));
		}
	}

	[ClientRpc]
	void RpcShield (bool enabled) {
		shield.SetActive (enabled);
		enable = enabled;
	}
}
