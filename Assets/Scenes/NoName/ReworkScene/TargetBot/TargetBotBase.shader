Shader "Custom/TargetBotBase"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _MetallicGlossiness ("Metalic Smoothness", 2D) = "white" {}
        [HDR]_EmissiveColor ("Color", Color) = (1,1,1,1)
        _EmissiveIntensity("Emissive Intensity", Range(0,10)) = 1
        _BumpMap ("Bumpmap", 2D) = "bump" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _BumpMap;
        sampler2D _MetallicGlossiness;
        float4 _EmissiveColor;
 
        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
            float3 vertColors;
        };

        void vert(inout appdata_full v, out Input o)
        {
            o.uv_MainTex = v.texcoord;
            o.uv_BumpMap = v.texcoord;
            o.vertColors = v.color.rgb;    // grab vertex colors from appdata
        }

       

        fixed4 _Color;
        half _EmissiveIntensity;
        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = tex2D (_MetallicGlossiness, IN.uv_MainTex).r;
            o.Smoothness = 1-tex2D (_MetallicGlossiness, IN.uv_MainTex).a;
            o.Alpha = c.a;
            o.Emission = _EmissiveColor * _EmissiveIntensity * IN.vertColors;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
