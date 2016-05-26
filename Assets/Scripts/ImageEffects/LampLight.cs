using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.ImageEffects {
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Lamp Light")]
	public class LampLight : ImageEffectBase {

		public Transform player;

		public float radius = 100.0f;

		// Called by camera to apply image effect
		void OnRenderImage (RenderTexture source, RenderTexture destination) {
			material.SetFloat ("_radius", radius);
			Vector2 screenPos = Camera.main.WorldToScreenPoint (player.position);
			material.SetFloat ("_playerX", screenPos.x);
			material.SetFloat ("_playerY", screenPos.y);
			material.SetFloat ("_screenW", GameCamera.size.x);
			material.SetFloat ("_screenH", GameCamera.size.y);
			material.SetFloat ("_playerY", screenPos.y);
			Graphics.Blit (source, destination, material);
		}
	}
}

