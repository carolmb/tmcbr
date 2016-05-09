using UnityEngine;
using System.Collections;

public class HallMaze : ProceduralMaze {

	public HallMaze(int i, int w, int h) : base(i, w, h) {}

	public override string GetTheme () {
		return "Hall";
	}

	protected override Tile GetNeighbour(Tile t, bool[,] visited) {
		bool vertical = tiles [t.x, t.y - 1].wallID == 0 || tiles [t.x, t.y + 1].wallID == 0;

		if (Random.Range (0, 100) < 15) {
			vertical = !vertical;
		}

		if (vertical) {
			if (t.y + 2 < height - 1 && !visited [t.x, t.y + 2]) {
				return tiles [t.x, t.y + 2];
			} else if (t.y - 2 > 0 && !visited [t.x, t.y - 2]) {
				return tiles [t.x, t.y - 2];
			} else if (t.x + 2 < width - 1 && !visited [t.x + 2, t.y]) {
				return tiles [t.x + 2, t.y];
			} else {
				return tiles [t.x - 2, t.y];
			}
		} else {
			if (t.x + 2 < width - 1 && !visited [t.x + 2, t.y]) {
				return tiles [t.x + 2, t.y];
			} else if (t.x - 2 > 0 && !visited [t.x - 2, t.y]) {
				return tiles [t.x - 2, t.y];
			} else if (t.y + 2 < height - 1 && !visited [t.x, t.y + 2]) {
				return tiles [t.x, t.y + 2];
			} else {
				return tiles [t.x, t.y - 2];
			}
		}
	}

	public override void CreateObstacles () {
		foreach (Tile t in tiles) {
			if (!HasTransitionNear (t) && t.isWalkable) {
				if (EmptyRadiusToEnemies (t)) {
					if (GetAllWallNeighbours(t).Count > 0) { //mimics
						if (Random.Range (0, 100) < 20 && !tiles [t.x, t.y - 1].isWall) { //fator random
							t.objectName = "Enemies/Mimic";
						} else if (Random.Range (0, 100) < 30) { //fator random
							t.objectName = "Enemies/KnightArmor";
						}
					}
				} else if (!HasObstaclesNear (t)) {
					if (Random.Range (0, 100) < 15) {
						t.obstacleID = 3; // Flores
					} else if (Random.Range (0, 100) < 15 && !tiles [t.x, t.y - 1].isWall) {
						t.obstacleID = 1; // Mesa
					} else if (Random.Range (0, 100) < 15 && tiles [t.x, t.y + 1].isWall) {
						t.obstacleID = 2; // Cadeira
					} else if (Random.Range (0, 100) < 50 && !tiles [t.x, t.y - 1].isWall) {
						t.chest = 1;
					} else if (Random.Range (0, 100) < 60 && !tiles [t.x, t.y - 1].isWall) {
						t.obstacleID = 4;
					}
				}
			}
		}
	}

}
