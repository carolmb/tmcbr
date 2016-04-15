using UnityEngine;
using System.Collections;

public class Medicine : Item {

	// Player
	public Player player;

	public Medicine(int id, string spriteName, bool consumable, Player player)
		: base(id, spriteName, consumable){
		this.id = id;
		this.spriteName = spriteName;
		this.consumable = consumable;
		this.player = player;
	}

	//
	public override void OnUse() {
		player.character.lifePoints++;
	}
}
