﻿Shader "Custom/her0in/Dissolve" {
	Properties{
		[Header(Default)]
		_MainColor("Main Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}

		_BumpMap("Normal Texture", 2D) = "bump" {}
		_BumpDepth("Bump Depth", Range(-2.0, 2.0)) = 0.5

		[Header(Dissolve)]
		_NoiseTex("Noise", 2D) = "white" {}
		_DissolvedTint("Dissolved Edge Color", Color) = (1,0,0,1)
		_Dissolved("Dissolve Rate", Range(0,1)) = 0.0
		_DissolvedSmooth("Dissolve Edge Smooth", Range(0, 1)) = 0.15

		[Header(Light)]
		_SpecColor("Specular Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_Shininess("Shininess", Range(0.1, 20.0)) = 10

		_RimColor("Rim Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_RimPower("Rim Power", Range(0.1, 10.0)) = 3.0
	}

	SubShader{
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "LightMode" = "ForwardBase" }

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma exclude_renderers flash

			#include "UnityCG.cginc"

			fixed4 _MainColor;

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			sampler2D _BumpMap;
			float4 _BumpMap_ST;
			float _BumpDepth;

			sampler2D _NoiseTex;
			fixed4 _DissolvedTint;

			half _Dissolved;
			half _DissolvedSmooth;

			uniform float4 _SpecColor;
			uniform float _Shininess;

			uniform float4 _RimColor;
			uniform float _RimPower;

			uniform float4 _LightColor0;

			struct appdata
			{
				float4 vertex: POSITION;
				float2 uv: TEXCOORD0;
				float3 normal: NORMAL;
				float4 tangent: TANGENT;
			};

			struct v2f {
				float4 vertex: SV_POSITION;
				float2 uv: TEXCOORD0;

				float4 worldVertex: TEXCOORD1;
				float3 normal: NORMAL;

				float3 tangent: TANGENT;
				float3 binormal: TEXCOORD3;
			};

			v2f vert(appdata v) {
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				o.normal = UnityObjectToWorldNormal(v.normal);
				o.tangent = UnityObjectToWorldNormal(v.tangent);
				o.binormal = cross(o.normal, o.tangent);

				o.worldVertex = mul(unity_ObjectToWorld, v.vertex);

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float3 viewDir = normalize(UnityWorldSpaceViewDir(i.worldVertex.xyz));
				float3 lightDir;
				float atten;

				if (_WorldSpaceLightPos0.w == 0.0) { // Directional Light
					atten = 1.0;
					lightDir = normalize(_WorldSpaceLightPos0.xyz);
				}
				else {													// Point Light
					float3 fragmentToLightSource = _WorldSpaceLightPos0.xyz - i.worldVertex.xyz;
					float distance = length(fragmentToLightSource);
					float atten = 1 / distance;
					lightDir = normalize(fragmentToLightSource);
				}

				float4 tangenNormal = tex2D(_BumpMap, i.uv);

				// unpackNormal
				float3 localCoords = float3(2.0 * tangenNormal.ag - float2(1.0, 1.0), 0.0);
				localCoords.z = _BumpDepth;

				// normal transpose matrix
				float3x3 local2WorldTranspose = float3x3(i.tangent, i.binormal, i.normal);

				// calculate normal direction
				float3 normalDir = normalize(mul(localCoords, local2WorldTranspose));

				// diffuse lighting
				float3 diffuse = atten * _LightColor0.rgb * saturate(dot(normalDir, lightDir));

				// specular lighting
				float3 specular = diffuse * _SpecColor.rgb * pow(saturate(dot(reflect(-lightDir, normalDir), viewDir)), _Shininess);

				// rim lighting
				float rim = 1 - saturate(dot(viewDir, normalDir));
				float3 rimLighting = saturate(pow(rim, _RimPower) * _RimColor.rgb * diffuse);
				float3 lightFinal = diffuse + specular + rimLighting + UNITY_LIGHTMODEL_AMBIENT.rgb;


				// Compute texture disolved
				half noise = 1 - tex2D(_NoiseTex, i.uv).r;
				half dissolve = noise - _Dissolved * 1.01;

				clip(dissolve);
				fixed4 c = tex2D(_MainTex, i.uv);

				half dissolvedBorder = _DissolvedSmooth * saturate(_Dissolved * 10);
				if (dissolve < dissolvedBorder) {
					half cp = saturate(dissolve / dissolvedBorder);
					half bp = 1 - cp;

					c = (c * cp) + (_DissolvedTint * bp);
				}

				float4 finalColor = float4(c.rgb * lightFinal, 1);

				return finalColor;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}