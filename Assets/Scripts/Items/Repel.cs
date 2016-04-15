using UnityEngine;
using System.Collections;

public class Repel : Item {

	// Character
	public Character character;

	public Repel(int id, string spriteName, bool consumable, Character character)
		: base(id, spriteName, consumable){
		this.id = id;
		this.spriteName = spriteName;
		this.consumable = consumable;
	}

	//
	public override void OnUse() {
		// Percorrer todos os inimigos do labirinto e pôr true
		// Esperar 10s
		// Pôr false em todos
	}
}
