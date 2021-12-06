using Backend.Managers;
using Game;
using Sirenix.Serialization;
using UnityEngine;

namespace Backend.Level {
	public class Tee : LevelObject {

		public GameObject ballPrefab;
		public GameObject placeholder;

		protected override void LevelObjectEnterPlayMode() {
			placeholder.gameObject.SetActive(false);
			
			// Instantiate a ball if one does not already exist
			if (GameManager.currentLevel.ball) {
				return;
			}
			
			Ball ball = Instantiate(ballPrefab, LevelManager.levelObjectUtility.levelContainer).GetComponent<Ball>();
			GameManager.currentLevel.ball = ball;
			ball.transform.position = transform.position + new Vector3(0,5,0);
		}
	}
}