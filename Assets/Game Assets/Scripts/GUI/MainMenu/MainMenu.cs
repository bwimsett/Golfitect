using Backend.Enums;
using Game_Assets.Scripts.GUI;
using Game_Assets.Scripts.GUI.CourseCreator;
using Game_Assets.Scripts.GUI.GenericComponent;
using Game_Assets.Scripts.GUI.MainMenu.CourseOverview;
using Game_Assets.Scripts.GUI.OptionList;
using GUI.MainMenu.CourseSelector;
using GUI.OptionList;
using UnityEngine;

namespace GUI.MainMenu {
	public class MainMenu : MonoBehaviour {
		[SerializeField] private CourseCreator.CourseCreator _courseCreator;
		public static CourseCreator.CourseCreator courseCreator;
		[SerializeField] private MainMenu_OptionList _optionList;
		public static MainMenu_OptionList optionList;
		[SerializeField] private MainMenu_Subwindow_CourseSelector _courseSelector;
		public static MainMenu_Subwindow_CourseSelector courseSelector;

		[SerializeField] private MainMenu_CourseOverview _courseOverview;
		public static MainMenu_CourseOverview courseOverview;
		
		[SerializeField] private MainMenuPersistantUI _persistantUI;
		public static MainMenuPersistantUI persistantUI;

		public static MainMenuLeaf currentLeaf;

		void Awake() {
			Initialise();
		}

		private void Initialise() {
			courseCreator = _courseCreator;
			persistantUI = _persistantUI;
			optionList = _optionList;
			courseSelector = _courseSelector;
			courseOverview = _courseOverview;

			GenerateMainMenuTree();
		}

		/// <summary>
		/// Leaves govern the overall navigation structure of the menu. Allows forward and back movement between screens, even when screens are procedural.
		/// </summary>
		private void GenerateMainMenuTree() {
			MainMenuLeaf root = new MainMenuLeaf(optionList, GenerateMainMenuOptions);
			MainMenuLeaf buildMenu = new MainMenuLeaf(optionList, GenerateBuildMenuOptions);
			MainMenuLeaf playMenu = new MainMenuLeaf(optionList, GeneratePlayMenuOptions);

			MainMenuLeaf courseBuilderLeaf = new MainMenuLeaf(courseCreator, ()=>courseCreator.Open());
			MainMenuLeaf courseSelectorLeaf = new MainMenuLeaf(courseSelector, () => courseSelector.Open());
			
			root.AddChild(buildMenu);
				buildMenu.AddChild(courseBuilderLeaf);
				buildMenu.AddChild(courseSelectorLeaf);
			root.AddChild(playMenu);
			
			
			root.GoTo();
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
				new TextButtonCallback("build_menu_option_hole_builder", () => {
					LoadingScreenManager.Load(null, GameMode.Build);
				}),
				new TextButtonCallback("mainmenu_title_course_creator", () => {courseCreator.Open();})
			};
			
			optionList.SetHeading("mainmenu_title_build");
			optionList.SetOptions(options);
			optionList.Open();
		}

		private void GeneratePlayMenuOptions() {

		}

		public void Back() {
			currentLeaf.Back();
		}
		
	}
}