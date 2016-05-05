using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour {

	public static ShopMenu instance;

	public Text coins;
	public ShopWindow shopWindow;
	public ShopChoices shopChoices;

	void Awake() {
		instance = this;
		gameObject.SetActive (false);
	}

	public void OpenShop(int[] itemList) {
		gameObject.SetActive (true);
		ShopWindow.items = itemList;
		GameMenu.instance.ClickItemSound ();
		Player.instance.Pause ();
		GameMenu.instance.gameWindow.gameObject.SetActive (false);
		shopWindow.gameObject.SetActive (true);
		GameMenu.instance.UpdateCoins (SaveManager.currentSave.bag.coins, coins);
	}

	public void CloseMenu() {
		GameMenu.instance.ClickItemSound ();
		shopWindow.gameObject.SetActive (false);
		shopChoices.gameObject.SetActive (false);
		GameMenu.instance.gameWindow.SetActive (true);
		Player.instance.Resume ();
		gameObject.SetActive (false);
	}

}
