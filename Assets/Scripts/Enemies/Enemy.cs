using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public GameObject coin;

	protected Character character;
	protected Tile originalTile;

	public int damage = 1;
	public int vision = 10;
	public float spawnTime = 1200f; // 20min

	protected virtual void Awake () {
		character = GetComponent<Character> ();
	}

	protected virtual void Start () {
		originalTile = character.currentTile;
	}

	protected virtual Tile ClosestToPlayer () {
		if (Player.visible) {
			Tile playerTile = Player.instance.character.currentTile;
			Tile myTile = character.currentTile;
			if (PathFinder.EstimateCost (myTile, playerTile) >= vision)
				return null;

			GridPath path = PathFinder.FindPath (playerTile, myTile, vision);
			if (path != null && path.PreviousSteps != null) {
				return path.PreviousSteps.LastStep;
			} else {
				return null;
			}
		}
		return null;
	}

	protected virtual Tile FarestToPlayer () {
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

	protected bool ChasePlayer () {
		Tile nextTile = ClosestToPlayer ();
		if (nextTile != null) {
			Vector2 nextPosition = (Vector2)MazeManager.TileToWorldPos (nextTile.coordinates) + new Vector2 (0, Tile.size / 2);
			character.TurnTo (nextPosition);
			character.MoveTo (nextPosition);
			return true;
		} else {
			return false;
		}
	}

	protected bool RunFromPlayer () {
		Tile nextTile = FarestToPlayer ();
		if (nextTile != null) {
			Vector2 nextPosition = (Vector2)MazeManager.TileToWorldPos (nextTile.coordinates) + new Vector2 (0, Tile.size / 2);
			character.TurnTo (nextPosition);
			character.MoveTo (nextPosition);
			return true;
		} else {
			return false;
		}
	}

	void OnTriggerStay2D (Collider2D other) {
		if (other.CompareTag ("Player")) {
			if (!Player.instance.character.damaging  && !Player.instance.immune)
				StartCoroutine(Player.instance.character.Damage ((Vector2) transform.position, damage));
		}
	}

	protected virtual void OnDamage () {
		StartCoroutine (DamageLight ());
	}

	private IEnumerator DamageLight () {
		Color color = Color.red;
		while (color.r < 1) {
			color.r += 0.25f;
			character.spriteRenderer.color = color;
			yield return null;
		}
		while (color.r > 0) {
			color.r -= 0.25f;
			character.spriteRenderer.color = color;
			yield return null;
		}
		character.spriteRenderer.color = Color.white;
	}

	protected void OnDie() {
		if (spawnTime >= 0)
			originalTile.spawnTime = spawnTime;
		Instantiate (coin, transform.position, transform.rotation);
	}

}
