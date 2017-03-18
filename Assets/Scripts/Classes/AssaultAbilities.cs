using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;

public class AssaultAbilities : NetworkBehaviour {
	float startTime;
	float endTime;
	public float duration;
	public float cooldownTime;
	public GameObject bullet;

	void Update(){
		if (isLocalPlayer) {
			if (CrossPlatformInputManager.GetButtonDown ("Fire1")) {
				GameObject bulletInstance = Instantiate(bullet);
				Destroy (bulletInstance, 3.0f);
			}
		}
	}

	[Command]
	void CmdShoot () {
	
	}

}
