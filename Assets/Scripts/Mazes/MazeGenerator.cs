using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class MazeGenerator {

	protected Maze maze;
	protected bool[,] visited;

	protected const int deltaEnemys = 4;

	protected abstract string Theme ();
	public abstract void CreateEnemies (Maze maze);
	protected abstract Tile GetNeighbour(Tile t);

	protected bool HasTransitionNear(Tile t) {
		if (t.transition != null)
			return true;
		foreach (Tile n in GetNeighbours(t, 1)) {
			if (n.transition != null)
				return true;
		}
		return false;
	}

	protected bool HasObstaclesNear (Tile t) {
		if (t.obstacleID > 0)
			return true;
		foreach (Tile n in GetNeighbours(t, 1)) {
			if (n.obstacleID > 0)
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

	protected Tile FirstTile () {
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
		

	public Maze Create(int id, int width, int height) {
		maze = new Maze (id, Theme(), width, height);
		visited = new bool[width, height];
		InicializeNullMaze ();
		Vector2 begin = FirstTile ().coordinates;

		Tile currentTile = maze.tiles[(int)begin.x, (int)begin.y];
		//maze.beginTile = currentTile;
		Stack<Tile> stack = new Stack<Tile> ();

		stack.Push (currentTile);
		while (stack.Count > 0) {
			currentTile = stack.Pop ();
			visited [currentTile.x, currentTile.y] = true;
			if (NotVisitedNeighbours (currentTile, 2)) {
				Tile temp = GetNeighbour (currentTile);
				stack.Push (currentTile);
				stack.Push (temp);
				RemoveWall (currentTile, temp);
			}
		}
		//maze.beginTile = null;
		//maze.endTile = null;
		return maze;
	}

	// Cria um generator de acordo com o tema da fase
	public static MazeGenerator GetGenerator (string theme) {
		switch (theme) {
		case "Hall":
			return new HallGenerator ();
		case "Cave":
			return new CaveGenerator ();
		case "Forest":
			return new ForestGenerator ();
		}
		return null;
	}

}
