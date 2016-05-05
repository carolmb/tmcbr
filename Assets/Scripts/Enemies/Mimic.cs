using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mimic : Enemy {

	bool isChasing;

	protected override void Awake () {
		base.Awake ();
		isChasing = false;
	}

	void Update () {
		if (Player.instance.paused)
			return;

		if (!character.moving && !character.damaging) {
			if (Player.instance.repelling) {
				RunFromPlayer ();
			} else if (isChasing) {
				ChasePlayer ();
			}
		}
	}

	void OnInteract() {
		isChasing = true;
	}
}
