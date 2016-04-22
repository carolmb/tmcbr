using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent (typeof(Character))]
public class Player : MonoBehaviour {

	public static Player instance;

	public List<AudioSource> sounds = new List<AudioSource>();

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
		visible = true;
		character.lifePoints = SaveManager.currentSave.lifePoints;
		GameMenu.instance.UpdateLife (character.lifePoints);
	}

	// Inputs e checagem de estados
	void Update () {
		CheckPause ();
		if (paused) 
			return;

		// Guardar tile visitado
		character.currentTile.visited = true;

		CheckItems ();
		CheckMovement ();
	}

	// ===============================================================================
	// Movimento
	// ===============================================================================

	public Button menuButton;

	// Verificar botão de pause
	void CheckPause() {
		if (Input.GetButtonDown ("Menu")) {
			if (!paused) {
				menuButton.onClick.Invoke ();
			} else {
				Resume ();
				GameMenu.instance.CloseMenu ();
			}
		}
	}

	// Atualizar uso de itens
	void CheckItems() {
		Knife.CheckAttackEnd();
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

	void MoveByMouse() {
		if (Input.GetMouseButton (0)) {
			Vector2 point = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			point = MazeManager.TileToWorldPos (MazeManager.WorldToTilePos (point));
			character.TurnTo (point);
			character.MoveTo (point);
		}
	}

	void MoveByKeyboard() {
		if (!canMove || character.damaging)
			return;

		moveVector.x = Input.GetAxisRaw ("Horizontal");
		moveVector.y = Input.GetAxisRaw ("Vertical");

		if (moveVector.x == 0 && moveVector.y == 0) {
			// Se não apertou botão
			if (!character.moving)
				character.Stop ();
		} else {
			if (character.moving)
				character.Stop();

			float angle = character.TryMove (moveVector);
			if (!float.IsNaN(angle)) {
				// Se moveu
				character.TurnTo (angle);
				CheckTransition ();
			} else {
				character.TurnTo(GameManager.VectorToAngle(moveVector));
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

	public Bag bag { get { return SaveManager.currentSave.bag; } }

	public void IncrementCoins (int value) {
		bag.coins += value;
		GameMenu.instance.UpdateCoins (bag.coins);
	}

	public void IncrementRoses (int value) {
		bag.roses += value;
		GameMenu.instance.UpdateRoses (bag.roses);
	}

	public Item selectedItem {
		get { 
			if (bag.selectedSlot > -1) {
				int itemID = bag.selectedItemID;
				if (itemID > -1)
					return Item.DB [itemID];
			}
			return null;
		}
	}

	public void ChooseItem (int id) {
		bag.selectedSlot = id;
		GameMenu.instance.UpdateItem (selectedItem);
		Resume ();
	}

	public void UseItem () {
		if (selectedItem != null) {
			selectedItem.OnUse ();
			if (selectedItem.consumable) {
				bag.itemIDs[bag.selectedSlot] = -1;
				bag.selectedSlot = -1;
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
		GameMenu.instance.UpdateLife (character.lifePoints);
		if (character.lifePoints > 0)
			StartCoroutine (Blink ());
	}

	// Som dos passos
	public void Footstep() {
		//
		int i = Random.Range(0, 3);
		sounds[i].Play();
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

	// Checar se está sob o efeito da capa
	// TODO: mudar para o ID do item da capa
	public bool visible;

	// Tempo restante para acabar o efeito do relepente
	public float repelTime = 0;

	// Checar se está sob efeito do repelente
	public bool repelling {
		get { return repelTime > 0; }
	}

}
