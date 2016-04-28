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
	public void openChest() {
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

		GameObject prefab = Resources.Load<GameObject> ("Prefabs/OpenedChest");
		GameObject obj = Instantiate (prefab);
		obj.transform.position = MazeManager.TileToWorldPos (new Vector2 (t.x, t.y)) + new Vector3(0, Tile.size / 2, Tile.size / 2);
		obj.transform.SetParent (transform);
		obj.name = "OpenedChest";

		Destroy (this);
	}
}
