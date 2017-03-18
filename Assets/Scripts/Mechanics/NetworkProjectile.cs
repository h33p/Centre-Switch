using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkProjectile : NetworkBehaviour {

	public Weapon weapon;

	private float startTime;
	public float timeout = 10f;

	public GameObject explosionPrefab;

	[SyncVar]
	public float movementSpeed = 50f;

	void Update () {

		RaycastHit hit;

		if (isServer && Physics.Raycast (transform.position, transform.forward, out hit)) {

			transform.Translate (transform.forward * movementSpeed * Time.deltaTime);

			if (Vector3.Dot ((hit.point - transform.position).normalized, transform.forward) <= 0) {
				RpcExplode (hit.point);
				NetworkServer.Destroy (gameObject);
			}
		} else {
			transform.Translate (transform.forward * movementSpeed * Time.deltaTime);
		}

		if (isServer && timeout < Time.time - startTime) {
			RpcExplode (transform.position);
			NetworkServer.Destroy (gameObject);
		}
	}

	[ClientRpc]
	void RpcExplode (Vector3 pos) {
		GameObject.Instantiate (explosionPrefab, pos, new Quaternion (0, 0, 0, 1));
	}
}
