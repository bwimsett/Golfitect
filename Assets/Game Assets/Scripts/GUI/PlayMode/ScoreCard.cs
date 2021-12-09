using Backend.Managers;
using Game_Assets.Scripts.GUI.PlayMode;
using UnityEngine;

namespace Game_Assets.Scripts.GUI.GenericComponent {
	public class ScoreCard : MonoBehaviour {

		[SerializeField] private GameObject scoreCardScorePrefab;
		public ScoreCard_Score[] scoreItems;
		[SerializeField] private Transform scoreItemsContainer;

		public void Refresh() {
			Clear();
			scoreItems = new ScoreCard_Score[GameManager.courseTracker.course.holes.Length];
			
			scoreCardScorePrefab.gameObject.SetActive(false);

			for (int i = 0; i < scoreItems.Length; i++) {
				scoreItems[i] = Instantiate(scoreCardScorePrefab, scoreItemsContainer).GetComponent<ScoreCard_Score>();
			
				scoreItems[i].gameObject.SetActive(true);
				
				if (i >= GameManager.courseTracker.currentHoleIndex) {
					scoreItems[i].Clear();
					continue;
				}
				
				scoreItems[i].SetScore(GameManager.courseTracker.GetScoreForHole(i), GameManager.courseTracker.course.holes[i].par);
			}

		}

		private void Clear() {
			if (scoreItems == null) {
				return;
			}

			foreach (ScoreCard_Score score in scoreItems) {
				Destroy(score.gameObject);
			}
		}
		
		
		
	}
}