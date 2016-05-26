Shader "Hidden/LampLight" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_radius("Radius", float) = 100
		_playerX ("Player X", float) = 320
		_playerY ("Player Y", float) = 240
		_screenW ("Screen Width", float) = 640
		_screenH ("Screen Height", float) = 480
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
			float _screenW;
			float _screenH;
			 
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
				float2 worldPos = IN.screenpos * _ScreenParams.xy;

				float2 playerPos = float2(_playerX, _playerY);
				float2 screenSize = float2(_screenW, _screenH);
				_radius *= _ScreenParams.xy;

				float a = 1 - (distance(worldPos, playerPos) / _radius);
				tex.r *= a;
				tex.g *= a;
				tex.b *= a;

				return tex;
			}
			ENDCG
		}
	}
}