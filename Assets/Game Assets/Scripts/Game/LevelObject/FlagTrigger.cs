using System;
using System.Collections;
using System.Collections.Generic;
using Backend.Managers;
using Game;
using UnityEngine;

public class FlagTrigger : MonoBehaviour {
	private void OnTriggerEnter(Collider other) {
		Ball ball = other.GetComponent<Ball>();

		if (!ball) {
			return;
		}

		GameManager.currentLevel.SetCompletable(true);
		GameManager.courseTracker.FinishHole();
	}
}
