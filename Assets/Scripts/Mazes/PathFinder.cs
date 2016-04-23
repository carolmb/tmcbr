using UnityEngine;
using System.Collections;

public class PathFinder {

	// A* alrogithm.
	public static GridPath FindPath(Tile initial, Tile final, int maxDistance) {

		// Fila de prioridade com os menores custos para o tile final
		PriorityQueue<GridPath> queue = new PriorityQueue<GridPath>();
		queue.Enqueue (0, new GridPath(initial));

		if (initial == final) {
			return queue.Dequeue ();
		}
			
		// Para armazenar se um tile foi visitado ou não
		bool[,] closedTiles = new bool[MazeManager.maze.width, MazeManager.maze.height];
		for (int i = 0; i < MazeManager.maze.width; i++)
			for (int j = 0; j < MazeManager.maze.height; j++)
				closedTiles [i, j] = false;

		while (!queue.IsEmpty) {
			GridPath currentPath = queue.Dequeue ();
			Tile currentTile = currentPath.LastStep;

			if (closedTiles [currentTile.x, currentTile.y] == false) {
				closedTiles [currentTile.x, currentTile.y] = true;

				foreach (Tile neighbour in currentTile.GetNeighbours4()) {
					if (neighbour.isWalkable && (currentPath.TotalCost + 1 <= maxDistance)) {
						GridPath newPath = currentPath.AddStep (neighbour, 1);

						if (neighbour == final) {
							return newPath;		
						}
						queue.Enqueue (newPath.TotalCost + EstimateCost (neighbour, final), newPath);
					}
				}
			}
		}
		return null;
	}

	public static int EstimateCost(Tile init, Tile final) {
		int dx = Mathf.Abs (final.x - init.x);
		int dy = Mathf.Abs (final.y - init.y);
		return dx + dy;
	}

}
