using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopChoiceWindow : SlotChoiceWindow {

	public Text priceText;
	public Button buyButton;
	public AudioClip buySound;

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
		if (item.consumable) {
			// Procura o item na mochila, e se achar, incrementa
			for (int i = 0; i < Bag.maxItems; i++) {
				Item item2 = Bag.current.GetItem (i);
				if (item2 != null && item2.id == item.id) {
					BuySound ();
					Bag.current.coins -= item.totalPrice;
					Bag.current.Increment(i);
					BackToShopWindow ();
					return;
				}
			}
			// Não achou o item na mochila, então cria slot novo
			for (int i = 0; i < Bag.maxItems; i++) {
				ItemSlot slot = Bag.current.GetSlot (i);
				if (slot == null) {
					BuySound ();
					Bag.current.Add (item, i);
					Bag.current.coins -= item.totalPrice;
					BackToShopWindow ();
					return;
				}
			}
			// Se a mochila estiver cheia
			FullBagError();
			return;
		}
	}

	public void FullBagError () {
		GameHUD.ClickItemSound ();
		gameObject.SetActive (false);
		fullBagMessage.SetActive (true);
	}

	public void FullBagReturn() {
		GameHUD.ClickItemSound ();
		gameObject.SetActive (true);
		fullBagMessage.SetActive (false);
	}

	public void Return () {
		GameHUD.ClickItemSound ();
		BackToShopWindow ();
	}

	public void BackToShopWindow () {
		gameObject.SetActive (false);
		GameHUD.instance.shopMenu.UpdateCoins (Bag.current.coins);
		GameHUD.instance.shopMenu.shopWindow.gameObject.SetActive (true);
	}

	public void BuySound () {
		GameCamera.PlayAudioClip (buySound);
	}

}
