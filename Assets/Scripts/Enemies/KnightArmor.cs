using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Character))]
public class KnightArmor : MonoBehaviour {

	private Character character;

	void Awake() {
		character = GetComponent<Character> ();
	}

	void Update() {
		// Checar o raio, depois o caminho, e então andar para o primeiro tile desse caminho
		// Acessa o player por Player.instance
		// Maze.WorldToTilePos()
	}

}
