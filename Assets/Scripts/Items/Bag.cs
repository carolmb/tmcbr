using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Bag {

	public static readonly int maxItems = 12;

	public int coins;
	public int roses;
	public int[] itemIDs;
	public int selectedSlot;

	public Bag() {
		itemIDs = new int[maxItems];
		for (int i = 0; i < maxItems; i++) {
			itemIDs [i] = -1;
		}
		itemIDs [0] = 0;
		itemIDs [1] = 1;
		itemIDs [2] = 2;
		itemIDs [3] = 3;
		coins = 0;
		roses = 0;
		selectedSlot = 0;
	}

	public int selectedItemID {
		get { return itemIDs [selectedSlot]; }
	}

	public Item GetItem(int position) {
		if (position < 0 || position >= maxItems)
			return null;
		if (itemIDs [position] == -1)
			return null;
		return Item.DB [itemIDs [position]];
	}

}
