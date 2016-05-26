using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.ImageEffects {
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Fade Effect")]
	public class FadeEffect : ImageEffectBase {

		public Transform player;

		public static float intensity = 0;

		// Called by camera to apply image effect
		void OnRenderImage (RenderTexture source, RenderTexture destination) {
			material.SetFloat ("_intensity", intensity);
			Graphics.Blit (source, destination, material);
		}
	}

}

