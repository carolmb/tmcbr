using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Character))]
public class KnightArmor : Enemy {

	bool isChasing;

	protected override void Awake () {
		base.Awake ();
	}

	protected override void Start() {
		base.Start ();
		isChasing = false;
		InitialDirection ();
	}

	void InitialDirection () {
		int x = character.currentTile.x;
		int y = character.currentTile.y;
		if (MazeManager.maze.tiles [x, y + 1].isWall) {
			Vector3 wpos = MazeManager.TileToWorldPos (new Vector2 (x, y - 1)) + new Vector3 (0, Tile.size / 2, 0);
			character.TurnTo (new Vector2 (wpos.x, wpos.y));
		} else if (MazeManager.maze.tiles [x + 1, y].isWall) {
			Vector3 wpos = MazeManager.TileToWorldPos (new Vector2 (x - 1, y)) + new Vector3 (0, Tile.size / 2, 0);
			character.TurnTo (new Vector2 (wpos.x, wpos.y));
		} else if (MazeManager.maze.tiles [x - 1, y].isWall) {
			Vector3 wpos = MazeManager.TileToWorldPos (new Vector2 (x + 1, y)) + new Vector3 (0, Tile.size / 2, 0);
			character.TurnTo (new Vector2 (wpos.x, wpos.y));
		}
		character.Stop ();
	}

	void Update () {
		if (Player.instance.paused)
			return;

		if (!character.moving && !character.damaging) {
			if (Player.instance.repelling) {
				RunFromPlayer ();
			} else {
				UpdateChasing();
				if (isChasing)
					ChasePlayer ();
			}
		}
	}

	void UpdateChasing () {
		if (NextToPlayer ()) {
			isChasing = true;
		}
	}

}
