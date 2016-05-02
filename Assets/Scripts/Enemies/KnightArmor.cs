using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Character))]
public class KnightArmor : Enemy {

	protected override void Awake () {
		base.Awake ();
	}

	protected override void Start() {
		base.Start ();
		InitialDirection ();
	}

	void InitialDirection () {
		int x = character.currentTile.x;
		int y = character.currentTile.y;
		Debug.Log (character.currentTile.coordinates);
		if (MazeManager.maze.tiles [x, y + 1].isWall) {
			Debug.Log (x + ", " + y + " + 1");
			Debug.Log (MazeManager.maze.tiles[x, y].isWalkable);
			Vector3 wpos = MazeManager.TileToWorldPos (new Vector2 (x, y - 1) + new Vector2 (0, Tile.size / 2));
			character.TurnTo (new Vector2 (wpos.x, wpos.y));
			//Debug.Log ("depois " + character.direction);
		} else if (MazeManager.maze.tiles [x + 1, y].isWall) {
			Debug.Log (x + " + 1" + ", " + y);
			//Debug.Log ("antes " + character.direction);
			Vector3 wpos = MazeManager.TileToWorldPos (new Vector2 (x - 1, y) + new Vector2 (0, Tile.size / 2));
			character.TurnTo (new Vector2 (wpos.x, wpos.y));
			//Debug.Log ("depois " + character.direction);
		} else if (MazeManager.maze.tiles [x - 1, y].isWall) {
			Debug.Log (x + " - 1" + ", " + y);
			//Debug.Log ("antes " + character.direction);
			Vector3 wpos = MazeManager.TileToWorldPos (new Vector2 (x + 1, y) + new Vector2 (0, Tile.size / 2));
			character.TurnTo (new Vector2 (wpos.x, wpos.y));
			//Debug.Log ("depois " + character.direction);
		}
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

}
