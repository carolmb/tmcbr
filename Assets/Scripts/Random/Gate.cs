using UnityEngine;
using System.Collections;

public class Gate : MonoBehaviour {

	Animator animator;
	bool closed = true;

	bool PlayerIsNear {
		get {
			if (Mathf.Abs (Player.instance.transform.position.x - transform.position.x) < 24) {
				if (transform.position.y - Player.instance.transform.position.y < 48) {
					return true;
				}
			}
			return false;
		}
	}

	void Awake () {
		animator = GetComponent<Animator> ();
		animator.speed = 0;
	}

	// Use this for initialization
	void Start () {
		Vector3 pos = transform.position;
		pos.x += Tile.size / 2;
		pos.y -= Tile.size / 2;
		pos.z = pos.y + Tile.size / 2;
		transform.position = pos;
	}

	void Update() {
		if (PlayerIsNear) {
			if (closed) {
				Open ();
			}
		} else {
			if (!closed) {
				Close ();
			}
		}
	}

	void Open () {
		closed = false;
		animator.SetBool ("Closed", false);
		animator.speed = 1;
	}

	void Close () {
		closed = true;
		animator.SetBool ("Closed", true);
	}

}
