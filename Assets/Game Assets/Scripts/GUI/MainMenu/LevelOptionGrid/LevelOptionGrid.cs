using Backend.Level;
using Backend.Managers;
using Game_Assets.Scripts.Backend.Server;
using Game_Assets.Scripts.GUI.GenericComponent;
using GUI.MainMenu;
using UnityEngine;

namespace GUI.LevelOptionGrid {
	public class LevelOptionGrid : MonoBehaviour, IPageNavigatable {

		[SerializeField] private GameObject optionPrefab;
		[SerializeField] private Vector2Int layout;
		[SerializeField] private float padding;
		[SerializeField] private RectTransform _rectTransform;
		[SerializeField] private PageNavigation _pageNavigation;

		public int page { get; private set; }
		public int pages { get; private set;  }
		private DBObject[] items; // x = page, y = option
		private LevelOptionGrid_Option[] options;
		private MainMenu_Subwindow subwindow;

		private string[] itemIDs;
		private bool isCourses;

		public void SetIDs(string[] courseIDs, bool isCourses, MainMenu_Subwindow subwindow) {
			this.isCourses = isCourses;
			this.itemIDs = courseIDs;
			this.subwindow = subwindow;
			items = new DBObject[courseIDs.Length];
			pages = Mathf.CeilToInt((float)courseIDs.Length / (layout.x * layout.y));
			page = 0;
			_pageNavigation.SetNavigatable(this);
			LoadCurrentPageFromItemIDs();
		}

		public void Refresh() {
			// Generate optionObjects if the array doesn't exist yet
			if (options == null) {
				// First calculate dimensions per option
				Vector2 gridArea = _rectTransform.rect.size;
				Vector2 usableArea = gridArea - padding * new Vector2(layout.x - 1, layout.y - 1);
				Vector2 optionSize = usableArea / layout;

				options = new LevelOptionGrid_Option[layout.x * layout.y];
				int optionIndex = 0;

				for (int y = 0; y < layout.y; y++) {
					for (int x = 0; x < layout.x; x++) {
						options[optionIndex] = Instantiate(optionPrefab, _rectTransform)
							.GetComponent<LevelOptionGrid_Option>();
						// Set position and size
						options[optionIndex].rectTransform.sizeDelta = optionSize;
						options[optionIndex].rectTransform.anchoredPosition =
							GetOptionPosition(new Vector2Int(x, y), optionSize);
						optionIndex++;
					}
				}
			}

			// Set values for the optionObjects
			for (int i = 0; i < options.Length; i++) {
				int index = GetItemIndexFromPage(page, i);
				if (index >= items.Length) {
					options[i].SetObject(null, subwindow);
					continue;
				}
				options[i].SetObject(items[index], subwindow);
			}
		}

		private int GetItemIndexFromPage(int page, int index) {
			int itemsPerPage = layout.x * layout.y;
			int pageOrigin = page * itemsPerPage;

			return pageOrigin + index;
		}
		
		private void LoadCurrentPageFromItemIDs() {
			int itemsPerPage = layout.x * layout.y;
			int startingIndex = itemsPerPage * page;
			int endIndex = Mathf.Min(this.itemIDs.Length, startingIndex + itemsPerPage);

			string[] itemIDs = new string[endIndex - startingIndex];

			for (int i = 0; i < itemIDs.Length; i++) {
				itemIDs[i] = this.itemIDs[startingIndex+i];
			}

			if (isCourses) {
				GameSceneManager.serverManager.GetCourses(itemIDs, result => {
					for (int i = 0; i < itemIDs.Length; i++) {
						items[startingIndex+i] = result[i];
					}
				
					Refresh();
				});
			}
			else {
				GameSceneManager.serverManager.GetHoles(itemIDs, result => {
					for (int i = 0; i < itemIDs.Length; i++) {
						items[startingIndex + i] = result[i];
					}
					
					Refresh();
				});
			}
			
		}

		private Vector2 GetOptionPosition(Vector2Int index, Vector2 optionDimensions) {
			float xPos = index.x * optionDimensions.x + index.x * padding;
			float yPos = index.y * optionDimensions.y + index.y * padding;
			return new Vector2(xPos, -yPos);
		}

		public void NextPage() {
			page = Mathf.Min(pages, page+1);
			LoadCurrentPageFromItemIDs();
		}

		public void PrevPage() {
			page = Mathf.Max(0, page-1);
			LoadCurrentPageFromItemIDs();
		}

		public void SetPage(int page) {
			this.page = Mathf.Max(0, Mathf.Min(page, pages));
			LoadCurrentPageFromItemIDs();
		}
	}
}
