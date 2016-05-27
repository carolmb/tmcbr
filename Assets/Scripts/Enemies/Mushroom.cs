using UnityEngine;
using System.Collections;

public class Mushroom : KnightArmor {

	void Update () {
		if (Player.instance.paused)
			return;

		if (Player.instance.repelling) {
			RunFromPlayer ();
		} else {
			character.Stop ();
		}
	}

	void OnTriggerStay2D (Collider2D other) {
		if (other.CompareTag ("Player")) {
			if (!Player.instance.immune) {
				GameCamera.instance.MushroomEffect ();
			}
		}
	}
}
