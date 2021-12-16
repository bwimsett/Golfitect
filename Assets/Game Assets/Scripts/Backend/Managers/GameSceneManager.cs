using Steamworks;
using UnityEngine;

namespace Backend.Managers {

	public class GameSceneManager : MonoBehaviour {
		public BWConsole _console;
		public static BWConsole console;
		public ServerManager _serverManager;
		public static ServerManager serverManager;

		void Awake() {
			console = _console;
			serverManager = _serverManager;
		}
	}
}