using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class HallStage : ProceduralStage {

	bool puzzle;

	class NodeGraph {
		public int id;
		public int type;
		public NodeGraph father;
		public List<NodeGraph> children;
		public HallMaze maze;
		public NodeGraph(int id, int type = 0, NodeGraph father = null){
			this.id = id;
			this.type = type;
			maze = null;
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
		puzzle = false;
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
		NodeGraph first = new NodeGraph (beginIndex);
		NodeGraph current;

		queue.Enqueue (first);

		int mazeCount = 1, maxMazeCount = 2;//Random.Range (7, 12);
		int currentId = beginIndex + 1;

		while (mazeCount < maxMazeCount) {
			current = queue.Dequeue ();
			int childrenNumber = Random.Range (Mathf.Min(maxMazeCount - mazeCount + 1, 3), Mathf.Min(maxMazeCount - mazeCount + 1, 4));
			NodeGraph[] children = CreateChildren (current, childrenNumber, currentId);
			foreach (NodeGraph c in children) {
				if (c.type == 0) {
					queue.Enqueue (c);
				}
			}

			currentId += childrenNumber;
			mazeCount += childrenNumber;
		}

		NodeGraph final = GetEndWay (first);
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

	// variar os possiveis ultimos
	NodeGraph GetEndWay(NodeGraph first) {
		NodeGraph current = first;
		while (current.children.Count > 0) {
			List<NodeGraph> children = new List<NodeGraph>();
			foreach (NodeGraph node in current.children) {
				if (node.type == 0) {
					children.Add (node);
				}
			}
			current = children [Random.Range (0, children.Count)];
		}
		return current;
	}

	NodeGraph[] CreateChildren (NodeGraph father, int childrenNumber, int currentId) {
		for (int i = 0, id = currentId; i < childrenNumber; i++, id++) {
			NodeGraph newNode = new NodeGraph (
				id,
				DefineTypeHallMaze (i, childrenNumber), 
				father);
			father.children.Add (newNode);
		}
		return father.children.ToArray();
	}

	//transforma a estrutura árvore em array, cria os labirintos e transições
	HallMaze[] TreeToArray(NodeGraph initialNode, NodeGraph finalNode, int max) {
		List<HallMaze> hallMazes = new List<HallMaze> ();
		NodeGraph current = null;
		Queue<NodeGraph> queue = new Queue<NodeGraph>();
		queue.Enqueue (initialNode);
		while (queue.Count > 0) {
			current = queue.Dequeue ();

			if (current.children.Count == 0 && current != finalNode && current.type != 2) {
				current.type = 1;
			}
			hallMazes.Add (FromNodeToArray (current));

			foreach (NodeGraph node in current.children) {
				queue.Enqueue (node);
			}

		}
		return hallMazes.ToArray ();
	}

	HallMaze FromNodeToArray (NodeGraph node) {
		int w, h;
		if (node.type == 0) {
			//hall normal
			w = Random.Range (4, 6) * 2 + 1;
			h = Random.Range (4, 6) * 2 + 1;
		} else {
			//salinha
			w = 5;//Random.Range (5, 8);
			h = 5;//Random.Range (5, 8);
		}
		HallMaze hallMaze = new HallMaze (node.id, w, h, node.type);
		hallMaze.Expand (expansionFactor, expansionFactor);
		node.maze = hallMaze;
		int beginDir = Random.Range (0, 4);

		if (node.father != null) {
			beginDir = GenerateDir (beginDir);
			SetTransitions (
				node.father.maze, 
				node.maze, 
				beginDir
			);
		}

		return hallMaze;
	}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         

	int DefineTypeHallMaze (int i, int childrenNumber) {
		int type = 0;
		if (i == 0 && !puzzle && childrenNumber > 1) {
			type = 2;
			puzzle = true;
		} else if (i == 0 && childrenNumber > 1) {
			type = 1;
		}
		Debug.Log (type);
		return type;
	}
}
