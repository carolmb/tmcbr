using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Animator))]
[RequireComponent (typeof(SpriteRenderer))]
[RequireComponent (typeof(BoxCollider2D))]
public class Character : MonoBehaviour {

	private Animator animator;
	public SpriteRenderer spriteRenderer { get; private set; }
	private BoxCollider2D boxCollider;
		
	void Awake () {
		animator = GetComponent<Animator> ();
		boxCollider = GetComponent<BoxCollider2D> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		Stop ();
	}

	// Ajusta a coordenada Z do personagem para ser igual à Y
	void LateUpdate () {
		Vector3 pos = transform.position;
		pos.z = pos.y;
		transform.position = pos;
	}

	public bool isPlayer {
		get { return Player.instance.gameObject == gameObject; }
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

	public bool moving;
	public float speed = 2; // pixels por frame

	private Coroutine currentMovement;

	// Move, dentro de um frame, o personagem em direção translation
	// Retorna se foi possível mover
	public bool InstantMove(Vector2 translation, bool animate = true) {
		return InstantMoveTo((Vector2) transform.position + translation, animate);
	}

	// Move, dentro de um frame, o personagem para o point
	// Retorna se foi possível mover
	public bool InstantMoveTo(Vector2 point, bool animate = true) {
		if (CanMoveTo (point)) {
			transform.position = point;
			if (animate)
				animator.speed = 1;
			return true;
		} else {
			return false;
		}
	}

	// USE ESSA FUNÇÃO AQUI (lembrar de usar o StartCoroutine)
	// Move o personagem uma distância gradativamente
	// É possível verificar se o personagem está andando pela variável moving
	public Coroutine Move(Vector2 transition) {
		return MoveTo(transition + (Vector2)transform.position);
	}

	// OU ESSA
	// Move o personagem para o ponto dest, gradativamente
	public Coroutine MoveTo(Vector2 dest) {
		if (currentMovement != null)
			StopCoroutine (currentMovement);
		currentMovement = StartCoroutine (MoveTo_coroutine (dest));
		return currentMovement;
	}

	private IEnumerator MoveTo_coroutine(Vector2 dest) {
		moving = true;
		Vector2 orig = (Vector2)transform.position;
		float distance = (dest - orig).magnitude;
		float percentage = 0;
		float percSpeed = speed / distance;
		while (percentage <= 1) {
			if (!Player.instance.paused) {
				percentage += percSpeed * 60 * Time.deltaTime;
				InstantMoveTo (Vector2.Lerp (orig, dest, percentage));
			}
			yield return null;
		}
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

	public bool collides = true;

	// Tenta se mover na direção do atual moveVector
	// Se conseguir, move e retorna o ângulo que andou; se não, retorna NaN
	public float TryMove(Vector2 moveVector, bool animate = true) {
		float angle = GameManager.VectorToAngle (moveVector);
		angle = Mathf.Round (angle / 45) * 45;

		if (TryMove (angle, animate)) {
			return angle;
		} else if (TryMove (angle + 45, animate)) {
			return angle + 45; 
		} else if (TryMove (angle - 45, animate)) {
			return angle - 45;
		} else {
			return float.NaN;
		}
	}

	// Tenta se mover no dado ângulo
	// Se conseguir, move e retorna true; se não, apenas retorna false
	bool TryMove(float angle, bool animate) {
		Vector2 translation = GameManager.AngleToVector (angle) * speed * 60 * Time.deltaTime;
		return InstantMove (translation, animate);
	}

	// Verifica se tal posição é passável para o personagem (checa cada ponto de seu colisor)
	private bool CanMoveTo(Vector2 newPosition) {
		if (!collides) {
			return newPosition.x >= 0 && newPosition.y >= 0 &&
				newPosition.x <= (MazeManager.maze.width - 1) * Tile.size &&
				newPosition.y <= (MazeManager.maze.height - 1) * Tile.size;
		}
		
		float left 		= newPosition.x - boxCollider.bounds.extents.x + boxCollider.offset.x;
		float right 	= newPosition.x + boxCollider.bounds.extents.x + boxCollider.offset.x;
		float bottom 	= newPosition.y - boxCollider.bounds.extents.y + boxCollider.offset.y - Tile.size / 2;
		float top 		= newPosition.y + boxCollider.bounds.extents.y + boxCollider.offset.y - Tile.size / 2;

		if (Collides (left, top) || Collides (left, bottom) || Collides (right, top) || Collides (right, bottom)) {
			return false;
		} else {
			return true;
		}
	}

	// Verifica se um dado ponto está colidindo em algum tile
	public bool Collides(float x, float y) {
		return MazeManager.Collides (x, y);
	}

	// Tile atual do personagem
	public Tile currentTile {
		get {
			Vector2 tileCoord = MazeManager.WorldToTilePos(transform.position - new Vector3(0, Tile.size / 2, 0));
			return MazeManager.maze.tiles [(int)tileCoord.x, (int)tileCoord.y];
		}
	}

	// ===============================================================================
	// Dano e Morte
	// ===============================================================================

	// Pontos de vida do personagem
	public int lifePoints = 100;

	// Quantos pixels o personagem vai andar para trás quando levar dano
	public float damageSpeed = 3;
	private float damageDuration = 0.25f;

	// Serve para verificar se o personagem está levando dano
	public bool damaging;

	// Animação de dano
	public IEnumerator Damage(Vector2 origin, int value) {
		damaging = true;

		lifePoints = Mathf.Max (0, lifePoints - value);
		SendMessage ("OnDamage");

		// Step
		Stop();
		if (damageSpeed > 0) {
			TurnTo (origin);
			yield return StartCoroutine (DamageStep (origin));
			Stop ();
		}
		damaging = false;

		// Death
		if (lifePoints == 0) {
			yield return StartCoroutine(Die());
		}
	}

	// Passo que o personagem dá para trás quando leva dano
	private IEnumerator DamageStep(Vector2 origin) {
		if (currentMovement != null) {
			StopCoroutine (currentMovement);
			currentMovement = null;
		}

		Vector2 direction = ((Vector2)transform.position - origin).normalized;
		float previousSpeed = speed;
		speed = damageSpeed;
		float time = 0;
		while (time < damageDuration) {
			TryMove (direction, false);
			yield return null;
			time += Time.deltaTime;
		}
		damaging = false;
		speed = previousSpeed;
	}

	// Animação de morte
	public IEnumerator Die() {
		// TODO: animaçãozinha simples de morte
		SendMessage ("OnDie");
		yield return null;
		Destroy (gameObject);
	}

}
