using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Golem2 : Enemy {

	public GameObject golemrock;
	public GameObject golem1;

	protected override void Start () {
		MazeManager.maze.tiles [character.currentTile.x, character.currentTile.y].objectName = "Enemies/Golem2";
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
		Invoke ("Spawn", 1);
	}

	void GolemRock() {
		Instantiate (golemrock, character.transform.position, character.transform.rotation);
	}

	protected override void OnDie() {
		MazeManager.maze.tiles [originalTile.x, originalTile.y].objectName = "";
		List<Tile> t = character.currentTile.GetNeighbours4Walkeable ();
		GameObject spawGolem1 = Instantiate (golem1) as GameObject;
		GameObject spawGolem2 = Instantiate (golem1) as GameObject;
		spawGolem1.transform.position = MazeManager.TileToWorldPos (t [Random.Range (0, t.Count)].coordinates);
		spawGolem2.transform.position = MazeManager.TileToWorldPos (t [Random.Range (0, t.Count)].coordinates);
		base.OnDie ();
	}
}
