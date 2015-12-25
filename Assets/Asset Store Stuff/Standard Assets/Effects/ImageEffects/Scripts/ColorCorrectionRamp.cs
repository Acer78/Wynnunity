using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
    [ExecuteInEditMode]
    [AddComponentMenu("Image Effects/Color Adjustments/Color Correction (Ramp)")]
    public class ColorCorrectionRamp : ImageEffectBase {
        public Texture  textureRamp;
		public float Intensity = 1;

        // Called by camera to apply image effect
        void OnRenderImage (RenderTexture source, RenderTexture destination) {
            material.SetTexture ("_RampTex", textureRamp);
			material.SetFloat ("_Intensity", Intensity);
            Graphics.Blit (source, destination, material);
        }
    }
}
