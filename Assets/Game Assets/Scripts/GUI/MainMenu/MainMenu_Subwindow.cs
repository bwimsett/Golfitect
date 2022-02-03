using System;
using System.Diagnostics.Contracts;
using Backend.Managers;
using BWLocaliser;
using DG.Tweening;
using Sirenix.OdinInspector;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace GUI.MainMenu {
	[RequireComponent(typeof(CanvasGroup))]
	public class MainMenu_Subwindow : MonoBehaviour {
		
		[SerializeField] private CanvasGroup _canvasGroup;

		[Header("GUI Elements")] [SerializeField]
		private TextLocalizer heading;
		[ShowIf("heading"), SerializeField] private string titleID;

		public MainMenuLeaf leaf { private get; set; }

		void Awake() {
			_canvasGroup = GetComponent<CanvasGroup>();
			if (heading) {
				heading.SetString(new LocString(titleID));
			}
		}
		
		public void Open() {
			if (leaf == null) {
				throw new Exception("No leaf set for subwindow. Cannot navigate without one: " + gameObject.name);
			}
			
			Game_Assets.Scripts.GUI.MainMenu.MainMenu.currentLeaf?.Close();
			Game_Assets.Scripts.GUI.MainMenu.MainMenu.currentLeaf = leaf;
			_canvasGroup.DOFade(1, 0.25f).SetDelay(0.25f).OnPlay(Refresh).OnComplete(() => {
				_canvasGroup.interactable = _canvasGroup.blocksRaycasts = true;
			});
		}

		protected virtual void Refresh() {
			heading.SetString(new LocString(titleID));
		}

		public void SetHeading(string id) {
			this.titleID = id;
		}

		public void Close() {
			_canvasGroup.interactable = _canvasGroup.blocksRaycasts = false;
			Tween tween = _canvasGroup.DOFade(0, 0.25f);
		}

		public virtual void Back() {
			
		}
		
	}
}