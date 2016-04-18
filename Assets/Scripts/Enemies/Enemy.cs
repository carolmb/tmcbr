using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public GameObject coin;

	protected Character character;

	public int damage = 1;
	public int vision = 10;

	protected virtual void Awake() {
		character = GetComponent<Character> ();
	}

	protected virtual Tile ClosestToPlayer() {
		if (Player.instance.visible) {
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

	protected virtual Tile FarestToPlayer() {
		Tile playerTile = Player.instance.character.currentTile;
		Tile myTile = character.currentTile;
		Vector2 coord;
		return null;
		//TODO
	}

	protected void ChasePlayer() {
		Tile nextTile = ClosestToPlayer ();
		if (nextTile != null) {
			Vector2 nextPosition = (Vector2)MazeManager.TileToWorldPosition (nextTile.coordinates) + new Vector2 (0, Tile.size / 2);
			character.TurnTo (nextPosition);
			character.MoveTo (nextPosition);
		}
	}

	protected void RunFromPlayer() {
		Tile nextTile = FarestToPlayer ();
		if (nextTile != null) {
			Vector2 nextPosition = (Vector2)MazeManager.TileToWorldPosition (nextTile.coordinates) + new Vector2 (0, Tile.size / 2);
			Vector2 np = new Vector2 (nextPosition.x - transform.position.x, nextPosition.y - transform.position.y);
			np.x *= -1;
			np.y *= -1;
			character.TurnTo (np);
			character.MoveTo (np + (Vector2)transform.position);
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.CompareTag ("Player")) {
			if (!Player.instance.character.damaging  && !Player.instance.immune)
				StartCoroutine(Player.instance.character.Damage ((Vector2) transform.position, damage));
		}
	}

	protected virtual void OnDamage() {
		StartCoroutine (DamageLight ());
	}

	private IEnumerator DamageLight() {
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
		Instantiate (coin, transform.position, transform.rotation);
	}

}
