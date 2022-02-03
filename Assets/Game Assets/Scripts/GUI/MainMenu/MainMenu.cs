using Backend.Enums;
using Game_Assets.Scripts.GUI.GenericComponent;
using Game_Assets.Scripts.GUI.MainMenu.CourseOverview;
using GUI.MainMenu;
using GUI.MainMenu.CourseSelector;
using GUI.OptionList;
using UnityEngine;

namespace Game_Assets.Scripts.GUI.MainMenu {
	public class MainMenu : MonoBehaviour {
		[SerializeField] private global::GUI.MainMenu.CourseCreator.CourseCreator _courseCreator;
		public static global::GUI.MainMenu.CourseCreator.CourseCreator courseCreator;
		[SerializeField] private MainMenu_OptionList _optionList;
		public static MainMenu_OptionList optionList;
		[SerializeField] private MainMenu_Subwindow_CourseSelector _courseSelector;
		public static MainMenu_Subwindow_CourseSelector courseSelector;

		[SerializeField] private MainMenu_CourseOverview _courseOverview;
		public static MainMenu_CourseOverview courseOverview;
		
		[SerializeField] private MainMenuPersistantUI _persistantUI;
		public static MainMenuPersistantUI persistantUI;

		public static MainMenuLeaf currentLeaf;

		private MainMenuLeaf rootLeaf, buildMenuLeaf, playMenuLeaf, courseBuilderLeaf, courseSelectorLeaf;

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
			currentLeaf = null;
			
			rootLeaf = new MainMenuLeaf(optionList, GenerateMainMenuOptions);
			buildMenuLeaf = new MainMenuLeaf(optionList, GenerateBuildMenuOptions);
			playMenuLeaf = new MainMenuLeaf(optionList, GeneratePlayMenuOptions);

			courseBuilderLeaf = new MainMenuLeaf(courseCreator, ()=>courseCreator.New());
			courseSelectorLeaf = new MainMenuLeaf(courseSelector, () => courseSelector.Open());
			
			rootLeaf.AddChild(buildMenuLeaf);
				buildMenuLeaf.AddChild(courseBuilderLeaf);
				buildMenuLeaf.AddChild(courseSelectorLeaf);
			rootLeaf.AddChild(playMenuLeaf);
			
			rootLeaf.GoTo();
		}
		
		private void GenerateMainMenuOptions() {
			TextButtonCallback[] mainMenuOptions = {
				new TextButtonCallback("main_menu_option_build", () => { GenerateBuildMenuOptions(); }),
				new TextButtonCallback("main_menu_option_play", () => { GeneratePlayMenuOptions(); })
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
					courseSelector.Refresh(true);
					courseSelectorLeaf.SetParent(buildMenuLeaf);
					courseSelector.Open();
				}),
				new TextButtonCallback("build_menu_option_hole_builder", () => {
					LoadingScreenManager.Load(null, GameMode.Build);
				}),
				new TextButtonCallback("mainmenu_title_course_creator", () => {courseCreator.New();})
			};
			
			optionList.SetHeading("mainmenu_title_build");
			optionList.SetOptions(options);
			optionList.Open();
		}

		private void GeneratePlayMenuOptions() {
			courseSelector.SetHeading("play_heading");
			courseSelector.Refresh(false);
			courseSelectorLeaf.SetParent(rootLeaf);
			courseSelector.Open();
		}

		public void Back() {
			currentLeaf.Back();
		}
		
	}
}