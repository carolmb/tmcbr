using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ItemSlot {
	// Classe auxiliar para armazenas ID + quantidade
	public int id;
	public int count;
	public ItemSlot(int id, int count = 1) {
		this.id = id;
		this.count = count;
	}
}

[System.Serializable]
public class Bag : IEnumerable<Item> {

	public static readonly int maxItems = 12;

	private ItemSlot[] itemSlots;
	public int selectedPosition;
	public int coins;
	public int roses;

	public Bag () {
		itemSlots = new ItemSlot[maxItems];
		itemSlots [0] = new ItemSlot (5);
		itemSlots [1] = new ItemSlot (0, 4);
		itemSlots [2] = new ItemSlot (1);
		itemSlots [3] = new ItemSlot (2, 5);
		itemSlots [4] = new ItemSlot (3, 4);
		itemSlots [5] = new ItemSlot (4);
		itemSlots [6] = new ItemSlot (6);
		coins = 50;
		roses = 0;
		selectedPosition = 1;
	}

	public static Bag current {
		get { return SaveManager.currentSave.bag; }
	}

	// ===============================================================================
	// Acesso aos slots
	// ===============================================================================

	public Item selectedItem {
		get { return GetItem(selectedPosition); }
	}

	public ItemSlot selectedSlot {
		get { return GetSlot (selectedPosition); }
	}

	public Item GetItem (int position) {
		if (position < 0 || position >= maxItems)
			return null;
		if (itemSlots [position] == null)
			return null;
		return Item.DB [itemSlots [position].id];
	}

	public ItemSlot GetSlot (int position) {
		if (position < 0 || position >= maxItems)
			return null;
		return itemSlots [position];
	}

	// ===============================================================================
	// Adicionar e remover itens
	// ===============================================================================

	public void Add (Item item, int slot) {
		itemSlots [slot] = new ItemSlot (item.id, item.count);
	}

	public void Increment (int slot) {
		itemSlots [slot].count += GetItem(slot).count;
	}

	public void Discard (int slot) {
		itemSlots[slot] = null;
	}

	public void Consume (int slot) {
		itemSlots [slot].count --;
		if (itemSlots[slot].count == 0) {
			itemSlots[slot] = null;
		}
	}

	// ===============================================================================
	// Percorrer os itens
	// ===============================================================================

	public IEnumerator<Item> GetEnumerator () {
		for (int i = 0; i < itemSlots.Length; i++) {
			if (itemSlots [i] != null)
				yield return GetItem (i);
		}
	}

	IEnumerator IEnumerable.GetEnumerator () {
		return this.GetEnumerator();
	}

}
