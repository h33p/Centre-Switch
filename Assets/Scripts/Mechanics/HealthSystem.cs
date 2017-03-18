using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class HealthSystem : NetworkBehaviour {

	[SyncVar]
	public float health;

	public GameObject deadObject;

	public override void OnStartLocalPlayer () {
		Debug.Log ("hi");
		UIStatic.singleton.playerHealth = this;
	}

	void Update () {
		if (isServer && transform.position.y < 8f)
			DoDamage (9999f);
	}


	[ClientRpc]
	public void RpcDestroyed () {
		deadObject.SetActive (true);
	}

	[Server]
	public void DoDamage (float dmg) {
		if (health <= 0)
			return;
		health -= dmg;
		Debug.Log (gameObject);

		if (health <= 0) {
			if (transform.tag == "Player") {
				transform.position = NetworkManager.singleton.GetStartPosition ().position;
				GetComponent<GreenByteSoftware.UNetController.Controller> ().SetupThings ();
				health = 100;
			} else
				RpcDestroyed ();
		}
	}
}
