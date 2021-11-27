using Game_Assets.Scripts.GUI.CourseCreator;
using UnityEngine;

namespace Backend.Managers {
	public class MainMenuManager : MonoBehaviour {
		[SerializeField] private CourseCreator _courseCreator;
		public static CourseCreator courseCreator;

		void Awake() {
			Initialise();
		}

		private void Initialise() {
			courseCreator = _courseCreator;
		}
	}
}