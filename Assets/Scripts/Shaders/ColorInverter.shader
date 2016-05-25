Shader "Hidden/ColorInverter"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_angle ("Angle", float) = 0
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			float _angle;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 tex = tex2D(_MainTex, i.uv);
				return fixed4(
				tex.r - _angle*(2*tex.r - tex.g - tex.b),
				tex.g - _angle*(2*tex.g - tex.r - tex.b),
				tex.b - _angle*(2*tex.b - tex.r - tex.g), tex.a);
			}
			ENDCG
		}
	}
}
