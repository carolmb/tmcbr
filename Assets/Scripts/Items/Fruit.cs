using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fruit : Item {
	static List<int> fruitsCatched;

	public Fruit(int id, string name, int number = -1) : base(id, name, 0, 0) {
		if (fruitsCatched == null) {
			fruitsCatched = new List<int> ();
		}
		if(number >= 0)
			fruitsCatched.Add (number);
	}

	public void Add(int i){
		fruitsCatched.Add (i);
	}

	public override bool CanUse () {
		return true;
	}

	public override void OnUse () {
		
	}

}
