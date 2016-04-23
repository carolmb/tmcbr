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
	public MapWindow mapWindow;
	public SaveWindow saveWindow;

	void Awake() {
		instance = this;
	}

	void Start() {
		saveWindow.UpdateSaveButtons ();
		itemWindow.UpdateItemButtons ();
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
		coinText.text = "x" + value;
	}

	public void UpdateRoses(int value) {
		roseText.text = "x" + value;
	}

	public void UpdateItem(Item item) {
		if (item == null) {
			currentItem.gameObject.SetActive (false);
		} else {
			currentItem.gameObject.SetActive (true);
			currentItem.sprite = Resources.Load<Sprite> ("Images/Items/" + item.spriteName);
		}
	}

}
