

const float pi=3.141592;

// Global Lighting
uniform fixed3 _Global_LightColor;
uniform fixed3 _Gbl_Pnt;
uniform fixed3 _Gbl_Amb;
uniform fixed3 _Gbl_Spc;
uniform fixed3 _Gbl_Rim;
uniform fixed3 _Gbl_Wat;
uniform fixed3 _Gbl_Sfx1;

//fog
uniform fixed3 _FogColor;
uniform fixed _FogClamp;
uniform float _FogDistMin;
uniform float _FogDistMax;

			
#ifdef LIGHTMAP_ON
	// uniform sampler2D unity_Lightmap;
	// uniform half4 unity_LightmapST;
#endif

inline fixed inlineFogVert ( float3 worldVertex ) {
	#ifdef QUALITY_HGH
		float fogDistance = length ( worldVertex.xyz - _WorldSpaceCameraPos.xyz ) - _FogDistMin;
		fixed fogAmount = clamp ( fogDistance / _FogDistMax, 0, _FogClamp );
	#else
		fixed fogAmount = 0;
	#endif
	return fogAmount;
}

inline fixed3 inlineFogFrag ( fixed3 outcolor, float fogAmount ) {
	#ifdef QUALITY_HGH
		outcolor = lerp ( outcolor, _FogColor, fogAmount );
	#endif
	return outcolor;
}

#ifdef BEND_3
	uniform half3 _ObjPos0;
	uniform half3 _ObjPos1;
	uniform half3 _ObjPos2;
	uniform half _ObjRange;
	uniform half _BendDist;
#endif
#ifdef BEND_6
	uniform half3 _ObjPos0;
	uniform half3 _ObjPos1;
	uniform half3 _ObjPos2;
	uniform half3 _ObjPos3;
	uniform half3 _ObjPos4;
	uniform half3 _ObjPos5;
	uniform half _ObjRange;
	uniform half _BendDist;
#endif
#ifdef BEND_9
	uniform half3 _ObjPos0;
	uniform half3 _ObjPos1;
	uniform half3 _ObjPos2;
	uniform half3 _ObjPos3;
	uniform half3 _ObjPos4;
	uniform half3 _ObjPos5;
	uniform half3 _ObjPos6;
	uniform half3 _ObjPos7;
	uniform half3 _ObjPos8;
	uniform half _ObjRange;
	uniform half _BendDist;
#endif


inline half2 inlineGrassBend ( half3 worldVertex ) {
	half2 tempPos = (0,0);
	#ifdef BEND_3
		half3 bendDir;
		half vertDist;
		half bendDist;
		// character 1
			bendDir = normalize ( worldVertex - _ObjPos0 );
			vertDist = min ( _ObjRange, distance ( worldVertex, _ObjPos0 ) );
			bendDist = ( _ObjRange - vertDist) / _ObjRange;
			tempPos += bendDir.xz * bendDist;
		// character 2
			bendDir = normalize ( worldVertex - _ObjPos1 );
			vertDist = min ( _ObjRange, distance ( worldVertex, _ObjPos1 ) );
			bendDist = ( _ObjRange - vertDist) / _ObjRange;
			tempPos += bendDir.xz * bendDist;
		// character 3
			bendDir = normalize ( worldVertex - _ObjPos2 );
			vertDist = min ( _ObjRange, distance ( worldVertex, _ObjPos2 ) );
			bendDist = ( _ObjRange - vertDist) / _ObjRange;
			tempPos += bendDir.xz * bendDist;
		// distance of displacement
			tempPos *= _BendDist;
			tempPos.x = clamp(tempPos.x, -_BendDist,_BendDist);
			tempPos.y = clamp(tempPos.y, -_BendDist,_BendDist);
	#endif
	#ifdef BEND_6
		half3 bendDir;
		half vertDist;
		half bendDist;
		// character 1
			bendDir = normalize ( worldVertex - _ObjPos0 );
			vertDist = min ( _ObjRange, distance ( worldVertex, _ObjPos0 ) );
            bendDist = ( _ObjRange - vertDist ) / _ObjRange;
            tempPos += bendDir.xz * bendDist;
		// character 2
			bendDir = normalize ( worldVertex - _ObjPos1 );
			vertDist = min ( _ObjRange, distance ( worldVertex, _ObjPos1 ) );
			bendDist = ( _ObjRange - vertDist) / _ObjRange;
			tempPos += bendDir.xz * bendDist;
		// character 3
			bendDir = normalize ( worldVertex - _ObjPos2 );
			vertDist = min ( _ObjRange, distance ( worldVertex, _ObjPos2 ) );
			bendDist = ( _ObjRange - vertDist) / _ObjRange;
			tempPos += bendDir.xz * bendDist;
		// character 4
			bendDir = normalize ( worldVertex - _ObjPos3 );
			vertDist = min ( _ObjRange, distance ( worldVertex, _ObjPos3 ) );
			bendDist = ( _ObjRange - vertDist) / _ObjRange;
			tempPos += bendDir.xz * bendDist;
		// character 5
			bendDir = normalize ( worldVertex - _ObjPos4 );
			vertDist = min ( _ObjRange, distance ( worldVertex, _ObjPos4 ) );
			bendDist = ( _ObjRange - vertDist) / _ObjRange;
			tempPos += bendDir.xz * bendDist;
		// character 6
			bendDir = normalize ( worldVertex - _ObjPos5 );
			vertDist = min ( _ObjRange, distance ( worldVertex, _ObjPos5 ) );
			bendDist = ( _ObjRange - vertDist) / _ObjRange;
			tempPos += bendDir.xz * bendDist;
		// distance of displacement
			tempPos *= _BendDist;
			tempPos.x = clamp(tempPos.x, -_BendDist,_BendDist);
			tempPos.y = clamp(tempPos.y, -_BendDist,_BendDist);
	#endif
	#ifdef BEND_9
		half3 bendDir;
		half vertDist;
		half bendDist;
		// character 1
			bendDir = normalize ( worldVertex - _ObjPos0 );
			vertDist = min ( _ObjRange, distance ( worldVertex, _ObjPos0 ) );
			bendDist = ( _ObjRange - vertDist) / _ObjRange;
			tempPos += bendDir.xz * bendDist;
		// character 2
			bendDir = normalize ( worldVertex - _ObjPos1 );
			vertDist = min ( _ObjRange, distance ( worldVertex, _ObjPos1 ) );
			bendDist = ( _ObjRange - vertDist) / _ObjRange;
			tempPos += bendDir.xz * bendDist;
		// character 3
			bendDir = normalize ( worldVertex - _ObjPos2 );
			vertDist = min ( _ObjRange, distance ( worldVertex, _ObjPos2 ) );
			bendDist = ( _ObjRange - vertDist) / _ObjRange;
			tempPos += bendDir.xz * bendDist;
		// character 4
			bendDir = normalize ( worldVertex - _ObjPos3 );
			vertDist = min ( _ObjRange, distance ( worldVertex, _ObjPos3 ) );
			bendDist = ( _ObjRange - vertDist) / _ObjRange;
			tempPos += bendDir.xz * bendDist;
		// character 5
			bendDir = normalize ( worldVertex - _ObjPos4 );
			vertDist = min ( _ObjRange, distance ( worldVertex, _ObjPos4 ) );
			bendDist = ( _ObjRange - vertDist) / _ObjRange;
			tempPos += bendDir.xz * bendDist;
		// character 6
			bendDir = normalize ( worldVertex - _ObjPos5 );
			vertDist = min ( _ObjRange, distance ( worldVertex, _ObjPos5 ) );
			bendDist = ( _ObjRange - vertDist) / _ObjRange;
			tempPos += bendDir.xz * bendDist;
		// character 7
			bendDir = normalize ( worldVertex - _ObjPos6 );
			vertDist = min ( _ObjRange, distance ( worldVertex, _ObjPos6 ) );
			bendDist = ( _ObjRange - vertDist) / _ObjRange;
			tempPos += bendDir.xz * bendDist;
		// character 8
			bendDir = normalize ( worldVertex - _ObjPos7 );
			vertDist = min ( _ObjRange, distance ( worldVertex, _ObjPos7 ) );
			bendDist = ( _ObjRange - vertDist) / _ObjRange;
			tempPos += bendDir.xz * bendDist;
		// character 9
			bendDir = normalize ( worldVertex - _ObjPos8 );
			vertDist = min ( _ObjRange, distance ( worldVertex, _ObjPos8 ) );
			bendDist = ( _ObjRange - vertDist) / _ObjRange;
			tempPos += bendDir.xz * bendDist;
		// distance of displacement
			tempPos *= _BendDist;
			tempPos.x = clamp(tempPos.x, -_BendDist,_BendDist);
			tempPos.y = clamp(tempPos.y, -_BendDist,_BendDist);
	#endif
	return tempPos;
}

inline half inlinePlantSquash ( half3 worldVertex ) {
	half tempPos = 0;
	#ifdef BEND_3
		half vertDist;
		half bendDist;
		half plantDist = _BendDist * 1.666;
		half plantRange = _ObjRange * 0.666;
		// character 1
			vertDist = min ( plantRange, distance ( worldVertex, _ObjPos0 ) );
			bendDist = ( plantRange - vertDist) / plantRange;
			tempPos += bendDist;
		// character 2
			vertDist = min ( plantRange, distance ( worldVertex, _ObjPos1 ) );
			bendDist = ( plantRange - vertDist) / plantRange;
			tempPos += bendDist;
		// character 3
			vertDist = min ( plantRange, distance ( worldVertex, _ObjPos2 ) );
			bendDist = ( plantRange - vertDist) / plantRange;
			tempPos += bendDist;
		// distance of displacement
			tempPos = clamp(tempPos, -plantRange,plantRange);
	#endif
	#ifdef BEND_6
		half vertDist;
		half bendDist;
		half plantDist = _BendDist * 1.666;
		half plantRange = _ObjRange * 0.666;
		// character 1
			vertDist = min ( plantRange, distance ( worldVertex, _ObjPos0 ) );
			bendDist = ( plantRange - vertDist) / plantRange;
			tempPos += bendDist;
		// character 2
			vertDist = min ( plantRange, distance ( worldVertex, _ObjPos1 ) );
			bendDist = ( plantRange - vertDist) / plantRange;
			tempPos += bendDist;
		// character 3
			vertDist = min ( plantRange, distance ( worldVertex, _ObjPos2 ) );
			bendDist = ( plantRange - vertDist) / plantRange;
			tempPos += bendDist;
		// character 4
			vertDist = min ( plantRange, distance ( worldVertex, _ObjPos3 ) );
			bendDist = ( plantRange - vertDist) / plantRange;
			tempPos += bendDist;
		// character 5
			vertDist = min ( plantRange, distance ( worldVertex, _ObjPos4 ) );
			bendDist = ( plantRange - vertDist) / plantRange;
			tempPos += bendDist;
		// character 6
			vertDist = min ( plantRange, distance ( worldVertex, _ObjPos5 ) );
			bendDist = ( plantRange - vertDist) / plantRange;
			tempPos += bendDist;
		// distance of displacement
			tempPos = clamp(tempPos, -plantRange,plantRange);
	#endif
	#ifdef BEND_9
		half vertDist;
		half bendDist;
		half plantDist = _BendDist * 1.666;
		half plantRange = _ObjRange * 0.666;
		// character 1
			vertDist = min ( plantRange, distance ( worldVertex, _ObjPos0 ) );
			bendDist = ( plantRange - vertDist) / plantRange;
			tempPos += bendDist;
		// character 2
			vertDist = min ( plantRange, distance ( worldVertex, _ObjPos1 ) );
			bendDist = ( plantRange - vertDist) / plantRange;
			tempPos += bendDist;
		// character 3
			vertDist = min ( plantRange, distance ( worldVertex, _ObjPos2 ) );
			bendDist = ( plantRange - vertDist) / plantRange;
			tempPos += bendDist;
		// character 4
			vertDist = min ( plantRange, distance ( worldVertex, _ObjPos3 ) );
			bendDist = ( plantRange - vertDist) / plantRange;
			tempPos += bendDist;
		// character 5
			vertDist = min ( plantRange, distance ( worldVertex, _ObjPos4 ) );
			bendDist = ( plantRange - vertDist) / plantRange;
			tempPos += bendDist;
		// character 6
			vertDist = min ( plantRange, distance ( worldVertex, _ObjPos5 ) );
			bendDist = ( plantRange - vertDist) / plantRange;
			tempPos += bendDist;
		// character 7
			vertDist = min ( plantRange, distance ( worldVertex, _ObjPos6 ) );
			bendDist = ( plantRange - vertDist) / plantRange;
			tempPos += bendDist;
		// character 8
			vertDist = min ( plantRange, distance ( worldVertex, _ObjPos7 ) );
			bendDist = ( plantRange - vertDist) / plantRange;
			tempPos += bendDist;
		// character 9
			vertDist = min ( plantRange, distance ( worldVertex, _ObjPos8 ) );
			bendDist = ( plantRange - vertDist) / plantRange;
			tempPos += bendDist;
		// distance of displacement
			tempPos = clamp(tempPos, -plantRange,plantRange);
	#endif
	return tempPos;
}
	
inline half2 inlineLightmapTransform ( half2 coords ) {
	#ifdef LIGHTMAP_ON
   		half2 uv2 = ( unity_LightmapST.xy * coords ) + unity_LightmapST.zw;
	#else
		half2 uv2 = coords;
	#endif
	return uv2;
}

inline fixed inlineShadow ( fixed shd ) {
	//fixed hardness = smoothstep ( 0.3, 0.7, shd );
	//fixed hardness = step (0.5,shd);
	fixed hardness = shd;
	return hardness;
}

inline fixed3 inlineLightmapWithShadows ( half2 uv, half shd ) {
	#ifdef LIGHTMAP_ON
		fixed3 lm = UNITY_SAMPLE_TEX2D ( unity_Lightmap, uv );
		#ifndef QUALITY_LOW
			fixed3 lightmap = _Gbl_Amb + shd * lm.r * _Global_LightColor + lm.g * _Gbl_Pnt;
		#else
			fixed3 lightmap = _Gbl_Amb + shd * lm.r * _Global_LightColor;
		#endif
	#else
		fixed3 lightmap = (1,1,1);
	#endif
	return lightmap;
}


inline fixed3 inlineLightmapBasic ( half2 uv ) {
	#ifdef LIGHTMAP_ON
		fixed3 lm = UNITY_SAMPLE_TEX2D ( unity_Lightmap, uv );
		#ifndef QUALITY_LOW
			fixed3 lightmap = _Gbl_Amb + lm.r * _Global_LightColor + lm.g * _Gbl_Pnt;
		#else
			fixed3 lightmap = _Gbl_Amb + lm.r * _Global_LightColor;
		#endif
	#else
		fixed3 lightmap = (1,1,1);
	#endif
	return lightmap;
}

inline fixed3 inlineVLM_Full ( fixed4 vc ) {
	#ifndef QUALITY_LOW
		fixed3 vlmlight = _Gbl_Amb + _Global_LightColor * vc.r + _Gbl_Pnt * vc.g;
	#else
		fixed3 vlmlight = _Gbl_Amb + _Global_LightColor * vc.r;
	#endif
	return vlmlight;
}

inline half3 inlineVLM_Amb ( half4 amb ) {
	half3 amblight = _Gbl_Amb + amb.g * _Gbl_Pnt;
	return amblight;
}

inline fixed3 inlineVLM_Shade ( fixed3 sun, fixed shd ) {
	fixed3 shdlight = shd.xxx * sun;
	return shdlight;
}

inline fixed3 inlineVLM_Sun ( fixed sun ) {
	fixed3 sunlight = sun * _Global_LightColor;
	return sunlight;
}

inline half2 inlineShadeProbes (half4 normal)
{
	half4 vB = normal.xyzz * normal.yzzx;
	half vC = normal.x * normal.x - normal.y * normal.y;
	
	half2 x1, x2, x3;
	x1.r = dot(unity_SHAr,normal);
	x1.g = dot(unity_SHAg,normal);
	x2.r = dot(unity_SHBr,vB);
	x2.g = dot(unity_SHBg,vB);
	x3 = unity_SHC.rg * vC;
	
    return x1 + x2 + x3;
} 

inline half3 inlineShadowBasic ( half shd ) {
	half3 shdcolor = _Gbl_Amb + shd * _Global_LightColor;
	return shdcolor;
}


inline float4 VertexTranslate(float4 _vertex, float4 _offset ){

	float4x4 xyzw=float4x4(1.0,0.0,0.0,_offset.x,
							 0.0,1.0,0.0,_offset.y,
							 0.0,0.0,1.0,_offset.z,
							 0.0,0.0,0.0,1.0  );

	return mul(xyzw,_vertex);

}

inline float4 VertexScale(float4 _vertex, float4 _scale ){

	float4x4 xyzw=float4x4(_scale.x,0.0,0.0,0.0,
							 0.0,_scale.y,0.0,0.0,
							 0.0,0.0,_scale.z,0.0,
							 0.0,0.0,0.0,1.0  );

	return mul(xyzw,_vertex);

}

//暂时不可用
inline float4 VertexRotation(float4 _vertex, float4 _rotation ){

	float radx=radians(_rotation.x);
	float rady=radians(_rotation.y);
	float radz=radians(_rotation.z);

	float sinx=sin(radx);
	float cosx=cos(radx);
	float siny=sin(rady);
	float cosy=cos(rady);
	float sinz=sin(radz);
	float cosz=cos(radz);


	float4x4 xyzw=(cosy*cosz,-cosy*sinz,siny,0.0,
				   cosx*sinz+sinx*siny*cosz,cosx*cosz-sinx*siny*sinz,-sinx*cosy,0.0,
				   sinx*sinz-cosx*siny*cosz,sinx*cosz+cosx*siny*sinz,cosx*cosy,0.0,
				   0.0,0.0,0.0,1.0);

	return mul(xyzw,_vertex);

}

inline float3 RotationZ(float3 _vertex,float _angle ){

	float radz=radians(_angle);
	float sinz=sin(radz);
	float cosz=cos(radz);

	return float3( _vertex.x*cosz-_vertex.y*sinz,_vertex.x*sinz+_vertex.y*cosz,	_vertex.z);
}

inline float2 RotationUV(float2 _uv,float _angle ){

	_uv-=float2(0.5,0.5);
	float radz=radians(_angle);
	float sinz=sin(radz);
	float cosz=cos(radz);

	_uv=float2( _uv.x*cosz-_uv.y*sinz,_uv.x*sinz+_uv.y*cosz)+float2(0.5,0.5);

	return _uv;
}

inline float2 TranslateUV(float2 _uv ,float2 _offset){

	float2 uv=_uv+_offset;
	return uv-floor(uv);

}

