using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mimic : MonoBehaviour {

	private Character character;
	Vector2 iniPos;
	Queue<Tile> way;

	void Awake () {
		transform.position = MazeManager.TileToWorldPosition (new Vector2 (2, 4));
		character = GetComponent<Character> ();
		iniPos = MazeManager.WorldToTilePos (new Vector2(transform.position.x, transform.position.y));
	}

	// Use this for initialization
	void Start () {
		way = FindPlayer ();
		Debug.Log("tamanho do way: " + way.Count);
	}

	/*void WalkWay () {
		while (way.Count > 0) {
			Tile currentTile = way.Dequeue ();
		//	Debug.Log (currentTile.x + ", " + currentTile.y + " passou aqui");
			StartCoroutine(character.MoveTo(new Vector2(currentTile.x, currentTile.y)));
			Debug.Log ("terminou de passar");
		}
	
	}
	*/
	Queue<Tile> FindPlayer () {
		Vector2 playerPos = MazeManager.WorldToTilePos (new Vector2 (Player.instance.transform.position.x, 
			Player.instance.transform.position.y));
		Debug.Log (playerPos.x + ", " + playerPos.y);

		MazeGenerator.ClearMazeVisits (MazeManager.maze);
		Tile currentTile = MazeManager.maze.tiles[(int)iniPos.x, (int)iniPos.y];
		Stack<Tile> stack = new Stack<Tile> ();
		List<Tile> neighbours;
		Queue<Tile> way = new Queue<Tile> ();
		stack.Push (currentTile);
		while (stack.Count > 0) {
			//Debug.Log (currentTile.x + " " + currentTile.y);
			currentTile = stack.Pop ();
			way.Enqueue (currentTile);
			if (currentTile.x == (int)playerPos.x && currentTile.y == (int)playerPos.y) {
				//Debug.Log ("LALALA");
				Debug.Log (stack.Count);
				break;
			}
			currentTile.visited = true;
			if (MazeGenerator.NotVisitedNeighbours (MazeManager.maze, currentTile, 1)) {
				neighbours = MazeGenerator.GetNeighbours (MazeManager.maze, currentTile, 1);
				stack.Push (currentTile);
				stack.Push (neighbours [0]);
			}
		}
		return way;

	}

	
	// Update is called once per frame
	void Update () {
		
	}
}
