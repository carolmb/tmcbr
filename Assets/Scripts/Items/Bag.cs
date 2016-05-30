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

	public static readonly int maxItems = 20;

	private ItemSlot[] itemSlots;
	public int selectedPosition;
	public int coins;
	public int roses;

	public Bag () {
		itemSlots = new ItemSlot[maxItems];
		itemSlots [0] = new ItemSlot (5);
		itemSlots [1] = new ItemSlot (7);
		itemSlots [2] = new ItemSlot (0, 4);
		itemSlots [3] = new ItemSlot (1);
		itemSlots [4] = new ItemSlot (2, 5);
		itemSlots [5] = new ItemSlot (3, 4);
		itemSlots [6] = new ItemSlot (4);
		itemSlots [7] = new ItemSlot (6);
		coins = 50;
		roses = 0;
		selectedPosition = 7;
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

	public bool HasItem(Item item) {
		foreach (ItemSlot slot in itemSlots) {
			if (slot != null) {
				if (slot.id == item.id)
					return true;
			}
		}
		return false;
	}

	// ===============================================================================
	// Adicionar e remover itens
	// ===============================================================================

	public void AddOne(Item item) {
		for (int i = 0; i < Bag.maxItems; i++) {
			Item item2 = Bag.current.GetItem (i);
			if (item2 != null && item2.id == item.id) {
				Bag.current.Increment(i, 1);
				return;
			}
		}
		// Não achou o item na mochila, então cria slot novo
		for (int i = 0; i < Bag.maxItems; i++) {
			ItemSlot slot = Bag.current.GetSlot (i);
			if (slot == null) {
				Bag.current.Add (item, i, 1);
				return;
			}
		}
	}

	public bool Add(Item item) {
		for (int i = 0; i < Bag.maxItems; i++) {
			Item item2 = Bag.current.GetItem (i);
			if (item2 != null && item2.id == item.id) {
				Bag.current.Increment(i);
				return true;
			}
		}
		// Não achou o item na mochila, então cria slot novo
		for (int i = 0; i < Bag.maxItems; i++) {
			ItemSlot slot = Bag.current.GetSlot (i);
			if (slot == null) {
				Bag.current.Add (item, i);
				return true;
			}
		}
		return false;
	}

	public void Add (Item item, int slot, int count) {
		itemSlots [slot] = new ItemSlot (item.id, count);
	}

	public void Add (Item item, int slot) {
		itemSlots [slot] = new ItemSlot (item.id, item.count);
	}

	public void Increment (int slot) {
		itemSlots [slot].count += GetItem(slot).count;
	}

	public void Increment (int slot, int count) {
		itemSlots [slot].count += count;
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
