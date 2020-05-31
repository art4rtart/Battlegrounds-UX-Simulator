Shader "Custom/her0in/ImageEffect/Vignette"
{
	Properties
	{
		[HideInInspector]_MainTex("Main Texture", 2D) = "white" {}
		[HideInInspector]_MaskTex("Mask Texture", 2D) = "white" {}

		_MainColor("Main Color", Color) = (1, 1, 1, 1)
		_Radius("Radius", float) = 1.0

		_CenterX("CenterX", float) = 0.5
		_CenterY("CenterY", float) = 0.5

		_SizeX("SizeX", float) = 1
		_SizeY("SizeY", float) = 1

		_Hardness("Harndess", float) = 1.0
		_HitHardness("HitHardness", float) = 2
		_Invert("Invert", Range(-1.0, 1.0)) = 0
	}
		SubShader
		{
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

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = v.uv;
					return o;
				}

				sampler2D _MainTex;
				sampler2D _MaskTex;
				float4 _MainColor;

				float _Radius, _HitHardness;
				float _Hardness;
				float _CenterX;
				float _CenterY;
				float _SizeX;
				float _SizeY;
				float _Invert, _Invert2;

				float4 frag(v2f i) : SV_Target
				{
					fixed4 mainTex = tex2D(_MainTex, i.uv);
					fixed4 maskTex = tex2D(_MaskTex, i.uv) * _MainColor;

					float dist = length(float2(i.uv.x - _CenterX, i.uv.y - _CenterY) * float2(_SizeX, _SizeY));

					float circle = saturate(dist / _Radius);
					float circleAlpha = pow(circle, pow(_Hardness + _HitHardness, 2));

					float a = (_Invert > 0) ? circleAlpha * _Invert : (1 - circleAlpha) * (-_Invert);
					half4 col = (mainTex.rgb, a * mainTex.a);

					maskTex = maskTex * col + mainTex * (1 - col);

					return maskTex;
				}
				ENDCG
			}
		}
}