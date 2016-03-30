using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeGenerator {

	bool notVisitedCells(){
		return true;	
	}

	List<Tile> GetNeighbors(Tile tile) {
		List<Tile> neighbours = new List<Tile> ();
		foreach (Tile n in tile.GetNeighbours4()) {
			if (!n.visited)
				neighbours.Add (n);
		}

		/*List<Cell> neighbors = new List<Cell> ();
		if (tile.x > 0 && !cells[tile.x - 1, tile.y].visited) {
			neighbors.Add (cells [tile.x - 1, tile.y]);
		} else if (tile.x < 40 && !cells[tile.x + 1, tile.y].visited) {
			neighbors.Add (cells [tile.x + 1, tile.y]);
		} else if (tile.y > 0 && !cells[tile.x, tile.y - 1].visited) {
			neighbors.Add (cells [tile.x, tile.y - 1]);
		} else if (tile.y < 40 && !cells[tile.x, tile.y + 1].visited) {
			neighbors.Add (cells [tile.x, tile.y + 1]);
		}*/

		return neighbours;
	}

	static bool NotVisitedNeighbors(Tile tile) {
		return tile.GetNeighbours4().Count > 0;
	}

	static Tile GetRandomNeighbor(Tile tile) {
		List<Tile> neighbors = tile.GetNeighbours4();
		int r = Random.Range(0, neighbors.Count);
		return neighbors [r];
	}

	static void InicializeNullMaze() {
		
	}

	static Tile BeginMazeGenerator() {

		Tile c = new Tile();
		return c;	
	}

	public static Tile[,] CreateMaze(int w, int h) { // vai ser esse

		return TempMaze ();

		/*
		Tile currentTile = BeginMazeGenerator();
		List<Tile> n;

		InicializeNullMaze ();

		while (NotVisitedNeighbors(currentTile)) {
			if (NotVisitedNeighbors(currentTile)) {
				GetRandomNeighbor (currentTile);
			}	
		
		}*/

	}


	private static Tile[,] TempMaze() { // provisório
	
		int[,] tempMap = new int[,] {
			{0,0,0,0,0,0,0,0,1,1},
			{0,1,0,1,1,1,1,1,1,1},
			{0,1,0,0,1,0,0,0,0,0},
			{0,1,0,0,1,0,1,1,1,1},
			{0,0,0,0,1,0,1,0,0,0},
			{0,0,0,0,0,0,0,0,0,0},
			{0,1,0,1,1,1,1,1,1,1},
			{0,1,0,0,1,0,0,0,0,0},
			{1,1,0,0,1,0,1,1,1,1},
			{1,1,0,0,1,0,1,0,1,1}
		};

		int w = tempMap.GetLength (0);
		int h = tempMap.GetLength (1);

		Tile[,] tiles = new Tile[w, h];
	
		for (int i = 0; i < w; i++) {
			for (int j = 0; j < h; j++) {
				tiles [i, j] = new Tile ();
				tiles [i, j].isWall = tempMap [i, j] == 1 ? true : false;
				tiles [i, j].x = i;
				tiles [i, j].y = j;
			}
		}

		return tiles;

	}

}
