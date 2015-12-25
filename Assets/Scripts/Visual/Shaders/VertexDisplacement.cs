using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Color Adjustments/Color Correction (Ramp)")]
	public class VertexDisplacement : ImageEffectBase {
		public float DisplacementX = 1;
		public float DisplacementY = 1;
		
		// Called by camera to apply image effect
		void OnRenderImage (RenderTexture source, RenderTexture destination) {
			material.SetTexture ("_MainTex", source);
			material.SetFloat("_DisplaceX", DisplacementX);
			material.SetFloat ("_DisplaceY", DisplacementY);
			Graphics.Blit (source, destination, material);
		}
	}
}
