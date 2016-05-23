using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class ProceduralMaze : Maze {

	public ProceduralMaze(int i, int w, int h) : base(i, w, h) {}

	public abstract void CreateObstacles ();
	protected abstract Tile GetNeighbour(Tile t, bool[,] visited);

	public virtual void GenerateTiles () {
		bool[,] visited = new bool[width, height];
		InicializeNullMaze (visited);
		Tile beginTile = FirstTile ();
		beginTile.wallID = 0;

		Tile currentTile = beginTile;
		Stack<Tile> stack = new Stack<Tile> ();

		stack.Push (currentTile);
		while (stack.Count > 0) {
			currentTile = stack.Pop ();
			visited [currentTile.x, currentTile.y] = true;
			if (!HasVisitedNeighbours (currentTile, 2, visited)) {
				Tile temp = GetNeighbour (currentTile, visited);
				stack.Push (currentTile);
				stack.Push (temp);
				Tile wall = RemoveWall (currentTile, temp);
				if (wall != null)
					visited [wall.x, wall.y] = true;
			}
		}
	}

	// Multiplica os tiles de um labirinto
	// Apenas obstáculos, chão, parede e transição são replicados
	public void Expand (int factorX, int factorY){
		Tile[,] expandedTiles = new Tile[width * factorX, height * factorY];
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {

				for (int ki = 0; ki < factorX; ki++) {
					for (int kj = 0; kj < factorY; kj++) {
						int x = i * factorX + ki;
						int y = j * factorY + kj;

						// Criar tiles com as novas coordenadas
						expandedTiles [x, y] = new Tile (tiles [i, j], x, y);
					}
				}
			}
		}

		tiles = expandedTiles;
	}

	protected bool EmptyRadiusToEnemies (Tile tile, int delta) {
		for (int i = tile.x - delta; i <= tile.x + delta; i++) {
			for (int j = tile.y - delta; j <= tile.y + delta; j++) {
				if (i >= 0 && i < width && j >= 0 && j < height) {
					if (tiles [i, j].objectName != "" || tiles [i, j].obstacle != "") {
						return false;
					}
				}
			}
		}
		return true;
	}

	protected List<Tile> GetAllWallNeighbours (Tile tile) {
		List<Tile> neighbours = new List<Tile> ();
		if (tile.x - 1 >= 0 && tiles[tile.x - 1, tile.y].isWall) {
			neighbours.Add (tiles [tile.x - 1, tile.y]);
		}
		if (tile.x + 1 < width && tiles[tile.x + 1, tile.y].isWall) {
			neighbours.Add (tiles [tile.x + 1, tile.y]);
		} 
		if (tile.y - 1 >= 0 && tiles[tile.x, tile.y - 1].isWall) {
			neighbours.Add (tiles [tile.x, tile.y - 1]);
		} 
		if (tile.y + 1 < height && tiles[tile.x, tile.y + 1].isWall) {
			neighbours.Add (tiles [tile.x, tile.y + 1]);
		}

		return neighbours;
	}

	protected List<Tile> GetVisitedNeighbours (Tile tile, int delta, bool[,] visited) {
		List<Tile> neighbours = new List<Tile> ();
		if (tile.x - delta > 0 && !visited[tile.x - delta, tile.y]) {
			neighbours.Add (tiles [tile.x - delta, tile.y]);
		}
		if (tile.x + delta < width - 1 && !visited [tile.x + delta, tile.y]) {
			neighbours.Add (tiles [tile.x + delta, tile.y]);
		} 
		if (tile.y - delta > 0 && !visited[tile.x, tile.y - delta]) {
			neighbours.Add (tiles [tile.x, tile.y - delta]);
		} 
		if (tile.y + delta < height - 1 && !visited[tile.x, tile.y + delta]) {
			neighbours.Add (tiles [tile.x, tile.y + delta]);
		}
		return neighbours;
	}

	protected bool HasTransitionNear(Tile t) {
		if (t.transition != null)
			return true;
		foreach (Tile n in GetNeighbours(t, 2)) {
			if (n.transition != null)
				return true;
		}
		return false;
	}

	protected bool HasObstaclesNear (Tile t, int vision = 6) {
		if (t.obstacle != "")
			return true;
		foreach (Tile n in GetNeighbours(t, vision)) {
			if (n.obstacle != "")
				return true;
		}
		return false;
	}

	protected bool HasVisitedNeighbours (Tile tile, int delta, bool[,] visited) {
		if (GetVisitedNeighbours (tile, delta, visited).Count > 0) {
			return false;
		}
		return true;
	}

	protected void InicializeNullMaze (bool[,] visited) {
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				tiles [i, j] = new Tile (i, j);
				if (i % 2 == 1 && j % 2 == 1) {
					tiles [i, j].wallID = 0;
					visited [i, j] = false;
				} else {
					tiles [i, j].wallID = 1;
					visited [i, j] = true;
				}
			}
		}
	}

	protected Tile RemoveWall(Tile current, Tile neighbor) {
		int deltaX = neighbor.x - current.x ;
		int deltaY = neighbor.y - current.y;

		if (current.x + deltaX / 2 > 0 && current.y + deltaY / 2 > 0 && 
			current.x + deltaX / 2 < width && current.y + deltaY / 2 < height) {
			Tile tile = tiles [current.x + deltaX / 2, current.y + deltaY / 2];
			tile.wallID = 0;
			return tile;
		} 
		return null;
	}

	protected Tile FirstTile () {
		int y = Random.Range (1, height - 2);
		if (y % 2 == 0) {
			y++;
		}
		return tiles [1, y];
	}
		
}
