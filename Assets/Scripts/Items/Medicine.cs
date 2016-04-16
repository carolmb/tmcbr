using UnityEngine;
using System.Collections;

public class Medicine : Item {

	private GameObject medicine;

	public Medicine(int id) : base(id, "Medicine", false) {
		medicine = Resources.Load<GameObject>("Prefabs/Medicine");
	}

	public override void OnUse () {
		Player.instance.character.lifePoints++;
		GameMenu.instance.UpdateLife(Player.instance.character.lifePoints);
	}
}
