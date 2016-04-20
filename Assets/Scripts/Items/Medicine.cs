using UnityEngine;
using System.Collections;

public class Medicine : Item {

	public Medicine(int id) : base(id, "Medicine", true) {}

	public override void OnUse () {
		Player.instance.character.lifePoints++;
		GameMenu.instance.UpdateLife(Player.instance.character.lifePoints);
	}

}
