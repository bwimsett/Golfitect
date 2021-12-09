using UnityEngine;

namespace Backend.Managers {

	public class GameSceneManager : MonoBehaviour {
		public BWConsole _console;
		public static BWConsole console;

		void Awake() {
			console = _console;
		}
	}
}