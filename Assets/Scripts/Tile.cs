using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile {

	public readonly static int size = 32;

	public int x, y; // coordenates

	public bool isWall = false; // floor or wall
	public int obstacle = -1; // -1 is no obstacle
		
	public bool visited; // usado para algoritmos e posteriormente para checar se foi visitado pelo jogador

	public string objectName; // nome do objeto (inimigo ou item), se tiver 

	public Tile() {}

	public Tile(int x, int y) {
		this.x = x;
		this.y = y;
		objectName = "";
	}

	public bool isWalkable {
		get { return !isWall && obstacle < 0; }
	}

	public List<Tile> GetNeighbours4() {
		List<Tile> neightbours = new List<Tile> ();
		neightbours.Add (MazeManager.maze.tiles [x, y + 1]);
		neightbours.Add (MazeManager.maze.tiles [x, y - 1]);
		neightbours.Add (MazeManager.maze.tiles [x + 1, y]);
		neightbours.Add (MazeManager.maze.tiles [x - 1, y]);
		return neightbours;
	}

	public List<Tile> GetNeighbours8() {
		List<Tile> neightbours = new List<Tile> ();
		neightbours.Add (MazeManager.maze.tiles [x, y + 1]);
		neightbours.Add (MazeManager.maze.tiles [x, y - 1]);
		neightbours.Add (MazeManager.maze.tiles [x + 1, y]);
		neightbours.Add (MazeManager.maze.tiles [x + 1, y + 1]);
		neightbours.Add (MazeManager.maze.tiles [x + 1, y - 1]);
		neightbours.Add (MazeManager.maze.tiles [x - 1, y]);
		neightbours.Add (MazeManager.maze.tiles [x - 1, y + 1]);
		neightbours.Add (MazeManager.maze.tiles [x - 1, y - 1]);
		return neightbours;
	}
}
