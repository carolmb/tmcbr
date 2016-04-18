using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mimic : Enemy {

	protected override void Awake () {
		base.Awake ();
	}

	void Update () {
		if (Player.instance.paused)
			return;

		if (!character.moving && !character.damaging) {
			if (Player.instance.repel) {
				RunFromPlayer ();
			} else {
				ChasePlayer ();
			}
		}
	}
}
