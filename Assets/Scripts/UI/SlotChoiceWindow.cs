using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class SlotChoiceWindow : WindowBase {

	public int position;
	public Image itemIcon;

	protected Bag bag {
		get { return SaveManager.currentSave.bag; }
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

}
