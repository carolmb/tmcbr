using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MenuBase {

	public static MainMenu instance;

	public MainWindow mainWindow;
	public ItemWindow itemWindow;
	public MapWindow mapWindow;
	public SaveWindow saveWindow;

	public Text coinText;
	public Text roseText;

	public Image itemIcon;

	void Awake () {
		instance = this;
	}

	public override void Open () {
		base.Open ();
		UpdateCoins (Bag.current.coins);
		UpdateRoses (Bag.current.roses);
		mainWindow.gameObject.SetActive (true);
		GameHUD.instance.gameObject.SetActive (false);
	}

	public override void Close () {
		base.Close ();
		GameHUD.instance.gameObject.SetActive (true);
	}

	public void UpdateItem(Item item, int count) {
		if (item == null) {
			itemIcon.gameObject.SetActive (false);
		} else {
			itemIcon.gameObject.SetActive (true);
			itemIcon.sprite = Resources.Load<Sprite> ("Images/Items/" + item.spriteName);
			Text t = itemIcon.GetComponentInChildren<Text> ();
			t.text = item.consumable ? "x" + count : "";
		}
	}

	public void UpdateRoses(int value) {
		roseText.text = "x" + value;
	}

	public void UpdateCoins(int value) {
		coinText.text = "x" + value;
	}

}
