using System;
using UnityEngine;

namespace Game {
	public class LevelTimer : MonoBehaviour {

		public float time { get; private set; }
		private bool active;
		
		public void StartTimer() {
			time = 0;
			active = true;
		}
		
		public float StopTimer() {
			active = false;
			return time;
		}

		void Update() {
			if (active) {
				time += Time.deltaTime;
			}
		}

		/// <summary>
		/// Returns a formatted time string from an input time (in seconds)
		/// </summary>
		public static string GetTimeString(float time) {
			TimeSpan timeSpan = TimeSpan.FromSeconds(time);
			string output = timeSpan.ToString(@"mm\:ss");
			return output;
		}
	}
}