Shader "Character/Normal,Spec"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white" {}
	}

	Subshader
	{

		Tags {
			"Queue" = "Geometry"
			"IgnoreProjector" = "True"
			"LightMode" = "ForwardBase"
		}

		Pass
		{
		Cull Back
		Fog {Mode Off}

		CGPROGRAM
		#pragma target 3.0
		#pragma vertex vert
		#pragma fragment frag

		#pragma multi_compile_fwdbase
		#pragma exclude_renderers flash 

		#include "UnityCG.cginc"
		#include "FishCG.cginc"

		struct vertexInput
		{
			half4 vertex : POSITION;
			half3 normal : NORMAL;
			half4 tangent : TANGENT;
			half2 texcoord	: TEXCOORD0;
		};

		struct v2f
		{
			half4 pos : SV_POSITION;
			half2 uv0 : TEXCOORD0;
		};

		uniform sampler2D _MainTex;

		v2f vert(vertexInput v)
		{
			v2f o;
			o.pos = v.vertex;
			o.uv0 = v.texcoord;

			return o;
		}

		fixed4 frag(v2f i) : SV_Target{
			half4 color;
			return color;
		}

		ENDCG
		}
	}

	FallBack "Diffuse"
}