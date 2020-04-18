Shader "Custom/Lava" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_MetallicGlossMap("_SpecMap (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 1
		_BumpMap("Normal", 2D) = "gray" {}
		_BumpScale("Bump Scale", Float) = 1
		
		[HDR]_EmissionColor("EmissionColor", Color) = (1,1,1,1)
		_EmissionTex("_Emission (RGB)", 2D) = "white" {}
		_LavaTex("Lava Tex (RGB)", 2D) = "black" {}
		_NoiseTex("Noise Tex (RGB)", 2D) = "black" {}
		_DistortSpeed("Distort Speed Scale (xy/zw)", Vector) = (10,5,1,1)
	

	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _MetallicGlossMap;
		sampler2D _BumpMap;
		sampler2D _EmissionTex;
		sampler2D _NoiseTex;
		sampler2D _LavaTex;
		float _BumpScale;
		float4 _DistortSpeed;
		float4 _EmissionColor;

		struct Input {
			float2 uv_MainTex;
			float2 uv_MetallicGlossMap;
			float2 uv_BumpMap;
			float2 uv_EmissionTex;
			float2 uv_NoiseTex;
			float2 uv_LavaTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;

			fixed4 noise = tex2D(_NoiseTex, IN.uv_NoiseTex);
			fixed4 lava1 = tex2D(_LavaTex, IN.uv_LavaTex + float2(_Time.x * _DistortSpeed.x, 0) + noise * _DistortSpeed.z);
			fixed4 lava2 = tex2D(_LavaTex, IN.uv_LavaTex + float2(0, _Time.x * _DistortSpeed.y) + noise * _DistortSpeed.w);

			o.Emission = tex2D(_EmissionTex, IN.uv_EmissionTex).r * lava1 * lava2 * _EmissionColor;
			// Metallic and smoothness come from slider variables
			fixed4 metal = tex2D(_MetallicGlossMap, IN.uv_MetallicGlossMap);
			o.Metallic = metal.r;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap) ) * _BumpScale;
			o.Smoothness = metal.a * _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
