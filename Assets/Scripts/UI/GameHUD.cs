using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour {

	public static GameHUD instance;

	public MainMenu mainMenu;
	public ShopMenu shopMenu;
	public DialogWindow dialog;

	void Awake () {
		instance = this;
	}

	void Start () {
		if (Bag.current.selectedSlot == null)
			UpdateItem (null, 0);
		else
			UpdateItem (Bag.current.selectedItem, Bag.current.selectedSlot.count);
	}

	public void MenuButton () {
		ClickItemSound ();
		Player.instance.Pause ();
		mainMenu.Open ();
		gameObject.SetActive (false);
	}

	public void ItemSlot() {
		Player.instance.UseItem ();
	}

	// ===============================================================================
	// Informações na interface
	// ===============================================================================

	public Text lifeText;
	public Image itemIcon;

	public void UpdateLife(int value) {
		lifeText.text = "x" + value;
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

	public Image miniMap;

	public void UpdateMap () {
		MapWindow.UpdateTexture (miniMap, 1);
	}
		
	// ===============================================================================
	// Sons
	// ===============================================================================

	public AudioClip confirmSound;

	public static void ClickItemSound () {
		GameCamera.PlayAudioClip (instance.confirmSound);
	}
		
}
