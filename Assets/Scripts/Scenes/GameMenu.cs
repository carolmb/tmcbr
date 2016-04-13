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

	void Awake() {
		instance = this;
	}

	void Start() {
		UpdateSaveButtons ();
		UpdateItemButtons ();
		UpdateItem (Player.instance.selectedItem);
	}

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

	public void UpdateItemButtons() {
		for (int i = 0; i < Bag.maxItems; i++) {
			int id = Player.instance.bag.itemIDs [i];
			GameObject icon = itemButtons [i].transform.GetChild (0).gameObject;
			if (id > -1) {
				itemButtons [i].interactable = true;
				icon.SetActive (true);
				Image img = icon.GetComponent<Image> ();
				img.sprite = Resources.Load<Sprite> ("Images/" + Item.DB [id].spriteName);
			} else {
				itemButtons [i].interactable = false;
				icon.SetActive (false);
			}
		}
	}

	public void Save(int id) {
		SaveManager.SaveGame (id, saveName.text);
		UpdateSaveButtons ();
	}

	public void Quit() {
		SceneManager.LoadScene ("MainMenu");
	}

	public void CloseMenu() {
		// TODO
	}

	public void CloseAll() {
		// TODO
	}

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
			currentItem.sprite = Resources.Load<Sprite> ("Images/" + item.spriteName);
		}
	}
}
