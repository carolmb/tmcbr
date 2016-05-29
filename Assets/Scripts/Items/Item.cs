using UnityEngine;
using System.Collections;
using System;

public abstract class Item {

	// Item id
	public int id;
	// Item picture
	public string spriteName;

	public int price;
	public int count;

	public bool canDiscard;
	public bool canEquip;

	public Item(int id, string spriteName, int price, int count = 0, bool canDiscard = true, bool canEquip = true) {
		this.id = id;
		this.spriteName = spriteName;
		this.price = price;
		this.count = count;
		this.canDiscard = canDiscard;
		this.canEquip = canEquip;
	}

	public bool consumable {
		get { return count > 0; }
	}

	public int totalPrice {
		get { return consumable ? price * count : price; }
	}

	// Checa se pode usar o item
	public abstract bool CanUse ();

	// O que acontece quando o jogador usa o item
	public abstract void OnUse ();

	// ===============================================================================
	// Database
	// ===============================================================================

	// Essa lista vai ser acessada para pegar os items pelo ID
	public static Item [] DB = InitializeDB ();

	private static Item [] InitializeDB () {
		Item [] db = new Item [20];

		db[0] = new Slingshot (0);
		db[1] = new Knife (1);
		db[2] = new Medicine (2);
		db[3] = new Repel (3);
		db[4] = new InvisibilityCloak (4);
		db[5] = new KeyItem (5, "Photography");
		db[6] = new Pick (6);
		db[7] = new KeyItem (7, "Map");
		db[8] = new Poison (8);
		db[9] = new KeyItem (9, "Key");
		db [10] = new Fruit (10, "Fruit");
		return db;
	}

}
