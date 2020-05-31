Shader "Custom/her0in/Decal/Add" {
	Properties{
		[Header(Decal)]
		_DecalTex("Decal Texture", 2D) = "gray" {}
		_GardientTex("Gardient Texture", 2D) = "white" {}

		[HideInInspector]_MainTex("Main Texture", 2D) = "white" {}
		[Header(Dissolve)]
		_NoiseTex("Noise Texture", 2D) = "white" {}
		_DissolvedTint("Dissolved Edge Color", Color) = (1,0,0,1)
		_Dissolved("Dissolve Rate", Range(0,1)) = 0.0
		_DissolvedSmooth("Dissolve Edge Smooth", Range(0, 1)) = 0.15
	}
		Subshader{
			Tags {"Queue" = "Transparent"}
			Pass {
				ZWrite Off
				ColorMask RGB
				Blend DstColor Zero
				Offset -1, -1

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 3.0
				#include "UnityCG.cginc"

				float4x4 unity_Projector;
				float4x4 unity_ProjectorClip;

				sampler2D _DecalTex;
				sampler2D _GardientTex;

				sampler2D _MainTex;

				sampler2D _NoiseTex;
				fixed4 _DissolvedTint;

				half _Dissolved;
				half _DissolvedSmooth;

				struct appdata
				{
					float4 vertex : POSITION;
				};

				struct v2f {
					float4 vertex : SV_POSITION;
					float4 duv : TEXCOORD0;
					float4 guv : TEXCOORD1;
				};

				float value;

				v2f vert(appdata v)
				{
					v2f o;

					value = _Time.x;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.duv = mul(unity_Projector, v.vertex);
					o.guv = mul(unity_ProjectorClip, v.vertex);

					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 decal = tex2Dproj(_DecalTex, UNITY_PROJ_COORD(i.duv));
					decal.a = 1.0 - decal.a;

					fixed4 gradient = tex2Dproj(_GardientTex, UNITY_PROJ_COORD(i.guv));
					fixed4 finalColor = lerp(fixed4(1,1,1,1), decal, gradient.a);

					// Compute texture disolved
					fixed4 c = tex2D(_MainTex, i.duv);
					float noise = 1 - tex2D(_NoiseTex, i.duv).r;
					float dissolve = noise - _Dissolved * 1.01;

					clip(dissolve);

					float dissolvedBorder = _DissolvedSmooth * saturate(_Dissolved * 10);

					if (dissolve < dissolvedBorder) {
						float cp = saturate(dissolve / dissolvedBorder);
						float bp = 1 - cp;

						c = (c * cp) + (_DissolvedTint * bp);
					}

					return finalColor * c;
				}
				ENDCG
			}
	}
}
