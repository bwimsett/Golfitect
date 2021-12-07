using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class FlagTrigger : MonoBehaviour {
	private void OnTriggerEnter(Collider other) {
		Ball ball = other.GetComponent<Ball>();

		if (!ball) {
			return;
		}
		
		LevelManager.FinishLevel();
	}
}
