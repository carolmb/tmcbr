using UnityEngine;
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

		// db[0] = new Slingshot(0);
		db[0] = new Stab(0);

		// TODO: criar os diferentes itens aqui
		// OBS: todos os itens devem herdar dessa classe. Ex:
		// db[3] = new Knife(3, "knife");

		return db;
	}

}
