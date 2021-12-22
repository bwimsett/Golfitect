using Backend.Managers;
using Game_Assets.Scripts.Backend.Server;
using UnityEngine;

namespace Game_Assets.Scripts.GUI.PlayerProfile {
	public class PlayerProfileTrigger : MonoBehaviour {

		public DBUser user;

		public void SetUser(DBUser user) {
			this.user = user;
		}
		
		public void OnMouseOver() {
			GameSceneManager.profileDisplay.SetUser(user);
			GameSceneManager.profileDisplay.SetOrigin(transform);
			GameSceneManager.profileDisplay.Open();
		}

		public void OnMouseExit() {
			GameSceneManager.profileDisplay.Close();
		}
		
	}
}