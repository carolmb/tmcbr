using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mimic : Enemy {

	bool isChasing;

	protected override void Start () {
		base.Start ();
		character.InitialDirection ();
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

	protected override void OnDamage () {
		base.OnDamage ();
		isChasing = true;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag ("Player")) {
			isChasing = true;
		}
	}

}
