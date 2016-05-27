using UnityEngine;
using System.Collections;

public class CaveStage : ProceduralStage {

	Transition mirrorRoom;
	Transition fireplace;

	public CaveStage(int i, Transition mirrorRoom, Transition fireplace) : base(i) {
		this.mirrorRoom = mirrorRoom;
		this.fireplace = fireplace;
		CreateMazes ();
		endIndex = mazes.Length + beginIndex - 1;
	}

	public Transition toHall {
		get { return transitions [0]; }
	}

	public Transition toForest {
		get { return transitions [1]; }
	}

	protected void CreateMazes () {
		int mazeCount = 2;
		CaveMaze[] mazes = new CaveMaze [mazeCount];

		int beginDir = mirrorRoom.dir;

		for (int i = 0; i < mazeCount; i++) {
			mazes[i] = new CaveMaze(i + beginIndex, 1 + 2 * Random.Range(5, 8), 1 + 2 * Random.Range(5, 8));
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

		Tile roomTile = GenerateBorderTile (mazes [0], 3 - mirrorRoom.dir, mirrorRoom.size);
		Tile fireTile;
		do {
			fireTile = GenerateBorderTile (mazes [mazeCount - 1], 3 - fireplace.dir, fireplace.size);
		} while (roomTile.x == fireTile.x);

		AddTransition (mazes [0], roomTile.x, roomTile.y, 3 - mirrorRoom.dir, expansionFactor);
		AddTransition (mazes [mazeCount - 1], fireTile.x, fireTile.y, 3 - fireplace.dir, expansionFactor);

		this.mazes = mazes;
	}

}
