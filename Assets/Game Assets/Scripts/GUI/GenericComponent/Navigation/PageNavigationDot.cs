using System.Runtime.CompilerServices;
using Backend.Managers;
using DG.Tweening;
using MPUIKIT;
using UnityEngine;

namespace Game_Assets.Scripts.GUI.GenericComponent {
	public class PageNavigationDot : MonoBehaviour {

		private PageNavigation pageNavigation;
		private int page;

		[SerializeField] private float selectDuration, deselectDuration;
		[SerializeField] private float selectedSize, deselectedSize;
		[SerializeField] private Ease selectEase, deselectEase;
		[SerializeField] private string selectedColorID, deselectedColorID;
		[SerializeField] private MPImageBasic image;
		[SerializeField] private RectTransform rectTransform;

		private bool selected;
		private Tween colorTween, sizeTween;
		
		public void AssignPage(int page, PageNavigation navigation) {
			this.pageNavigation = navigation;
			this.page = page;
		}

		public void Select() {
			if (selected) {
				return;
			}
			
			selected = true;
			colorTween?.Kill();
			sizeTween?.Kill();
			colorTween = image.DOColor(GameSceneManager.colorBank.GetColor(selectedColorID), selectDuration).SetEase(selectEase);
			sizeTween = rectTransform.DOSizeDelta(new Vector2(selectedSize, selectedSize), selectDuration).SetEase(selectEase);
		}
		
		public void Deselect() {
			if (!selected) {
				return;
			}

			selected = false;
			colorTween?.Kill();
			sizeTween?.Kill();
			colorTween = image.DOColor(GameSceneManager.colorBank.GetColor(deselectedColorID), deselectDuration).SetEase(deselectEase);
			sizeTween = rectTransform.DOSizeDelta(new Vector2(deselectedSize, deselectedSize), deselectDuration).SetEase(deselectEase);
		}

		public void OnClick() {
			pageNavigation.SetPage(page);
			Select();
		}

		
		
	}
}