/* (C) Aurimas Blažulionis
 * NetworkManagerReplace component
 * An extended version of NetworkManager component which sends player profile data to the server on connection.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.VR;

public class NetworkManagerReplace : NetworkManager {

	public class NetworkMessage : MessageBase {
		public string nickname;
		public bool VR;
	}

	public bool gotVRPlayer;
	public GameObject vrPrefab;
	public bool forceNoVR;

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader) {
		NetworkMessage message = extraMessageReader.ReadMessage<NetworkMessage>();

		if (message.VR && !gotVRPlayer) {
			gotVRPlayer = true;
			GameObject player = Instantiate (vrPrefab);
			player.GetComponent<Shooting> ().vr = true;
			NetworkServer.AddPlayerForConnection (conn, player, playerControllerId);
		} else if (!message.VR) {
			Transform start = GetStartPosition ();
			Debug.Log (start);
			GameObject player;
			if (start != null)
				player = Instantiate (playerPrefab, start.position, start.rotation);
			else
				player = Instantiate (playerPrefab);
			NetworkServer.AddPlayerForConnection (conn, player, playerControllerId);
		} else {
			conn.Disconnect ();
		}
	}

	public override void OnClientConnect(NetworkConnection conn) {
		NetworkMessage clientData = new NetworkMessage();


		clientData.VR = VRDevice.isPresent && !forceNoVR;

		ClientScene.AddPlayer(conn, 0, clientData);
	}

	public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController pcon) {
		NetworkServer.Destroy (pcon.gameObject);
	}


	public override void OnStartServer () {
		//Enable admin UI
	}

	public override void OnStopServer () {
		//Disable admin UI
	}

	public override void OnMatchList (bool success, string extendedInfo, List<UnityEngine.Networking.Match.MatchInfoSnapshot> matchList) {
		if (success) {
			matches = matchList;
			gameObject.SendMessage ("OnMatchesListed");
		} else
			Debug.LogError (extendedInfo);
	}
}
