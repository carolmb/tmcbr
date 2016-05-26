using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects {
    [ExecuteInEditMode]
    [AddComponentMenu("Image Effects/Color Inverter")]
    public class ColorInverter : ImageEffectBase {

		public static float angle = 0;

        // Called by camera to apply image effect
        void OnRenderImage (RenderTexture source, RenderTexture destination) {
			material.SetFloat ("_angle", angle / 360);
            Graphics.Blit (source, destination, material);
        }

    }
}
