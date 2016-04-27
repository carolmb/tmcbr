using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour {

	// Moedas
	private int coins;
	private bool opened;

	// Use this for initialization
	void Start () {
		opened = false;
		coins = Random.Range (0, 5);
	}
	
	// Update is called once per frame
	void Update () {
		//
	}

	// Abre o baú
	public void OnMouseDown() {
		Vector2 position = MazeManager.WorldToTilePos(transform.position);
		// Verifica se o player está em alguma das quatro posições adjacentes ao baú
		//if (MazeManager.WorldToTilePos (Player.instance.character.transform.position) == position) {
		//	return;
		//}
		opened = true;
		// Adiciona as moedas
		Player.instance.IncrementCoins(coins);
		// Muda para o baú aberto
		Tile t = MazeManager.maze.tiles [(int)position.x, (int)position.y];
		if (t.isWalkable) {
			Debug.Log ("Certo");
		} else {
			Debug.Log ("Errado");
		}
		t.objectName = "Obstacles/obstacle5";
		Destroy (this.gameObject);
	}
}
