Shader "Displace/Vertex Displace" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_DisplaceX ("Displace Amount (Right)", Float) = 1
	_DisplaceY ("Displace Amount (Up)", Float) = 1
}

SubShader
{
	Pass
	{
		ZTest Always Cull Off ZWrite Off

CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

uniform sampler2D _MainTex;

uniform float _DisplaceX;
uniform float _DisplaceY;

struct v2f {
	float4 pos : SV_POSITION;
	float2 uv : TEXCOORD0;
	float2 uvOrig : TEXCOORD1;
};

v2f vert (appdata_img v)
{
	
	v2f o;
	o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
	o.uv = v.texcoord.xy;
	return o;
}

float4 frag (v2f i) : SV_Target
{
	return tex2D(_MainTex, i.uv);
}
ENDCG

	}
}

Fallback off

}
