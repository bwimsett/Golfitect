using System.Diagnostics.Contracts;
using Backend.Managers;
using BWLocaliser;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Game_Assets.Scripts.GUI {
	[RequireComponent(typeof(CanvasGroup))]
	public class MainMenu_Subwindow : MonoBehaviour {
		
		[SerializeField] private CanvasGroup _canvasGroup;

		[Header("GUI Elements")] [SerializeField]
		private TextLocalizer heading;
		[ShowIf("heading"), SerializeField] private string titleID;

		void Awake() {
			_canvasGroup = GetComponent<CanvasGroup>();
			if (heading) {
				heading.SetString(new LocString(titleID));
			}
		}
		
		public void Open() {
			if (MainMenu.GetCurrentSubwindow()) {
				MainMenu.GetCurrentSubwindow().Close();
			}

			MainMenu.SetCurrentSubwindow(this);
			_canvasGroup.DOFade(1, 0.25f).SetDelay(0.25f);
		}

		public void SetHeading(string id) {
			this.titleID = id;
			heading.SetString(new LocString(id));
		}

		public void Close() {
			_canvasGroup.DOFade(0, 0.25f);
		}
		
	}
}