using Game;
using TMPro;
using UnityEngine;

namespace Game_Assets.Scripts.GUI.PlayMode {
	public class LevelTimerDisplay : MonoBehaviour {

		public LevelTimer timer;
		public TextMeshProUGUI text;

		void Update() {
			text.text = LevelTimer.GetTimeString(timer.time);
		}

	}
}