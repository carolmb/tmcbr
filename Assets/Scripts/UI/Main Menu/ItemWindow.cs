using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemWindow : MonoBehaviour {

	public bool slotSelection = false;

	public SlotChoiceWindow choiceWindow;
	public Button[] itemButtons;

	protected int beginItemIndex = 0;

	public Button itemLeft;
	public Button itemRight;

	public virtual Item GetItem(int slot) {
		return Bag.current.GetItem (slot);
	}

	public virtual int GetCount(int slot) {
		return Bag.current.GetSlot (slot).count;
	}

	public virtual int MaxItems() {
		return Bag.maxItems;
	}

	public virtual bool ButtonEnable(int i) {
		return true;
	}

	public void OnEnable () {
		SetButtons ();
		if (Bag.current.selectedSlot == null) {
			MainMenu.instance.UpdateItem (null, 0);
		} else {
			MainMenu.instance.UpdateItem (Bag.current.selectedItem, Bag.current.selectedSlot.count);
		}
	}

	protected void SetButtons() {
		itemLeft.interactable = beginItemIndex > 0;
		itemRight.interactable = beginItemIndex + itemButtons.Length < MaxItems();

		for (int i = 0; i < itemButtons.Length; i++) {
			GameObject icon = itemButtons [i].transform.GetChild (0).gameObject;
			Item item = GetItem (i + beginItemIndex);
			if (item != null) {
				itemButtons [i].interactable = ButtonEnable(i);
				SetItemButton (icon, item, i + beginItemIndex);
			} else {
				itemButtons [i].interactable = false;
				icon.SetActive (false);
			}
		}
	}

	void SetItemButton(GameObject icon, Item item, int slot) {
		icon.SetActive (true);
		Image itemIcon = icon.GetComponent<Image> ();

		if (item == null) {
			itemIcon.gameObject.SetActive (false);
		} else {
			int count = GetCount (slot);
			itemIcon.gameObject.SetActive (true);
			itemIcon.sprite = Resources.Load<Sprite> ("Images/Items/" + item.spriteName);
			Text t = itemIcon.GetComponentInChildren<Text> ();
			t.text = item.consumable ? "x" + count : "";
		}
	}

	public void OnDisable () {
		choiceWindow.gameObject.SetActive (false);
	}

	public void Left() {
		GameHUD.ClickItemSound ();
		beginItemIndex -= itemButtons.Length;
		OnEnable ();
	}

	public void Right() {
		GameHUD.ClickItemSound ();
		beginItemIndex += itemButtons.Length;
		OnEnable ();
	}

	public void ItemButton(int i) {
		if (slotSelection) {

		} else {
			GameHUD.ClickItemSound ();
			choiceWindow.position = i + beginItemIndex;
			gameObject.SetActive (false);
			choiceWindow.gameObject.SetActive (true);
		}
	}

	public virtual void Return() {
		GameHUD.ClickItemSound ();
		if (slotSelection) {
			choiceWindow.gameObject.SetActive (true);
		} else {
			MainMenu.instance.mainWindow.gameObject.SetActive (true);
		}
		gameObject.SetActive (false);
	}

}
