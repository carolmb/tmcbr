using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Animator))]
[RequireComponent (typeof(SpriteRenderer))]
[RequireComponent (typeof(BoxCollider2D))]
public class Character : MonoBehaviour {

	private Animator animator;
	private BoxCollider2D boxCollider;
		
	void Awake () {
		animator = GetComponent<Animator> ();
		boxCollider = GetComponent<BoxCollider2D> ();
		Stop ();
	}

	// Ajusta a coordenada Z do personagem para ser igual à Y
	void LateUpdate () {
		//Vector3 pos = new Vector3 (Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
		Vector3 pos = transform.position;
		pos.z = pos.y;
		transform.position = pos;
	}

	// ===============================================================================
	// Direção
	// ===============================================================================

	// Valores do Animator Controller equivalente a cada direção
	public static readonly int DOWN = 0;
	public static readonly int LEFT = 1;
	public static readonly int RIGHT = 2;
	public static readonly int UP = 3;

	// Parâmaetro direção (usa diretamente o parâmetro do Animator Controller
	public int direction {
		get {
			return animator.GetInteger ("Direction");
		}
		set {
			animator.SetInteger ("Direction", value);
		}
	}

	// Converte um ângulo para uma direção (usar a imagem do personagem como referência)
	public static int AngleToDirection(int angle) {
		while (angle < 0)
			angle += 360;
		while (angle >= 360)
			angle -= 360;
		switch (angle) {
		case 0:
			return RIGHT;
		case 90:
			return UP;
		case 180:
			return LEFT;
		case 270:
			return DOWN;
		}
		return 0;
	}

	// Olha na direção de um ângulo
	public void TurnTo(float angle) {
		direction = AngleToDirection(Mathf.RoundToInt (angle / 90) * 90);
	}

	// Olha em direção a um ponto
	public void TurnTo(Vector2 point) {
		TurnTo (GameManager.VectorToAngle (point - (Vector2)transform.position));
	}

	// ===============================================================================
	// Movimento
	// ===============================================================================

	public bool moving { get; private set; }
	public float speed = 2; // pixels per frame

	// Move, dentro de um frame, o personagem em direção translation
	// Retorna se foi possível mover
	public bool InstantMove(Vector2 translation) {
		return InstantMoveTo((Vector2) transform.position + translation);
	}

	// Move, dentro de um frame, o personagem para o point
	// Retorna se foi possível mover
	public bool InstantMoveTo(Vector2 point) {
		if (CanMoveTo (point)) {
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
			percentage += percSpeed;
			InstantMoveTo (Vector2.Lerp(orig, dest, percentage));
			yield return null;
		}
		transform.position = dest;
		moving = false;
	}

	// Sempre use essa função para finalizar o movimento do personagem
	public void Stop () {
		animator.Play("Walking" + direction, -1, 0);
		animator.speed = 0;
		moving = false;
	}

	// ===============================================================================
	// Colisão
	// ===============================================================================

	// Verifica se tal posição é passável para o personagem (checa cada ponto de seu colisor)
	private bool CanMoveTo(Vector2 newPosition) {
		float x1 = newPosition.x - boxCollider.bounds.extents.x;
		float x2 = newPosition.x + boxCollider.bounds.extents.x;
		float y1 = newPosition.y - boxCollider.bounds.extents.y - Tile.size / 2;
		float y2 = newPosition.y + boxCollider.bounds.extents.y - Tile.size / 2;

		if (Collides (x1, y1) || Collides (x1, y2) || Collides (x2, y1) || Collides (x2, y2)) {
			return false;
		} else {
			return true;
		}
	}

	// Verifica se um dado ponto está colidindo em algum tile
	public bool Collides(float x, float y) {
		return MazeManager.Collides (x, y);
	}

	public Tile currentTile {
		get {
			Vector2 tileCoord = MazeManager.WorldToTilePos(transform.position - new Vector3(0, Tile.size / 2, 0));
			return MazeManager.maze.tiles [(int)tileCoord.x, (int)tileCoord.y];
		}
	}

}
