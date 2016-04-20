using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MazeManager : MonoBehaviour {

	public static Maze maze; // Se quiser acesso a qualquer informação do labirinto, use isso
	public static BoxCollider2D[,] obstacles;

	// ===============================================================================
	// Transição entre labirintos
	// ===============================================================================

	public static Transition currentTransition {
		get { return SaveManager.currentSave.transition; }
	}

	public static void GoToMaze(Transition transition) {
		SaveManager.currentSave.transition = transition;
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	// ===============================================================================
	// Criação dos tiles
	// ===============================================================================

	// Coloca o player na posição inicial
	void Start() {
		Vector2 tilePos = new Vector2 (currentTransition.tileX, currentTransition.tileY);
		Vector2 initPos = TileToWorldPosition (tilePos) + new Vector3 (0, Tile.size / 2, 0);
		Player.instance.transform.position = initPos;
		Player.instance.character.direction = currentTransition.direction;
	}

	// Resgata o labirinto atual e inicializa os tiles
	void Awake () {
		
		if (SaveManager.currentSave == null) {
			SaveManager.NewGame ();
		}
		maze = SaveManager.currentSave.mazes [currentTransition.mazeID];
		obstacles = new BoxCollider2D[maze.width, maze.height];
		foreach (Tile t in maze) {
			CreateTileFloor (t.x, t.y, t.floorID);

			if (t.wallID > 0) {
				CreateTileWall (t.x, t.y, t.wallID);
			}
			if (t.obstacleID > 0) {
				CreateTileObstacle (t.x, t.y, t.obstacleID);
			}
			if (t.objectName != "") {
				CreateTileObject (t.x, t.y, t.objectName);
			}
		}
	}

	// Cria o sprite de chão
	GameObject CreateTileFloor(int x, int y, int id) {
		GameObject floor = CreateTileGraphic (x, y, "floor" + id);
		Vector3 pos = floor.transform.position;
		pos.z = 999;
		floor.transform.position = pos;
		return floor;
	}

	// Cria o sprite de parede
	GameObject CreateTileWall(int x, int y, int id) {
		GameObject g = CreateTileGraphic (x, y, "wall" + id);
		g.transform.Translate (0, 0, 1);
		return g;
	}

	// Cria um objeto fazia com um sprite na posição do tile
	GameObject CreateTileGraphic(int x, int y, string spriteName) {
		GameObject obj = new GameObject ();
		obj.name = "Tile[" + spriteName + "] (" + x + ", " + y + ")";
		obj.transform.position = TileToWorldPosition (new Vector2 (x, y));
		SpriteRenderer sr = obj.AddComponent<SpriteRenderer> ();
		sr.sprite = Resources.Load<Sprite> ("Images/Tilesets/" + maze.theme + "/" + spriteName);
		obj.transform.SetParent (transform);
		return obj;
	}

	// Cria objeto do tile (prefab do item/inimigo deve estar na pasta Resources/Prefabs)
	GameObject CreateTileObject(int x, int y, string prefabName) {
		GameObject prefab = Resources.Load<GameObject> ("Prefabs/" + prefabName);
		GameObject obj = Instantiate (prefab);
		obj.transform.position = TileToWorldPosition (new Vector2 (x, y)) + new Vector3(0, Tile.size / 2, Tile.size / 2);
		obj.transform.SetParent (transform);
		obj.name = prefabName;
		return obj;
	}

	// Criar prefabs de obstáculos
	GameObject CreateTileObstacle(int x, int y, int id) {
		GameObject g = CreateTileObject (x, y, "Obstacles/" + maze.theme + "/obstacle" + id);
		g.name = "Tile[obstacle] (" + x + ", " + y + ")";
		obstacles [x, y] = g.GetComponent<BoxCollider2D> ();
		return g;
	}

	// ===============================================================================
	// Posição dos tiles
	// ===============================================================================

	// Converte coordenada em tiles para posição de jogo (em pixels)
	public static Vector3 TileToWorldPosition(Vector2 tilePos) {
		tilePos = tilePos * Tile.size;
		return new Vector3 (tilePos.x, tilePos.y, tilePos.y);
	}

	// Converte posição de jogo (pixels) para coordenada em tiles
	public static Vector2 WorldToTilePos(Vector2 worldPos) {
		Vector2 tilePos = new Vector2(Mathf.Round(worldPos.x / Tile.size), Mathf.Round(worldPos.y / Tile.size));
		//Debug.Log ("world: " + worldPos);
		//Debug.Log ("tile: " + tilePos);
		return tilePos;
	}

	// Arredonda uma coordenada para um múltiplo de Tile.size
	public static int RoundToTile(float value) {
		return Mathf.RoundToInt (value / Tile.size) * Tile.size;
	}

	// Pega o tile na dada posição
	public static Tile GetTile(Vector2 position) {
		Vector2 p = WorldToTilePos(position);
		if (p.x < 0 || p.x >= maze.width || p.y < 0 || p.y >= maze.height) {
			return null;
		}
		return maze.tiles [(int)p.x, (int)p.y];
	}

	// Verifica de um ponto está colidindo com algum tile
	public static bool Collides(float x, float y) {
		Vector2 p = WorldToTilePos(new Vector2 (x, y));
		if (p.x < 0 || p.x >= maze.width || p.y < 0 || p.y >= maze.height) {
			return true;
		}
		int tx = (int)p.x;
		int ty = (int)p.y;
		if (maze.tiles [tx, ty].isWall) {
			return true;
		}
		if (obstacles [tx, ty] == null) {
			return false;
		}
		Rect r = new Rect ();
		r.size = obstacles [tx, ty].size;
		r.center = obstacles [tx, ty].bounds.center - new Vector3(0, Tile.size / 2, 0);
		return r.Contains (new Vector2 (x, y));
	}
		
}
	