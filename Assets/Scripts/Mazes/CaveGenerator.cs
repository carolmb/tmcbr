using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CaveGenerator : MazeGenerator {

	public CaveGenerator(string theme, int w, int h) : base(theme, w, h) { } 

	protected override Tile GetNeighbour(List<Tile> n) {
		return null;
	}

	public override void CreateEnemies (Maze maze) {

	}

}
