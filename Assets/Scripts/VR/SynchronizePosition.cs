using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SynchronizePosition : NetworkBehaviour {

	public Vector3 sPos;
	public Quaternion sRot;

	public Vector3 ePos;
	public Quaternion eRot;

	public Transform target;

	public float rate;
	private float startTime;

	void Start () {
		sRot = target.localRotation;
		eRot = target.localRotation;

	}

	void FixedUpdate () {

		if (isLocalPlayer) {
			if (Time.fixedTime - startTime >= rate) {
				startTime = Time.fixedTime;
				CmdSendInp (target.localPosition, target.localRotation);
			}
		} else {
			target.localPosition = Vector3.Lerp (sPos, ePos, (Time.fixedTime - startTime) / rate);
			target.localRotation = Quaternion.Lerp (sRot, eRot, (Time.fixedTime - startTime) / rate);

		}

	}

	[Command]
	void CmdSendInp (Vector3 pos, Quaternion rot) {
		Debug.Log ("ddd");
		RpcSendInp (pos, rot);
	}

	[ClientRpc]
	void RpcSendInp (Vector3 pos, Quaternion rot) {
		Debug.Log ("gfgd");
		if (isLocalPlayer)
			return;
		sPos = target.localPosition;
		sRot = target.localRotation;
		ePos = pos;
		eRot = rot;
		startTime = Time.fixedTime;
	}
}
