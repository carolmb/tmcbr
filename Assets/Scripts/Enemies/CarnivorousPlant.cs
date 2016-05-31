using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarnivorousPlant : MonoBehaviour {

	public GameObject rose;

	static int[] fruits;
	public int damage = 5;

	// Use this for initialization
	void Start () {
		fruits = new int[6];
		for (int i = 0; i < 6; i++)
			fruits [i] = i;
		SaveManager.currentSave.currentFruit = 0;
	}

	void Eat() {
		Item fruitEquiped = SaveManager.currentSave.bag.selectedItem;
		if (fruitEquiped != null && fruitEquiped is Fruit) {
			Fruit f = (Fruit)fruitEquiped;
			if (f.number == fruits [SaveManager.currentSave.currentFruit]) {
				Player.instance.UseItem ();
				SaveManager.currentSave.currentFruit++;
				SoundManager.Coin ();
			} else {
				Attack ();
			}
		} else {
			Attack ();
		}

		if (SaveManager.currentSave.currentFruit == 6) {
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
			SaveManager.currentSave.currentFruit = 0;
		}
	}
		
	void OnInteract () {
		SoundManager.Click ();
		Eat();
	}
}
