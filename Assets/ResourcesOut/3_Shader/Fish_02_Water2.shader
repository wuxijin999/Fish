// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Environment/Water_2" {

	Properties
	{
		_MainCol("_MainCol", Color) = (.5,.5,.5,1)
		_MainTex("_MainTex", 2D) = "black" {}
		_MaskTex("_MaskTex", 2D) = "black" {}

		_Speed("_Speed", Vector) = (0,6,0,6)
		_Disturbance("_Disturbance", Range(0,5)) = 0.6
	}

		SubShader
	{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "PreviewType" = "Plane" }
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask RGB
		Cull Off Lighting Off ZWrite Off

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma target 2.0
#pragma multi_compile_particles
#pragma multi_compile_fog
#include "UnityCG.cginc"
		float4 _MainCol;
	sampler2D _MainTex;
	sampler2D _MaskTex;
	sampler2D _WhiteWaterTex;
	struct appdata_t
	{
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
	};

	struct v2f
	{
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
		float2 texcoordM : TEXCOORD1;
		UNITY_FOG_COORDS(2)
	};

	float4 _MainTex_ST;
	float4 _MaskTex_ST;
	float4 _Speed;
	float _Disturbance;
	v2f vert(appdata_t v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
		o.texcoordM = TRANSFORM_TEX(v.texcoord, _MaskTex);

		UNITY_TRANSFER_FOG(o,o.vertex);
		return o;
	}

	fixed4 frag(v2f i) : COLOR
	{
		half2 distortOffset = i.texcoord + (_Time.zw * _Speed.xy * 0.05 + _Time.wz * _Speed.zw * 0.1) * _Disturbance;
		half4 col = tex2D(_MainTex, distortOffset);
		float2 uv = i.texcoord + _Speed * _Time.x + col.rg * _Disturbance;
		half4 mask = tex2D(_MaskTex, i.texcoordM);

		col = tex2D(_MainTex,uv)* _MainCol;

		col.a = mask.r * _MainCol.a;
		UNITY_APPLY_FOG(i.fogCoord, col);
		return col;
	}
		ENDCG
	}

	}
}
