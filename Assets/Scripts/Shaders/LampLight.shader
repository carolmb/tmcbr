Shader "Hidden/LampLight" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_playerX ("Player X", float) = 320
		_playerY ("Player Y", float) = 240
		_radius("Radius", float) = 100
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
			float _radius;
			float _playerX;
			float _playerY;
			 
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
				float2 worldpos = IN.screenpos * _ScreenParams.xy;
				float2 playerpos;
				playerpos.x = _playerX;
				playerpos.y = _playerY;
				float bla = 1 - distance(worldpos, playerpos) / _radius;
				tex.r *= bla;
				tex.g *= bla;
				tex.b *= bla;
				return tex;
			}
			ENDCG
		}
	}
}