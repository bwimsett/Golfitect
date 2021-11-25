using System;
using System.Collections;
using System.Collections.Generic;
using Game_Assets.Scripts.GUI.ReorderableList;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UI;

public class GUIReorderableList : MonoBehaviour {

	private List<GUIReorderableList_Item> items;
	[SerializeField] private Transform listContainer;
	[SerializeField] private VerticalLayoutGroup verticalLayoutGroup;
	[SerializeField] private GameObject listItemPrefab, listBlankPrefab;
	
	private RectTransform blankItem;
	private int blankIndex, originalItemIndex;
	private int startingItemsInList;

	private float heightPerItem;

	void Awake() {
		items = new List<GUIReorderableList_Item>();
		startingItemsInList = transform.childCount;
		
		listItemPrefab.gameObject.SetActive(false);
		listBlankPrefab.gameObject.SetActive(false);

		for (int i = 0; i < 18; i++) {
			AddItem(i);
		}
	}
	
	public void AddItem(object obj, bool allowDuplicates = false) {
		if (!allowDuplicates && ContainsItem(obj)) {
			return;
		}

		GUIReorderableList_Item newItem = Instantiate(listItemPrefab, listContainer).GetComponent<GUIReorderableList_Item>();
		newItem.gameObject.SetActive(true);

		if (heightPerItem.Equals(0)) {
			heightPerItem = newItem.GetComponent<RectTransform>().rect.height + verticalLayoutGroup.spacing;
		}
		
		items.Add(newItem);
		
		newItem.SetList(this, items.Count-1);
		newItem.SetItem(obj);
	}

	public bool ContainsItem(object obj) {
		foreach (GUIReorderableList_Item item in items) {
			if (item.Equals(obj)) {
				return true;
			}
		}

		return false;
	}

	public void PickupItem(GUIReorderableList_Item item) {
		// Instantiate the blank prefab at the given index
		if (!blankItem) {
			blankItem = Instantiate(listBlankPrefab, listContainer).GetComponent<RectTransform>();
		}

		originalItemIndex = item.index;
		blankItem.SetSiblingIndex(item.index+startingItemsInList);
		blankItem.gameObject.SetActive(true);
	}

	public void DropItem(GUIReorderableList_Item item) {
		int newIndex = blankIndex;

		if (blankIndex > originalItemIndex) {
			newIndex = blankIndex - 1;
		}

		items.Remove(item);
		items.Insert(newIndex, item);
		
		string indexString = "";
		foreach (GUIReorderableList_Item itemx in items) {
			indexString += itemx.obj + " ";
		}
		Debug.Log(indexString);
		
		Destroy(blankItem.gameObject);
		item.transform.SetSiblingIndex(blankIndex+startingItemsInList);
		item.index = newIndex;
	}

	public void SetBlankIndex(int index) {
		blankIndex = index;
		blankItem.SetSiblingIndex(index+startingItemsInList);
	}

	public int GetIndexAtAnchorPosition(Vector2 localPosition) {
		// Offset for top padding
		localPosition = localPosition - new Vector2(0, verticalLayoutGroup.padding.top);

		int index = Mathf.Min(items.Count,Mathf.Max(0,Mathf.RoundToInt(localPosition.y / heightPerItem)));

		return index;
	}

}
