using UnityEngine;
using System.Collections;

public class CharacterBase : MonoBehaviour {

	public SpriteRenderer spriteRenderer { get; private set; }
	public Animator animator { get; private set; }
	public BoxCollider2D boxCollider { get; private set; }

	protected virtual void Awake () {
		animator = GetComponent<Animator> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		boxCollider = GetComponent<BoxCollider2D> ();
	}

	public bool isPlayer {
		get { return Player.instance.gameObject == gameObject; }
	}

	// Tile atual do personagem
	public Tile currentTile {
		get {
			Vector2 tileCoord = MazeManager.WorldToTilePos(transform.position - new Vector3(0, Tile.size / 2, Tile.size / 2));
			return MazeManager.maze.tiles [(int)tileCoord.x, (int)tileCoord.y];
		} set {
			transform.position = MazeManager.TileToWorldPos (value.coordinates) + new Vector3 (0, Tile.size / 2, Tile.size / 2);
		}
	}
		
	public virtual void InitialDirection () {
		Vector2 offset = Vector2.zero;
		int x = currentTile.x;
		int y = currentTile.y;

		if (MazeManager.maze.tiles [x, y + 1].isWall) { // Virado para baixo
			Vector3 wpos = MazeManager.TileToWorldPos (new Vector2 (x, y - 1)) + new Vector3 (0, Tile.size / 2, 0);
			TurnTo (new Vector2 (wpos.x, wpos.y));
			offset.y += (Tile.size - boxCollider.size.y) / 2;

		} else if (MazeManager.maze.tiles [x + 1, y].isWall) { // Virado para a esquerda
			Vector3 wpos = MazeManager.TileToWorldPos (new Vector2 (x - 1, y)) + new Vector3 (0, Tile.size / 2, 0);
			TurnTo (new Vector2 (wpos.x, wpos.y));
			offset.x += (Tile.size - boxCollider.size.x) / 2;

		} else if (MazeManager.maze.tiles [x - 1, y].isWall) { // Virado para a direita
			Vector3 wpos = MazeManager.TileToWorldPos (new Vector2 (x + 1, y)) + new Vector3 (0, Tile.size / 2, 0);
			TurnTo (new Vector2 (wpos.x, wpos.y));
			offset.x -= (Tile.size - boxCollider.size.x) / 2;

		} else if (MazeManager.maze.tiles [x, y - 1].isWall) { // Virado para cima
			Vector3 wpos = MazeManager.TileToWorldPos (new Vector2 (x, y + 1)) + new Vector3 (0, Tile.size / 2, 0);
			TurnTo (new Vector2 (wpos.x, wpos.y));
			offset.y -= (Tile.size - boxCollider.size.y) / 2;
		}

		currentTile = currentTile;
		transform.Translate (offset.x, offset.y, offset.y);
	}

	// ===============================================================================
	// Direção
	// ===============================================================================

	// Valores do Animator Controller equivalente a cada direção
	public const int DOWN = 0;
	public const int LEFT = 1;
	public const int RIGHT = 2;
	public const int UP = 3;

	// Parâmaetro direção (usa diretamente o parâmetro do Animator Controller
	public int direction {
		get {
			return animator.GetInteger ("Direction");
		}
		set {
			animator.SetInteger ("Direction", value);
		}
	}

	public int lookingAngle {
		get {
			return DirectionToAngle (direction);
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

	// Converte um ângulo para uma direção
	public static int DirectionToAngle(int direction) {
		switch (direction) {
		case RIGHT:
			return 0;
		case UP:
			return 90;
		case LEFT:
			return 180;
		case DOWN:
			return 270;
		}
		return 0;
	}

	// ===============================================================================
	// Animação
	// ===============================================================================

	// Sempre use essa função para finalizar o movimento do personagem
	public virtual void Stop () {
		animator.Play("Walking" + direction, -1, 0);
		animator.speed = 0;
	}

	public void PlayAnimation(string name, bool usesDirection = true) {
		animator.Play (usesDirection ? name + direction : name);
		animator.speed = 1;
	}

}
