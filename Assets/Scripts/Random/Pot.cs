using UnityEngine;
using System.Collections;

public class Pot : MonoBehaviour {

	public Sprite left;
	public Sprite right;
	private Sprite center;
	SpriteRenderer sr;

	void Awake() {
		sr = GetComponent<SpriteRenderer> ();
		center = sr.sprite;
	}

	void Update () {
		Vector2 p = Player.instance.transform.position;
		if ((p.x - transform.position.x) < -16) {
			if (Mathf.Abs (p.y - transform.position.y) <= Mathf.Abs (p.x - transform.position.x) + 32) {
				sr.sprite = left;
			} else {
				sr.sprite = center;
			}
		} else if ((p.x - transform.position.x) > 16) {
			if (Mathf.Abs (p.y - transform.position.y) <= Mathf.Abs (p.x - transform.position.x) + 32) {
				sr.sprite = right;
			} else {
				sr.sprite = center;
			}
		} else {
			sr.sprite = center;
		}
	}

}
