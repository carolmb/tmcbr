using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects {
    [ExecuteInEditMode]
    [AddComponentMenu("Image Effects/Twirl")]
    public class Twirl : ImageEffectBase {
		
		public static float angle = 0;
		public Vector2 radius = new Vector2(0.3F,0.3F);
        public Vector2 center = new Vector2 (0.5F, 0.5F);

        // Called by camera to apply image effect
        void OnRenderImage (RenderTexture source, RenderTexture destination) {
            ImageEffects.RenderDistortion (material, source, destination, angle, center, radius);
        }
    }

}
