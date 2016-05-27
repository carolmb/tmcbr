using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Character))]
public class Curupira : Enemy {

	bool isChasing;
	bool awaken;
	public bool inAttackMode;

	protected override void Start() {
		base.Start ();
		isChasing = false;
		awaken = false;
		inAttackMode = false;
		character.InitialDirection ();
	}

	void Update () {
		if (Player.instance.paused)
			return;

		if (!character.moving && !character.damaging) {
			if (Player.instance.repelling) {
				RunFromPlayer ();
			} else {
				if (isChasing) {
					if (!ChasePlayer ()) {
						isChasing = false;
						character.InitialDirection ();
					}
				} else if (!awaken && PlayerInFront ()) { // Verficar se o player está cortando plantas
					awaken = true;
					Invoke ("StartChasing", 1);
				} else {
					DefaultMovement ();
				}
			}
		}
	}

	void StartChasing() {
		isChasing = true;
	}

	protected override bool ChasePlayer () {
		Tile nextTile = ClosestToPlayer ();
		if (nextTile != null && nextTile.isWalkable) {
			Vector2 nextPosition = (Vector2)MazeManager.TileToWorldPos (nextTile.coordinates) + new Vector2 (0, Tile.size / 2);
			character.TurnTo (nextPosition);
			character.MoveTo (nextPosition, true);
//			Instantiate (footsteps, character.transform.position, character.transform.rotation);
			return true;
		} else {
			return false;
		}
	}

	protected override bool RunFromPlayer () {
		Tile nextTile = FarestFromPlayer ();
		if (nextTile != null && nextTile.isWalkable) {
			Vector2 nextPosition = (Vector2)MazeManager.TileToWorldPos (nextTile.coordinates) + new Vector2 (0, Tile.size / 2);
			character.TurnTo (nextPosition);
			character.MoveTo (nextPosition, true);
//			Instantiate (footsteps, character.transform.position, character.transform.rotation);
			return true;
		} else {
			return false;
		}
	}

	protected override void OnDamage () {
		base.OnDamage ();
		StartChasing ();
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag ("Player")) {
		}
	}

	void DefaultMovement () {
		if (inAttackMode) {
			if (!character.moving && !character.damaging) {
				if (!ChasePlayer ()) {
					inAttackMode = false;
				}
			}
		} else if (!character.moving && !character.damaging) {
			try
			{
				List<Vector2> neighbours = new List<Vector2> ();
				Tile t = character.currentTile;
				// trocar chão

				// grande comentário que não acrescenta nada (exceto uma linha)
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

				Vector2 nextTile = neighbours [Random.Range (0, neighbours.Count)];

				if (MazeManager.maze.tiles[(int)nextTile.x, (int)nextTile.y].isWalkable) {
					Vector2 nextPosition = (Vector2)MazeManager.TileToWorldPos (nextTile) + 
						new Vector2(0, Tile.size / 2);
					character.TurnTo (nextPosition);
					character.MoveTo (nextPosition);
				} else {
					character.Stop();
				}
			} catch (System.IndexOutOfRangeException) {
				//
			}
		}

	}
}
