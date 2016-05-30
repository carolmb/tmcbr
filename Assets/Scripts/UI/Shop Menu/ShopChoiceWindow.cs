using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopChoiceWindow : SlotChoiceWindow {

	public Text priceText;
	public Button buyButton;

	public GameObject fullBagMessage;

	private Item item {
		get { return GameHUD.instance.shopMenu.shopWindow.GetItem (position); }
	}

	void OnEnable () {
		UpdatePrice (item.totalPrice);
		UpdateItem (item, item.count);
		buyButton.interactable = Bag.current.coins >= item.totalPrice;
	}
		
	public void UpdatePrice (int value) {
		priceText.text = "x" + value;
	}

	public void Buy () {
		if (Bag.current.Add (item)) {
			BuySound ();
			Bag.current.coins -= item.totalPrice;
			BackToShopWindow ();
		} else {
			FullBagError ();
		}
	}

	public void FullBagError () {
		SoundManager.Click ();
		gameObject.SetActive (false);
		fullBagMessage.SetActive (true);
	}

	public void FullBagReturn() {
		SoundManager.Click ();
		gameObject.SetActive (true);
		fullBagMessage.SetActive (false);
	}

	public void Return () {
		SoundManager.Click ();
		BackToShopWindow ();
	}

	public void BackToShopWindow () {
		gameObject.SetActive (false);
		GameHUD.instance.shopMenu.UpdateCoins (Bag.current.coins);
		GameHUD.instance.shopMenu.shopWindow.gameObject.SetActive (true);
	}

	public void BuySound () {
		SoundManager.Buy ();
	}

}
