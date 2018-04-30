// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Character/Character, OutLine"
{
	Properties
	{
		_MainTex("MainTexture",2D)="white"{}
		_XRayColor("XRay Color", Color) = (1,1,1,1)
	}

		Subshader
		{
			Name "XRayPass"

			Tags{
			"Queue" = "Geometry"
			"IgnoreProjector" = "True"
			"LightMode" = "ForwardBase"
			}

			Pass
			{

			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			
			CGPROGRAM
			#include "Lighting.cginc"
			#pragma vertex vert
			#pragma fragment frag

			fixed4 _XRayColor;

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
					//v.vertex.xyz += v.normal * _Factor;
					//o.vertex = mul(UNITY_MATRIX_MVP,v.vertex);
	
					//变换到视坐标空间下，再对顶点沿法线方向进行扩展
					float4 view_vertex = mul(UNITY_MATRIX_MV,v.vertex);
					float3 view_normal = mul(UNITY_MATRIX_IT_MV,v.normal);
					view_vertex.xyz += normalize(view_normal) * 0.01; //记得normalize
					o.vertex = mul(UNITY_MATRIX_P,view_vertex);
					return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{

				return  _XRayColor+float4(1,1,1,1)*(sin(_Time.y*10)+1)*0.5;
			}

			ENDCG
		}


		 Pass
        {
            Cull Back //剔除后面
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
 
            struct v2f
            {
                float4 vertex :POSITION;
                float4 uv:TEXCOORD0;
            };
 
            sampler2D _MainTex;
 
            v2f vert(appdata_full v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }
 
            half4 frag(v2f IN) :COLOR
            {
                //return half4(1,1,1,1);
                half4 c = tex2D(_MainTex,IN.uv);
                return c;
            }
            ENDCG
        }

		}
			FallBack "Diffuse"
}