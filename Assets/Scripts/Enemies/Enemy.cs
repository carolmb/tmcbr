using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public GameObject coin;

	protected Character character;
	protected Coroutine currentMovement;

	public int damage = 1;
	public int vision = 10;

	public bool canSeePlayer;

	protected virtual void Awake() {
		canSeePlayer = false;
		character = GetComponent<Character> ();
	}

	protected GridPath PathToPlayer() {
		if (!canSeePlayer) {
			Tile playerTile = Player.instance.character.currentTile;
			Tile myTile = character.currentTile;
			if (PathFinder.EstimateCost (myTile, playerTile) >= vision)
				return null;
			return PathFinder.FindPath (playerTile, myTile, vision);
		}
		return null;
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.CompareTag ("Player")) {
			if (!Player.instance.character.damaging  && !Player.instance.immune)
				StartCoroutine(Player.instance.character.Damage ((Vector2) transform.position, damage));
		}
	}

	void OnDamage() {
		if (currentMovement != null) {
			StopCoroutine (currentMovement);
			currentMovement = null;
		}
	}

	protected void OnDie() {
		Instantiate (coin, transform.position, transform.rotation);
	}

}
