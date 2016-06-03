using UnityEngine;
using System.Collections;

public class FruitObject : MonoBehaviour {
	public int number;

	void OnTriggerEnter2D(Collider2D col) {
		if (col.tag == "Player") {
			Item item = Item.DB [10 + number];
			SoundManager.Coin ();
			SaveManager.currentSave.bag.Add (item);
			Destroy (gameObject);
		}
	}
}
