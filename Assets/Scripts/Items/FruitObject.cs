using UnityEngine;
using System.Collections;

public class FruitObject : MonoBehaviour {
	public int number;

	void OnTriggerEnter2D(Collider2D col) {
		if (col.tag == "Player") {
			Fruit fruit = new Fruit (10, "fruit", number);
			SaveManager.currentSave.bag.AddOne (fruit);
			Destroy (gameObject);
		}
	}
}
