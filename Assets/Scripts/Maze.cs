using UnityEngine;
using System.Collections;

public class Maze : MonoBehaviour {

	public static Maze instance;

	Tile[,] tiles; //GameObject Tile
	//enemys
	//blablabla

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
		Map map = new Map(0, 0); //creates de logic maze (just 0s and 1s)
		CreateTiles (map);
	}

	void CreateTiles (Map map) { 
		tiles = new Tile[map.width, map.height];
		for (int i = 0; i < map.height; i++) {
			for(int j = 0; j < map.width; j++) {
				tiles [i, j] = new Tile ();
				tiles [i, j].isWalkable = map.map[i, j].isWalkeable;
				tiles [i, j].x = i;
				tiles [i, j].y = j;
			}
		}
	}

}
