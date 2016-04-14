using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Transition {
	public int mazeID;
	public int tileX;
	public int tileY;
	public int direction;
	public Transition(int id, int x, int y, int dir) {
		mazeID = id;
		tileX = x;
		tileY = y;
		direction = dir;
	}
}

[System.Serializable]
public class Tile {

	public readonly static int size = 32;

	public int x, y; // coordenadas
	public Vector2 coordinates {
		get { return new Vector2 (x, y); }
	}
		
	public bool isWall = false; // chão ou parede
	public int obstacle = -1; // -1 é sem obstáculo
	public string objectName = ""; // nome do objeto (inimigo ou item), se tiver 
	public bool visited = false; // usado para checar se foi visitado pelo jogador
	public Transition transition; // se ao tocar, para onde o player vai

	public Tile() {}

	public Tile(int x, int y) {
		this.x = x;
		this.y = y;
	}

	public Tile(Tile copy) {
		this.x = copy.x;
		this.y = copy.y;
		this.isWall = copy.isWall;
		this.obstacle = copy.obstacle;
		//this.transition = copy.transition;
	}

	public bool isWalkable {
		get { return !isWall && obstacle < 0; }
	}

	public List<Tile> GetNeighbours4() {
		List<Tile> neighbours = new List<Tile> ();
		if (x - 1 >= 0) {
			neighbours.Add (MazeManager.maze.tiles [x - 1, y]);
		}
		if (x + 1 <= MazeManager.maze.width - 1) {
			neighbours.Add (MazeManager.maze.tiles [x + 1, y]);
		} 
		if (y - 1 >= 0) {
			neighbours.Add (MazeManager.maze.tiles [x, y - 1]);
		} 
		if (y + 1 <= MazeManager.maze.height - 1) {
			neighbours.Add (MazeManager.maze.tiles [x, y + 1]);
		}
		return neighbours;
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
