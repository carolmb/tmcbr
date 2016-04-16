using UnityEngine;
using System.Collections;

public class Tomato : Enemy {

	public GameObject tomato;

	// Use this for initialization

	void Start () {
		Spawn ();
	}
	
	void Update () {
		if (Player.instance.paused)
			return;
	}

	void Spawn () {
		if (!character.moving && !character.damaging) {
			GridPath path = PathToPlayer ();
			if (path != null && path.PreviousSteps != null) {
				Invoke ("MiniTomato", 3);
			}
		}
		Invoke ("Spawn", 6);
	}

	void MiniTomato () {
		Instantiate (tomato, character.transform.position, character.transform.rotation);
	}
}
