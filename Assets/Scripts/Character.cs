using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : CharacterBase {

	protected override void Awake () {
		base.Awake ();
		Stop ();
	}

	// Ajusta a coordenada Z do personagem para ser igual à Y
	void LateUpdate () {
		Vector3 pos = transform.position;
		pos.z = pos.y;
		transform.position = pos;
	}

	public override void InitialDirection () {
		base.InitialDirection ();
		Stop ();
	}

	// ===============================================================================
	// Movimento
	// ===============================================================================

	public bool moving;
	public float speed = 2; // pixels por frame

	private Coroutine currentMovement;

	// Move, dentro de um frame, o personagem em direção translation
	// Retorna se foi possível mover
	private bool InstantMove(Vector2 translation, bool animate = true) {
		return InstantMoveTo((Vector2) transform.position + translation, animate);
	}

	// Move, dentro de um frame, o personagem para o point
	// Retorna se foi possível mover
	private bool InstantMoveTo(Vector2 point, bool animate = true) {
		if (CanMoveTo (point)) {
			transform.position = point;
			if (animate)
				animator.speed = 1;
			return true;
		} else {
			//Debug.Log ("couldn't move");
			return false;
		}
	}

	// USE ESSA FUNÇÃO AQUI (lembrar de usar o StartCoroutine)
	// Move o personagem uma distância gradativamente
	// É possível verificar se o personagem está andando pela variável moving
	public Coroutine Move(Vector2 transition, bool breakOnCollision = false) {
		return MoveTo(transition + (Vector2)transform.position, breakOnCollision);
	}

	// OU ESSA
	// Move o personagem para o ponto dest, gradativamente
	public Coroutine MoveTo(Vector2 dest, bool breakOnCollision = false) {
		if (currentMovement != null)
			StopCoroutine (currentMovement);
		currentMovement = StartCoroutine (MoveTo_coroutine (dest, breakOnCollision));
		return currentMovement;
	}

	private IEnumerator MoveTo_coroutine(Vector2 dest, bool breakOnCollision) {
		moving = true;
		Vector2 orig = (Vector2)transform.position;
		float distance = (dest - orig).magnitude;
		float percentage = 0;
		float percSpeed = speed / distance;
		while (percentage <= 1) {
			if (!Player.instance.paused) {
				percentage += percSpeed * 60 * Time.deltaTime;
				Vector2 moveVetor = Vector2.Lerp (orig, dest, percentage) - (Vector2)transform.position;
				float angle = TryMove(moveVetor);
				if (breakOnCollision && float.IsNaN(angle)) {
					break;
				}
			}
			yield return null;
		}
		moving = false;
	}

	public override void Stop() {
		base.Stop ();
		moving = false;
	}

	// ===============================================================================
	// Colisão
	// ===============================================================================

	public bool collides = true;

	// Tenta se mover na direção do atual moveVector
	// Se conseguir, move e retorna o ângulo que andou; se não, retorna NaN
	public float TryMove(Vector2 moveVector, bool animate = true) {
		float spd = moveVector.magnitude;
		float angle = GameManager.VectorToAngle (moveVector);
		//angle = Mathf.Round (angle / 45) * 45;

		if (TryMove (angle, spd, animate)) {
			return angle;
		} else if (TryMove (angle + 45, spd, animate)) {
			return angle + 45; 
		} else if (TryMove (angle - 45, spd, animate)) {
			return angle - 45;
		} else {
			return float.NaN;
		}
	}

	// Tenta se mover no dado ângulo
	// Se conseguir, move e retorna true; se não, apenas retorna false
	bool TryMove (float angle, float speed, bool animate) {
		Vector2 translation = GameManager.AngleToVector (angle) * speed * 60 * Time.deltaTime;
		return InstantMove (translation, animate);
	}

	// Verifica se tal posição é passável para o personagem (checa cada ponto de seu colisor)
	private bool CanMoveTo (Vector2 newPosition) {
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
	public bool Collides (float x, float y) {
		return MazeManager.Collides (x, y);
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

	// Som ao levar dano
	public AudioClip damageSound;

	// Animação de dano
	public Coroutine Damage(Vector2 origin, int value) {
		if (currentMovement != null) {
			StopCoroutine (currentMovement);
		}
		currentMovement = StartCoroutine (Damage_coroutine (origin, value));
		return currentMovement;
	}

	private IEnumerator Damage_coroutine(Vector2 origin, int value) {
		damaging = true;

		lifePoints = Mathf.Max (0, lifePoints - value);
		SendMessage ("OnDamage");

		if (damageSound != null) {
			GameCamera.PlayAudioClip (damageSound);
		}

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
		Vector2 direction = ((Vector2)transform.position - origin).normalized * damageSpeed;
		float time = 0;
		bool broke = false;
		while (time < damageDuration) {
			if (!broke) {
				if (float.IsNaN(TryMove (direction, false))) {
					broke = true;
				}
			}
			yield return null;
			time += Time.deltaTime;
		}
		damaging = false;
	}

	// Animação de morte
	public IEnumerator Die() {
		// TODO: animaçãozinha simples de morte
		SendMessage ("OnDie");
		yield return null;
		Destroy (gameObject);
	}

}
