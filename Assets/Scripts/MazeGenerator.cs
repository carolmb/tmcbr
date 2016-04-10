using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeGenerator {

	static List<Tile> GetAllNeighbours(Maze maze, Tile tile) {
		List<Tile> neighbours = new List<Tile> ();
		if (tile.x - 2 > 0 && maze.tiles[tile.x - 2, tile.y].isWalkable) {
			neighbours.Add (maze.tiles [tile.x - 2, tile.y]);
		}
		if (tile.x + 2 < maze.width - 1 && maze.tiles[tile.x + 2, tile.y].isWalkable) {
			neighbours.Add (maze.tiles [tile.x + 2, tile.y]);
		} 
		if (tile.y - 2 > 0 && maze.tiles[tile.x, tile.y - 2].isWalkable) {
			neighbours.Add (maze.tiles [tile.x, tile.y - 2]);
		} 
		if (tile.y + 2 < maze.height - 1 && maze.tiles[tile.x, tile.y + 2].isWalkable) {
			neighbours.Add (maze.tiles [tile.x, tile.y + 2]);
		}

		return neighbours;
	}

	static List<Tile> GetNeighbours(Maze maze, Tile tile) {
		List<Tile> neighbours = new List<Tile> ();
		if (tile.x - 2 > 0 && !maze.tiles [tile.x - 2, tile.y].visited) {
			neighbours.Add (maze.tiles [tile.x - 2, tile.y]);
		}
		if (tile.x + 2 < maze.width - 1 && !maze.tiles [tile.x + 2, tile.y].visited) {
			neighbours.Add (maze.tiles [tile.x + 2, tile.y]);
		} 
		if (tile.y - 2 > 0 && !maze.tiles [tile.x, tile.y - 2].visited) {
			neighbours.Add (maze.tiles [tile.x, tile.y - 2]);
		} 
		if (tile.y + 2 < maze.height - 1 && !maze.tiles [tile.x, tile.y + 2].visited) {
			neighbours.Add (maze.tiles [tile.x, tile.y + 2]);
		}

		return neighbours;
	}

	static bool NotVisitedNeighbours(Maze maze, Tile tile) {
		if (GetNeighbours (maze, tile).Count > 0) {
			return true;
		}
		return false;
	}
		
	static void InicializeNullMaze(Maze maze) {
		for (int i = 0; i < maze.width; i++) {
			for (int j = 0; j < maze.height; j++) {
				maze.tiles [i, j] = new Tile (i, j);
				if (i % 2 == 1 && j % 2 == 1) {
					maze.tiles [i, j].isWall = false;
					maze.tiles [i, j].visited = false;
				} else {
					maze.tiles [i, j].isWall = true;
					maze.tiles [i, j].visited = true;
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
			maze.tiles [current.x + deltaX / 2, current.y + deltaY / 2].visited = true;
		} 
	}

	static Tile BeginMazeGenerator(Maze maze) {
		int h = maze.height;
		int y = Random.Range (1, h-2);
		if (y % 2 == 0) {
			y++;
		}
		maze.tiles [1, y].isWall = false;
		return maze.tiles [1, y];
	}

	public static void ClearMazeVisits(Maze maze) {
		foreach(Tile t in maze.tiles) {
			if(t.isWalkable == true) {
				t.visited = false;
			}
		}
	}

	public static void CreateEnemysHall(Maze maze) {
		//0 mimics
		//1 armadura enfeite
		//2 armadura fantasma
		//3 espelho por onde sai inimigos

		Tile currentTile = BeginMazeGenerator(maze); //trocar pelo ponto inicial do labirinto
		Tile temp;
		Stack<Tile> stack = new Stack<Tile> ();
		List<Tile> neighbours, allNeighbours;

		ClearMazeVisits (maze);

		stack.Push (currentTile);
		while (stack.Count > 0) {
			currentTile = stack.Pop ();
			currentTile.visited = true;
			if (NotVisitedNeighbours (maze, currentTile)) {
				neighbours = GetNeighbours (maze, currentTile);
				allNeighbours = GetAllNeighbours (maze, currentTile);
				temp = neighbours [Random.Range (0, neighbours.Count)];
				if (allNeighbours.Count == 1) { //verificar se é diferente do final
					currentTile.obstacle = 0;
				}
				stack.Push (currentTile);
				stack.Push (temp);
			}
		}

	}

	public static void ExpandMaze(Maze maze, int factorX, int factorY){
		Tile[,] expandedTiles = new Tile[maze.width * factorX, maze.height * factorY];
		for(int i = 0; i < maze.width; i++) {
			for(int j = 0; j < maze.height; j++) {
				
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
		maze.tiles = expandedTiles;
	}

	private static void SetTransition(Tile origTile, Tile destTile, Maze origMaze, Maze destMaze) {
		float angle = GameManager.VectorToAngle (origTile.coordinates - origMaze.center);
		int direction = Character.AngleToDirection (Mathf.RoundToInt (angle / 90) * 90);
		origTile.transition = new Transition (destMaze.id, destTile.x, destTile.y, direction);
	}

	private static Maze CreateHall(Maze maze) {
		// TODO: uma forma diferente de criar o labirinto
		// Por ex: esse tem mais corredores "retos", verticais ou horizontais
		return CreateForest(maze);
	}

	private static Maze CreateCave(Maze maze) {
		return CreateForest(maze);
	}

	private static Maze CreateForest(Maze maze) {
		InicializeNullMaze (maze);
		Tile currentTile = BeginMazeGenerator(maze);
		Tile temp;
		Stack<Tile> stack = new Stack<Tile> ();
		List<Tile> neighbours;

		stack.Push (currentTile);
		while (stack.Count > 0) {
			currentTile = stack.Pop ();
			currentTile.visited = true;
			if (NotVisitedNeighbours (maze, currentTile)) {
				neighbours = GetNeighbours (maze, currentTile);
				temp = neighbours [Random.Range (0, neighbours.Count)];
				stack.Push (currentTile);
				stack.Push (temp);
				RemoveWall (maze, currentTile, temp);
			}
		}

		// Gerar tiles inicial e final
		int x, y;

		// Gerar um tile na borda esquerda
		x = 1;
		do {
			y = Random.Range (1, maze.height - 1);
		} while (maze.tiles [x, y].isWall);
		Tile initialTile = maze.tiles [x - 1, y];
		initialTile.isWall = false;

		// Gerar um tile na borda direita
		x = maze.width - 2;
		do {
			y = Random.Range (1, maze.height - 1);
		} while (maze.tiles [x, y].isWall);
		Tile finalTile = maze.tiles [x + 1, y];
		finalTile.isWall = false;

		// Destino da transição no tile inicial (que é um tile à frente do tile final)
		Tile initialTile_dest = maze.tiles [finalTile.x - 1, finalTile.y]; 
		// Destino da transição no tile final (que é um tile à frente do tile inicial)
		Tile finalTile_dest = maze.tiles [initialTile.x + 1, initialTile.y];

		// Colocar as transições entre o primeiro e o último tile (TEMPORÁRIO)
		// TODO: Colocar depois as transições entre diferentes labirintos
		SetTransition (initialTile, initialTile_dest, maze, maze);
		SetTransition (finalTile, finalTile_dest, maze, maze);

		// Multiplicar o maze
		int f = 2; // Scale factor

		ExpandMaze (maze, f, f);
		CreateEnemysHall (maze);
		// Deixar como se tivesse acabado de terminar o labirinto (ou seja, volta para o início)
		MazeManager.currentTransition = maze.tiles[finalTile.x * f, finalTile.y * f].transition;

		Debug.Log ("Inicial: " + initialTile.coordinates);
		Debug.Log ("Final: " + finalTile.coordinates);

		return maze;
	}

	public static Maze CreateMaze(int id, string theme, int w, int h) {
		Maze maze = new Maze (id, theme, w, h);
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
