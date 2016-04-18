using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Character))]
public class KnightArmor : Enemy {

	protected override void Awake () {
		base.Awake ();
	}

	void Update () {
		if (Player.instance.paused)
			return;

		if (!character.moving && !character.damaging) {
			ChasePlayer ();
		}
		if (Player.instance.repelling) {
			RunFromPlayer ();
		}
	}
}
