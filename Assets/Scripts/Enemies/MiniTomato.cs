using UnityEngine;
using System.Collections;

public class MiniTomato : Enemy {
	
	// Update is called once per frame
	void Update () {
		if (Player.instance.paused)
			return;
		
		if (!character.moving && !character.damaging) {
			if (Player.instance.repelling) {
				RunFromPlayer ();
			} else {
				if (!ChasePlayer ()) {
					character.Stop();
				}
			}
		}
	}
}
