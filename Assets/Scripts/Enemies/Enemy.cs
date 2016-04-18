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

	protected virtual GridPath PathToPlayer() {
		if (Player.instance.visible) {
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
