using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	void Update () {
		transform.Translate (Vector3.forward * 3 * Time.deltaTime);
	}
}
