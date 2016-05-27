﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicEnemy : Enemy {

	void Update () {
		if (Player.instance.paused)
			return;

		if (!character.moving && !character.damaging) {
			if (Player.instance.repelling) {
				RunFromPlayer ();
			} else if (!ChasePlayer ()) {
				RandomMove ();
			}
		}
	}

	void RandomMove () {
		List<Tile> possibleTiles = character.currentTile.GetNeighbours4Walkeable();
		Tile t = possibleTiles[Random.Range(0, possibleTiles.Count)];
		Vector2 nextPosition = (Vector2)MazeManager.TileToWorldPos (t.coordinates) + new Vector2 (0, Tile.size / 2);
		character.TurnTo (nextPosition);
		character.MoveTo (nextPosition, true);
	}

}
