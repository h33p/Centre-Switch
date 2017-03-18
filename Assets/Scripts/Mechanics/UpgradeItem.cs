using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UpgradeItem : NetworkBehaviour {

	[ServerCallback]
	void OnTriggerEnter (Collider col) {
		if (col.tag == "Player") {
			col.transform.root.GetComponent<UpgradePlayerHook> ().carrying = true;
			NetworkServer.Destroy (gameObject);
		}
	}
}
