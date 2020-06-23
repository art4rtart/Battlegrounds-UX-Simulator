//https://github.com/bgolus/Normal-Mapping-for-a-Triplanar-Shader/blob/master/TriplanarSurfaceShader.shader
//Holdimprovae's Triplaner Shader
Shader "Custom/TriPlaner MappingShader" {
	Properties {
		_Color ("TintColor", Color) = (1,1,1,1)
		_FrontPlaneTex("FrontPlane", 2D) = "white" {}
		_TopPlaneTex("TopPlane", 2D) = "white" {}
		_SidePlaneTex("SidePlane", 2D) = "white" {}
		_FrontPlaneNormTex("FrontPlane Normal", 2D) = "bump" {}
		_TopPlaneNormTex("TopPlane Normal", 2D) = "bump" {}
		_SidePlaneNormTex("SidePlane Normal", 2D) = "bump" {}
		_FrontPlaneSpecTex("FrontPlane Specular", 2D) = "white" {}
		_TopPlaneSpecTex("TopPlane Specular", 2D) = "white" {}
		_SidePlaneSpecTex("SidePlane Specular", 2D) = "white" {}

		_TopTilingFactor("Top Tiling Factor", Range(0,2)) = 1.0
		_FrontTilingFactor("Front Tiling Factor", Range(0,2)) = 1.0
		_SideTilingFactor("Side Tiling Factor", Range(0,2)) = 1.0

		_BlendSharpValue("BlendSharpValue", Range(0,8)) = 0.0001

		_FrontPlaneRoughness ("FrontRoughness", 2D) = "white" {}
		_SidePlaneRoughness ("TopRoughness", 2D) = "white" {}
		_TopPlaneRoughness ("SideRoughness", 2D) = "white" {}

		_FrontPlaneMetallic ("FrontMetallic", 2D) = "white" {}
		_SidePlaneMetallic ("TopMetallic", 2D) = "white" {}
		_TopPlaneMetallic ("SideMetallic", 2D) = "white" {}

		[Toggle(DeSaturation)]_Desaturation ("Desaturation" ,Float) = 0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows 
		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0
		#pragma shader_feature DeSaturation

		#if defined(INTERNAL_DATA) && (defined(UNITY_PASS_FORWARDBASE) || defined(UNITY_PASS_FORWARDADD) || defined(UNITY_PASS_DEFERRED) || defined(UNITY_PASS_META))
			#define WorldToTangentNormalVector(data,normal) mul(normal, half3x3(data.internalSurfaceTtoW0, data.internalSurfaceTtoW1, data.internalSurfaceTtoW2))
		#else
			#define WorldToTangentNormalVector(data,normal) normal
		#endif

		sampler2D _FrontPlaneTex;
		sampler2D _TopPlaneTex;
		sampler2D _SidePlaneTex;
		sampler2D _FrontPlaneNormTex;
		sampler2D _TopPlaneNormTex;
		sampler2D _SidePlaneNormTex;
		sampler2D _FrontPlaneSpecTex;
		sampler2D _TopPlaneSpecTex;
		sampler2D _SidePlaneSpecTex;

		sampler2D _FrontPlaneRoughness;
		sampler2D _TopPlaneRoughness;
		sampler2D _SidePlaneRoughness;

		sampler2D _FrontPlaneMetallic;
		sampler2D _TopPlaneMetallic;
		sampler2D _SidePlaneMetallic;

		float4 _MainTex_ST;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		half _BlendSharpValue;
		half _Glossiness;
		half _TopTilingFactor;
		half _FrontTilingFactor;
		half _SideTilingFactor;
		fixed _SpecularSetup;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

			// 참조 :: http://blog.selfshadow.com/publications/blending-in-detail/
		half3 blend_rnm(half3 n1, half3 n2)
		{
			n1.z += 1;
			n2.xy = -n2.xy;

			return n1 * dot(n1, n2) / n1.z - n2;
		}

		//RBG Perceptual Correction desaturation
		float3 Grayscale(float3 inputColor)
		{
    		float gray = dot(inputColor.rgb, float3(0.2126, 0.7152, 0.0722 ));
    		return float3(gray, gray, gray);
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {

			float2 uv_front = IN.worldPos.xy*_FrontTilingFactor;
			float2 uv_side = IN.worldPos.zy*_SideTilingFactor;
			float2 uv_top = IN.worldPos.xz*_TopTilingFactor;
			
			fixed4 SideAlbedo = tex2D(_SidePlaneTex, uv_side);
			fixed4 FrontAlbedo = tex2D(_FrontPlaneTex, uv_front);
			fixed4 TopAlbedo = tex2D(_TopPlaneTex, uv_top);

			fixed4 SideRoughness = tex2D(_SidePlaneRoughness, uv_side);
			fixed4 FrontRoughness = tex2D(_FrontPlaneRoughness, uv_front);
			fixed4 TopRoughness = tex2D(_TopPlaneRoughness, uv_top);

			fixed4 SideMetallic = tex2D(_SidePlaneMetallic, uv_side);
			fixed4 FrontMetallic = tex2D(_FrontPlaneMetallic, uv_front);
			fixed4 TopMetallic = tex2D(_TopPlaneMetallic, uv_top);

			fixed4 SideSpecular = tex2D(_SidePlaneSpecTex, uv_side);
			fixed4 FrontSpecular = tex2D(_FrontPlaneSpecTex, uv_front);
			fixed4 TopSpecular = tex2D(_TopPlaneSpecTex, uv_top);

			IN.worldNormal = WorldNormalVector(IN, float3(0, 0, 1));
			

			half3 axisSign = IN.worldNormal < 0 ? -1 : 1;

			#if defined(TRIPLANAR_CORRECT_PROJECTED_U)
				uv_side.x *= axisSign.x;
				uv_top.x *= axisSign.y;
				uv_front.x *= -axisSign.z;
			#endif
			
			half3 absVertNormal = abs(IN.worldNormal);

			// 탄젠트 공간의 노멀값들
			fixed3 TangentSideNormal = UnpackNormal(tex2D(_SidePlaneNormTex, uv_side));
			fixed3 TangentFrontNormal = UnpackNormal(tex2D(_FrontPlaneNormTex, uv_front));
			fixed3 TangentTopNormal = UnpackNormal(tex2D(_TopPlaneNormTex, uv_top));

		#if defined(TRIPLANAR_CORRECT_PROJECTED_U)
			TangentSideNormal.x *= axisSign.x;
			TangentTopNormal.x *= axisSign.y;
			TangentFrontNormal.x *= -axisSign.z;
		#endif

			TangentSideNormal = blend_rnm(half3(IN.worldNormal.zy, absVertNormal.x), TangentSideNormal);
			TangentTopNormal = blend_rnm(half3(IN.worldNormal.xz, absVertNormal.y), TangentTopNormal);
			TangentFrontNormal = blend_rnm(half3(IN.worldNormal.xy, absVertNormal.z), TangentFrontNormal);

			TangentSideNormal.z *= axisSign.x;
			TangentTopNormal.z *= axisSign.y;
			TangentFrontNormal.z *= axisSign.z;

			float3 weights = saturate(pow(IN.worldNormal, _BlendSharpValue));
			weights /= max(dot(weights, half3(1, 1, 1)), 0.0001);	

			FrontAlbedo *= weights.z;
			TopAlbedo *= weights.y;
			SideAlbedo *= weights.x;

			FrontSpecular *= weights.z;
			TopSpecular *= weights.y;
			SideSpecular *= weights.x;

			FrontMetallic *= weights.z;
			TopMetallic *= weights.y;
			SideMetallic *= weights.x;

			FrontRoughness *= weights.z;
			TopRoughness *= weights.y;
			SideRoughness *= weights.x;

			half3 worldNormal = normalize(
				TangentSideNormal.zyx * weights.x +
				TangentTopNormal.xzy * weights.y +
				TangentFrontNormal.xyz * weights.z
			);

			// Albedo comes from a texture tinted by color
			fixed4 c = FrontAlbedo + SideAlbedo + TopAlbedo;
			c *= _Color;
			o.Albedo = c.rgb;
			#ifdef DeSaturation
			o.Albedo = Grayscale(c.rgb);

			#endif

			o.Albedo *= _Color;

			// Metallic and smoothness come from slider variables
			o.Smoothness = 1-(FrontRoughness + TopRoughness + SideRoughness).xyz;
			o.Metallic = (FrontMetallic + TopMetallic + SideMetallic).xyz;
			o.Normal = WorldToTangentNormalVector(IN, worldNormal);
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
