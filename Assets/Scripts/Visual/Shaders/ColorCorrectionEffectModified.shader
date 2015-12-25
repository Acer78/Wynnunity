Shader "Hidden/Color Correction Effect Modified" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_RampTex ("Base (RGB)", 2D) = "grayscaleRamp" {}
	_RampTexDef ("Base (RGB)", 2D) = "grayscaleRamp" {}
	_Intensity ("Intensity", Float) = 1.0
}

SubShader {
	Pass {
		ZTest Always Cull Off ZWrite Off

CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag
#include "UnityCG.cginc"

uniform sampler2D _MainTex;
uniform sampler2D _RampTex;
uniform sampler2D _RampTexDef;
uniform float _Intensity;

fixed4 frag (v2f_img i) : SV_Target
{
	fixed4 orig = tex2D(_MainTex, i.uv);
	fixed3 ramp = tex2D(_RampTex, orig.rgb);
	
	fixed rr = lerp(tex2D(_RampTexDef, orig.rr).r, tex2D(_RampTex, orig.rr).r, _Intensity);
	fixed gg = lerp(tex2D(_RampTexDef, orig.gg).g, tex2D(_RampTex, orig.gg).g, _Intensity);
	fixed bb = lerp(tex2D(_RampTexDef, orig.bb).b, tex2D(_RampTex, orig.bb).b, _Intensity);
	
//	fixed rr = tex2D(_RampTex, orig.rr).r;
//	fixed gg = tex2D(_RampTex, orig.gg).g;
//	fixed bb = tex2D(_RampTex, orig.bb).b;
	
	fixed4 color = fixed4(rr, gg, bb, orig.a);

	return color;
}
ENDCG

	}
}

Fallback off

}
