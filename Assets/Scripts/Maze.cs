using UnityEngine;
using System.Collections;

public class Maze : MonoBehaviour {
	
	Tile[,] maze; //GameObject Tile
	//enemys
	//blablabla

	// Use this for initialization
	void Start () {
		Map map = new Map(0, 0); //creates de logic maze (just 0s and 1s)
		CreateTiles (map);
	}

	// Update is called once per frame
	void Update () {
	
	}

	void CreateTiles (Map map) { 
		for (int i = 0; i < map.height; i++) {
			for(int j = 0; j < map.width; j++) {
				maze [i, j].isWalkable = map.map[i, j] == 1 ? true : false;
				maze [i, j].x = i;
				maze [i, j].y = j;
			}
		}
	}

}
