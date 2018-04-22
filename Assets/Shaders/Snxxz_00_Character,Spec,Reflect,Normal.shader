
Shader "Character/Normal ,Spec,Reflect"
{
	Properties
	{
		_MainTex ("Main Texture", 2D) = "white" {}
		_NormTex ("Norm Texture", 2D) = "white" {}
		_ChanTex ("Spec Texture", 2D) = "white" {}
		_CubeMap ("Cubemap", CUBE) = "white" {}
  		
  		[HideInInspector] _Hit ( "Hidden Hit", Float ) = 0
  		[HideInInspector] _BaTi ( "Hidden BaTi", Float ) = 0
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
			
			#pragma exclude_renderers flash d3d11
			
			#include "UnityCG.cginc"
			#include "FishCG.cginc"
			
			#if !defined (SHADOWS_OFF)
				#include "AutoLight.cginc"
			#endif
			
			struct vertexInput
            {
                half4 vertex : POSITION;
                half3 normal : NORMAL;
                half4 tangent : TANGENT;
                half2 texcoord	: TEXCOORD0;
            };
			
			struct vertShader
			{
				// basics
				half4 pos : SV_POSITION;
				half2 uv1 : TEXCOORD0;
				
				// frag lighting
			    half3 nrm : TEXCOORD1;
			    half3 tng : TEXCOORD2;
			    half3 bin : TEXCOORD3;
			    half3 cam : TEXCOORD4;
				half3 ref : TEXCOORD6;
				
				#if !defined (SHADOWS_OFF)
		   	    	SHADOW_COORDS(7)
				#endif

			};
			
			// assets
			uniform sampler2D _MainTex;
			uniform sampler2D _NormTex;
			uniform sampler2D _ChanTex;
			uniform samplerCUBE _CubeMap;
			
			// properties
			uniform half _Hit;
			uniform half _BaTi;
			
			vertShader vert ( vertexInput v )
			{
				vertShader o;
               	half3 worldVertex = mul ( unity_ObjectToWorld, v.vertex ).xyz;
				
				// pos and uv tex coord
				o.pos = UnityObjectToClipPos (v.vertex );
				o.uv1 = v.texcoord;
				
				// frag lighting
				o.nrm = normalize ( mul ( half4 ( v.normal, 0.0 ), unity_WorldToObject ).xyz );
				o.tng = normalize ( mul ( unity_ObjectToWorld, half4 ( v.tangent.xyz, 0.0 ) ).xyz );
            	o.bin = normalize ( cross ( o.nrm, o.tng ) * v.tangent.w );
            	o.cam = normalize ( WorldSpaceViewDir (v.vertex) );
            	o.ref = normalize ( worldVertex - _WorldSpaceCameraPos );
				
				// hit effect
				half hit = max ( 0, _Hit * pow ( 1.0 - max ( 0, dot ( v.normal, normalize ( ObjSpaceViewDir ( v.vertex ) ) ) ), 4 ) ); // hit effect
				half3 bati = _BaTi * half3(1,0,0) * sin ( _Time.z * 3 );
				
				
				#if !defined (SHADOWS_OFF)
	      			TRANSFER_SHADOW(o);
				#endif
				
				return o;
			}
			
			fixed4 frag ( vertShader i ) : SV_Target {
			
				// textures
				fixed3 outcolor = tex2D ( _MainTex, i.uv1 );
				fixed3 channels = tex2D ( _ChanTex, i.uv1 );
				fixed3 bumpmap = tex2D ( _NormTex, i.uv1 );
				
				// normals
				fixed3 normRG = fixed3(bumpmap.rg,1) - fixed3(0.5,0.5,0.5);
				fixed3x3 matrixDIR = half3x3 (i.tng, i.bin, i.nrm);
				fixed3 normDIR = normalize ( mul ( normRG, matrixDIR ) );
				
				// shadows
				#if !defined (SHADOWS_OFF)
					fixed shdlight = dot (_WorldSpaceLightPos0, normDIR ) * channels.b * min(1, SHADOW_ATTENUATION(i) + bumpmap.b);
				#else
					fixed shdlight = dot (_WorldSpaceLightPos0, normDIR ) * channels.b;
				#endif
				shdlight = smoothstep( 0.333, 0.333 + 0.666 * bumpmap.b, shdlight );

				// spec, rim
				fixed dotprod = max ( 0, dot ( i.cam, normDIR ) );
				fixed spclight = pow ( dotprod, 30 ) * channels.r;
				fixed rimlight = pow ( 1 - dotprod, 2 ) * channels.b * 0.5;
				fixed3 additive = _Gbl_Spc * spclight + _Gbl_Rim * rimlight;
				
				// reflection
				fixed3 reflDIR = reflect ( i.ref, normDIR );
				fixed3 reflight = texCUBE( _CubeMap, reflDIR );
				reflight *= 0.5 + outcolor;
				
				// composite
				outcolor = lerp ( outcolor, reflight, channels.g);
				outcolor *= 0.3 + _Gbl_Amb + _Global_LightColor * shdlight;
				outcolor *= 0.6 + additive;
				
				return fixed4 (outcolor.rgb,1);
			}
				
			ENDCG
		}
	}
	FallBack "Diffuse"
}