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
		if (closed) {
			Open ();
		} else {
			Close ();
		}
	}

	void Open () {
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
