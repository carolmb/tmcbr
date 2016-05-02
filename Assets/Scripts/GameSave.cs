using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameSave {

	public string name;
	public Maze[] mazes;
	public Transition transition;
	public Bag bag;
	public int lifePoints;
	public float playTime;

	public GameSave() {
		Stage bigStage = GameGenerator.Create ();
		mazes = bigStage.mazes;
		transition = new Transition (0, bigStage.beginTile.x + (bigStage.beginSize - 1) * 0.5f, bigStage.beginTile.y, bigStage.beginDir);

		bag = new Bag ();
		lifePoints = 99;
		playTime = 0;
	}

}
