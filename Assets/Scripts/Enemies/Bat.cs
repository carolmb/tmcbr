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
		//inAttackMode = true;
	}

	protected override GridPath PathToPlayer () {
		if (Player.instance.visible) {
			Tile tile = Player.instance.character.currentTile;
			Tile bestTile = null;
			foreach (Tile n in character.currentTile.GetNeighbours4()) {
				if (bestTile == null || PathFinder.EstimateCost(n, tile) < PathFinder.EstimateCost(bestTile, tile)) { 
					bestTile = n;
				}
			}
			return new GridPath(character.currentTile).AddStep(bestTile, 1);
		}
		return null;
	}

	void Update () {
		if (inAttackMode) {
			if (!character.moving && !character.damaging) {
				GridPath path = PathToPlayer ();
				if (path != null && path.PreviousSteps != null) {
					Tile nextTile = path.PreviousSteps.LastStep;
					Vector2 nextPosition = (Vector2)MazeManager.TileToWorldPosition (nextTile.coordinates) + new Vector2 (0, Tile.size / 2);
					character.TurnTo (nextPosition);
					character.MoveTo (nextPosition);
				}
			} 
		} else {
			//List<Tile> neighbours = character.currentTile.GetNeighbours4();
			if (!character.moving && !character.damaging) {
				List<Vector2> neighbours = new List<Vector2> ();

				if (transform.position.x - Tile.size * 1 >= 0) {
					neighbours.Add ((Vector2)transform.position - new Vector2 (Tile.size, 0));
				}
				if (transform.position.x + Tile.size * 1 <= (MazeManager.maze.width - 1) * Tile.size) {
					neighbours.Add ((Vector2)transform.position + new Vector2 (Tile.size, 0));
				}
				if (transform.position.y - Tile.size * 1 >= 0) {
					neighbours.Add ((Vector2)transform.position - new Vector2 (0, Tile.size));
				}
				if (transform.position.y + Tile.size * 1 <= (MazeManager.maze.height - 1) * Tile.size) {
					neighbours.Add ((Vector2)transform.position + new Vector2 (0, Tile.size));
				}


				/*int x = character.currentTile.x;
				int y = character.currentTile.y;

				if (x - 4 >= 0) {
					//Debug.Log ("x - 4: " + x);
					//neighbours.Add (MazeManager.maze.tiles [x - 1, y]);
				}
				if (x + 4 <= MazeManager.maze.width - 1) {
					//Debug.Log ("x + 4: " + x);
					neighbours.Add (MazeManager.maze.tiles [x + 1, y]);
				} 
				if (y - 4 >= 0) {
					//Debug.Log ("y - 4: " + x);
					neighbours.Add (MazeManager.maze.tiles [x, y - 1]);
				} 
				if (y + 4 <= MazeManager.maze.height - 1) {
					//Debug.Log ("y + 4: " + x);
					neighbours.Add (MazeManager.maze.tiles [x, y + 1]);
				}
				
				Tile nextPositionTile = neighbours [Random.Range (0, neighbours.Count)];
				Debug.Log ("current: " + character.currentTile.coordinates);
				Debug.Log ("next: " + nextPositionTile.coordinates);
				Vector2 nextPosition = (Vector2)MazeManager.TileToWorldPosition (nextPositionTile.coordinates) + new Vector2(0, Tile.size / 2);
				*/
				Vector2 nextPosition = neighbours [Random.Range (0, neighbours.Count)];
				character.TurnTo (nextPosition);
				character.MoveTo (nextPosition);
			}
		}

	}
}
