using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HallGenerator : MazeGenerator {

	protected override string Theme () {
		return "Hall";
	}

	protected override Tile GetNeighbour(List<Tile> n){
		if (Random.Range (1, 100) < 80 || n.Count == 1) {
			return n[0];
		} else {
			return n [Random.Range (1, n.Count - 1)];
		}
	}

	public override void CreateEnemies (Maze maze) {
		this.maze = maze;
		foreach (Tile t in maze.tiles) {
			if (!HasTransitionNear (t)) {
				if (EmptyRadiusToEnemies (t)) {
					if (GetAllWallNeighbours (t).Count == 2 && t.isWalkable) { //mimics
						if (Random.Range (0, 100) < 50) { //fator random
							t.objectName = "Enemies/Mimic"; //trocar pelo nome do prefab
						}
					} else if (GetAllWallNeighbours (t).Count == 3 && t.isWalkable) { //armadura
						if (Random.Range (0, 100) < 30) { //fator random
							t.objectName = "Enemies/KnightArmor"; 
						}
					} else if (GetAllWallNeighbours (t).Count == 3 && t.isWall) { //espelho
						if (Random.Range (0, 100) < 60) {
							t.objectName = "Enemies/EnemyMirror";
						}
					}
				} else if (t.isWalkable && NoObstaclesNear (t)) {
					if (Random.Range (0, 100) < 15) {
						t.obstacleID = 3; // Flores
					} else if (Random.Range (0, 100) < 15 && maze.tiles [t.x, t.y + 1].isWall) {
						t.obstacleID = 1; // Mesa
					} else if (Random.Range (0, 100) < 15 && maze.tiles [t.x, t.y + 1].isWall) {
						t.obstacleID = 2; // Cadeira
					}
				}
			}
		}
	}

}
