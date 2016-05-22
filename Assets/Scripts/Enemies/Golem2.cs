using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Golem2 : Enemy {

	protected override void Start () {
		base.Start ();
		Spawn ();
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

	void Spawn () {
		if (!character.moving && !character.damaging) {
			Tile t = ClosestToPlayer();
			if (t != null) {
				Invoke ("GolemRock", 3);
			}
		}
		Invoke ("Spawn", 6);
	}

	void GolemRock() {
		//Instantiate (golemrock, character.transform.position, character.transform.rotation);
	}
}
