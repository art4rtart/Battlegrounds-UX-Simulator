// Upgrade NOTE: replaced '_Projector' with 'unity_Projector'
// Upgrade NOTE: replaced '_ProjectorClip' with 'unity_ProjectorClip'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "her0in/Custom/GlowCircle" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_ShadowTex("Cookie", 2D) = "" {}
	}

		Subshader{
			Tags {"Queue" = "Transparent"}
			Pass {
				ZWrite Off
				ColorMask RGB
				Blend One One
				Offset -1, -1

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_fog
				#include "UnityCG.cginc"

				struct v2f {
					float4 uvShadow : TEXCOORD0;
					UNITY_FOG_COORDS(2)
					float4 pos : SV_POSITION;
				};

				float4x4 unity_Projector;
				float4x4 unity_ProjectorClip;

				v2f vert(float4 vertex : POSITION)
				{
					v2f o;
					o.pos = UnityObjectToClipPos(vertex);
					o.uvShadow = mul(unity_Projector, vertex);
					UNITY_TRANSFER_FOG(o,o.pos);
					return o;
				}

				fixed4 _Color;
				sampler2D _ShadowTex;

				fixed4 frag(v2f i) : SV_Target
				{
					float2 polar = (i.uvShadow.xy * 2) - 1.0;

					float d = length(polar);

					if (d > 1)
					{
						discard;
					}

					fixed4 texS = tex2Dproj(_ShadowTex, UNITY_PROJ_COORD(half4(d, 0, i.uvShadow.zw)));
					texS.rgb *= _Color.rgb;
					texS.a = 1.0 - texS.a;

					UNITY_APPLY_FOG_COLOR(i.fogCoord, texS, fixed4(0,0,0,0));
					return texS;
				}
				ENDCG
			}
	}
}