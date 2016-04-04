using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Animator))]
[RequireComponent (typeof(SpriteRenderer))]
[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(BoxCollider2D))]
public class Character : MonoBehaviour {

	private Animator animator;
	private Rigidbody2D rb2D;
	private BoxCollider2D boxCollider;

	public bool moving { get; private set; }
	public float speed = 60; // pixels per second

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
		boxCollider = GetComponent<BoxCollider2D> ();
		Stop ();
	}

	void LateUpdate () {
		Vector3 pos = new Vector3 (Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
		pos.z = pos.y;
		transform.position = pos;
	}

	// Move, dentro de um frame, o personagem em direção translation
	public void InstantMove(Vector2 translation) {
		Vector2 newPosition = (Vector2)transform.position + translation;
		if (IsWalkable (newPosition)) {
			TurnTo (newPosition);
			rb2D.MovePosition (newPosition);
			animator.speed = 1;
		}
	}

	// Move, dentro de um frame, o personagem para o point
	public void InstantMoveTo(Vector2 point) {
		InstantMove (point - (Vector2) transform.position);
	}

	// USE ESSA FUNÇÃO AQUI
	// Move o personagem para o ponto dest, gradativamente
	// É possível verificar se o personagem está andando pela variável moving
	public void MoveTo(Vector2 dest) {
		StartCoroutine (MoveTo_coroutine (dest));
	}

	// OU ESSA
	// Move o personagem uma distância gradativamente
	public void Move(Vector2 transition) {
		StartCoroutine (MoveTo_coroutine (transition + (Vector2)transform.position));
	}

	private IEnumerator MoveTo_coroutine(Vector2 dest) {
		moving = true;
		Vector2 orig = (Vector2)transform.position;
		float distance = (dest - orig).magnitude;
		float percentage = 0;
		float percSpeed = speed / distance;
		while (percentage < 1) {
			percentage += percSpeed * Time.deltaTime;
			InstantMoveTo (Vector2.Lerp(orig, dest, percentage));
			yield return null;
		}
		transform.position = dest;
		moving = false;
	}

	public void Stop () {
		animator.Play("Walking" + direction, -1, 0);
		animator.speed = 0;
		moving = false;
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
		float y1 = newPosition.y - boxCollider.bounds.extents.y - Tile.size / 2;
		float y2 = newPosition.y + boxCollider.bounds.extents.y - Tile.size / 2;

		if (Collides (x1, y1) | Collides (x1, y2) || Collides (x2, y1) || Collides (x2, y2)) {
			return false;
		} else {
			return true;
		}
	}

	public bool Collides(float x, float y) {
		Vector2 p = Maze.WorldToTilePos(new Vector2 (x, y));
		if (p.x < 0 || p.x >= Maze.instance.width || p.y < 0 || p.y >= Maze.instance.height) {
			return true;
		}
		return Maze.instance [(int)p.x, (int)p.y].isWalkable == false;
	}

}
