using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeGenerator {

	private static bool[,] visited;

	private const int deltaEnemys = 4;

	static List<Tile> GetAllWallNeighbours (Maze maze, Tile tile) {
		List<Tile> neighbours = new List<Tile> ();
		if (tile.x - 1 >= 0 && maze.tiles[tile.x - 1, tile.y].isWall) {
			neighbours.Add (maze.tiles [tile.x - 1, tile.y]);
		}
		if (tile.x + 1 < maze.width && maze.tiles[tile.x + 1, tile.y].isWall) {
			neighbours.Add (maze.tiles [tile.x + 1, tile.y]);
		} 
		if (tile.y - 1 >= 0 && maze.tiles[tile.x, tile.y - 1].isWall) {
			neighbours.Add (maze.tiles [tile.x, tile.y - 1]);
		} 
		if (tile.y + 1 < maze.height && maze.tiles[tile.x, tile.y + 1].isWall) {
			neighbours.Add (maze.tiles [tile.x, tile.y + 1]);
		}

		return neighbours;
	}

	public static List<Tile> GetNeighbours (Maze maze, Tile tile, int delta) {
		List<Tile> neighbours = new List<Tile> ();
		if (tile.x - delta > 0 && !visited[tile.x - delta, tile.y]) {
			neighbours.Add (maze.tiles [tile.x - delta, tile.y]);
		}
		if (tile.x + delta < maze.width - 1 && !visited [tile.x + delta, tile.y]) {
			neighbours.Add (maze.tiles [tile.x + delta, tile.y]);
		} 
		if (tile.y - delta > 0 && !visited[tile.x, tile.y - delta]) {
			neighbours.Add (maze.tiles [tile.x, tile.y - delta]);
		} 
		if (tile.y + delta < maze.height - 1 && !visited[tile.x, tile.y + delta]) {
			neighbours.Add (maze.tiles [tile.x, tile.y + delta]);
		}

		return neighbours;
	}

	public static bool NotVisitedNeighbours (Maze maze, Tile tile, int delta) {
		if (GetNeighbours (maze, tile, delta).Count > 0) {
			return true;
		}
		return false;
	}
		
	static void InicializeNullMaze (Maze maze) {
		for (int i = 0; i < maze.width; i++) {
			for (int j = 0; j < maze.height; j++) {
				maze.tiles [i, j] = new Tile (i, j);
				if (i % 2 == 1 && j % 2 == 1) {
					maze.tiles [i, j].isWall = false;
					visited [i, j] = false;
				} else {
					maze.tiles [i, j].isWall = true;
					visited [i, j] = true;
				}
			}
		}
	}

	static void RemoveWall(Maze maze, Tile current, Tile neighbor) {
		int deltaX = neighbor.x - current.x ;
		int deltaY = neighbor.y - current.y;

		if (current.x + deltaX / 2 > 0 && current.y + deltaY / 2 > 0 && 
			current.x + deltaX / 2 < maze.width && current.y + deltaY / 2 < maze.height) {
			maze.tiles [current.x + deltaX / 2, current.y + deltaY / 2].isWall = false;
			visited [current.x + deltaX / 2, current.y + deltaY / 2] = true;
		} 
	}

	static Tile BeginMazeGenerator (Maze maze) {
		int h = maze.height;
		int y = Random.Range (1, h-2);
		if (y % 2 == 0) {
			y++;
		}
		maze.tiles [1, y].isWall = false;
		return maze.tiles [1, y];
	}

	public static bool EmptyRadiusToEnemies(Maze maze, Tile tile) {
		for (int i = tile.x - deltaEnemys; i < tile.y + deltaEnemys; i++) {
			for (int j = tile.y - deltaEnemys; j < tile.y + deltaEnemys; j++) {
				if (i >= 0 && i < maze.width && j >= 0 && j < maze.height) {
					if (maze.tiles [i, j].objectName != "") { //trocar por .objectName != ""
						return false;
					}
				}
			}
		}
		return true;
	}

	public static void CreateEnemiesHall(Maze maze) {
		//mimics
		//armadura enfeite
		//armadura fantasma
		//espelho por onde sai inimigos

		foreach (Tile t in maze.tiles) {
			if (EmptyRadiusToEnemies (maze, t)) {
				if (GetAllWallNeighbours (maze, t).Count == 2 && t.isWalkable) { //mimics
					if (Random.Range (0, 100) < 50) { //fator random
						t.objectName = "Mimic"; //trocar pelo nome do prefab
					}
				} else if (GetAllWallNeighbours (maze, t).Count == 3 && t.isWalkable) { //armadura
					if (Random.Range (0, 100) < 30) { //fator random
						t.objectName = "KnightArmor"; 
					}
				} else if (GetAllWallNeighbours (maze, t).Count == 3 && t.isWall) { //espelho
					if (Random.Range (0, 100) < 60) {
						t.objectName = "";
					}
				}
			} else {
				if (Random.Range (0, 100) < 10 && t.isWalkable && NotObstaclesNear(maze, t)) {
					t.obstacle = 0;
				}
			}
		}
				
	}

	static bool NotObstaclesNear(Maze maze, Tile tile) {
		if (tile.x - 1 >= 0 && maze.tiles[tile.x - 1, tile.y].obstacle >= 0) {
			return false;
		}
		if (tile.x - 1 >= 0 && tile.y - 1 >= 0 && maze.tiles[tile.x - 1, tile.y - 1].obstacle >= 0) {
			return false;
		}
		if (tile.x + 1 < maze.width && maze.tiles[tile.x + 1, tile.y].obstacle >= 0) {
			return false;
		} 
		if (tile.x + 1 < maze.width && tile.y + 1 < maze.height && maze.tiles[tile.x + 1, tile.y + 1].obstacle >= 0) {
			return false;
		} 
		if (tile.y - 1 >= 0 && maze.tiles[tile.x, tile.y - 1].obstacle >= 0) {
			return false;
		} 
		if (tile.x + 1 < maze.width && tile.y - 1 >= 0 && maze.tiles[tile.x + 1, tile.y - 1].obstacle >= 0) {
			return false;
		} 
		if (tile.x - 1 >= 0 && tile.y + 1 < maze.height && maze.tiles[tile.x - 1, tile.y + 1].obstacle >= 0) {
			return false;
		}
		return true;
	}

	public static void CreateObstaclesForest(Maze maze) {
		foreach (Tile t in maze.tiles) {
			if (t.isWall) {	
				if (Random.Range (1, 100) < 30) { //fator random
					t.obstacle = 0; //trocar pelo nome do prefab
				} else if (Random.Range (1, 100) < 20) {
					t.obstacle = 2;
					t.isWall = false;
				} else if (Random.Range (1, 100) < 20) {
					t.obstacle = 4;
					t.isWall = false;
				}
			}
			if (t.isWalkable && NotObstaclesNear(maze, t) && t.transition == null) { 
				if (Random.Range (1, 100) < 30) {
					t.obstacle = 1;
				} else if (Random.Range (1, 100) < 20) {
					t.obstacle = 2;
				} else if (Random.Range (1, 100) < 30) {
					t.obstacle = 3;
				}
			}
		}
	}
		
	public static void CreateEnemies(Maze maze, string theme) {
		if (theme == "Hall")
			CreateEnemiesHall (maze);
		else if (theme == "Cave")
			CreateEnemiesHall (maze);
		else if (theme == "Forest")
			CreateObstaclesForest (maze);
	}

	public static void ExpandMaze (Maze maze, int factorX, int factorY){
		Tile[,] expandedTiles = new Tile[maze.width * factorX, maze.height * factorY];
		for (int i = 0; i < maze.width; i++) {
			for (int j = 0; j < maze.height; j++) {
				
				for (int ki = 0; ki < factorX; ki++) {
					for (int kj = 0; kj < factorY; kj++) {
						int x = i * factorX + ki;
						int y = j * factorY + kj;

						// Criar tiles com as novas coordenadas
						expandedTiles [x, y] = new Tile (maze.tiles [i, j]);
						expandedTiles [x, y] .x = x;
						expandedTiles [x, y] .y = y;

						// Ajustar a transição que tiver no tile
						if (maze.tiles [i, j].transition != null) {
							
							int id = maze.tiles [i, j].transition.mazeID;
							int dir = maze.tiles [i, j].transition.direction;
							int dx = maze.tiles [i, j].transition.tileX * factorX + ki;
							int dy = maze.tiles [i, j].transition.tileY * factorY + kj;
							expandedTiles [x, y].transition = new Transition (id, dx, dy, dir);
						}

					}
				}
			}
		}
		maze.beginMaze = expandedTiles[maze.beginMaze.x * factorX, maze.beginMaze.y * factorY];
		maze.tiles = expandedTiles;
	}

	public static void SetTransition (Tile origTile, Tile destTile, Maze origMaze, Maze destMaze) {
		float angle = GameManager.VectorToAngle (origTile.coordinates - origMaze.center);
		int direction = Character.AngleToDirection (Mathf.RoundToInt (angle / 90) * 90);
		origTile.transition = new Transition (destMaze.id, destTile.x, destTile.y, direction);
	}

	private static Tile GetNeighbourHall(List<Tile> n){
		if (Random.Range (1, 100) < 80 || n.Count == 1) {
			return n[0];
		} else {
			return n [Random.Range (1, n.Count - 1)];
		}
	}
		
	private static Maze CreateCave (Maze maze) {
		return CreateForest(maze);
	}
		
	private static Tile GetNeighbourForest(List<Tile> n){
		int i = Random.Range (0, n.Count - 1);
	//		Debug.Log (i + " " + n.Count);
		return n [i];
	}

	private static Maze CreateHall (Maze maze) {
		InicializeNullMaze (maze);

		Tile currentTile = BeginMazeGenerator(maze);
		maze.beginMaze = currentTile;
		Tile temp;
		Stack<Tile> stack = new Stack<Tile> ();
		List<Tile> neighbours;

		stack.Push (currentTile);
		while (stack.Count > 0) {
			currentTile = stack.Pop ();
			visited [currentTile.x, currentTile.y] = true;
			if (NotVisitedNeighbours (maze, currentTile, 2)) {
				neighbours = GetNeighbours (maze, currentTile, 2);
				temp = GetNeighbourHall (neighbours);
				stack.Push (currentTile);
				stack.Push (temp);
				RemoveWall (maze, currentTile, temp);
			}
		}
		return maze;		
	}

	private static Maze CreateForest (Maze maze) {
		InicializeNullMaze (maze);
		Tile currentTile = BeginMazeGenerator(maze);
		maze.beginMaze = currentTile;
		Tile temp;
		Stack<Tile> stack = new Stack<Tile> ();
		List<Tile> neighbours;

		stack.Push (currentTile);
		while (stack.Count > 0) {
			currentTile = stack.Pop ();
			visited [currentTile.x, currentTile.y] = true;
			if (NotVisitedNeighbours (maze, currentTile, 2)) {
				neighbours = GetNeighbours (maze, currentTile, 2);
				temp = GetNeighbourForest (neighbours);
				stack.Push (currentTile);
				stack.Push (temp);
				RemoveWall (maze, currentTile, temp);
			}
		}
		return maze;
	}
		
	public static Maze CreateMaze (int id, string theme, int w, int h) {
		Maze maze = new Maze (id, theme, w, h);
		visited = new bool[w, h];
		switch (theme) {
		case "Hall":
			return CreateHall (maze);
		case "Cave":
			return CreateCave (maze);
		case "Forest":
			return CreateForest (maze);
		}
		return null;
	}

}
