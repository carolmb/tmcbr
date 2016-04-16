using UnityEngine;
using System.Collections;

public class Repel : Item {

	private GameObject repel;

	public Repel(int id) : base(id, "Repel", false) {
		repel = Resources.Load<GameObject>("Prefabs/Repel");
	}

	public override void OnUse () {
		Player.instance.character.lifePoints++;
		GameMenu.instance.UpdateLife(Player.instance.character.lifePoints);
	}
}
