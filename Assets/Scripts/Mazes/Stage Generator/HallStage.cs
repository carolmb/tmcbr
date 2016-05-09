using UnityEngine;
using System.Collections;

public class HallStage : ProceduralStage {

	Transition entrance;
	Transition mirrorRoom;

	public HallStage(int i, Transition entrance, Transition mirrorRoom) : base(i) {
		this.entrance = entrance;
		this.mirrorRoom = mirrorRoom;
		CreateMazes ();
		endIndex = mazes.Length + beginIndex - 1;
	}

	// TODO: colocar pra gerar em árvore
	protected void CreateMazes () {
		int mazeCount = 1;
		HallMaze[] mazes = new HallMaze[mazeCount];

		int beginDir = mirrorRoom.dir;

		for (int i = 0; i < mazeCount; i++) {
			mazes[i] = new HallMaze(i + beginIndex, 1 + 2 * Random.Range(5, 8), 1 + 2 * Random.Range(5, 8));
			mazes[i].Expand (expansionFactor, expansionFactor);
		}

		for (int i = 0; i < mazeCount - 1; i++) {
			beginDir = GenerateDir (beginDir);
			SetTransitions (
				mazes [i], 
				mazes [i + 1], 
				beginDir
			);
		}

		// Transições aleatórias
		for (int i = 0; i < mazeCount; i++) {
			int x = Random.Range (0, mazeCount);
			int y = Random.Range (0, mazeCount);
			beginDir = GenerateDir (beginDir);
			SetTransitions (
				mazes [x],
				mazes [y],
				beginDir
			);
		}

		Tile entranceTile = GenerateBorderTile (mazes [0], 3 - entrance.dir, entrance.size);
		Tile roomTile = GenerateBorderTile (mazes [mazeCount - 1], 3 - mirrorRoom.dir, mirrorRoom.size);

		AddTransition (mazes [0], entranceTile.x, entranceTile.y, 3 - entrance.dir, expansionFactor);
		AddTransition (mazes [mazeCount - 1], roomTile.x, roomTile.y, 3 - mirrorRoom.dir, expansionFactor);

		this.mazes = mazes;
	}

}
