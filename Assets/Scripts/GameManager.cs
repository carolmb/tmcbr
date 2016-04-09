using UnityEngine;
using System.Collections;



public static class GameManager {
	
	// ===============================================================================
	// Bagulho matemático
	// ===============================================================================

	public static float VectorToAngle(Vector2 direction) {
		return VectorToAngle (direction.x, direction.y);
	}

	public static float VectorToAngle(float dx, float dy) {
		float angle = Mathf.Atan2 (dy, dx) * Mathf.Rad2Deg;
		return angle;
	}

	public static Vector3 AngleToVector(float angle) {
		angle *= Mathf.Deg2Rad;
		return new Vector3 (Mathf.Cos(angle), Mathf.Sin(angle), 0);
	}

}
