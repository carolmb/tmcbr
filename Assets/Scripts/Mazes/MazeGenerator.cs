﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class MazeGenerator {

	protected Maze maze;
	protected bool[,] visited;

	protected int width, height;
	protected string theme;

	protected const int deltaEnemys = 4;

	public MazeGenerator(string theme, int w, int h) {
		this.theme = theme;
		this.width = w;
		this.height = h;
		visited = new bool[w, h];
	}
		
	public abstract void CreateEnemies (Maze maze);
	protected abstract Tile GetNeighbour(List<Tile> n);

	protected bool HasTransitionNear(Tile t) {
		if (t.transition != null)
			return true;
		foreach (Tile n in GetNeighbours(t, 1)) {
			if (n.transition != null)
				return true;
		}
		return false;
	}

	protected List<Tile> GetAllWallNeighbours (Tile tile) {
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

	protected List<Tile> GetNeighbours (Tile tile, int delta) {
		List<Tile> neighbours = new List<Tile> ();
		if (tile.x - delta > 0) {
			neighbours.Add (maze.tiles [tile.x - delta, tile.y]);
		}
		if (tile.x + delta < maze.width - 1) {
			neighbours.Add (maze.tiles [tile.x + delta, tile.y]);
		} 
		if (tile.y - delta > 0) {
			neighbours.Add (maze.tiles [tile.x, tile.y - delta]);
		} 
		if (tile.y + delta < maze.height - 1) {
			neighbours.Add (maze.tiles [tile.x, tile.y + delta]);
		}
		return neighbours;
	}

	protected List<Tile> GetVisitedNeighbours (Tile tile, int delta) {
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

	protected bool NotVisitedNeighbours (Tile tile, int delta) {
		if (GetVisitedNeighbours (tile, delta).Count > 0) {
			return true;
		}
		return false;
	}
		
	protected void InicializeNullMaze () {
		for (int i = 0; i < maze.width; i++) {
			for (int j = 0; j < maze.height; j++) {
				maze.tiles [i, j] = new Tile (i, j);
				if (i % 2 == 1 && j % 2 == 1) {
					maze.tiles [i, j].wallID = 0;
					visited [i, j] = false;
				} else {
					maze.tiles [i, j].wallID = 1;
					visited [i, j] = true;
				}
			}
		}
	}

	protected void RemoveWall(Tile current, Tile neighbor) {
		int deltaX = neighbor.x - current.x ;
		int deltaY = neighbor.y - current.y;

		if (current.x + deltaX / 2 > 0 && current.y + deltaY / 2 > 0 && 
			current.x + deltaX / 2 < maze.width && current.y + deltaY / 2 < maze.height) {
			maze.tiles [current.x + deltaX / 2, current.y + deltaY / 2].wallID = 0;
			visited [current.x + deltaX / 2, current.y + deltaY / 2] = true;
		} 
	}

	protected Tile BeginMazeGenerator () {
		int h = maze.height;
		int y = Random.Range (1, h-2);
		if (y % 2 == 0) {
			y++;
		}
		maze.tiles [1, y].wallID = 0;
		return maze.tiles [1, y];
	}

	protected bool EmptyRadiusToEnemies (Tile tile) {
		for (int i = tile.x - deltaEnemys; i < tile.y + deltaEnemys; i++) {
			for (int j = tile.y - deltaEnemys; j < tile.y + deltaEnemys; j++) {
				if (i >= 0 && i < maze.width && j >= 0 && j < maze.height) {
					if (maze.tiles [i, j].objectName != "") {
						return false;
					}
				}
			}
		}
		return true;
	}

	protected bool NoObstaclesNear (Tile tile) {
		for (int i = tile.x - 1; i <= tile.x + 1; i++) {
			for (int j = tile.y - 1; j <= tile.y + 1; j++) {
				if (i >= 0 && i < maze.width && j >= 0 && j < maze.height) {
					if (maze.tiles [i, j].obstacleID > 0) {
						return false;
					}
				}
			}
		}
		return true;
	}

	protected bool NearBegin (Tile tile) {
		for (int i = maze.beginMaze.x - 1; i <= maze.beginMaze.x + 1; i++) {
			for (int j = maze.beginMaze.y - 1; j <= maze.beginMaze.y + 1; j++) {
				if (i >= 0 && i < maze.width && j >= 0 && j < maze.height) {
					if (maze.tiles[i, j] == tile) {
						return true;
					}
				}
			}
		}
		return false;
	}

	// Cria um novo labirinto com o dado id
	public Maze Create(int id) {
		maze = new Maze (id, theme, width, height);

		InicializeNullMaze ();
		Tile currentTile = BeginMazeGenerator();
		maze.beginMaze = currentTile;
		Tile temp;
		Stack<Tile> stack = new Stack<Tile> ();
		List<Tile> neighbours;

		stack.Push (currentTile);
		while (stack.Count > 0) {
			currentTile = stack.Pop ();
			visited [currentTile.x, currentTile.y] = true;
			if (NotVisitedNeighbours (currentTile, 2)) {
				neighbours = GetVisitedNeighbours (currentTile, 2);
				temp = GetNeighbour (neighbours);
				stack.Push (currentTile);
				stack.Push (temp);
				RemoveWall (currentTile, temp);
			}
		}
		return maze;
	}

	public Maze Create(int id, Vector2 begin) {
		maze = new Maze (id, theme, width, height);

		InicializeNullMaze ();
		Tile currentTile = maze.tiles[(int)begin.x, (int)begin.y];
		maze.beginMaze = currentTile;
		Tile temp;
		Stack<Tile> stack = new Stack<Tile> ();
		List<Tile> neighbours;

		stack.Push (currentTile);
		while (stack.Count > 0) {
			currentTile = stack.Pop ();
			visited [currentTile.x, currentTile.y] = true;
			if (NotVisitedNeighbours (currentTile, 2)) {
				neighbours = GetVisitedNeighbours (currentTile, 2);
				temp = GetNeighbour (neighbours);
				stack.Push (currentTile);
				stack.Push (temp);
				RemoveWall (currentTile, temp);
			}
		}
		return maze;
	}


	// Multiplica os tiles de um labirinto
	// Apenas obstáculos, chão, parede e transição são replicados
	public void ExpandMaze (Maze maze, int factorX, int factorY){
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
		this.maze = maze;
		maze.beginMaze = expandedTiles[maze.beginMaze.x * factorX, maze.beginMaze.y * factorY];
		if(maze.endMaze != null)
			maze.endMaze = expandedTiles [maze.endMaze.x * factorX, maze.endMaze.y * factorY];
		maze.tiles = expandedTiles;
	}

	// Cria uma transição de um tile de um labirinto para outro
	public void SetTransition (Tile origTile, Tile destTile, Maze origMaze, Maze destMaze) {
		float angle = GameManager.VectorToAngle (origTile.coordinates - origMaze.center);
		int direction = Character.AngleToDirection (Mathf.RoundToInt (angle / 90) * 90);
		origTile.transition = new Transition (destMaze.id, destTile.x, destTile.y, direction);
	}

	// Cria um generator de acordo com o tema da fase
	public static MazeGenerator GetGenerator (string theme, int w, int h) {
		switch (theme) {
		case "Hall":
			return new HallGenerator (theme, w, h);
		case "Cave":
			return new CaveGenerator (theme, w, h);
		case "Forest":
			return new ForestGenerator (theme, w, h);
		}
		return null;
	}

}
