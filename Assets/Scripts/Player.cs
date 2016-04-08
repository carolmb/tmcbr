using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Character))]
public class Player : MonoBehaviour {

	public static Player instance;

	private Character character;
	private Vector2 moveVector;

	void Awake() {
		instance = this;
		character = GetComponent<Character> ();
	}

	// Movimento pelo Input
	void Update() {
		moveVector.x = Input.GetAxisRaw ("Horizontal");
		moveVector.y = Input.GetAxisRaw ("Vertical");
		if (moveVector.x != 0 || moveVector.y != 0) {
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
			}
		} else {
			character.Stop ();
		}
	}

	// Tenta se mover no dado ângulo
	// Se conseguir, move e retorn true; se não, apenas retorna false
	bool TryMove(float angle) {
		Vector2 translation = GameManager.AngleToVector (angle) * character.speed;
		return character.InstantMove (translation);
	}

}
