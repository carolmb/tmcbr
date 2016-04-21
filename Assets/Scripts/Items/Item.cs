﻿using UnityEngine;
using System.Collections;
using System;

public abstract class Item {

	// Item id
	public int id;
	// Item picture
	public string spriteName;

	public bool consumable;

	public Item(int id, string spriteName, bool consumable) {
		this.id = id;
		this.spriteName = spriteName;
		this.consumable = consumable;
	}

	// O que acontece quando o jogador usa o item
	public abstract void OnUse();

	// ===============================================================================
	// Database
	// ===============================================================================

	// Essa lista vai ser acessada para pegar os items pelo ID
	public static Item[] DB = InitializeDB();

	private static Item[] InitializeDB() {
		Item[] db = new Item[10];

		db[0] = new Slingshot(0);
		db[1] = new Knife(1);
		db[2] = new Medicine(2);
		db[3] = new Repel(3);
		db[4] = new InvisibilityCloak(4);

		return db;
	}

}