using UnityEngine;
using System.Collections;

public class FruitObject : MonoBehaviour {
	public int number;

	void OnTriggerEnter2D(Collider2D col) {
		if (col.tag == "Player") {
			Fruit fruit = new Fruit (10 + number, "Fruit", number);
			SaveManager.currentSave.bag.Add (fruit);
			Destroy (gameObject);
		}
	}
}
