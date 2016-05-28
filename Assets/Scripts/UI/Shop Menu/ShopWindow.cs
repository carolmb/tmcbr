using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopWindow : SlotWindow {

	public override Item GetItem(int position) {
		if (position < 0 || position >= ShopMenu.itemList.Length)
			return null;
		if (ShopMenu.itemList [position] == -1)
			return null;
		return Item.DB [ShopMenu.itemList [position]];
	}

	public override int GetCount (int slot) {
		return GetItem (slot).count;
	}

	public override int MaxItems() {
		return ShopMenu.itemList.Length;
	}

	public override bool ButtonEnable(int slot) {
		Item item = GetItem (slot + beginItemIndex);
		if (item.consumable)
			return true;
		foreach(Item i in Bag.current) {
			if (i.id == item.id)
				return false;
		}
		return true;
	}

	public void Return () {
		SoundManager.Click ();
		GameHUD.instance.shopMenu.Close ();
		if (Bag.current.selectedSlot == null) {
			GameHUD.instance.UpdateItem (null, 0);
		} else {
			GameHUD.instance.UpdateItem (Bag.current.selectedItem, Bag.current.selectedSlot.count);
		}
	}

}
