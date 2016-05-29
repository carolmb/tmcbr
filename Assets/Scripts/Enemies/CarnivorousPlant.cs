using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarnivorousPlant : MonoBehaviour {

	static List<int> fruits;
	public int damage = 5;

	// Use this for initialization
	void Start () {
		fruits = new List<int> ();
		for(int i = 0; i < 7; i++)
			fruits.Add (i);
	}

	void Eat(int item) {
		if (fruits [0] == item) {
			fruits.Remove (0);
		} else {
			Atack ();
		}
	}

	void Atack() {
		Player.instance.character.Damage (transform.position, damage);
	}
}
