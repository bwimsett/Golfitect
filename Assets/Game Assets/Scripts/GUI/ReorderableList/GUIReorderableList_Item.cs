using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game_Assets.Scripts.GUI.ReorderableList {
	[RequireComponent(typeof(LayoutElement))]
	public class GUIReorderableList_Item : MonoBehaviour {

		public object obj { get; private set; }

		private GUIReorderableList list;
		private LayoutElement layoutElement;
		private Camera camera;
		private bool pickedUp;
		private RectTransform _transform;

		public int index;

		private Vector2 mousePosOnPickup, posOnPickup;

		void Update() {
			if (!pickedUp) {
				return;
			}

			Vector2 mousePos = Input.mousePosition;

			float mouseYDifference = mousePosOnPickup.y - mousePos.y;

			_transform.anchoredPosition = new Vector2(posOnPickup.x, posOnPickup.y - mouseYDifference);
			int index = list.GetIndexAtAnchorPosition(-_transform.anchoredPosition);
			list.SetBlankIndex(index);
		}
		
		public void SetList(GUIReorderableList list, int index) {
			this.index = index;
			this.list = list;
			_transform = GetComponent<RectTransform>();
			camera = Camera.main;
			layoutElement = GetComponent<LayoutElement>();
		}
		
		public virtual void SetItem(object obj) {
			this.obj = obj;
		}

		public void PickUp() {
			layoutElement.ignoreLayout = true;
			pickedUp = true;
			mousePosOnPickup = Input.mousePosition;
			posOnPickup = _transform.anchoredPosition;
			list.PickupItem(this);
		}

		public void Drop() {
			if (!pickedUp) {
				return;
			}
			
			pickedUp = false;
			layoutElement.ignoreLayout = false;
			list.DropItem(this);
		}
		
	}
}