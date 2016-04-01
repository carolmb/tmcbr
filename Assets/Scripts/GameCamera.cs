using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {

	public Transform player;

	void LateUpdate() {
		// TODO: seguir o player considerando os limites do mapa
		Vector3 newPos = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
		gameObject.transform.position = newPos;
	}
}
