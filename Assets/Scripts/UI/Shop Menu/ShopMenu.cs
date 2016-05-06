using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopMenu : MenuBase {

	public static ShopMenu instance;

	public Text coinText;
	public ShopWindow shopWindow;

	public static int[] itemList;

	void Awake () {
		instance = this;
	}

	public override void Open () {
		base.Open ();
		UpdateCoins (Bag.current.coins);
		ShopMenu.instance.shopWindow.slotSelection = false;
		shopWindow.gameObject.SetActive (true);
	}

	public void UpdateCoins(int value) {
		coinText.text = "x" + value;
	}

}
