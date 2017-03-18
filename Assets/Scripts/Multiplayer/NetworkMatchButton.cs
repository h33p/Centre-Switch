/* (C) Aurimas Blažulionis
 * NetworkMatchButton component
 * Calls NetworkManagerGUI to join a specific match
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkMatchButton : MonoBehaviour {

	public void Click () {
		NetworkManagerGUI.singleton.JoinMatch (gameObject);
	}
}
