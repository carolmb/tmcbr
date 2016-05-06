using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopChoiceWindow : SlotChoiceWindow {

	public Text priceText;
	public Button buyButton;
	public AudioClip buySound;

	void OnEnable () {
		UpdatePrice (item.price);
		UpdateItem (item, item.count);
		buyButton.interactable = Bag.current.coins >= item.totalPrice;
	}
		
	public void UpdatePrice(int value) {
		priceText.text = "x" + value;
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
			ShopMenu.instance.shopWindow.slotSelection = true;
		}
		Return ();
	}

	public void Return () {
		gameObject.SetActive (false);
		ShopMenu.instance.shopWindow.gameObject.SetActive (true);
	}

	public void BuySound () {
		GameCamera.PlayAudioClip (buySound);
	}

}
