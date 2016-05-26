﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Golem3 : Enemy {

	public GameObject golem2;
	public GameObject stalactite;

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
			Invoke ("Earthquake", Random.Range(4, 10));
		}
		Invoke ("Spawn", 3);
	}

	void Earthquake() {
		foreach(Tile t in MazeManager.maze.tiles){
			if (Random.Range (0, 100) < 30) {
				GameObject s = Instantiate (stalactite) as GameObject;
				s.transform.position = MazeManager.TileToWorldPos (t.coordinates);
			}
		}
		if(Camera.current != null)
			Camera.current.GetComponent<GameCamera>().quake = true;
	}

	protected override void OnDie() {
		List<Tile> t = character.currentTile.GetNeighbours4Walkeable ();
		for (int i = 0; i < 4; i++) {
			GameObject spawGolem = Instantiate (golem2) as GameObject;
			spawGolem.transform.position = MazeManager.TileToWorldPos (t [Random.Range (0, t.Count)].coordinates);
		}
		base.OnDie ();
	}
}