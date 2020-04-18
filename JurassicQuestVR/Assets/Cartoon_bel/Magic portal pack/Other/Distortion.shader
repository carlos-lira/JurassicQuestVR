// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Distortion" {
Properties {
        [HDR]_TintColor ("Tint Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB) Gloss (A)", 2D) = "black" {}
        _BumpMap ("Normalmap", 2D) = "bump" {}
		_Mask("Mask", 2D) = "white" {}
		_BumpAmt ("Distortion", Float) = 100
		_Clamp("Clamp", Float) = 10
}

Category {

	Tags { "Queue"="Transparent"  "IgnoreProjector"="True"  "RenderType"="Transparent" }
	Blend SrcAlpha OneMinusSrcAlpha
	Cull Off
			Zwrite Off
	Fog { Mode Off}

	SubShader {
		GrabPass {							
 		}
		Pass {
			
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

struct appdata_t {
	float4 vertex : POSITION;
	float2 texcoord: TEXCOORD0;
	float2 texcoordMask: TEXCOORD1;
	fixed4 color : COLOR;
};

struct v2f {
	float4 vertex : POSITION;
	float4 uvgrab : TEXCOORD0;
	float2 uvbump : TEXCOORD1;
	float2 uvmain : TEXCOORD2;
	float2 uvMask : TEXCOORD3;
	fixed4 color : COLOR;
};

sampler2D _MainTex;
sampler2D _BumpMap;
sampler2D _Mask;

float _BumpAmt;
float _Clamp;

sampler2D _GrabTexture;
float4 _GrabTexture_TexelSize;
fixed4 _TintColor;

float4  _Mask_ST;
float4 _BumpMap_ST;
float4 _MainTex_ST;

v2f vert (appdata_t v)
{
	v2f o;
	o.vertex = UnityObjectToClipPos(v.vertex);
	o.color = v.color;
	#if UNITY_UV_STARTS_AT_TOP
	float scale = -1.0;
	#else
	float scale = 1.0;
	#endif
	o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y*scale) + o.vertex.w) * 0.5;
	o.uvgrab.zw = o.vertex.w;
#if UNITY_SINGLE_PASS_STEREO
	o.uvgrab.xy = TransformStereoScreenSpaceTex(o.uvgrab.xy, o.uvgrab.w);
#endif
	o.uvgrab.z /= distance(_WorldSpaceCameraPos, mul(unity_ObjectToWorld, v.vertex));
	o.uvbump = TRANSFORM_TEX( v.texcoord, _BumpMap );
	o.uvmain = TRANSFORM_TEX( v.texcoord, _MainTex );
	
	o.uvMask = (v.vertex.xz) * _Mask_ST.xy + _Mask_ST.zw;
	return o;
}

half4 frag( v2f i ) : COLOR
{
	fixed mask = tex2D(_Mask, i.uvMask).r;
	half2 bump = UnpackNormal(tex2D( _BumpMap, i.uvbump )).rg;
	float2 offset = bump * _BumpAmt * _GrabTexture_TexelSize.xy * i.color.a * mask;
	i.uvgrab.xy = offset * i.uvgrab.z + i.uvgrab.xy;
	
	half4 col = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD(i.uvgrab));
	fixed4 tex = tex2D(_MainTex, i.uvmain + offset * .1) * i.color * mask;
	
	
	fixed4 emission = col * i.color + tex  * _TintColor* i.color.a;
    emission.a = _TintColor.a;
	half alphaBump = saturate((abs(bump.r + bump.g) - 0.01) * 10);
	if (_Clamp > 0) {
		clip(step(0.5, alphaBump) - 0.1);
	}
	return emission;
}
ENDCG
		}
	}

	FallBack "Effects/Distortion/Free/CullOff"

}

}

