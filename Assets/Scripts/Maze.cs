using UnityEngine;
using System.Collections;

public class Maze : MonoBehaviour {

	public static Maze instance;
	public static Tile begin;

	public string mazeTheme;

	Tile[,] tiles; //GameObject Tile

	bool visited = false;

	public int width {
		get { return tiles.GetLength (0); }
	}

	public int height {
		get { return tiles.GetLength (1); }
	}

	public Tile this[int i, int j] {
		get { return tiles [i, j]; }
	}

	// Use this for initialization
	void Awake () {
		instance = this;
		LoadMaze ();
	}

	void LoadMaze() {
		if (visited) {
			// TODO: carregar do salvo
		} else {
			tiles = MazeGenerator.CreateMaze (10, 10);
		}

		foreach (Tile t in tiles) {
			CreateTileObject (t.x, t.y, "floor").transform.Translate (0, 0, 999 - transform.position.z);
			if (t.isWall) {
				CreateTileObject (t.x, t.y, "wall").transform.Translate (0, 0, 1);
			}
			if (t.obstacle >= 0) {
				CreateTileObject (t.x, t.y, "obstacle" + t.obstacle);
			}
		}
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
		return new Vector2(Mathf.Round(worldPos.x / Tile.size), Mathf.Round(worldPos.y / Tile.size));
	}

}
	