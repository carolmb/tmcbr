using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Bat : Enemy {

	bool inAttackMode;

	void Start() {
		inAttackMode = false;
		character.spriteRenderer.sortingLayerName = "Bat";
	}

	protected override void OnDamage () {
		base.OnDamage ();
		inAttackMode = true;
	}

	protected override Tile ClosestToPlayer () {
		if (Player.instance.visible) {
			Tile tile = Player.instance.character.currentTile;
			if (PathFinder.EstimateCost (character.currentTile, tile) > vision)
				return null;
			Tile bestTile = null;
			foreach (Tile n in character.currentTile.GetNeighbours4()) {
				if (bestTile == null || PathFinder.EstimateCost(n, tile) < PathFinder.EstimateCost(bestTile, tile)) { 
					bestTile = n;
				}
			}
			return bestTile;
		}
		return null;
	}

	void Update () {
		if (Player.instance.repelling) {
			RunFromPlayer ();
			inAttackMode = false;
		}

		if (inAttackMode) {
			if (!character.moving && !character.damaging) {
				if (!ChasePlayer ()) {
					inAttackMode = false;
				}
			}
		} else {
			//List<Tile> neighbours = character.currentTile.GetNeighbours4();
			if (!character.moving && !character.damaging) {
				List<Vector2> neighbours = new List<Vector2> ();
				Tile t = character.currentTile;

				if (t.x - 4 >= 0) {
					neighbours.Add (new Vector2(t.x - 1, t.y));
				}
				if (t.x + 4 <= MazeManager.maze.width - 1) {
					neighbours.Add (new Vector2(t.x + 1, t.y));
				} 
				if (t.y - 4 >= 0) {
					neighbours.Add (new Vector2(t.x, t.y - 1));
				} 
				if (t.y + 4 <= MazeManager.maze.height - 1) {
					neighbours.Add (new Vector2(t.x, t.y + 1));
				}

				Vector2 nextPosition = (Vector2)MazeManager.TileToWorldPosition (neighbours [Random.Range (0, neighbours.Count)]) + 
					new Vector2(0, Tile.size / 2);

				character.TurnTo (nextPosition);
				character.MoveTo (nextPosition);
			}
		}

	}
}
