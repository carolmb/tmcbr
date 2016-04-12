using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Character))]
public class Player : MonoBehaviour {

	public static Player instance;

	public bool paused;

	public Character character;
	private Vector2 moveVector;

	void Awake() {
		instance = this;
		character = GetComponent<Character> ();
		Resume ();
	}

	void Start() {
		character.lifePoints = SaveManager.currentSave.lifePoints;
		GameMenu.instance.UpdateLife (character.lifePoints);
	}

	// ===============================================================================
	// Movimento
	// ===============================================================================

	// Movimento pelo Input
	void Update() {
		if (paused || character.damaging)
			return;

		moveVector.x = Input.GetAxisRaw ("Horizontal");
		moveVector.y = Input.GetAxisRaw ("Vertical");
		if (moveVector.x == 0 && moveVector.y == 0) {
			// Se não apertou botão
			character.Stop ();
		} else {
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
	void CheckTransition() {
		Tile tile = character.currentTile;
		if (tile.transition != null) {
			MazeManager.GoToMaze (tile.transition);
		}
	}

	// ===============================================================================
	// Pause
	// ===============================================================================

	public void Pause() {
		paused = true;
		Time.timeScale = 0;
	}

	public void Resume() {
		paused = false;
		Time.timeScale = 1;
	}

	// ===============================================================================
	// Itens
	// ===============================================================================

	public Bag bag { get { return SaveManager.currentSave.bag; } }

	public void IncrementCoins(int value) {
		bag.coins += value;
		GameMenu.instance.UpdateCoins (bag.coins);
	}

	public void IncrementRoses(int value) {
		bag.roses += value;
		GameMenu.instance.UpdateRoses (bag.roses);
	}

	public Item currentItem;

	public void ChooseItem(int id) {
		int itemID = bag.itemIDs [id];
		currentItem = Item.DB [itemID];
		GameMenu.instance.UpdateItem (currentItem);
		Resume ();
	}

	// ===============================================================================
	// Dano e Morte
	// ===============================================================================

	public bool immune { get; private set; }
	public float immuneTime = 1f;

	public float blinkFreq = 0.075f;

	public void OnDamage() {
		SaveManager.currentSave.lifePoints = character.lifePoints;
		GameMenu.instance.UpdateLife (character.lifePoints);
		StartCoroutine (Blink ());
	}

	private IEnumerator Blink() {
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

	public void OnDie() {
		GameMenu.instance.Quit ();
	}

}
