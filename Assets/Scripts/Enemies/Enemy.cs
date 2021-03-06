﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Character))]
[RequireComponent (typeof(Animator))]
public class Enemy : MonoBehaviour {

	public GameObject coin;

	protected Character character;
	protected Tile originalTile;

	public int damage = 1;
	public int vision = 10;
	public float spawnTime = 300f; // 5min

	protected virtual void Awake () {
		character = GetComponent<Character> ();
	}

	protected virtual void Start () {
		originalTile = character.currentTile;
		transform.position = originalTile.lastObjectPos;
		originalTile.lastSpawn = 0f;
		originalTile.spawnTime = 0f;
	}

	protected virtual bool PlayerInFront () {
		Tile currentTile = character.currentTile;
		if (character.direction == Character.DOWN) {
			if (MazeManager.maze.tiles [currentTile.x, currentTile.y - 1] == Player.instance.character.currentTile) {
				return true;
			}
		} else if (character.direction == Character.LEFT) {
			if (MazeManager.maze.tiles [currentTile.x - 1, currentTile.y] == Player.instance.character.currentTile) {
				return true;
			}
		} else if (character.direction == Character.RIGHT) {
			if (MazeManager.maze.tiles [currentTile.x + 1, currentTile.y] == Player.instance.character.currentTile) {
				return true;
			}
		} else {
			if (MazeManager.maze.tiles [currentTile.x, currentTile.y + 1] == Player.instance.character.currentTile) {
				return true;
			}
		}
		return false;
	}

	protected virtual Tile ClosestToPlayer () {
		if (Player.visible) {
			Tile playerTile = Player.instance.character.currentTile;
			Tile myTile = character.currentTile;
			if (PathFinder.EstimateCost (myTile, playerTile) >= vision)
				return null;

			if (!playerTile.isWalkable) {
				Vector2 playerPos = Player.instance.transform.position;
				Vector2 tileCenter = MazeManager.TileToWorldPos(playerTile.coordinates) + new Vector3(0, Tile.size / 2, 0);
				Vector2 d = playerPos - tileCenter;

				if (Mathf.Abs(d.x) > Mathf.Abs(d.y)) {
					int dx = d.x > 0 ? 1 : -1;
					playerTile = MazeManager.maze.tiles[playerTile.x + dx, playerTile.y];
				} else {
					int dy = d.y > 0 ? 1 : -1;
					playerTile = MazeManager.maze.tiles[playerTile.x, playerTile.y + dy];
				}
			}

			GridPath path = PathFinder.FindPath (playerTile, myTile, vision);
			if (path == null) {
				float best = vision + 1;
				GridPath bestp = null;
				foreach (Tile n in myTile.GetNeighbours4Walkeable()) {
					path = PathFinder.FindPath (n, playerTile, vision - 1);
					if (path != null && path.TotalCost < best) {
						best = path.TotalCost;
						bestp = path;
					}
				}
				path = bestp;
			}
			if (path != null) {
				if (path.PreviousSteps != null && path.PreviousSteps.LastStep.isWalkable) {
					return path.PreviousSteps.LastStep;
				} else {
					return path.LastStep;
				}
			}
		}
		return null;
	}

	protected virtual Tile FarestFromPlayer () {
		Tile playerTile = Player.instance.character.currentTile;
		Tile myTile = character.currentTile;
		Vector2 dif = playerTile.coordinates - myTile.coordinates;
		if (Mathf.Abs (dif.x) > Mathf.Abs (dif.y)) {
			int x = dif.x > 0 ? -1 : 1;
			x += myTile.x;
			if (x >= 0 && x < MazeManager.maze.width)
				return MazeManager.maze.tiles [x, myTile.y];
			else
				return null;
		} else {
			int y = dif.y > 0 ? -1 : 1;
			y += myTile.y;
			if (y >= 0 && y < MazeManager.maze.height)
				return MazeManager.maze.tiles [myTile.x, y];
			else
				return null;
		}
	}

	protected virtual bool ChasePlayer () {
		Tile nextTile = ClosestToPlayer ();
		if (nextTile != null) {
			Vector2 nextPosition;

			if (nextTile == character.currentTile) {
				nextPosition = Player.instance.transform.position;
			} else {
				nextPosition = (Vector2)MazeManager.TileToWorldPos (nextTile.coordinates) + 
					new Vector2 (0, Tile.size / 2);
			}
			character.TurnTo (nextPosition);
			character.MoveTo (nextPosition, true);
			return true;
		} else {
			return false;
		}
	}

	protected virtual bool RunFromPlayer () {
		Tile nextTile = FarestFromPlayer ();
		if (nextTile != null && nextTile.isWalkable) {
			Vector2 nextPosition = (Vector2)MazeManager.TileToWorldPos (nextTile.coordinates) + new Vector2 (0, Tile.size / 2);
			character.TurnTo (nextPosition);
			character.MoveTo (nextPosition, true);
			return true;
		} else {
			return false;
		}
	}

	void OnTriggerStay2D (Collider2D other) {
		if (other.CompareTag ("Player")) {
			if (!Player.instance.character.damaging  && !Player.instance.immune)
				Player.instance.character.Damage ((Vector2) transform.position, damage);
		}
	}

	Coroutine damageRoutine = null;

	protected virtual void OnDamage () {
		SoundManager.EnemyDamage ();
		damageRoutine = StartCoroutine (DamageLight ());
	}

	private IEnumerator DamageLight () {
		Color color = Color.red;
		while (color.r < 1 && character.spriteRenderer != null) {
			color.r += 0.25f;
			character.spriteRenderer.color = color;
			yield return null;
		}
		while (color.r > 0 && character.spriteRenderer != null) {
			color.r -= 0.25f;
			character.spriteRenderer.color = color;
			yield return null;
		}
		character.spriteRenderer.color = Color.white;
		damageRoutine = null;
	}

	protected virtual void OnDie() {
		if (damageRoutine != null) {
			StopCoroutine (damageRoutine);
		}
		Destroy (character.spriteRenderer);
		if (spawnTime >= 0) {
			originalTile.spawnTime = spawnTime;
			originalTile.lastSpawn = SaveManager.currentPlayTime;
		}
		Instantiate (coin, transform.position, transform.rotation);

		character.currentTile = originalTile;

		Destroy (gameObject);
	}

	void OnDestroy() {
		originalTile.lastObjectPos = transform.position;
	}

}
