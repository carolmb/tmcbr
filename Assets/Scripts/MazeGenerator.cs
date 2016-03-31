using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeGenerator {

	static Tile[,] temporaryMaze;
	static Tile begin;
	static int width;
	static int height;

	static List<Tile> GetNeighbours(Tile tile) {
		List<Tile> neighbours = new List<Tile> ();
		/*foreach (Tile n in tile.GetNeighbours4()) {
			if (!n.visited)
				neighbours.Add (n);
		}*/

		if (tile.x - 2 > 0 && !temporaryMaze [tile.x - 2, tile.y].visited) {
			neighbours.Add (temporaryMaze [tile.x - 2, tile.y]);
		} else if (tile.x + 2 < width && !temporaryMaze [tile.x + 2, tile.y].visited) {
			neighbours.Add (temporaryMaze [tile.x + 2, tile.y]);
		} else if (tile.y - 2 > 0 && !temporaryMaze [tile.x, tile.y - 2].visited) {
			neighbours.Add (temporaryMaze [tile.x, tile.y - 2]);
		} else if (tile.y + 2 < height && !temporaryMaze [tile.x, tile.y + 2].visited) {
			neighbours.Add (temporaryMaze [tile.x, tile.y + 2]);
		}

		return neighbours;
	}

	static bool NotVisitedNeighbours(Tile tile) {
		//return GetNeighbours(tile).Count > 0;
		if (tile.x - 2 >= 0)
			if (!temporaryMaze [tile.x - 2, tile.y].visited)
				return true; 

		if (tile.x + 2 < width)
			if (!temporaryMaze[tile.x + 2, tile.y].visited)
				return true;

		if (tile.y - 2 > 0)
			if (!temporaryMaze[tile.x, tile.y - 2].visited) 
				return true;
			
		if (tile.y + 2 < height) 
			if (!temporaryMaze[tile.x, tile.y + 2].visited)
				return true;

		return false;
	}

	/*static Tile GetRandomNeighbor(List<Tile> neighbors) {
		int r = Random.Range(0, neighbors.Count);
		return neighbors [r];
	}*/

	static void InicializeNullMaze() {
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				temporaryMaze [i, j] = new Tile ();
				temporaryMaze [i, j].x = i;
				temporaryMaze [i, j].y = j;
				if (i % 2 == 1 && j % 2 == 1) {
					temporaryMaze [i, j].isWall = false;
				} else {
					temporaryMaze [i, j].isWall = true;
					temporaryMaze [i, j].visited = true;
				}
			}
		}
	}

	static void RemoveWall(Tile current, Tile neighbor) {
		int deltaX = current.x - neighbor.x;
		int deltaY = current.y - neighbor.y;

		Debug.Log ("deltaX: " + deltaX);
		Debug.Log ("deltaY: " + deltaY);

		Debug.Log ("remove wall x: " + current.x );
		Debug.Log ("remove wall y: " + current.y );

		if (current.x + deltaX / 2 > 0 && current.y + deltaY / 2 > 0 && current.x + deltaX / 2 < width && current.y + deltaY / 2 < height) {
			temporaryMaze [current.x + deltaX / 2, current.y + deltaY / 2].isWall = false;
			temporaryMaze [current.x + deltaX / 2, current.y + deltaY / 2].visited = true;
		}
	}

	static Tile BeginMazeGenerator(int h) {
		Tile begin = new Tile();
		int x = Random.Range (1, h-2);
		if (x % 2 == 0) {
			x++;
		}

		begin.isWall = false;
		begin.x = x;
		begin.y = 1;
		temporaryMaze [x, 1] = begin;
	
		return begin;	
	}

	public static Tile[,] CreateMaze(int w, int h) { // vai ser esse
		return TempMaze();
		/*width = w;
		height = h;

		temporaryMaze = new Tile[w, h];
		begin = BeginMazeGenerator(h);

		Tile currentTile = begin;
		Tile temp;
		Stack<Tile> stack = new Stack<Tile> ();
		List<Tile> neighbours;

		InicializeNullMaze ();

		stack.Push (currentTile);
		while (stack.Count > 0) {
			temp = stack.Pop ();
			currentTile.visited = true;
			Debug.Log ("currentTile x: " + currentTile.x + " currentTile.y: " + currentTile.y);

			if (NotVisitedNeighbours (currentTile)) {
				neighbours = GetNeighbours (currentTile);
				temp = neighbours [Random.Range (0, neighbours.Count)];
				stack.Push (currentTile);
				RemoveWall (currentTile, temp);
				currentTile = temp;
				currentTile.visited = true;
			} 
		}
		temporaryMaze [begin.x, begin.y-1].isWall = false;
		return temporaryMaze;*/
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
