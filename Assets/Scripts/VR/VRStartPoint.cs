using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRStartPoint : MonoBehaviour {

	public static Transform start;

	void Awake () {
		start = transform;
	}

}
