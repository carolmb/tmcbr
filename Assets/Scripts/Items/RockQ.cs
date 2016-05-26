using UnityEngine;
using System.Collections;

public class RockQ : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.CompareTag ("Player")) {
			Bag.current.Add (Item.DB [0]);
			Destroy (gameObject);
		}
	}
}
