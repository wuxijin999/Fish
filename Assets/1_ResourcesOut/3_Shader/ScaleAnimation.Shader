Shader "ScaleAnimation/"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white" {}
		_Color("Color" ,Color)=(1,1,1,1)
		_Vector("Rotation",Vector)=(0,0,0,1)
	}

	Subshader
	{
		LOD 200
		Tags {
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"LightMode" = "ForwardBase"
		}

		Pass
		{

		Cull Back
		Fog {Mode Off}

		Blend Srcalpha OneMinusSrcalpha

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
			half4 color : COLOR;
		};

		uniform sampler2D _MainTex;
		half4 _Color;
		float4 _Vector;
		
		v2f vert(vertexInput v)
		{
			v2f o;

			//v.vertex.xyz=RotationZ(v.vertex.xyz,-180*_Time.y);
			o.pos = UnityObjectToClipPos(v.vertex);
			//o.uv0 = RotationUV( v.texcoord,-180*_Time.y);
			o.uv0=v.texcoord;
			o.color=_Color;

			return o;
		}

		fixed4 frag(v2f i) : SV_Target{

			float2 uv=RotationUV(i.uv0,_Time.y*180);

			fixed4 color = tex2D(_MainTex,uv);

			if(pow(uv.x-0.5,2)+pow(uv.y-0.5,2)>0.25){
				color.a=0;
			}

			color.xyz=_Color.xyz+((sin(_Time.y*10)+1.5)*0.5).xxx;

			return color;
		}

		ENDCG
		}
	}

	FallBack "Diffuse"
}