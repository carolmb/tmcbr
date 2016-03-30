using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile {

	public readonly static int size = 32;

	public bool isWalkable; //false is a wall, true is a way
	public int x, y; //coordenates

	GameObject[] myObjects; //objects in the tile: player, enemys...

	void Start () {
		
	}
	
	void Update () {
	
	}
		
	public List<Tile> GetNeighbours() {
		List<Tile> neightbours = new List<Tile> ();
		neightbours.Add (Maze.instance [x, y + 1]);
		neightbours.Add (Maze.instance [x, y - 1]);
		neightbours.Add (Maze.instance [x + 1, y]);
		neightbours.Add (Maze.instance [x + 1, y + 1]);
		neightbours.Add (Maze.instance [x + 1, y - 1]);
		neightbours.Add (Maze.instance [x - 1, y]);
		neightbours.Add (Maze.instance [x - 1, y + 1]);
		neightbours.Add (Maze.instance [x - 1, y - 1]);
		return neightbours;
	}


}
