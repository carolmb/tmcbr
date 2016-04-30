using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemChoices : MonoBehaviour {

	public Image icon;
	public static int slot;

	public Image itemPicture;
	private Item item;

	public Button equipButton;
	public Button discardButton;

	void OnEnable () {
		item = SaveManager.currentSave.bag.GetItem (slot);
		GameMenu.instance.UpdateItem (item, icon);
		equipButton.interactable = item.canEquip;
		discardButton.interactable = item.canDiscard;
	}

	public void Equip () {
		Player.instance.ChooseItem (slot);
		Return ();
	}

	public void Visualize () {
		GameMenu.instance.ClickItemSound ();
		itemPicture.sprite = Resources.Load<Sprite> ("Images/Items/" + item.spriteName + "(big)");
		itemPicture.gameObject.SetActive (true);
	}

	public void ClosePicture() {
		GameMenu.instance.ClickItemSound ();
		gameObject.SetActive (true);
		itemPicture.gameObject.SetActive (false);
	}

	public void Discard () {
		SaveManager.currentSave.bag.Discard (slot);
		Return ();
	}

	public void Return () {
		GameMenu.instance.ClickItemSound ();
		GameMenu.instance.itemWindow.gameObject.SetActive (true);
		gameObject.SetActive (false);
	}

}
