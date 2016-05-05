using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Bag : IEnumerable<Item> {

	public static readonly int maxItems = 12;

	private ItemSlot[] itemSlots;
	public int selectedSlot;
	public int coins;
	public int roses;

	[System.Serializable]
	private class ItemSlot {
		public int id;
		public int count;
		public ItemSlot(int id, int count = 1) {
			this.id = id;
			this.count = count;
		}
	}

	public Bag () {
		itemSlots = new ItemSlot[maxItems];
		itemSlots [0] = new ItemSlot (0, 1);
		itemSlots [1] = new ItemSlot (1, 1);
		itemSlots [2] = new ItemSlot (2, 1);
		itemSlots [3] = new ItemSlot (3, 1);
		itemSlots [4] = new ItemSlot (4, 1);
		coins = 0;
		roses = 0;
		selectedSlot = 0;
	}

	public int selectedItemID {
		get { return itemSlots [selectedSlot] == null ? -1 : itemSlots [selectedSlot].id; }
	}

	public Item GetItem(int position) {
		if (position < 0 || position >= maxItems)
			return null;
		if (itemSlots [position] == null)
			return null;
		return Item.DB [itemSlots [position].id];
	}

	public int GetCount(int position) {
		if (position < 0 || position >= maxItems)
			return 0;
		return itemSlots [position].count;
	}

	public void Add(Item item, int slot) {
		itemSlots [slot] = new ItemSlot (item.id, item.count);
	}

	public void Increment(int slot) {
		itemSlots [slot].count++;
	}

	public void Consume(int slot) {
		itemSlots [slot].count --;
		if (itemSlots[slot].count == 0) {
			itemSlots[slot] = null;
		}
	}

	public void Discard(int slot) {
		itemSlots[slot] = null;
	}

	public IEnumerator<Item> GetEnumerator() {
		for (int i = 0; i < itemSlots.Length; i++) {
			if (itemSlots [i] != null)
				yield return GetItem (i);
		}
	}

	IEnumerator IEnumerable.GetEnumerator() {
		return this.GetEnumerator();
	}

}
