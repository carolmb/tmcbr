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
	public void OnMouseClick() {
		Vector2 position = MazeManager.WorldToTilePos(transform.position);
		// Verifica se o player está em alguma das quatro posições adjacentes ao baú
		//if (MazeManager.WorldToTilePos (Player.instance.character.transform.position) == position) {
		//	return;
		//}
		opened = true;
		Debug.Log ("Opened");
		Player.instance.bag.coins += coins;
		Tile t = MazeManager.maze.tiles [(int)position.x, (int)position.y];
		t.objectName = "OpenedChest";
		t.obstacleID = 1;
		Destroy (this);
	}
}
