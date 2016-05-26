using UnityEngine;
using System.Collections;

public class Poison : Item {

	public Poison(int id) : base(id, "Poision", 0, 1, false, true) {}

	public override bool CanUse () {
		return true;
	}

	public override void OnUse () {
		//Player.instance.character.PlayAnimation ("Drink", false);
		// Die
	}

}
