using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

class PriorityQueue<Node> {

	private SortedDictionary<float, Queue<Node>> list;

	public PriorityQueue() {
		list = new SortedDictionary<float, Queue<Node>>();
	}
	
	public void Enqueue(float priority, Node node) {
		Queue<Node> q;
		if (!list.TryGetValue(priority, out q)) {
			q = new Queue<Node>();
			list.Add(priority, q);
		}
		q.Enqueue(node);
	}
	
	public Node Dequeue() {
		// will throw if there isn’t any first element!
		var pair = list.First();
		var node = pair.Value.Dequeue();
		if (pair.Value.Count == 0) // nothing left of the top priority.
			list.Remove(pair.Key);
		return node;
	}

	public bool IsEmpty {
		get { return !list.Any(); }
	}

	public List<Node> ToList() {
		List<Node> toList = new List<Node>();
		while (!IsEmpty) {
			toList.Add(Dequeue());
		}
		return toList;
	}

}
