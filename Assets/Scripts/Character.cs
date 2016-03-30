using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Animator))]
[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(BoxCollider2D))]
public class Character : MonoBehaviour {

	private Animator animator;
	private Rigidbody2D rb2D;
	private BoxCollider2D boxCollider;

	public int direction {
		get {
			return animator.GetInteger ("Direction");
		}
		set {
			animator.SetInteger ("Direction", value);
		}
	}
		
	void Awake () {
		animator = GetComponent<Animator> ();
		rb2D = GetComponent<Rigidbody2D> ();
		boxCollider = GetComponent<BoxCollider2D>();
	}

	public void Move(Vector2 translation) {
		Vector2 newPosition = (Vector2)transform.position + translation;
		if (IsWalkable (newPosition)) {
			TurnTo (newPosition);
			rb2D.MovePosition (newPosition);
			animator.speed = 1;
		}
	}

	public void Stop() {
		animator.Play("Walking" + direction, -1, 0);
		animator.speed = 0;
	}

	public void TurnTo(Vector2 point) {
		float angle = Mathf.Atan2 (point.y - transform.position.y, point.x - transform.position.x) * Mathf.Rad2Deg;
		while (angle < 0)
			angle += 360;
		while (angle >= 360)
			angle -= 360;

		int dir = Mathf.RoundToInt (angle / 90) * 90;
		switch (dir) {
		case 0:
			direction = 2;
			break;
		case 90:
			direction = 3;
			break;
		case 180:
			direction = 1;
			break;
		case 270:
			direction = 0;
			break;
		}
	}

	private bool IsWalkable(Vector2 newPosition) {
		float x1 = newPosition.x - boxCollider.bounds.extents.x;
		float x2 = newPosition.x + boxCollider.bounds.extents.x;
		float y1 = newPosition.y - boxCollider.bounds.extents.y;
		float y2 = newPosition.y + boxCollider.bounds.extents.y;

		if (Collides (x1, y1) | Collides (x1, y2) || Collides (x2, y1) || Collides (x2, y2)) {
			return false;
		} else {
			return true;
		}
	}
	
	public bool Collides(float x, float y) {
		Vector2 p = WorldToTilePos(new Vector2 (x, y));
		return Maze.instance [(int)p.x, (int)p.y].isWalkable == false;
	}

	public Vector2 TileToWorldPosition(Vector2 tilePos) {
		return tilePos * Tile.size;
	}

	public Vector2 WorldToTilePos(Vector2 worldPos) {
		return new Vector2(Mathf.Round(worldPos.x / Tile.size), Mathf.Round(worldPos.y / Tile.size));
	}

}
