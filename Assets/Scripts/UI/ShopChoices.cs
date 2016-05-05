using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopChoices : MonoBehaviour {

	public Button buyButton;
	public Image icon;
	public Text price;
	public AudioClip buySound;

	public static Item item;

	public void SetItem(Item i) {
		item = i;
		price.text = "x" + (item.price) ;
		GameMenu.instance.UpdateItem(item, icon);
		buyButton.interactable = SaveManager.currentSave.bag.coins >= item.totalPrice;
	}

	public void Buy () {
		if (item.consumable) {
			for (int i = 0; i < Bag.maxItems; i++) {
				if (SaveManager.currentSave.bag.GetItem(i).id == item.id) {
					BuySound ();
					SaveManager.currentSave.bag.coins -= item.totalPrice;
					SaveManager.currentSave.bag.Increment(i);
					Return ();
					break;
				}
			}
			SlotChoice();
		}
		Return ();
	}

	public void SlotChoice() {

	}

	public void Return () {
		ShopMenu.instance.shopWindow.gameObject.SetActive (true);
		gameObject.SetActive (false);
	}

	public void BuySound () {
		GameCamera.PlayAudioClip (buySound);
	}

}
