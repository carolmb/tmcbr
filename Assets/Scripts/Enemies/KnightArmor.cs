using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Character))]
public class KnightArmor : Enemy {

	bool isChasing;
	bool awaken;

	protected override void Start() {
		base.Start ();
		isChasing = false;
		awaken = false;
		character.InitialDirection ();
	}

	void Update () {
		if (Player.instance.paused)
			return;

		if (!character.moving && !character.damaging) {
			if (Player.instance.repelling) {
				RunFromPlayer ();
			} else {
				if (isChasing) {
					if (!ChasePlayer ()) {
						isChasing = false;
						character.InitialDirection ();
					}
				} else if (!awaken && PlayerInFront ()) {
					awaken = true;
					Invoke ("StartChasing", 1);
				} else {
					awaken = false;
				}
			}
		}
	}

	void StartChasing() {
		isChasing = true;
	}

	protected override void OnDamage () {
		base.OnDamage ();
		StartChasing ();
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag ("Player")) {
			StartChasing ();
		}
	}

}
