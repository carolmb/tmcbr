using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Bag {

	public static readonly int maxItems = 4;

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
		itemIDs [3] = 4;
		coins = 0;
		roses = 0;
		selectedSlot = 0;
	}

	public int selectedItemID {
		get { return itemIDs [selectedSlot]; }
	}

}
