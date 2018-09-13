Shader "Test/Specual"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white" {}
		_Glow("Glow",Float)=1
	}

	Subshader
	{
		LOD 200
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
			half light:TEXCOORD1;
		};

		uniform sampler2D _MainTex;
 		uniform half	_Glow;
		v2f vert(vertexInput v)
		{
			v2f o;
			o.pos =UnityObjectToClipPos( v.vertex);
			o.uv0 = v.texcoord;
			
			float3 normal=normalize( v.normal);
			
			float3 lightDir=normalize( ObjSpaceLightDir(v.vertex));

			o.light=   pow(  saturate(dot(normal,lightDir)) ,_Glow);

			return o;
		}

		fixed4 frag(v2f i) : SV_Target{
			half4 color=tex2D(_MainTex,i.uv0);

			color+=float4(1,1,1,1)*i.light;
			return color;
		}

		ENDCG
		}
	}

	FallBack "Diffuse"
}