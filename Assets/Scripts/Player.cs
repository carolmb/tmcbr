using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent (typeof(Character))]
public class Player : MonoBehaviour {

	public static Player instance;

	public bool paused;

	public Character character;
	private Vector2 moveVector;

	public bool canMove;

	// Guardar referências
	void Awake () {
		instance = this;
		character = GetComponent<Character> ();
	}

	// Estado inicial
	void Start () {
		Resume ();
		canMove = true;
		character.lifePoints = SaveManager.currentSave.lifePoints;
		GameHUD.instance.UpdateLife (character.lifePoints);
		if (!visible) {
			visible = true;
			SetVisible (false);
		}
	}

	// Inputs e checagem de estados
	void Update () {
		moved = false;
		interactedPoint = Vector2.zero;

		// Checar o pause do jogo
		CheckPause ();

		if (paused)
			return;

		// Guardar tile visitado
		Tile tile = character.currentTile;
		if (tile.visited == false) {
			tile.visited = true;
		}

		if (character.moving)
			return;

		// Checar outros bagulho
		CheckInteract ();
		CheckItems ();
		CheckMovement ();

		if (character.currentTile != tile) {
			Minimap.Update (tile);
			Invoke ("UpdateMap", 0.1f);
		}
	}

	void UpdateMap() {
		GameHUD.instance.UpdateMap ();
	}

	void OnDestroy() {
		instance = null;
	}

	// ===============================================================================
	// Interação
	// ===============================================================================

	public Vector2 interactedPoint { get; private set; }

	private void CheckInteract() {
		if (Analog.input == Vector2.zero) {
			if (GameManager.ClickInteractInput ()) {
				interactedPoint = GameManager.InteractPosition ();
			} else if (GameManager.KeyBoardInteractInput ()) {
				interactedPoint = transform.position;
			} else {
				interactedPoint = Vector2.zero;
			}
		}
	}

	// ===============================================================================
	// Movimento
	// ===============================================================================

	// Movimento pelo Input
	void CheckMovement () {
		if (!canMove || character.damaging) {
			moved = false;
			return;
		}

		moveVector.x = Input.GetAxisRaw ("Horizontal");
		moveVector.y = Input.GetAxisRaw ("Vertical");

		if (moveVector == Vector2.zero) {
			moveVector = Analog.input;
		}

		moveVector.Normalize ();
		moveVector *= character.speed * 60 * Time.deltaTime * inputFactor;

		moved = moveVector != Vector2.zero;

		if (!moved) {
			// Se não apertou botão
			if (!character.moving)
				character.Stop ();
		} else {
			if (character.moving)
				character.Stop ();

			float angle = character.TryMove (moveVector);
			if (!float.IsNaN (angle)) {
				// Se moveu
				character.TurnTo (angle);
				CheckTransition ();
			} else {
				character.TurnTo (GameManager.VectorToAngle (moveVector));
				character.Stop ();
			}
		}
	}

	public static float inputFactor = 1;
	public bool moved = false;


	// Verifica se o player chegou nem tile que tem uma transição
	void CheckTransition () {
		Tile tile = character.currentTile;
		if (tile.transition != null && tile.transition.instant) {
			MazeManager.GoToMaze (tile.transition);
		}
	}

	// ===============================================================================
	// Pause
	// ===============================================================================

	public Button menuButton;
	public Button returnButton;

	// Verificar botão de pause
	void CheckPause() {
		if (Input.GetButtonDown ("Menu")) {
			if (!paused) {
				if (!GameHUD.instance.gameObject.activeSelf)
					return;
				menuButton.onClick.Invoke ();
			} else {
				returnButton.onClick.Invoke ();
			}
		}
	}

	// Pausar jogo
	public void Pause () {
		paused = true;
		Time.timeScale = 0;
	}

	// Despausar jogo
	public void Resume () {
		paused = false;
		Time.timeScale = 1;
	}

	// ===============================================================================
	// Itens
	// ===============================================================================

	// Atualizar uso de itens
	void CheckItems() {
		if (repelling) {
			repelTime -= Time.deltaTime;
		}
		if (canMove && !character.damaging && Input.GetButtonDown ("Item")) {
			UseItem ();
		}
	}

	public void IncrementCoins (int value) {
		Bag.current.coins += value;
	}

	public void IncrementRoses (int value) {
		Bag.current.roses += value;
	}

	public void ChooseItem (int pos) {
		Bag.current.selectedPosition = pos;
		GameHUD.instance.UpdateItem (Bag.current.selectedItem, Bag.current.selectedSlot.count);
		SetVisible (true);
	}

	public void UseItem () {
		Item item = Bag.current.selectedItem;
		if (item != null && item.CanUse()) {
			item.OnUse ();
			if (item.consumable) {
				ConsumeItem ();
			}
		}
	}

	public void ConsumeItem () {
		int pos = Bag.current.selectedPosition;
		ItemSlot slot = Bag.current.selectedSlot;
		Bag.current.Consume(pos);
		slot = Bag.current.selectedSlot;
		if (slot == null)
			GameHUD.instance.UpdateItem (null, 0);
		else
			GameHUD.instance.UpdateItem (Bag.current.selectedItem, slot.count);
	}

	// ===============================================================================
	// Dano
	// ===============================================================================

	// Imunidade a dano
	public bool immune;
	public float immuneTime = 1f;

	// Tempo de mudança de vermelho pra branco
	public float blinkFreq = 0.075f;

	// Atualizar interface e piscar
	public void OnDamage () {
		SoundManager.PlayerDamage ();
		SaveManager.currentSave.lifePoints = character.lifePoints;
		GameHUD.instance.UpdateLife (character.lifePoints);
		visible = true;
		Knife.DestroyKnife ();
		if (character.lifePoints > 0) {
			StartCoroutine (Blink ());
		}
	}

	// Piscar quando o jogador leva dano
	private IEnumerator Blink () {
		immune = true;
		float time = 0;
		bool red = false;

		while (time < immuneTime) {
			character.spriteRenderer.color = red ? Color.white : Color.red;
			time += blinkFreq;
			red = !red;
			yield return new WaitForSeconds (blinkFreq);
		}
		character.spriteRenderer.color = Color.white;
		immune = false;
	}

// ===============================================================================
	// Morte
	// ===============================================================================

	public Sprite deadSprite;

	// Sair do jogo quando morrer
	public void OnDie () {
		character.damaging = true;
		character.Stop ();
		canMove = false;
		GameHUD.instance.gameObject.SetActive (false);
		StartCoroutine (DieAnimation ());
	}

	private IEnumerator DieAnimation () {
		MazeManager.musicPlayer.Stop ();
		Coroutine c = StartCoroutine (GameCamera.instance.FadeOut (0.5f));
		yield return new WaitForSeconds (1f);
		SoundManager.DieCollision ();
		character.animator.enabled = false;
		character.spriteRenderer.sprite = deadSprite;
		yield return c;
		SceneManager.LoadScene ("Title");
	}

	// ===============================================================================
	// Efeitos de Itens
	// ===============================================================================

	// Tempo restante para acabar o efeito do repelente
	public float repelTime = 0;

	// Checar se está sob efeito do repelente
	public bool repelling {
		get { return repelTime > 0; }
	}

	// Checar se está sob o efeito da capa
	public static bool visible = true;

	// Mudar visibilidade alterando a cor junto
	public void SetVisible(bool value) {
		if (value) {
			if (!visible) {
				character.spriteRenderer.color += new Color (0, 0, 0, 0.5f);
			}
			visible = true;
		} else {
			if (visible) {
				character.spriteRenderer.color -= new Color (0, 0, 0, 0.5f);
			}
			visible = false;
		}
	}

	// ===============================================================================
	// Queda
	// ===============================================================================

	public IEnumerator Fall (Tile.Transition transition) {
		canMove = false;
		character.damaging = true;
		character.Stop ();
		SoundManager.Fall ();
		yield return new WaitForSeconds (0.2f);
		Destroy (transform.GetChild (0).gameObject);
		float speed = 120;
		while (character.spriteRenderer.color.a > 0) {
			transform.Translate (0, -Time.deltaTime * speed, 0);
			character.spriteRenderer.color -= new Color (0, 0, 0, Time.deltaTime * 4);
			yield return null;
		}
		MazeManager.GoToMaze (transition);
	}

}
