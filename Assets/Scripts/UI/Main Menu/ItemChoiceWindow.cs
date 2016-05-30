using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemChoiceWindow : SlotChoiceWindow {

	public Image itemDrawing;
	public Button equipButton;
	public Button discardButton;

	private Item item {
		get { return Bag.current.GetItem (position); }
	}

	void OnEnable () {
		UpdateItem (item, bag.GetSlot(position).count);
		equipButton.interactable = item.canEquip;
		discardButton.interactable = item.canDiscard;
	}

	public void Equip () {
		SoundManager.Click ();
		Player.instance.ChooseItem (position);
		GameHUD.instance.mainMenu.Close ();
	}

	public void Visualize () {
		SoundManager.Click ();
		itemDrawing.sprite = Resources.Load<Sprite> ("Images/Items/" + item.spriteName + "(big)");
		itemDrawing.gameObject.SetActive (true);
		gameObject.SetActive (false);
	}

	public void Move () {
		SoundManager.Click ();
		GameHUD.instance.mainMenu.itemWindow.slotSelection = true;
		Return ();
	}

	public void ClosePicture() {
		SoundManager.Click ();
		gameObject.SetActive (true);
		itemDrawing.gameObject.SetActive (false);
	}

	public void Discard () {
		SaveManager.currentSave.bag.Discard (position);
		Return ();
	}

	public void Return () {
		SoundManager.Click ();
		GameHUD.instance.mainMenu.itemWindow.gameObject.SetActive (true);
		gameObject.SetActive (false);
	}

}
