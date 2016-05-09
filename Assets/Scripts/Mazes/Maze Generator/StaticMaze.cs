using UnityEngine;
using System.Collections;

public class StaticMaze : Maze {

	string theme;

	public StaticMaze(int i, int w, int h, string theme) : base(i, w, h) {
		this.theme = theme;
	}

	public override string GetTheme () {
		return theme;
	}

}
