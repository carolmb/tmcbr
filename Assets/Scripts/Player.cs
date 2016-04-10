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

	// ===============================================================================
	// Movimento
	// ===============================================================================

	// Movimento pelo Input
	void Update() {
		if (paused)
			return;

		moveVector.x = Input.GetAxisRaw ("Horizontal");
		moveVector.y = Input.GetAxisRaw ("Vertical");
		if (moveVector.x == 0 && moveVector.y == 0 || !TryMove ()) {
			// Se não apertou botão ou colidiu com tile
			character.Stop ();
		} else {
			// Se moveu
			CheckTransition();
		}
	}

	// Tenta se mover na direção do atual moveVector
	// Se conseguir, move e retorna true; se não, apenas retorna false
	bool TryMove() {
		float angle = GameManager.VectorToAngle (moveVector);
		if (TryMove (angle)) {
			character.TurnTo (angle);
		} else if (TryMove (angle + 45)) {
			character.TurnTo (angle + 45); 
		} else if (TryMove (angle - 45)) {
			character.TurnTo(angle - 45);
		} else {
			character.TurnTo (angle);
			character.Stop ();
			return false;
		}
		return true;
	}

	// Tenta se mover no dado ângulo
	// Se conseguir, move e retorna true; se não, apenas retorna false
	bool TryMove(float angle) {
		Vector2 translation = GameManager.AngleToVector (angle) * character.speed;
		return character.InstantMove (translation);
	}

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

}
