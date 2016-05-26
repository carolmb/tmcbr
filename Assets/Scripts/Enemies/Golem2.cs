using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Golem2 : Enemy {

	public GameObject golemrock;
	public GameObject golem1;

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
		if (!character.damaging) {
			Invoke ("GolemRock", 1);
		}
		Invoke ("Spawn", 2);
	}

	void GolemRock() {
		Instantiate (golemrock, character.transform.position, character.transform.rotation);
	}

	void OnDestroy() {
		List<Tile> t = character.currentTile.GetNeighbours4Walkeable ();
		Instantiate (golem1, MazeManager.TileToWorldPos(t[Random.Range(0, t.Count)].coordinates), character.transform.rotation);
		Instantiate (golem1, MazeManager.TileToWorldPos(t[Random.Range(0, t.Count)].coordinates), character.transform.rotation);
	}
}
