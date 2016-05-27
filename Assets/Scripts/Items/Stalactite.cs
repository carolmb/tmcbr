using UnityEngine;
using System.Collections;

public class Stalactite : MonoBehaviour {

	public int damage = 1;
	public float lifeTime = 50;
	Vector3 moveVector;
	// Use this for initialization
	void Start () {
		moveVector = new Vector3(0, -1, 0);
		Invoke ("Destroy", lifeTime);
	}
	
	// Update is called once per frame
	void Update () {
		Tile t = MazeManager.GetTile ((Vector2)transform.position - new Vector2 (0, Tile.size)); 
		if (t == null || !t.isWalkable || lifeTime <= 0) {
			Destroy (gameObject);
		} else {
			Vector3 pos = transform.position;
			pos.x += moveVector.x * 60 * Time.deltaTime + Random.Range(-2, 3);
			pos.y += moveVector.y * 60 * Time.deltaTime;
			pos.z += moveVector.y * 60 * Time.deltaTime;
			transform.position = pos;
		}
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.CompareTag ("Player")) {
			Character comp = collider.GetComponent<Character> ();
			comp.Damage ((Vector2) transform.position - (Vector2)moveVector * 10, damage);
			Destroy (gameObject);
		}
	}

}
