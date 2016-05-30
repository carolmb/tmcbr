using UnityEngine;
using System.Collections;

public class Bush : MonoBehaviour {
	public GameObject[] fruit;

	static int currentNumber = 0;
	Vector3 iniPos;

	// Use this for initialization
	void Start () {
		iniPos = transform.position;
	}

	public void Cut(){
		if (currentNumber < 6) {
			GameObject f = Instantiate (fruit[currentNumber]) as GameObject;
			f.transform.position = iniPos;
			f.GetComponent<FruitObject> ().number = currentNumber;
			currentNumber++;
		} else {
			int randNumber = Random.Range (0, 6);
			GameObject f = Instantiate (fruit[randNumber]) as GameObject;
			f.transform.position = iniPos;
			f.GetComponent<FruitObject> ().number = randNumber;
		}
		Vector3 pos = MazeManager.WorldToTilePos (transform.position);
		MazeManager.maze.tiles [(int)pos.x, (int)pos.y].obstacle = "";
		Destroy (gameObject);
	}

}
