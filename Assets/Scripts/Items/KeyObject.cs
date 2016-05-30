using UnityEngine;
using System.Collections;

public class KeyObject : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Player")) {
			Item item = Item.DB [9];
			SoundManager.Rose ();
			SaveManager.currentSave.bag.Add (item);
			Destroy (gameObject);
		}
	}

}
