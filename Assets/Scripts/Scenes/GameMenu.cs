using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour {

	public static GameMenu instance;

	public AudioClip confirmSound;

	public Text coinText;
	public Text roseText;
	public Text lifeText;

	public Image currentItem;

	public GameObject gameWindow;
	public MenuWindow mainWindow;
	public ItemWindow itemWindow;
	public ItemChoices itemChoices;
	public MapWindow mapWindow;
	public SaveWindow saveWindow;

	void Awake() {
		instance = this;
	}

	void Start() {
		UpdateItem (Player.instance.selectedItem);
	}

	public void ClickItemSound() {
		GameCamera.PlayAudioClip (confirmSound);
	}

	public void CloseMenu() {
		ClickItemSound ();
		mainWindow.gameObject.SetActive (false);
		itemWindow.gameObject.SetActive (false);
		mapWindow.gameObject.SetActive (false);
		saveWindow.gameObject.SetActive (false);
		itemChoices.gameObject.SetActive (false);
		gameWindow.SetActive (true);
	}

	public void MenuButton() {
		ClickItemSound ();
		Player.instance.Pause ();
		gameWindow.gameObject.SetActive (false);
		mainWindow.gameObject.SetActive (true);
	}

	public void ItemSlot() {
		Player.instance.UseItem ();
	}

	// ===============================================================================
	// Game Interface
	// ===============================================================================

	public void UpdateLife(int value) {
		lifeText.text = "x" + value;
	}

	public void UpdateCoins(int value) {
		UpdateCoins (value, coinText);
	}

	public void UpdateCoins(int value, Text text) {
		text.text = "x" + value;
	}

	public void UpdateRoses(int value) {
		roseText.text = "x" + value;
	}

	public void UpdateItem(Item item, Image image) {
		if (item == null) {
			image.gameObject.SetActive (false);
		} else {
			image.gameObject.SetActive (true);
			image.sprite = Resources.Load<Sprite> ("Images/Items/" + item.spriteName);
		}
	}

	public void UpdateItem(Item item) {
		UpdateItem (item, currentItem);
	}

}
