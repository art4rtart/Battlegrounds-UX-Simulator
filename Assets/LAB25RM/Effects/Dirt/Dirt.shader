Shader "Custom/her0n/Effect/Dirt"
{
    Properties
    {
		[Header(Main)]
		_MainColor("Main Color", Color) = (1,1,1,1)
		_MainTex("Albedo", 2D) = "white" {}

		_BumpMap("Normal", 2D) = "bump" {}
		_BumpDepth("Bump Depth", Range(-2.0, 2.0)) = 0.5

		[Header(Dirt)]
		_DirtColor("Dirt Color", Color) = (0,0,0,0)
		[normal] _DirtTex("Dirt (Normalmap)", 2D) = "white" {}
		_Dirtiness("Dirt Rate", Range(0, 1)) = 0.0
		_Wetness("Wetness", Range(0.00, 1)) = 0.078125
		_Transparency("Transparency", Range(0, 1)) = 0.0

		[Header(Light)]
		_SpecColor("Specular Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_Shininess("Shininess", Range(0.1, 20.0)) = 10

		_RimColor("Rim Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_RimPower("Rim Power", Range(0.1, 10.0)) = 3.0
    }

    SubShader
    {
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "LightMode" = "ForwardBase" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

			float4 _MainColor;
			sampler2D _MainTex;
			float4 _MainTex_ST;

			sampler2D _DirtTex;
			sampler2D _BumpMap;

			float _Dirtiness;
			float _BumpDepth;
			float _Transparency;
			float3 _DirtColor;
			float _Wetness;

			uniform float4 _SpecColor;
			uniform float _Shininess;

			uniform float4 _RimColor;
			uniform float _RimPower;

			uniform float4 _LightColor0;

			struct appdata {
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

			float4 lightColor(v2f i, float3 dirtColor)
			{
				float3 viewDir = normalize(UnityWorldSpaceViewDir(i.worldVertex.xyz));
				float3 lightDir;
				float atten;

				if (_WorldSpaceLightPos0.w == 0.0) {
					atten = 1.0;
					lightDir = normalize(_WorldSpaceLightPos0.xyz);
				}
				else {									
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
				
				float3 wetness = diffuse * dirtColor.rgb * pow(saturate(dot(reflect(-lightDir, normalDir), viewDir)), _Shininess * (1 - _Wetness));

				float3 lightFinal = diffuse + specular + rimLighting + UNITY_LIGHTMODEL_AMBIENT.rgb + wetness;

				return float4(lightFinal * _MainColor.xyz, 1.0);
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 mainTex = tex2D(_MainTex, i.uv);
				fixed4 dirtTex = tex2D(_DirtTex, i.uv);

				float4 lightCol = lightColor(i, dirtTex.rgb);

				if (dirtTex.a > _Dirtiness)
				{
					return mainTex * lightCol;
				}
				else
				{
					float3 dirtColor = lerp(_DirtColor, mainTex, _Transparency);
					return mainTex * float4(dirtColor, 1) * lightCol;
				}
            }
            ENDCG
        }
    }
}
