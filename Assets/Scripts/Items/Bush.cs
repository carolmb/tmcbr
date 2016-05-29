using UnityEngine;
using System.Collections;

public class Bush : MonoBehaviour {
	public GameObject fruit;

	static int currentNumber = 0;
	Vector2 iniPos;

	// Use this for initialization
	void Start () {
		iniPos = transform.position;
	}

	void OnDestroy() {
		GameObject f = Instantiate (fruit) as GameObject;
		f.GetComponent<SpriteRenderer> ().sprite = Resources.Load ("Images/fruits_" + currentNumber, typeof(Sprite)) as Sprite;
		f.GetComponent<FruitObject> ().number = currentNumber;
		currentNumber++;
		f.transform.position = iniPos;
	}
}
