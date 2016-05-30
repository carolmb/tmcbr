using UnityEngine;
using System.Collections;

public class Rose : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag ("Player")) {
			SoundManager.Rose ();
			Bag.current.roses++;
			GameHUD.instance.mainMenu.UpdateRoses (Bag.current.roses);
			Destroy (gameObject);
		}
	}

}
