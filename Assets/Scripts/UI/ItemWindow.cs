using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemWindow : MonoBehaviour {

	public static bool moveAction = false;

	public Button[] itemButtons;

	int beginItemIndex = 0;

	public Button itemLeft;
	public Button itemRight;

	public Image itemIcon;

	public void OnEnable() {
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

		GameMenu.instance.UpdateItem (Player.instance.selectedItem, itemIcon);
	}

	public void ItemLeft() {
		GameMenu.instance.ClickItemSound ();
		beginItemIndex -= itemButtons.Length;
		OnEnable ();
	}

	public void ItemRight() {
		GameMenu.instance.ClickItemSound ();
		beginItemIndex += itemButtons.Length;
		OnEnable ();
	}

	public void ItemButton(int i) {
		if (moveAction) {
			// TODO
		} else {
			GameMenu.instance.ClickItemSound ();
			ItemChoices.slot = i + beginItemIndex;
			GameMenu.instance.itemChoices.gameObject.SetActive (true);
			gameObject.SetActive (false);
		}
	}

	public void Return() {
		GameMenu.instance.ClickItemSound ();
		if (moveAction) {
			GameMenu.instance.itemChoices.gameObject.SetActive (true);
		} else {
			GameMenu.instance.mainWindow.gameObject.SetActive (true);
		}
		gameObject.SetActive (false);
	}

}
