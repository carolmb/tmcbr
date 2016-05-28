using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	Animator animator;
	BoxCollider2D hitBox;
	bool closed = true;
	Vector2 size;

	void Awake () {
		hitBox = GetComponent<BoxCollider2D> ();
		animator = GetComponent<Animator> ();
		size = hitBox.bounds.size;
		animator.speed = 0;
	}

	void OnInteract() {
		if (Bag.current.HasItem (Item.DB[9])) {
			if (closed) {
				Open ();
			} else {
				Close ();
			}
		} else {
			SoundManager.Lock ();
		}
	}

	void Open () {
		SoundManager.OpenDoor ();
		hitBox.size = Vector2.zero;
		closed = false;
		animator.SetBool ("Closed", false);
		animator.speed = 1;
	}

	void Close () {
		hitBox.size = size;
		closed = true;
		animator.SetBool ("Closed", true);
	}

}
