﻿using UnityEngine;
using System.Collections;

public class Gate : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Vector3 pos = transform.position;
		pos.x += Tile.size / 2;
		pos.y -= Tile.size / 2;
		pos.z = pos.y - 1;
		transform.position = pos;
	}

	//TODO: animaçao de abrir quando o player se aproximar, parecida com o do espelho

}
