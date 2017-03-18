using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAndRotate : MonoBehaviour {

	public Transform target;

	RaycastHit hit;

	public LayerMask layerMask;

	void Update () {
		transform.position = target.position;
		transform.rotation = target.rotation;

		if (Physics.Raycast (transform.position, -Vector3.up, out hit, Mathf.Infinity, layerMask)) {

			transform.rotation = Quaternion.FromToRotation (transform.up, hit.normal) * transform.rotation;

		}
	}
}
