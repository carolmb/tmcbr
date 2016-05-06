using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;

public class GameManager {

	// ===============================================================================
	// Inputs
	// ===============================================================================

	public static bool InteractInput() {
		#if UNITY_ANDROID
			return TouchInput();
		#else
			return DefaultInput();
		#endif
	}

	private static bool TouchInput() {
		if (EventSystem.current.IsPointerOverGameObject (0))
			return false;
		return Input.GetTouch (0).phase == TouchPhase.Began;
	}

	private static bool DefaultInput() {
		if (EventSystem.current.IsPointerOverGameObject (-1))
			return false;
		return Input.GetMouseButtonUp (0);
	}

	public static Vector2 InputPosition() {
		#if UNITY_ANDROID
			return TouchPoint();
		#else
			return MousePoint();
		#endif
	}

	private static Vector2 TouchPoint() {
		return Input.GetTouch (0).position;
	}

	private static Vector2 MousePoint() {
		return Input.mousePosition;
	}

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
