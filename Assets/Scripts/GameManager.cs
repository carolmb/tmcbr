using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;

public class GameManager {

	// ===============================================================================
	// Interaction Inputs
	// ===============================================================================

	public static bool ClickInteractInput () {
	//	#if UNITY_ANDROID
	//		return TouchInteractInput();
	//	#else
			return MouseInteractInput();
	//	#endif
	}
		
	private static bool TouchInteractInput () {
		return !EventSystem.current.IsPointerOverGameObject (0) &&
		Input.GetTouch (0).phase == TouchPhase.Began;
	}

	private static bool MouseInteractInput () {
		if (EventSystem.current.IsPointerOverGameObject (-1))
			return false;
		return Input.GetMouseButtonUp (0);
	}

	public static bool KeyBoardInteractInput () {
		if (EventSystem.current.currentSelectedGameObject != null)
			return false;
		return Input.GetButtonDown("Submit");
	}

	public static Vector2 InteractPosition () {
		Vector2 point = SubmitPosition ();
		return Camera.main.ScreenToWorldPoint (point);
	}

	// ===============================================================================
	// Other Inputs
	// ===============================================================================

	public static bool SubmitInput () {
	//	#if UNITY_ANDROID
	//		return SubmitTouchInput ();
	//	#else
			return SubmitMouseInput ();
	//	#endif
	}

	private static bool SubmitMouseInput () {
		return Input.GetMouseButton (0);
	}

	private static bool SubmitTouchInput () {
		return Input.GetTouch (0).phase == TouchPhase.Moved || Input.GetTouch (0).phase == TouchPhase.Stationary;
	}

	public static Vector2 SubmitPosition () {
	//	#if UNITY_ANDROID
	//		return TouchPoint ();
	//	#else
			return MousePoint ();
	//	#endif
	}

	private static Vector2 TouchPoint () {
		return Input.GetTouch (0).position;
	}

	private static Vector2 MousePoint () {
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
