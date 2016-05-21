using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Golem1 : Enemy {
	
	protected override void Start () {
		base.Start ();
	}

	void Update () {
		if (Player.instance.paused)
			return;

		if (!character.moving && !character.damaging) {
			if (Player.instance.repelling) {
				RunFromPlayer ();
			} else {
				ChasePlayer ();
			}
		}
	}
}
