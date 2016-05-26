using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
		CheckPause ();
		CheckInteract ();
		if (paused) 
			return;

		// Guardar tile visitado
		character.currentTile.visited = true;

		CheckItems ();
		CheckMovement ();
	}

	void OnDestroy() {
		instance = null;
	}

	// ===============================================================================
	// Interação
	// ===============================================================================

	public Vector2 interactedPoint { get; private set; }

	private void CheckInteract() {
		if (GameManager.InteractInput ()) {
			Vector2 point = GameManager.InputPosition ();
			interactedPoint = Camera.main.ScreenToWorldPoint (point);
		} else {
			interactedPoint = Vector2.zero;
		}
	}

	// ===============================================================================
	// Movimento
	// ===============================================================================

	public Button menuButton;
	public Button returnButton;

	// Verificar botão de pause
	void CheckPause() {
		if (Input.GetButtonDown ("Menu")) {
			if (!paused) {
				menuButton.onClick.Invoke ();
			} else {
				returnButton.onClick.Invoke ();
			}
		}
	}

	// Atualizar uso de itens
	void CheckItems() {
		if (repelling) {
			repelTime -= Time.deltaTime;
		}
		if (canMove && Input.GetButtonDown ("Item")) {
			UseItem ();
		}
	}

	// Movimento pelo Input
	void CheckMovement() {
		//MoveByMouse ();
		MoveByKeyboard ();
	}

	void MoveByMouse () {
		if (Input.GetMouseButton (0)) {
			Vector2 point = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			point = MazeManager.TileToWorldPos (MazeManager.WorldToTilePos (point));
			character.TurnTo (point);
			character.MoveTo (point);
		}
	}

	public static float inputFactor = 1;
	public bool moved = false;

	void MoveByKeyboard () {
		if (!canMove || character.damaging) {
			moved = false;
			return;
		}

		moveVector.x = Input.GetAxisRaw ("Horizontal") * character.speed * inputFactor;
		moveVector.y = Input.GetAxisRaw ("Vertical") * character.speed * inputFactor;

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
				character.TurnTo(GameManager.VectorToAngle (moveVector));
				character.Stop ();
			}
		}
	}

	// Verifica se o player chegou nem tile que tem uma transição
	void CheckTransition () {
		Tile tile = character.currentTile;
		if (tile.transition != null) {
			MazeManager.GoToMaze (tile.transition);
		}
	}

	// ===============================================================================
	// Pause
	// ===============================================================================

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
				int pos = Bag.current.selectedPosition;
				ItemSlot slot = Bag.current.selectedSlot;
				Bag.current.Consume(pos);
				slot = Bag.current.selectedSlot;
				if (slot == null)
					GameHUD.instance.UpdateItem (null, 0);
				else
					GameHUD.instance.UpdateItem (item, slot.count);
			}
		}
	}

	// ===============================================================================
	// Dano e Morte
	// ===============================================================================

	// Imunidade a dano
	public bool immune;
	public float immuneTime = 1f;

	// Tempo de mudança de vermelho pra branco
	public float blinkFreq = 0.075f;

	// Atualizar interface e piscar
	public void OnDamage () {
		SaveManager.currentSave.lifePoints = character.lifePoints;
		GameHUD.instance.UpdateLife (character.lifePoints);
		visible = true;
		Knife.DestroyKnife ();
		if (character.lifePoints > 0)
			StartCoroutine (Blink ());
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

	// Sair do jogo quando morrer
	public void OnDie () {
		MazeManager.GoToMaze (MazeManager.currentTransition);
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
	// Sons
	// ===============================================================================

	public AudioClip[] stepSounds;

	// Som dos passos
	public void Footstep () {
		int type = character.currentTile.type;
		GameCamera.PlayAudioClip (stepSounds [type * 3 + Random.Range (0, 3)], 0.5f - 0.35f * type);
	}

}
