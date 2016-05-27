using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopMenu : MenuBase {

	public Text coinText;
	public ShopWindow shopWindow;

	public static int[] itemList;

	public override void Open () {
		base.Open ();
		UpdateCoins (Bag.current.coins);
		shopWindow.gameObject.SetActive (true);
		GameHUD.instance.gameObject.SetActive (false);
	}

	public void UpdateCoins (int value) {
		coinText.text = "x" + value;
	}

	void Update () {
		if (Input.GetButtonDown ("Menu")) {
			gameObject.SetActive (false);
		}
	}

}
