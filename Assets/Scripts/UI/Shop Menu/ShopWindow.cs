using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopWindow : ItemWindow {

	public static int[] items;

	public override Item GetItem(int position) {
		if (position < 0 || position >= items.Length)
			return null;
		if (items [position] == -1)
			return null;
		return Item.DB [items [position]];
	}

	public override int GetCount (int slot) {
		return GetItem (slot).count;
	}

	public override int MaxItems() {
		return items.Length;
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

	public override void Return() {
		if (slotSelection) {
			choiceWindow.gameObject.SetActive (true);
			gameObject.SetActive (false);
		} else {
			ShopMenu.instance.Close ();
		}
	}

}
