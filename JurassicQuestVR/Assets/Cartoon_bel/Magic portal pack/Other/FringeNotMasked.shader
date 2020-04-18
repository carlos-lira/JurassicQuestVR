Shader "Custom/FringeNotMasked" {
Properties {
	[HDR]_TintColor ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_Mask ("Turbulence Mask", 2D) = "white" {}
	_TimeVec ("Time (xy)", Vector) = (1, 1, 1, 1)

}




SubShader {
Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend SrcAlpha OneMinusSrcAlpha
	ZWrite Off
	Cull Off

		Pass {
			ZWrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
#pragma target 3.0
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _Mask;
			half4 _TintColor;
			half _ColorStrength;
			float4 _TimeVec;
			
			struct appdata_t {
				float4 vertex : POSITION;
				half4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				half4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;
			};
			
			float4 _MainTex_ST;
			float4 _Mask_ST;

			v2f vert (appdata_t v)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
				o.texcoord1 = TRANSFORM_TEX(v.texcoord, _Mask);
				return o;
			}

			half4 frag (v2f i) : SV_Target
			{
				half4 texDef = tex2D(_MainTex, i.texcoord);
				half4 tex1 = tex2D(_MainTex, i.texcoord + _Time.xx * _TimeVec.xy);
				half4 tex2 = tex2D(_MainTex, i.texcoord + _Time.xx * _TimeVec.xy + float2(0, 0.5));
				half mask = tex2D(_Mask, i.texcoord1);

				half3 res = _TintColor.rgb * lerp(texDef.rgb, tex1.rgb*tex1.a + tex2.rgb*tex2.a, _TimeVec.w);
				half alphaRes = saturate(texDef.a*texDef.a * mask);
				return half4(res, saturate(alphaRes*_TintColor.a));
			}
			ENDCG 
		}
	}	
}

