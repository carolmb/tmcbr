using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour {

	public static GameMenu instance;

	public Text saveName;
	public Button[] saveButtons;
	public Button[] itemButtons;

	public Text coinText;
	public Text roseText;
	public Text lifeText;

	public Image currentItem;

	public GameObject[] menuWindows;
	public GameObject gameInterface;

	void Awake() {
		instance = this;
	}

	void Start() {
		UpdateSaveButtons ();
		UpdateItemButtons ();
		UpdateItem (Player.instance.selectedItem);
	}

	// ===============================================================================
	// Save Window
	// ===============================================================================

	public void UpdateSaveButtons() {
		SaveManager.LoadSaves ();
		for (int i = 0; i < SaveManager.maxSaves; i++) {
			if (SaveManager.allSaves [i] != null) {
				saveButtons [i].GetComponentInChildren<Text> ().text = SaveManager.allSaves [i].name;
			} else {
				saveButtons [i].GetComponentInChildren<Text> ().text = "Empty";
			}
		}
	}

	public void Save(int id) {
		SaveManager.SaveGame (id, saveName.text);
		UpdateSaveButtons ();
	}

	// ===============================================================================
	// Item Window
	// ===============================================================================

	int beginItemIndex = 0;

	public Button itemLeft;
	public Button itemRight;

	public void UpdateItemButtons() {
		itemLeft.interactable = beginItemIndex > 0;
		itemRight.interactable = beginItemIndex + itemButtons.Length < Bag.maxItems;
			
		for (int i = 0; i < itemButtons.Length; i++) {
			GameObject icon = itemButtons [i].transform.GetChild (0).gameObject;
			Item item = Player.instance.bag.GetItem (i + beginItemIndex);
			if (item != null) {
				itemButtons [i].interactable = true;
				icon.SetActive (true);
				Image img = icon.GetComponent<Image> ();
				img.sprite = Resources.Load<Sprite> ("Images/Items/" + item.spriteName);
			} else {
				itemButtons [i].interactable = false;
				icon.SetActive (false);
			}
		}
	}

	public void ItemLeft() {
		beginItemIndex -= itemButtons.Length;
		UpdateItemButtons ();
	}

	public void ItemRight() {
		beginItemIndex += itemButtons.Length;
		UpdateItemButtons ();
	}

	// ===============================================================================
	// Main window
	// ===============================================================================

	public void Quit() {
		SceneManager.LoadScene ("MainMenu");
	}

	public void CloseMenu() {
		foreach (GameObject go in menuWindows) {
			go.SetActive (false);
		}
		gameInterface.SetActive (true);
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

	public void ClickItemSound() {
		Debug.Log("asd");
		AudioSource audio = GameObject.Find("click_confirm").GetComponent<AudioSource>();
		audio.Play ();
	}
}
