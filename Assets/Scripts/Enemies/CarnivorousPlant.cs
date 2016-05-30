using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarnivorousPlant : MonoBehaviour {

	public GameObject rose;

	static int[] fruits;
	int currentFruit;
	public int damage = 5;

	// Use this for initialization
	void Start () {
		fruits = new int[6];
		for (int i = 0; i < 6; i++)
			fruits [i] = i;
		currentFruit = 0;
	}

	void Eat() {
		Item fruitEquiped = SaveManager.currentSave.bag.selectedItem;
		if (fruitEquiped != null && fruitEquiped is Fruit) {
			Fruit f = (Fruit)fruitEquiped;
			if (f.number == fruits [currentFruit]) {
				Player.instance.UseItem ();
				currentFruit++;
				SoundManager.Coin ();
			} else {
				Attack ();
			}
		} else {
			Attack ();
		}

		if (currentFruit == 6) {
			Vector2 pos = MazeManager.WorldToTilePos (transform.position);
			MazeManager.maze.tiles [(int)pos.x, (int)pos.y].obstacle = "";
			GameObject r = Instantiate (rose) as GameObject;
			r.transform.position = transform.position;
			Destroy (gameObject);
		}
	}

	void Attack() {
		if (!Player.instance.character.damaging && !Player.instance.immune) {
			Player.instance.character.Damage (transform.position, damage);
		}
	}
		
	void OnInteract () {
		Player.instance.Pause ();
		SoundManager.Click ();
		Eat();
		Player.instance.Resume ();	
	}
}
