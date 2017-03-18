using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UpgradePlayerHook : NetworkBehaviour {
	[SyncVar]
	public bool carrying;
}
