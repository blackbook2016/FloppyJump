Shader "Custom/PeriodicHighlight" {
	Properties 
	{
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}

		[MaterialToggle] _AlphaSaturate ("Alpha Saturate", Float) = 0 
		_UseSaturation("Use Saturation", Range(0,1)) = 0 
		_HighLight ("HighLight ", Range(0,1)) = 1.0

		_SaturationColor ("Saturation Color", Color) = (1,1,1,1)
		_HighLightColor ("HighLight Color", Color) = (1,1,1,1)

		_Speed ("Speed", Float) = 1.0


	}
	SubShader 
	{
		Tags 
		{ 
			"Queue"="AlphaTest"
			"RenderType"="TransparentCutout" 
			"IgnoreProjector"="True"
			"DisableBatching" = "True"

		}

		Pass
		{
			Lighting Off
			//ZTest LEqual
			//ZWrite Off
			//Cull Back
			AlphaToMask On
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			#define PI 3.1416

			sampler2D _MainTex;
			float4 _MainTex_ST;

			fixed4 _Color;
			fixed4 _SaturationColor;
			fixed4 _HighLightColor;

			float _HighLight;
			float _Speed;
			float _UseSaturation;
			float _AlphaSaturate;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv0 : TEXCOORD0;
			};

			struct Input 
			{
				float2 uv : TEXCOORD0; 
				float4 localPos : SV_POSITION;
			};

			Input vert (appdata v)
			{
				Input o;

				o.localPos = mul(UNITY_MATRIX_MVP, v.vertex);;
				o.uv = v.uv0;

				return o;
			}

			half4 frag (Input IN) : COLOR 
			{

				//Albedo comes from a texture tinted by color
				fixed4 c = tex2D (_MainTex, IN.uv * _MainTex_ST.xy + _MainTex_ST.zw) * _Color;
				fixed4 h = _HighLightColor * _HighLight * (0.5 + 0.5*sin(_Time.y * PI * _Speed));

				float4 o;
				o.xyz = _SaturationColor * _UseSaturation + c.rgb *(1 - _UseSaturation) + h.rgb ;
				o.w =  c.a + _AlphaSaturate * h.a;

				return o;
			}

			ENDCG
		}
	}
}
