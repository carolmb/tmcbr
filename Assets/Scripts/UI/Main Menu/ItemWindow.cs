using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemWindow : SlotWindow {

	public bool slotSelection = false;

	public override Item GetItem(int slot) {
		return Bag.current.GetItem (slot);
	}

	public override int GetCount(int slot) {
		return Bag.current.GetSlot (slot).count;
	}

	public override int MaxItems() {
		return Bag.maxItems;
	}

	public override bool ButtonEnable(int i) {
		return true;
	}

	public override void OnEnable () {
		base.OnEnable ();
		if (Bag.current.selectedSlot == null) {
			GameHUD.instance.mainMenu.UpdateItem (null, 0);
		} else {
			GameHUD.instance.mainMenu.UpdateItem (Bag.current.selectedItem, Bag.current.selectedSlot.count);
		}
	}

	public override void ItemButton(int i) {
		if (slotSelection) {

		} else {
			base.ItemButton (i);
		}
	}

	public void Return() {
		SoundManager.Click ();
		if (slotSelection) {
			choiceWindow.gameObject.SetActive (true);
		} else {
			GameHUD.instance.mainMenu.mainWindow.gameObject.SetActive (true);
		}
		gameObject.SetActive (false);
	}

}
