using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(CharacterBase))]
[RequireComponent (typeof(Animator))]
public class DirectionObstacle : MonoBehaviour {

	public Sprite[] sprites;

	private CharacterBase character;

	void Start() {
		character = GetComponent<CharacterBase> ();
		character.InitialDirection ();

		GetComponent<SpriteRenderer> ().sprite = sprites [character.direction];
	}

}
