using System.Collections.Generic;
using Backend.Level;
using Game_Assets.Scripts.Backend.Server;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

namespace Game_Assets.Scripts.GUI.CourseCreator {
	public class CourseCreator_HolesList : MonoBehaviour {

		public GUIReorderableList holeList;
		public RectTransform numbersContainer;

		private List<CourseCreator_HolesList_Number> numbers;
		public GameObject listNumberPrefab;

		void Awake() {
			listNumberPrefab.gameObject.SetActive(false);
			numbers = new List<CourseCreator_HolesList_Number>();
		}
		
		public void AddHoleToList(DBHoleInfo dbHoleInfo) {
			holeList.AddItem(dbHoleInfo);

			// Create a new number to go alongside the hole
			CourseCreator_HolesList_Number newNumber = Instantiate(listNumberPrefab, numbersContainer).GetComponent<CourseCreator_HolesList_Number>();
			numbers.Add(newNumber);
			newNumber.gameObject.SetActive(true);
			newNumber.numberText.text = ""+numbers.Count;
			LayoutRebuilder.ForceRebuildLayoutImmediate(numbersContainer);
		}
		
		public void RemoveHoleFromList(DBHoleInfo steamItemData) {
			holeList.RemoveItem(steamItemData);

			// Update numbers alongside holesInfo
			if (numbers.Count == 0) {
				return;
			}
			
			Destroy(numbers[numbers.Count-1].gameObject);
			numbers.RemoveAt(numbers.Count-1);

			for (int i = 0; i < numbers.Count; i++) {
				numbers[i].numberText.text = "" + (i + 1);
			}
			
			LayoutRebuilder.ForceRebuildLayoutImmediate(numbersContainer);
		}

		public DBHoleInfo[] GetHoles() {
			object[] levelInfos = holeList.GetItems();
			DBHoleInfo[] output = new DBHoleInfo[levelInfos.Length];

			for (int i = 0; i < output.Length; i++) {
				output[i] = (DBHoleInfo) levelInfos[i];
			}

			return output;
		}

	}
}