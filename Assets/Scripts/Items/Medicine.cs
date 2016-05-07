using UnityEngine;
using System.Collections;

public class Medicine : Item {

	public Medicine(int id) : base(id, "Medicine", 10, 3) {}

	public override bool CanUse () {
		return true;
	}

	public override void OnUse () {
		Player.instance.character.lifePoints++;
		GameHUD.instance.UpdateLife(Player.instance.character.lifePoints);
		//Player.instance.character.PlayAnimation ("Drink", false);
	}

}
