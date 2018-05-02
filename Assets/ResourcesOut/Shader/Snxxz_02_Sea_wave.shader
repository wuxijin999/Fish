
Shader "Environment/Sea_wave" {

	Properties{
		_BaseTex("BaseTex", 2D) = "white" {}
		_MaskTex("MaskTex", 2D) = "white" {}
		_Speed("Speed", Float) = 0.2
		_Color("Color", Color) = (0.5,0.5,0.5,1)
		_MaskTex_pannerX("MaskTex_pannerX", Float) = 0.01
		_MaskTex_pannerY("MaskTex_pannerY", Float) = -0.01
	}

		SubShader{

			Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "PreviewType" = "Plane" }

			Pass {
				Tags {
					"LightMode" = "ForwardBase"
				}

				Blend One One
				Cull Off
				ZWrite Off

				CGPROGRAM
				#include "UnityCG.cginc"
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 3.0
				#pragma multi_compile_particles
				#pragma multi_compile_fog

				uniform float4 _TimeEditor;
				uniform sampler2D _BaseTex; uniform float4 _BaseTex_ST;
				uniform sampler2D _MaskTex; uniform float4 _MaskTex_ST;
				uniform float _Speed;
				uniform float4 _Color;
				uniform float _MaskTex_pannerX;
				uniform float _MaskTex_pannerY;
				struct VertexInput {
					float4 vertex : POSITION;
					float2 texcoord0 : TEXCOORD0;
					float4 vertexColor : COLOR;
				};

				struct VertexOutput {
					float4 pos : SV_POSITION;
					float2 uv0 : TEXCOORD0;
					float4 vertexColor : COLOR;
				};

				VertexOutput vert(VertexInput v) {
					VertexOutput o = (VertexOutput)0;
					o.uv0 = v.texcoord0;
					o.vertexColor = v.vertexColor;
					o.pos = UnityObjectToClipPos(v.vertex);
					return o;
				}

				float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
					float isFrontFace = step(0,facing);
					float faceSign = sign(facing);
					float4 node_9577 = _Time + _TimeEditor;
					float2 node_7241 = (float2(0.0,(-1 * (node_9577.g*_Speed))) + i.uv0);
					float4 _BaseTex_var = tex2D(_BaseTex,TRANSFORM_TEX(node_7241, _BaseTex));
					float2 node_9405 = (i.uv0 + (float2(_MaskTex_pannerX,_MaskTex_pannerY)*node_9577.g));
					float4 _MaskTex_var = tex2D(_MaskTex,TRANSFORM_TEX(node_9405, _MaskTex));
					float3 emissive = (_BaseTex_var.rgb*_MaskTex_var.r*i.vertexColor.a*_Color.rgb*_Color.a);
					float3 finalColor = emissive;
					return fixed4(finalColor,1);
				}
				ENDCG
			}
		}
			FallBack "Diffuse"
}
