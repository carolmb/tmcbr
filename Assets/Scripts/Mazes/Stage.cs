// Estrutura auxiliar
public class Stage {
	public Maze[] mazes;
	public Tile beginTile = null;
	public int beginDir = -1;
	public int beginSize = 2;
	public Tile endTile = null;
	public int endDir = -1;
	public int endSize = 2;

	public Stage(Maze[] mazes) {
		this.mazes = mazes;
	}
	public Stage(int size) {
		this.mazes = new Maze[size];
	}

}