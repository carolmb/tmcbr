Shader "Hidden/FadeOut" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_intensity ("Intensity", float) = 0
	}
	SubShader {
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			 
			struct appdata_t {
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};
	 
			struct v2f {
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float2 screenpos : TEXCOORD1;
			};
			 
			sampler2D _MainTex;
			float _intensity;
			 
			// vertex shader
			v2f vert(appdata_t IN) {
				v2f OUT;
				OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color;
				OUT.screenpos = ComputeScreenPos(OUT.vertex);
				 
				return OUT;
			}
			 
			// fragment shader
			fixed4 frag(v2f IN) : COLOR {
				fixed4 tex = tex2D(_MainTex, IN.texcoord) * IN.color;

				tex.r *= 1 - _intensity;
				tex.g *= 1 - _intensity;
				tex.b *= 1 - _intensity;

				return tex;
			}
			ENDCG
		}
	}
}