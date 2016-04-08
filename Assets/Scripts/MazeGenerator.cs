using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeGenerator {

	static List<Tile> GetNeighbours(Maze maze, Tile tile) {
		List<Tile> neighbours = new List<Tile> ();
		if (tile.x - 2 > 0 && !maze.tiles [tile.x - 2, tile.y].visited) {
			neighbours.Add (maze.tiles [tile.x - 2, tile.y]);
		}
		if (tile.x + 2 < maze.width && !maze.tiles [tile.x + 2, tile.y].visited) {
			neighbours.Add (maze.tiles [tile.x + 2, tile.y]);
		} 
		if (tile.y - 2 > 0 && !maze.tiles [tile.x, tile.y - 2].visited) {
			neighbours.Add (maze.tiles [tile.x, tile.y - 2]);
		} 
		if (tile.y + 2 < maze.height && !maze.tiles [tile.x, tile.y + 2].visited) {
			neighbours.Add (maze.tiles [tile.x, tile.y + 2]);
		}

		return neighbours;
	}

	static bool NotVisitedNeighbours(Maze maze, Tile tile) {
		if (GetNeighbours (maze, tile).Count > 0) {
			return true;
		}
		/*
		if (tile.x - 2 >= 0)
			if (!Maze.instance.tiles [tile.x - 2, tile.y].visited)
				return true; 

		if (tile.x + 2 < width)
			if (!Maze.instance.tiles[tile.x + 2, tile.y].visited)
				return true;

		if (tile.y - 2 > 0)
			if (!Maze.instance.tiles[tile.x, tile.y - 2].visited) 
				return true;
			
		if (tile.y + 2 < height) 
			if (!Maze.instance.tiles[tile.x, tile.y + 2].visited)
				return true;
		*/
		return false;
	}
		
	static void InicializeNullMaze(Maze maze) {
		for (int i = 0; i < maze.width; i++) {
			for (int j = 0; j < maze.height; j++) {
				maze.tiles [i, j] = new Tile ();
				maze.tiles [i, j].x = i;
				maze.tiles [i, j].y = j;
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

		//Debug.Log ("deltaX: " + deltaX);
		//Debug.Log ("deltaY: " + deltaY);

		//int debugX = current.x + deltaX/2;
		//int debugY = current.y + deltaY/2;
		//Debug.Log ("remove wall x: " + debugX);
		//Debug.Log ("remove wall y: " + debugY);

		if (current.x + deltaX / 2 > 0 && current.y + deltaY / 2 > 0 && 
			current.x + deltaX / 2 < maze.width && current.y + deltaY / 2 < maze.height) {
			maze.tiles [current.x + deltaX / 2, current.y + deltaY / 2].isWall = false;
			maze.tiles [current.x + deltaX / 2, current.y + deltaY / 2].visited = true;
		//	Debug.Log ("Removed");
		} else {
		//	Debug.Log ("Not removed");
		}
	}

	static Tile BeginMazeGenerator(Maze maze, int h) {
		Tile begin = new Tile();
		int y = Random.Range (1, h-2);
		if (y % 2 == 0) {
			y++;
		}

		begin.isWall = false;
		begin.x = 1;
		begin.y = y;
		maze.tiles [1, y] = begin;
		maze.beginCoord = new Vector2(begin.x, begin.y);
		return begin;	
	}

	public static void ExpandMaze(Maze maze){
		Tile[,] expandedTiles = new Tile[maze.width*2, maze.height*2];
		for(int i = 0, a = 0; i < maze.width; i++, a+=2) {
			for(int j = 0, b = 0; j < maze.height; j++, b+=2) {
				expandedTiles [a, b] = new Tile (a, b);
				expandedTiles [a, b + 1] = new Tile (a, b + 1);
				expandedTiles [a + 1, b] = new Tile (a + 1, b);
				expandedTiles [a + 1, b + 1] = new Tile (a + 1, b + 1);
				if(maze.tiles[i, j].isWall) {
					expandedTiles [a, b].isWall = true;
					expandedTiles [a, b + 1].isWall = true;
					expandedTiles [a + 1, b].isWall = true;
					expandedTiles [a + 1, b + 1].isWall = true;
				} else {
					expandedTiles [a, b].isWall = false;
					expandedTiles [a, b + 1].isWall = false;
					expandedTiles [a + 1, b].isWall = false;
					expandedTiles [a + 1, b + 1].isWall = false;					
				}
			}
		}
		maze.tiles = expandedTiles;
		maze.beginCoord.x = 3;
		maze.beginCoord.y *= 2;
	}

	public static Maze CreateMaze(string theme, int w, int h) { // vai ser esse
		//return TempMaze();
		Maze maze = new Maze (theme, w, h);

		Tile currentTile = BeginMazeGenerator(maze, h);
		Tile temp;
		Stack<Tile> stack = new Stack<Tile> ();
		List<Tile> neighbours;

		InicializeNullMaze (maze);

		stack.Push (currentTile);
		while (stack.Count > 0) {
			currentTile = stack.Pop ();
			currentTile.visited = true;
		//	Debug.Log ("currentTile x: " + currentTile.x + " currentTile.y: " + currentTile.y);

			if (NotVisitedNeighbours (maze, currentTile)) {
				neighbours = GetNeighbours (maze, currentTile);
				temp = neighbours [Random.Range (0, neighbours.Count)];
				stack.Push (currentTile);
				stack.Push (temp);
				RemoveWall (maze, currentTile, temp);
			} 
		}
		ExpandMaze (maze);
		return maze;
	}

	/*
	private static void TempMaze() { // provisório
	
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

		Maze.instance.tiles = new Tile[w, h];
	
		for (int i = 0; i < w; i++) {
			for (int j = 0; j < h; j++) {
				tiles [i, j] = new Tile ();
				tiles [i, j].isWall = tempMap [i, j] == 1 ? true : false;
				tiles [i, j].x = i;
				tiles [i, j].y = j;
			}
		}

	}*/

}
