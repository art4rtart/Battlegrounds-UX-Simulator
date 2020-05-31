Shader "Custom/her0in/Decal/Multiply" {
	Properties{
		_DecalTex("Decal", 2D) = "gray" {}
		_GardientTex("GardientTex", 2D) = "white" {}
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

					return finalColor;
				}
				ENDCG
			}
	}
}
