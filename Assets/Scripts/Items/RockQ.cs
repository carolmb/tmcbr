using UnityEngine;
using System.Collections;

public class RockQ : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.CompareTag ("Player")) {
			SoundManager.Coin ();
			Bag.current.AddOne (Item.DB [0]);
			Destroy (gameObject);
		}
	}
}
