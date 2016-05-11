using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HallStage : ProceduralStage {

	class NodeGraph {
		public HallMaze maze;
		public NodeGraph father;
		public List<NodeGraph> children;
		public NodeGraph(int id, int w, int h, int type = 0, NodeGraph father = null){
			maze = new HallMaze(id, w, h, type);
			maze.Expand (expansionFactor, expansionFactor);
			this.father = father;
			children = new List<NodeGraph>();
		}
	}

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
		/*
		 * current = maze (primeiro maze)
		 * enquanto mazeCount < maxMazeCount
		 * 		gerar numero random de filhos
		 * 		definir um para ser sala (e gerar sala e conexão com pai)
		 * 		gerar labirintos comuns (menores)
		 * 			gerar conexões
		 * 		colocar os filhos em uma fila
		 * 		atualizar contador
		 * 		current = tira da fila próximo maze
		*/

		Queue<NodeGraph> queue = new Queue<NodeGraph>();
		NodeGraph first = new NodeGraph (beginIndex, 1 + 2 * Random.Range (5, 8), 1 + 2 * Random.Range (5, 8));
		NodeGraph current;

		queue.Enqueue (first);

		int mazeCount = 0, maxMazeCount = Random.Range (5, 8);
		int currentId = beginIndex + 1;

		while (mazeCount < maxMazeCount) {
			current = queue.Dequeue ();
			int childrenNumber = Random.Range (1, Mathf.Min(maxMazeCount - mazeCount + 1, 4));
			NodeGraph[] children = CreateChildren (current, childrenNumber, currentId);
			foreach (NodeGraph c in children) {
				if (c.maze.type == 0) {
					queue.Enqueue (c);
				}
			}
			currentId += childrenNumber;
			mazeCount += childrenNumber;
		}

		NodeGraph final = GetLastMaze (first);

		HallMaze[] mazes = TreeToArray(first, final, maxMazeCount);

		Tile entranceTile = GenerateBorderTile (mazes [0], 3 - entrance.dir, entrance.size);
		Tile roomTile = GenerateBorderTile (final.maze, 3 - mirrorRoom.dir, mirrorRoom.size);

		AddTransition (mazes [0], entranceTile.x, entranceTile.y, 3 - entrance.dir, expansionFactor);
		AddTransition (final.maze, roomTile.x, roomTile.y, 3 - mirrorRoom.dir, expansionFactor);

		this.mazes = mazes;

		/*
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
		*/
	}

	NodeGraph GetLastMaze(NodeGraph first) {
		NodeGraph current = first;
		while (current.children.Count > 0) {
			foreach (NodeGraph node in current.children) {
				if (node.maze.type == 0) {
					current = node;
					break;
				}
			}
		}
		return current;
	}

	NodeGraph[] CreateChildren (NodeGraph father, int childrenNumber, int currentId) {
		int beginDir = Random.Range (0, 4);
		for (int i = 0, id = currentId; i < childrenNumber; i++, id++) {
			NodeGraph newNode = new NodeGraph (
				id, 1 + 2 * Random.Range (5, 8), 
				1 + 2 * Random.Range (5, 8), 
				DefineTypeHallMaze (i, childrenNumber), 
				father);
			father.children.Add (newNode);
			beginDir = GenerateDir (beginDir);
			SetTransitions (
				father.maze, 
				newNode.maze, 
				beginDir
			);
		}
		return father.children.ToArray();
	}

	HallMaze[] TreeToArray(NodeGraph initialNode, NodeGraph finalNode, int max) {
		List<HallMaze> hallMazes = new List<HallMaze> ();
		NodeGraph current;
		Queue<NodeGraph> queue = new Queue<NodeGraph>();
		queue.Enqueue (initialNode);
		while (queue.Count > 0) {
			current = queue.Dequeue ();

			if (current.children.Count == 0 && current != finalNode) {
				current.maze.ChangeType ();
			}

			hallMazes.Add (current.maze);
			foreach (NodeGraph node in current.children) {
				queue.Enqueue (node);
			}
		}
		return hallMazes.ToArray ();
	}

	int DefineTypeHallMaze (int i, int childrenNumber) {
		if (childrenNumber - i < 1) {
			return 1;
		} else {
			return 0;
		}
	}
}
