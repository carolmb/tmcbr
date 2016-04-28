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
	
	// Abre o baú
	public void OnMouseDown() {
		Vector2 position = MazeManager.WorldToTilePos(transform.position);
		Tile t = MazeManager.maze.tiles [(int)position.x, (int)position.y];

		// Muda para o baú aberto
		opened = true;

		// Adiciona as moedas
		Player.instance.IncrementCoins(coins);

		// Cria o objeto novo
		GameObject prefab = Resources.Load<GameObject> ("Prefabs/Obstacles/Hall/obstacle5");
		GameObject obj = Instantiate (prefab);
		obj.transform.position = MazeManager.TileToWorldPos (new Vector2 (t.x, t.y)) + new Vector3(0, Tile.size / 2, Tile.size / 2);;
		obj.name = "Tile[obstacle] (" + t.x + ", " + t.y + ")";
		MazeManager.obstacles [t.x, t.y] = obj.GetComponent<BoxCollider2D> ();

		// Muda o tipo do objeto 
		t.obstacleID = 5;

		// Destrói o antigo
		Destroy (this.gameObject);
	}
}
