using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct Cell {
	public int x;
	public int y;
	public bool visited;
	public bool isWalkeable;
}

public class Map {
	public Cell[,] map;

	public int width, height;

	// Use this for initialization

	public Map(int x, int y) {
		createMap (40, 20);
	}

	bool notVisitedCells(){
		return true;	
	
	}

	List<Cell> GetNeighbors(Cell tile){
		List<Cell> neighbors = new List<Cell> ();
		if (tile.x > 0 && !map[tile.x - 1, tile.y].visited) {
			neighbors.Add (map [tile.x - 1, tile.y]);
		} else if (tile.x < 40 && !map[tile.x + 1, tile.y].visited) {
			neighbors.Add (map [tile.x + 1, tile.y]);
		} else if (tile.y > 0 && !map[tile.x, tile.y - 1].visited) {
			neighbors.Add (map [tile.x, tile.y - 1]);
		} else if (tile.y < 40 && !map[tile.x, tile.y + 1].visited) {
			neighbors.Add (map [tile.x, tile.y + 1]);
		}

		return neighbors;
	}

	bool NotVisitedNeighbors(Cell cell) {
		return GetNeighbors (cell).Count > 0;
	}

	Cell GetRandomNeighbor(Cell cell) {
		List<Cell> neighbors = GetNeighbors (cell);
		int r = Random.Range(0, neighbors.Count);
		return neighbors [r];
	}

	void InicializeNullMaze() {
		
	}

	Cell BeginMazeGenerator() {

		Cell c = new Cell();
		return c;	
	}

	public void CreateMap(int w, int h) {
		width = w;
		height = h;

		Cell currentCell = BeginMazeGenerator();
		List<Cell> n;

		InicializeNullMaze ();

		while (NotVisitedNeighbors(currentCell)) {
			if (NotVisitedNeighbors(currentCell)) {
				GetRandomNeighbor (currentCell);
			}	
		
		}
	}


	public void createMap(int w, int h) {
		width = 40; 
		height = 20;
	
		int[,] tempMap = new int[,] {
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0},
			{0,1,0,1,1,1,1,1,1,1,0,1,1,1,0,0,1,0,0,0,0,1,1,1,0,0,1,0,0,0,0,1,0,0,0,1,1,1,1,1,0,0,0,1,1,0,1,0,0,0},
			{0,1,0,0,1,0,0,0,0,0,0,1,0,1,1,1,1,1,1,1,0,0,0,1,0,0,1,0,0,0,0,1,0,0,0,1,0,0,0,1,1,1,1,1,0,0,1,0,0,0},
			{0,1,0,0,1,0,1,1,1,1,1,1,0,1,0,0,0,0,0,1,1,1,1,1,1,1,1,0,0,1,1,1,1,1,0,1,0,0,0,1,0,0,0,0,0,0,1,0,0,0},
			{0,0,0,0,1,0,1,0,0,0,0,0,0,1,0,0,1,1,1,1,0,0,1,0,0,0,0,0,0,1,0,0,0,1,0,1,1,1,0,1,0,1,1,1,1,1,1,0,0,0},
			{0,1,1,1,1,0,1,0,0,1,1,1,1,1,0,0,1,0,0,1,0,0,1,0,1,1,1,1,0,1,0,0,0,1,0,1,0,1,0,0,0,1,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,1,0,0,1,0,1,0,0,0,0,1,1,0,1,0,0,1,0,0,0,0,1,0,1,1,1,0,1,0,1,0,1,0,0,0,1,0,0,1,1,1,1,1,0},
			{0,1,0,0,0,0,1,1,0,1,0,1,1,0,0,0,0,1,1,1,0,0,1,1,0,0,1,1,0,0,0,1,0,1,0,0,0,1,0,0,0,1,0,0,1,0,1,0,1,0},
			{0,1,0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,0,0,1,0,0,0,1,0,0,1,0,0,0,0,1,0,1,0,1,1,1,1,0,0,1,0,0,1,0,0,0,1,0},
			{0,1,0,0,0,0,0,1,0,0,0,0,1,0,1,1,1,1,0,1,1,0,0,1,1,1,1,1,0,0,0,1,0,1,1,1,0,0,1,0,0,1,1,1,1,1,0,0,1,0},
			{0,1,0,0,0,0,0,1,0,0,0,0,1,0,1,0,0,1,0,0,1,0,0,1,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,1,0},
			{0,1,1,1,1,1,1,1,0,0,0,0,1,0,1,0,0,1,0,0,1,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,1,0,0,1,0},
			{0,1,0,0,0,0,0,0,0,1,1,1,1,0,1,1,0,1,1,1,1,0,0,1,1,1,1,1,0,1,0,0,0,0,0,0,1,0,0,0,1,0,0,0,0,1,1,1,1,0},
			{0,1,0,1,1,1,1,1,1,1,0,0,0,0,0,1,0,1,0,0,0,0,0,1,0,0,0,1,0,1,0,1,1,1,0,0,1,0,0,0,1,0,0,0,0,1,0,0,1,0},
			{0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,1,0,1,1,1,0,1,0,0,0,1,0,1,0,1,0,1,0,0,1,1,1,0,1,0,0,0,0,1,0,0,1,0},
			{0,1,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,1,0,1,0,1,0,1,1,1,0,1,0,1,1,1,0,1,0,0,1,0,1,0,1,0,0,0,0,1,0,0,0,0},
			{0,1,0,0,1,0,1,1,1,1,1,1,1,1,1,1,0,0,0,1,0,1,0,0,0,1,0,1,0,0,0,0,0,1,0,0,1,0,1,0,1,0,0,0,0,1,0,0,0,0},
			{0,1,0,1,1,0,1,0,1,0,0,0,0,0,0,1,1,1,0,1,0,1,1,1,1,1,0,1,0,1,0,1,1,1,0,0,1,0,1,0,1,1,1,1,1,1,0,0,0,0},
			{1,1,1,1,1,1,1,0,1,0,1,1,1,1,1,1,0,1,1,1,0,0,0,0,0,0,0,1,1,1,1,1,0,1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
		};
	
		for (int i = 0; i < 20; i++) {
			for (int j = 0; j < 40; j++) {
				map [i, j].isWalkeable = tempMap [i, j] == 1 ? true : false;
			}
		}

	}

}
