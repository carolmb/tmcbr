using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Transition {
	public int mazeID;
	public float tileX;
	public float tileY;
	public int direction;
	public Transition(int id, float x, float y, int dir) {
		mazeID = id;
		tileX = x;
		tileY = y;
		direction = dir;
	}
}

[System.Serializable]
public class Tile {

	public const int size = 32;

	public int x, y; // coordenadas
	public Vector2 coordinates {
		get { return new Vector2 (x, y); }
	}

	// Informações dos gráficos de cenário do tile
	public int floorID = 1; // nunca é 0 (se não só tem um vácuo)
	public int wallID = 0; // 0 é sem parede
	public int obstacleID = 0; // 0 é sem obstáculo

	// Informações do objeto do tile
	public string objectName = ""; // nome do objeto (inimigo ou item), se tiver 

	// Spawn de objeto
	public float lastSpawn = 0f;
	public float spawnTime = 0f;

	// Se o tempo de spawn já deu
	public bool canSpawn {
		get {
			return spawnTime <= SaveManager.currentPlayTime - lastSpawn;
		}
	}

	public bool visited = false; // usado para checar se foi visitado pelo jogador
	public Transition transition; // se ao tocar, para onde o player vai

	public bool isWalkable {
		get { return !isWall && obstacleID <= 0; }
	}

	public bool isWall {
		get { return wallID > 0; }
	}

	public Tile() {}

	public Tile(int x, int y) {
		this.x = x;
		this.y = y;
	}

	public Tile(Tile copy) {
		this.x = copy.x;
		this.y = copy.y;
		this.wallID = copy.wallID;
		this.floorID = copy.floorID;
		this.obstacleID = copy.obstacleID;
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
