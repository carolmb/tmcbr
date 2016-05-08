using UnityEngine;
using System.Collections;

public class Puddle : MonoBehaviour {

	const float maxDistance = 160;

	SpriteRenderer sr;

	void Start () {
		sr = GetComponent<SpriteRenderer> ();
	}

	void Update () {
		float d = ((Vector2) Player.instance.transform.position - (Vector2)transform.position).magnitude;
		sr.color = Lerp (d / maxDistance);
	}

	Color Lerp(float value) {
		if (value > 1) {
			return Color.Lerp (Color.yellow, Color.red, (value - 1) * 4);
		} else if (value > 0.75f) {
			return Color.Lerp (Color.green, Color.yellow, (value - 0.75f) * 4);
		} else if (value > 0.50f) {
			return Color.Lerp (Color.blue, Color.green, (value - 0.50f) * 4);
		} else if (value > 0.25f) {
			return Color.Lerp (Color.magenta, Color.blue, (value - 0.25f) * 4);
		} else {
			return Color.Lerp (Color.red, Color.magenta, value * 4);
		}
	}

}
