using DefaultNamespace;
using Game_Assets.Scripts.GUI;
using Game_Assets.Scripts.GUI.CourseCreator;
using Game_Assets.Scripts.GUI.GenericComponent;
using Game_Assets.Scripts.GUI.OptionList;
using UnityEngine;

namespace Backend.Managers {
	public class MainMenu : MonoBehaviour {
		[SerializeField] private CourseCreator _courseCreator;
		public static CourseCreator courseCreator;
		[SerializeField] private MainMenu_OptionList _optionList;
		public static MainMenu_OptionList optionList;
		[SerializeField] private MainMenu_Subwindow_CourseSelector _courseSelector;
		public static MainMenu_Subwindow_CourseSelector courseSelector;
		
		[SerializeField] private MainMenuPersistantUI _persistantUI;
		public static MainMenuPersistantUI persistantUI;

		private static MainMenu_Subwindow previousSubwindow;
		private static MainMenu_Subwindow currentSubwindow;
		
		void Awake() {
			Initialise();
		}

		private void Initialise() {
			courseCreator = _courseCreator;
			persistantUI = _persistantUI;
			optionList = _optionList;
			courseSelector = _courseSelector;
			
			GenerateMainMenuOptions();
		}

		private void GenerateMainMenuOptions() {
			TextButtonCallback[] mainMenuOptions = {
				new TextButtonCallback("main_menu_option_build", () => { GenerateBuildMenuOptions(); }),
				new TextButtonCallback("main_menu_option_play", () => { })
			};
			
			optionList.SetHeading("mainmenu_title_main_menu");
			optionList.SetOptions(mainMenuOptions);
			optionList.Open();
		}

		private void GenerateBuildMenuOptions() {
			TextButtonCallback[] options = {
				new TextButtonCallback("build_menu_option_hole", () => { }),
				new TextButtonCallback("build_menu_option_course", () => {
					courseSelector.SetHeading("your_courses_heading");
					courseSelector.Refresh();
					courseSelector.Open();
				}),
				new TextButtonCallback("mainmenu_title_course_creator", () => {courseCreator.Open();})
			};
			
			optionList.SetHeading("mainmenu_title_build");
			optionList.SetOptions(options);
			optionList.Open();
		}

		private void GeneratePlayMenuOptions() {

		}

		public static void SetCurrentSubwindow(MainMenu_Subwindow subwindow) {
			if (currentSubwindow != subwindow) {
				previousSubwindow = currentSubwindow;
			}

			currentSubwindow = subwindow;
		}

		public static MainMenu_Subwindow GetCurrentSubwindow() {
			return currentSubwindow;
		}

		public void Back() {
			if (previousSubwindow == null) {
				return;
			}
			
			previousSubwindow.Open();
		}
		
	}
}