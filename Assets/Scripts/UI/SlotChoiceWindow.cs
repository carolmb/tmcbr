using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class SlotChoiceWindow : MonoBehaviour {

	public int position;
	public Image itemIcon;

	protected Bag bag {
		get { return SaveManager.currentSave.bag; }
	}

	protected Item item {
		get { return bag.GetItem (position); }
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
