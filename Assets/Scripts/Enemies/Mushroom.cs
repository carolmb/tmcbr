using UnityEngine;
using System.Collections;

public class Mushroom : Enemy {

	public GameObject tomato;

	// Use this for initialization

	protected override void Start () {
		base.Start ();
		Spawn ();
	}
	
	void Update () {
		if (Player.instance.paused)
			return;

		if (Player.instance.repelling) {
			RunFromPlayer ();
		} else {
			character.Stop ();
		}
	}

	void Spawn () {
		if (!character.moving && !character.damaging) {
			Tile t = ClosestToPlayer();
			if (t != null) {
				Invoke ("MiniTomato", 3);
			}
		}
		Invoke ("Spawn", 6);
	}

	void MiniTomato () {
		Instantiate (tomato, character.transform.position, character.transform.rotation);
	}
}
