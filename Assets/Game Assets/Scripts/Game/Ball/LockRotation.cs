using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour {

	private Quaternion targetRotation;

	void Awake() {
		targetRotation = Quaternion.Euler(new Vector3(0,0,0));
	}

	void Update() {
		transform.rotation = targetRotation;
	}
	
}
