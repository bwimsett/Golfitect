using System.Collections.Generic;
using Shapes;
using UnityEngine;
using UnityEngine.UI;

namespace Game_Assets.Scripts.GUI.GenericComponent {
	public class PageNavigation : MonoBehaviour {

		private IPageNavigatable nav;

		[SerializeField] private GameObject dotButtonPrefab;
		[SerializeField] private Button prevButton, nextButton;

		private List<PageNavigationDot> navDots;

		public void SetNavigatable(IPageNavigatable nav) {
			if (navDots == null) {
				navDots = new List<PageNavigationDot>();
			}
			this.nav = nav;
			RefreshPages();
			RefreshButtons();
		}

		public void RefreshPages() {
			dotButtonPrefab.gameObject.SetActive(false);
			
			// First clear the current dots
			if (navDots.Count > 0) {
				for (int i = 0; i < navDots.Count; i++) {
					Destroy(navDots[i].gameObject);
				}
			}

			navDots = new List<PageNavigationDot>();

			for (int i = 0; i < nav.pages; i++) {
				PageNavigationDot dot = Instantiate(dotButtonPrefab, transform).GetComponent<PageNavigationDot>();
				dot.gameObject.SetActive(true);
				dot.transform.SetSiblingIndex(dot.transform.parent.childCount-2);
				navDots.Add(dot);
				dot.AssignPage(i, this);
			}
		}
		
		public void Next() {
			navDots[nav.page].Deselect();
			nav.NextPage();
			RefreshButtons();
		}

		public void Prev() {
			navDots[nav.page].Deselect();
			nav.PrevPage();
			RefreshButtons();
		}

		public void SetPage(int page) {
			navDots[nav.page].Deselect();
			nav.SetPage(page);
			RefreshButtons();
		}

		private void RefreshButtons() {
			navDots[nav.page].Select();
			nextButton.interactable = nav.page < nav.pages-1;
			prevButton.interactable = nav.page > 0;
		}

	}
}