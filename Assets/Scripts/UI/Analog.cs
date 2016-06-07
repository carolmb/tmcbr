using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Analog : MonoBehaviour {

	public static Vector2 input;
	private float radius = 0;
	private Vector2 center = Vector2.zero;

	void Start () {
		#if UNITY_ANDROID
			RectTransform rt = GetComponent<RectTransform> ();
			input = Vector2.zero;
			radius = rt.sizeDelta.x / 2;
			center = new Vector2 (radius, radius);
		#else
			gameObject.SetActive(false);
			RectTransform rt = Player.instance.menuButton.GetComponent<RectTransform>();
			rt.anchoredPosition -= new Vector2 (124, 0);
		#endif
	}

	void Update () {
		if (GameManager.SubmitInput ()) {
			input = GameManager.SubmitPosition () - center;
			Debug.Log (GameManager.SubmitPosition ());
			Debug.Log ("center: " + center);
			//Debug.Log (input);
			if (input.magnitude > radius * 2) {
				input = Vector2.zero;
			} else {
				input.Normalize ();
				input.x = Mathf.Round (input.x);
				input.y = Mathf.Round (input.y);
				Debug.Log ("blah");
			}
		} else {
			input = Vector2.zero;
		}
	}

}
