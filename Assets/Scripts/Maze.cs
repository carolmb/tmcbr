using UnityEngine;
using System.Collections;

public class Maze : MonoBehaviour {

	public static Maze instance;

	public string mazeTheme;
	public int size;

	public Tile[,] tiles; //GameObject Tile
	public Tile beginTile;

	public int width {
		get { return tiles.GetLength (0); }
	}

	public int height {
		get { return tiles.GetLength (1); }
	}

	// Use this for initialization
	void Awake () {
		instance = this;
		LoadMaze ();
	}

	void LoadMaze() {
		// TODO: carregar do salvo (o gerador aqui é temporário)
		MazeGenerator.CreateMaze (size, size);

		foreach (Tile t in tiles) {
			
			GameObject floor = CreateTileObject (t.x, t.y, "floor");
			Vector3 pos = floor.transform.position;
			pos.z = 999;
			floor.transform.position = pos;

			if (t.isWall) {
				CreateTileObject (t.x, t.y, "wall").transform.Translate (0, 0, 1);
			}
			if (t.obstacle >= 0) {
				CreateTileObject (t.x, t.y, "obstacle" + t.obstacle);
			}
			if (t.prefab != null) {
				CreateTilePrefab (t.x, t.y, t.prefab);
			}

		}
	}

	void Start() {
		Vector2 tilePos = new Vector2 (beginTile.x, beginTile.y);
		Player.instance.transform.position = Maze.TileToWorldPosition (tilePos) + new Vector3(0, Tile.size / 2, 0);
	}

	GameObject CreateTilePrefab(int x, int y, GameObject prefab) {
		GameObject obj = Instantiate (prefab);
		obj.transform.position = TileToWorldPosition (new Vector2 (x, y));
		obj.transform.SetParent (transform);
		return obj;
	}

	GameObject CreateTileObject(int x, int y, string spriteName) {
		GameObject obj = new GameObject ();
		obj.name = "Tile (" + x + ", " + y + ")";
		obj.transform.position = TileToWorldPosition (new Vector2 (x, y));
		SpriteRenderer sr = obj.AddComponent<SpriteRenderer> ();
		sr.sprite = Resources.Load<Sprite> ("Images/Tilesets/" + mazeTheme + "/" + spriteName);
		obj.transform.SetParent (transform);
		return obj;
	}

	public static Vector3 TileToWorldPosition(Vector2 tilePos) {
		tilePos = tilePos * Tile.size;
		return new Vector3 (tilePos.x, tilePos.y, tilePos.y);
	}

	public static Vector2 WorldToTilePos(Vector2 worldPos) {
		Vector2 tilePos = new Vector2(Mathf.Round(worldPos.x / Tile.size), Mathf.Round(worldPos.y / Tile.size));


		Debug.Log ("world: " + worldPos);
		Debug.Log ("tile: " + tilePos);

		return tilePos;
	}

}
	