using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CaveMaze : ProceduralMaze {

	public CaveMaze(int id, int width, int height)  : base (id, width, height) { 
		GenerateTiles ();
	}

	public override string GetTheme () {
		return "Cave";
	}

	protected override Tile GetNeighbour (Tile t, bool[,] visited){
		List<Tile> n = GetVisitedNeighbours (t, 2, visited);
		int i = Random.Range (0, n.Count);
		return n [i];
	}

	public override void CreateObstacles () {

		foreach (Tile t in tiles) {
			if (HasTransitionNear (t)) {
				continue;
			}

			if (t.isWalkable) {
				if (EmptyRadiusToEnemies (t, 4) && Random.Range (0, 100) < 30) {
					t.objectName = "Enemies/Bat";
				} else if (!HasObstaclesNear (t)) {
					int r = Random.Range (0, 100);
					if (r < 20) {
						t.obstacle = "Puddle1";
					} else if (r < 35) {
						Debug.Log ("AQUI");
						t.objectName = "Enemies/Golem1";
					} else if (r < 50) {
						t.obstacle = "Puddle2";
					} else if (r < 65) {
						t.obstacle = "Rock";
					} else if (r < 80) {
						t.obstacle = "Bush4";
					} 
				}
			}
		}

		foreach (Tile t in tiles) {
			if (HasTransitionNear (t)) {
				continue;
			}
			if (t.isWall) {	
				if (Random.Range (0, 100) < 30) { //fator random
					t.obstacle = "Rock";
					t.wallID = 0;
				} else if (Random.Range (0, 100) < 30) {
					t.obstacle = "Bush4";
					t.wallID = 0;
				} 
			}
		}
	}

}
