using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Animator))]
[RequireComponent (typeof(SpriteRenderer))]
[RequireComponent (typeof(BoxCollider2D))]
public class Character : MonoBehaviour {

	private Animator animator;
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
		boxCollider = GetComponent<BoxCollider2D> ();
		Stop ();
	}

	void LateUpdate () {
		Vector3 pos = new Vector3 (Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
		pos.z = pos.y;
		transform.position = pos;
	}

	// Move, dentro de um frame, o personagem em direção translation
	// Retorna se foi possível mover
	public bool InstantMove(Vector2 translation) {
		return InstantMoveTo((Vector2) transform.position + translation);
	}

	// Move, dentro de um frame, o personagem para o point
	// Retorna se foi possível mover
	public bool InstantMoveTo(Vector2 point) {
		if (IsWalkable (point)) {
			transform.position = point;
			animator.speed = 1;
			return true;
		} else {
			return false;
		}
	}

	// USE ESSA FUNÇÃO AQUI (lembrar de usar o StartCoroutine)
	// Move o personagem uma distância gradativamente
	// É possível verificar se o personagem está andando pela variável moving
	public IEnumerator Move(Vector2 transition) {
		return MoveTo(transition + (Vector2)transform.position);
	}

	// OU ESSA
	// Move o personagem para o ponto dest, gradativamente
	public IEnumerator MoveTo(Vector2 dest) {
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

	public int AngleToDirection(int angle) {
		switch (angle) {
		case 0:
			return 2;
		case 90:
			return 3;
		case 180:
			return 1;
		case 270:
			return 0;
		}
		return 0;
	}

	public void TurnTo(float angle) {
		while (angle < 0)
			angle += 360;
		while (angle >= 360)
			angle -= 360;
		direction = AngleToDirection(Mathf.RoundToInt (angle / 90) * 90);
	}

	public void TurnTo(Vector2 point) {
		TurnTo (Math.VectorToAngle (point - (Vector2)transform.position));
	}

	private bool IsWalkable(Vector2 newPosition) {
		float x1 = newPosition.x - boxCollider.bounds.extents.x;
		float x2 = newPosition.x + boxCollider.bounds.extents.x;
		float y1 = newPosition.y - boxCollider.bounds.extents.y - Tile.size / 2;
		float y2 = newPosition.y + boxCollider.bounds.extents.y - Tile.size / 2;

		if (Collides (x1, y1) || Collides (x1, y2) || Collides (x2, y1) || Collides (x2, y2)) {
			return false;
		} else {
			Debug.Log ("walkable");
			return true;
		}
	}

	public bool Collides(float x, float y) {
		Vector2 p = Maze.WorldToTilePos(new Vector2 (x, y));
		if (p.x < 0 || p.x >= Maze.instance.width || p.y < 0 || p.y >= Maze.instance.height) {
			return true;
		}
		return Maze.instance.tiles [(int)p.x, (int)p.y].isWalkable == false;
	}

}
