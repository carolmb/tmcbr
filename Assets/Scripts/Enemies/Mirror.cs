using UnityEngine;
using System.Collections;

public class Mirror : MonoBehaviour {

	// Player
	public Player player;
	// If the mirror is a trap
	public bool isTrap;
	// If the mirror is the door
	public bool isDoor;
	// Colliders
	public Collider2D[] collidersOnTrigger;
	// Size
	public float size;

	// Use this for initialization
	void Start () {
		//
	}
	
	// Update is called once per frame
	void Update () {
		// Checks if the player is in front of mirror
		if (checkPlayerPosition()) {
			seeingMirror();
		}
	}

	// Checks if the player is in front of mirror
	bool checkPlayerPosition() {
		// Gets the objects
		collidersOnTrigger = Physics2D.OverlapCircleAll(transform.position, size);
		// Checks collision for each object
		foreach (Collider2D collider in collidersOnTrigger) {
			// If the player collide with the object
			if (collider.gameObject.name == "Player") {
				return true;
			}
		}
		return false;
	}

	// When the character see the mirror
	void seeingMirror() {
		// If this mirror is a trap
		if (isTrap) {
			// Shows an enemy
		} else if (isDoor) {
			// Animation
		} else {
			// Shows the reflection of the character
		}
	}
}
