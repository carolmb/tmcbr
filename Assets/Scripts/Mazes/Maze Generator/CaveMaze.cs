﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CaveMaze : ProceduralMaze {

	public int type;

	public CaveMaze(int id, int width, int height, int type = 0)  : base (id, width, height) { 
		this.type = type;
		if (type == 0)
			GenerateTiles ();
		else
			SpecialCave ();
	}

	public override string GetTheme () {
		return "Cave";
	}

	protected override Tile GetNeighbour (Tile t, bool[,] visited){
		List<Tile> n = GetVisitedNeighbours (t, 2, visited);
		int i = Random.Range (0, n.Count);
		return n [i];
	}

	void SpecialCave() {
		tiles = new Tile[width, height];
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				tiles [i, j] = new Tile (i, j);
				if (i == 0 || i == width - 1 || j == 0 || j == height - 1) {
					tiles [i, j].wallID = 1;
				}
			}
		}
		tiles [width / 2, height - 2].objectName = "Enemies/Golem3";
	}
		

	public override void CreateObstacles () {
		int beginID = GameGraph.current.caveStage.beginIndex;
		if (type == 1)
			return;
		foreach (Tile t in tiles) {
			if (HasTransitionNear (t))
				continue;

			if (t.isWalkable) {
				if (EmptyRadiusToEnemies (t, 4) && Random.Range (0, 100) < 10) {
					t.objectName = "Enemies/Bat";
				} else if (!HasObstaclesNear (t)) {
					int r = Random.Range (0, 100);

					if (r < 25 && this.id > beginID) {
						t.objectName = "Hidden Hole";
						t.transition = GenerateHole (beginID);
					} else if (r < 45) {
						t.obstacle = "Puddle1";
					} else if (r < 50) {
						t.objectName = "Enemies/Golem1";
					} else if (r < 65) {
						t.obstacle = "Puddle2";
					} else if (r < 80) {
						t.obstacle = "Rock";
					} else if (r < 95) {
						t.obstacle = "Mushroom";
					} 
				}
			}
		}

		foreach (Tile t in tiles) {
			if (HasTransitionNear (t)) {
				continue;
			}
			if (t.isWall && t.x > 0 && t.x < width - 1 && t.y > 0 && t.y < height - 1) {	
				if (Random.Range (0, 100) < 20) { //fator random
					t.obstacle = "Rock";
					t.wallID = 0;
				} else if (Random.Range (0, 100) < 30) {
					t.obstacle = "Mushroom";
					t.wallID = 0;
				} 
			}
		}
	}


	Tile.Transition GenerateHole (int beginID) {

		int id = Random.Range (beginID, this.id);

		Maze maze = GameGraph.current.caveStage.mazes [id - beginID];

		int x, y;

		do {
			x = Random.Range (2, maze.width - 3);
			y = Random.Range (2, maze.height - 3);
		} while (!maze.tiles [x, y].isWalkable);

		Tile.Transition transition = new Tile.Transition (id, x, y, 3);
		transition.instant = false;
		return transition;
	}

}
