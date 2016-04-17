using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CaveGenerator : MazeGenerator {

	protected override string Theme () {
		return "Cave";
	}

	protected override Tile GetNeighbour(Tile t) {
		return null;
	}

	public override void CreateEnemies (Maze maze) {

	}

}
