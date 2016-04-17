using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameSave {

	public string name;
	public Maze[] mazes;
	public Transition transition;
	public Bag bag;
	public int lifePoints;

	public GameSave() {
		mazes = GameGenerator.Create ();

		// TODO: Substituir pelo primeiro tile da cena de introdução
		transition = new Transition(0, mazes[0].beginTile.x, mazes[0].beginTile.y, Character.RIGHT);

		bag = new Bag ();
		lifePoints = 1000;
	}

}
