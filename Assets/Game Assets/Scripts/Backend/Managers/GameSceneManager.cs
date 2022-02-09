using Game_Assets.Scripts.GUI;
using Steamworks;
using UnityEngine;

namespace Backend.Managers {

	public class GameSceneManager : MonoBehaviour {
		public BWConsole _console;
		public static BWConsole console;
		public ServerManager _serverManager;
		public static ServerManager serverManager;
		public GUIColorBank _colorBank;
		public static GUIColorBank colorBank;
		public PlayerProfileDisplay _profileDisplay;
		public static PlayerProfileDisplay profileDisplay;
		public PopupManager _popupManager;
		public static PopupManager popupManager;
		
		void Awake() {
			console = _console;
			serverManager = _serverManager;
			colorBank = _colorBank;
			profileDisplay = _profileDisplay;
			popupManager = _popupManager;
		}
	}
}