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
		//Vector3 pos = new Vector3 (Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
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
		while (percentage <= 1) {
			if (!Player.instance.paused) {
				percentage += percSpeed;
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
		Vector2 translation = GameManager.AngleToVector (angle) * speed;
		return InstantMove (translation, animate);
	}

	// Verifica se tal posição é passável para o personagem (checa cada ponto de seu colisor)
	private bool CanMoveTo(Vector2 newPosition) {
		float left 		= newPosition.x - boxCollider.bounds.extents.x;
		float right 	= newPosition.x + boxCollider.bounds.extents.x;
		float bottom 	= newPosition.y - boxCollider.bounds.extents.y - Tile.size / 2;
		float top 		= newPosition.y + boxCollider.bounds.extents.y - Tile.size / 2;

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
	public int lifePoints = 5;

	// Quantos pixels o personagem vai andar para trás quando levar dano
	private int damageStep = 2;
	private float damageDuration = 0.5f;

	// Serve para verificar se o personagem está levando dano
	public bool damaging { get; private set; }

	// Animação de dano
	public IEnumerator Damage(Vector2 origin, int value) {
		damaging = true;

		lifePoints = Mathf.Max (0, lifePoints - value);
		SendMessage ("OnDamage");

		// Step
		Stop();
		TurnTo (origin);
		yield return StartCoroutine (DamageStep(origin));
		Stop();

		// Death
		if (lifePoints == 0) {
			yield return StartCoroutine(Die());
		} else {
			damaging = false;
		}
	}

	// Passo que o personagem dá para trás quando leva dano
	private IEnumerator DamageStep(Vector2 origin) {
		Vector2 direction = ((Vector2)transform.position - origin).normalized * damageStep;
		float time = 0;
		while (time < damageDuration) {
			TryMove (direction, false);
			yield return null;
			time += Time.deltaTime;
		}
	}

	// Animação de morte
	public IEnumerator Die() {
		// TODO: animaçãozinha simples de morte
		SendMessage ("OnDie");
		yield return null;
		Destroy (gameObject);
	}

}
