using UnityEngine;
using System.Collections;

public class Bush : MonoBehaviour {
	public GameObject[] fruit;

	static int currentNumber = 0;
	Vector2 iniPos;

	// Use this for initialization
	void Start () {
		iniPos = transform.position;
	}

	public void Cut(){
		if (currentNumber < 6) {
			GameObject f = Instantiate (fruit[currentNumber]) as GameObject;
			f.GetComponent<FruitObject> ().number = currentNumber;
			currentNumber++;
			f.transform.position = iniPos;
		}
		Destroy (gameObject);
	}

}
