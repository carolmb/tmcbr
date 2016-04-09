using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Character))]
public class Player : MonoBehaviour {

	// TODO: verificar a transição do tile em que ele está

	public static Player instance;

	public Character character;
	private Vector2 moveVector;

	void Awake() {
		instance = this;
		character = GetComponent<Character> ();
	}

	// Movimento pelo Input
	void Update() {
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

}
