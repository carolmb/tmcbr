using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Bag {

	public static readonly int maxItems = 4;

	public int coins;
	public int roses;
	public int[] itemIDs;

	public Bag() {
		itemIDs = new int[maxItems];
		for (int i = 0; i < maxItems; i++) {
			itemIDs [i] = -1;
		}
		coins = 0;
		roses = 0;
	}

}
