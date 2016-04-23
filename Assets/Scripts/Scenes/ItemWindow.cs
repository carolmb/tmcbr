using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemWindow : MonoBehaviour {

	public Button[] itemButtons;

	int beginItemIndex = 0;

	public Button itemLeft;
	public Button itemRight;

	public void UpdateItemButtons() {
		itemLeft.interactable = beginItemIndex > 0;
		itemRight.interactable = beginItemIndex + itemButtons.Length < Bag.maxItems;

		for (int i = 0; i < itemButtons.Length; i++) {
			GameObject icon = itemButtons [i].transform.GetChild (0).gameObject;
			Item item = Player.instance.bag.GetItem (i + beginItemIndex);
			if (item != null) {
				itemButtons [i].interactable = true;
				icon.SetActive (true);
				Image img = icon.GetComponent<Image> ();
				img.sprite = Resources.Load<Sprite> ("Images/Items/" + item.spriteName);
			} else {
				itemButtons [i].interactable = false;
				icon.SetActive (false);
			}
		}
	}

	public void ItemLeft() {
		GameMenu.instance.ClickItemSound ();
		beginItemIndex -= itemButtons.Length;
		UpdateItemButtons ();
	}

	public void ItemRight() {
		GameMenu.instance.ClickItemSound ();
		beginItemIndex += itemButtons.Length;
		UpdateItemButtons ();
	}

	public void ItemButton(int i) {
		//GameMenu.instance.ClickItemSound ();
		Player.instance.ChooseItem (i + beginItemIndex);
		GameMenu.instance.CloseMenu ();
	}

	public void Return() {
		GameMenu.instance.ClickItemSound ();
		GameMenu.instance.mainWindow.gameObject.SetActive (true);
		gameObject.SetActive (false);
	}

}
