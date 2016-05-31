using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class SlotWindow : WindowBase {

	public SlotChoiceWindow choiceWindow;
	public Button[] itemButtons;

	protected int beginItemIndex = 0;

	public Button itemLeft;
	public Button itemRight;

	public abstract Item GetItem (int i);
	public abstract int GetCount (int slot);
	public abstract int MaxItems ();
	public abstract bool ButtonEnable (int i);

	protected override void OnEnable () {
		SetButtons ();
		base.OnEnable ();
	}

	protected void SetButtons() {
		itemLeft.interactable = beginItemIndex > 0;
		itemRight.interactable = beginItemIndex + itemButtons.Length < MaxItems();

		for (int i = 0; i < itemButtons.Length; i++) {
			GameObject icon = itemButtons [i].transform.GetChild (0).gameObject;
			Item item = GetItem (i + beginItemIndex);
			if (item != null) {
				itemButtons [i].interactable = ButtonEnable(i);
				SetItemButton (icon, item, i + beginItemIndex);
			} else {
				itemButtons [i].interactable = false;
				icon.SetActive (false);
			}
		}
	}

	void SetItemButton(GameObject icon, Item item, int slot) {
		icon.SetActive (true);
		Image itemIcon = icon.GetComponent<Image> ();

		if (item == null) {
			itemIcon.gameObject.SetActive (false);
		} else {
			int count = GetCount (slot);
			itemIcon.gameObject.SetActive (true);
			itemIcon.sprite = Resources.Load<Sprite> ("Images/Items/" + item.spriteName);
			Text t = itemIcon.GetComponentInChildren<Text> ();
			t.text = item.consumable ? "x" + count : "";
		}
	}

	public void Left() {
		SoundManager.Click ();
		beginItemIndex -= itemButtons.Length;
		OnEnable ();
	}

	public void Right() {
		SoundManager.Click ();
		beginItemIndex += itemButtons.Length;
		OnEnable ();
	}

	public virtual void ItemButton(int i) {
		SoundManager.Click ();
		choiceWindow.position = i + beginItemIndex;
		gameObject.SetActive (false);
		choiceWindow.gameObject.SetActive (true);
	}

	public void OnDisable () {
		choiceWindow.gameObject.SetActive (false);
	}

}
