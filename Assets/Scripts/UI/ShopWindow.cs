using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopWindow : MonoBehaviour {

	public static int[] items;

	public static Item GetItem(int position) {
		if (position < 0 || position >= items.Length)
			return null;
		if (items [position] == -1)
			return null;
		return Item.DB [items [position]];
	}

	public Button[] itemButtons;
	
	int beginItemIndex = 0;
	
	public Button itemLeft;
	public Button itemRight;

	public void OnEnable() {
		itemLeft.interactable = beginItemIndex > 0;
		itemRight.interactable = beginItemIndex + itemButtons.Length < Bag.maxItems;
		
		for (int i = 0; i < itemButtons.Length; i++) {
			GameObject icon = itemButtons [i].transform.GetChild (0).gameObject;
			Item item = GetItem (i + beginItemIndex);
			if (item != null) {
				itemButtons [i].interactable = !AlreadyHave(item);

				icon.SetActive (true);
				Image img = icon.GetComponent<Image> ();
				GameMenu.instance.UpdateItem (item, img);
			} else {
				itemButtons [i].interactable = false;
				icon.SetActive (false);
			}
		}
	}

	public bool AlreadyHave(Item item) {
		if (item.consumable)
			return false;
		foreach(Item i in SaveManager.currentSave.bag) {
			if (i.id == item.id)
				return true;
		}
		return false;
	}
	
	public void ShopLeft() {
		GameMenu.instance.ClickItemSound ();
		beginItemIndex -= itemButtons.Length;
		OnEnable ();
	}
	
	public void ShopRight() {
		GameMenu.instance.ClickItemSound ();
		beginItemIndex += itemButtons.Length;
		OnEnable ();
	}
	
	public void ItemButton(int i) {
		GameMenu.instance.ClickItemSound ();
		ShopMenu.instance.shopChoices.SetItem (GetItem (i + beginItemIndex));
		ShopMenu.instance.shopChoices.gameObject.SetActive (true);
		gameObject.SetActive (false);
	}

	public void Return() {
		GameMenu.instance.CloseMenu ();
		gameObject.SetActive (false);
	}


}
