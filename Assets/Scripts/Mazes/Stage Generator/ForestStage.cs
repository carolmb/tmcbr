using UnityEngine;
using System.Collections;

public class ForestStage : ProceduralStage {

	Transition fireplace;

	public ForestStage(int i, Transition fireplace) : base(i) {
		this.fireplace = fireplace;
		CreateMazes ();
		endIndex = mazes.Length + beginIndex - 1;
	}

	protected void CreateMazes () {
		int mazeCount = 3;
		ForestMaze[] mazes = new ForestMaze[mazeCount + 1];

		int beginDir = fireplace.dir;

		for (int i = 0; i < mazeCount; i++) {
			mazes[i] = new ForestMaze(i + beginIndex, 1 + 2 * Random.Range(5, 8), 1 + 2 * Random.Range(5, 8));
			mazes[i].Expand (expansionFactor, expansionFactor);
		}

		for (int i = 0, j = 1; j < mazeCount; i++, j++) {
			beginDir = GenerateDir (beginDir);
			SetTransitions (
				mazes [i], 
				mazes [j], 
				beginDir
			);
		}

		ForestMaze specialForest = new ForestMaze (beginIndex + mazeCount, 10, 8, 1);

		mazes [mazeCount] = specialForest;
		for (int i = 0; i < mazeCount; i++) {
			int x = UnityEngine.Random.Range (0, mazeCount);
			int y = UnityEngine.Random.Range (0, mazeCount);
			beginDir = GenerateDir (beginDir);
			SetTransitions (
				mazes [x],
				mazes [y],
				beginDir
			);
		}

		for (int i = 0; i < mazeCount; i++) {
			int x = UnityEngine.Random.Range (0, mazeCount);
			int y = UnityEngine.Random.Range (0, mazeCount);
			beginDir = GenerateDir (beginDir);
			SetTransitions (
				mazes [x],
				mazes [y],
				beginDir
			);
		}

		Tile fireTile = GenerateBorderTile (mazes [0], 3 - fireplace.dir, fireplace.size);
		AddTransition (mazes [0], fireTile.x, fireTile.y, 3 - fireplace.dir, expansionFactor);

		this.mazes = mazes;
	}

}
