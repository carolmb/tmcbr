using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile {

	public readonly static int size = 32;

	public int x, y; // coordenates

	public bool isWall = false; // floor or wall
	public int obstacle = -1; // -1 is no obstacle
		
	public bool visited; // to algorithms and to check if visited by player

	public GameObject prefab; // 

	public Tile() {}

	public Tile(int x, int y) {
		this.x = x;
		this.y = y;
	}

	public bool isWalkable {
		get { return !isWall && obstacle < 0; }
	}

	public List<Tile> GetNeighbours4() {
		List<Tile> neightbours = new List<Tile> ();
		neightbours.Add (Maze.instance [x, y + 1]);
		neightbours.Add (Maze.instance [x, y - 1]);
		neightbours.Add (Maze.instance [x + 1, y]);
		neightbours.Add (Maze.instance [x - 1, y]);
		return neightbours;
	}

	public List<Tile> GetNeighbours8() {
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
