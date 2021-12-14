using System.Collections.Generic;
using UnityEngine.Events;

namespace GUI.MainMenu {
	public class MainMenuLeaf {

		private List<MainMenuLeaf> children;
		private MainMenuLeaf parent;

		private MainMenu_Subwindow subwindow;
		private UnityAction setupAction;
		
		public MainMenuLeaf(MainMenu_Subwindow subwindow, UnityAction setupAction) {
			this.subwindow = subwindow;
			this.setupAction = setupAction;
			children = new List<MainMenuLeaf>();
			subwindow.leaf = this;
		}
		
		public void AddChild(MainMenuLeaf child) {
			children.Add(child);
			child?.SetParent(this);
		}

		public void SetParent(MainMenuLeaf parent) {
			this.parent = parent;
		}

		public void Back() {
			parent.GoTo();
		}

		public void GoTo() {
			setupAction.Invoke();
		}

		public void Close() {
			subwindow.Close();
		}
		
	}
}