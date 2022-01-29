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

		private string[] courseIDs;

		public void SetOptions(DBObject[] optionObjects, MainMenu_Subwindow subwindow) {
			this.subwindow = subwindow;
			courseIDs = null;
			
			// Populate items array
			int numPerPage = layout.x * layout.y;
			pages = Mathf.CeilToInt(((float) optionObjects.Length / numPerPage));
			
			items = new DBObject[pages * numPerPage];
			int levelIndex = 0;

			for (int page = 0; page < pages; page++) {
				for (int slot = 0; slot < numPerPage; slot++) {
					if (levelIndex >= optionObjects.Length) {
						break;
					}

					items[GetItemIndexFromPage(page, slot)] = optionObjects[levelIndex];
					levelIndex++;
				}
			}

			// Set page to 0
			page = 0;
			_pageNavigation.SetNavigatable(this);

			Refresh();
		}

		public void SetCourseIDs(string[] courseIDs) {
			this.courseIDs = courseIDs;
			items = new DBObject[courseIDs.Length];
			pages = Mathf.CeilToInt((float)courseIDs.Length / (layout.x * layout.y));
			page = 0;
			_pageNavigation.SetNavigatable(this);
			LoadCurrentPageFromCourseIDs();
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
		
		private void LoadCurrentPageFromCourseIDs() {
			int itemsPerPage = layout.x * layout.y;
			int startingIndex = itemsPerPage * page;
			int endIndex = Mathf.Min(courseIDs.Length - 1, startingIndex + itemsPerPage);

			string[] courseids = new string[endIndex - startingIndex];

			for (int i = 0; i < courseids.Length; i++) {
				courseids[i] = courseIDs[startingIndex+i];
			}
			
			GameSceneManager.serverManager.GetCourses(courseids, result => {
				for (int i = 0; i < courseids.Length; i++) {
					items[startingIndex+i] = result[i];
				}
				
				Refresh();
			});
		}

		private Vector2 GetOptionPosition(Vector2Int index, Vector2 optionDimensions) {
			float xPos = index.x * optionDimensions.x + index.x * padding;
			float yPos = index.y * optionDimensions.y + index.y * padding;
			return new Vector2(xPos, -yPos);
		}

		public void NextPage() {
			page = Mathf.Min(pages, page+1);
			LoadCurrentPageFromCourseIDs();
		}

		public void PrevPage() {
			page = Mathf.Max(0, page-1);
			LoadCurrentPageFromCourseIDs();
		}

		public void SetPage(int page) {
			this.page = Mathf.Max(0, Mathf.Min(page, pages));
			LoadCurrentPageFromCourseIDs();
		}
	}
}
