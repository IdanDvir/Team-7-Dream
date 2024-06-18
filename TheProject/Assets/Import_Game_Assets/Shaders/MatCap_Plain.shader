Shader "MatCap/Vertex/VCol Matcap"
{
	Properties
	{
		[MainColor] _BaseColor ("Main Color", Color) = (0.5,0.5,0.5,1.0)
		[MainTexture] _MatCap ("MatCap (RGB)", 2D) = "white" {}
		_MatCapIntensity("Matcap Intensity", Range(0, 1)) = 1.0
		[KeywordEnum(McAdd, McMultiply)]_ENUM("MatCap Blend Mode", Float) = 0
		[ToggleOff] _Matcap_Pixelnormal ("MatCap Normals per Pixel", Float) = 0.0
		[Enum(UnityEngine.Rendering.CullMode)] _Cull ("Cull", Float) = 0
		[Enum(ZWriteMode)] _ZWrite("ZWrite", Float) = 1
		[TransparentBlendmodes] _BlendMode ("Transparency Blend Mode", Float) = 0
		
		[HideInInspector] _SrcBlend ("Source Blend Mode", Float) = 1 // UnityEngine.Rendering.BlendMode.One
		[HideInInspector] _DstBlend ("Destination Blend Mode", Float) = 2 // UnityEngine.Rendering.BlendMode.Zero
	}
	
	Subshader
	{
		Tags {"RenderPipeline" = "SRPDefaultUnlit"}
		

		
		Pass
		{
			Cull [_Cull]
			ZWrite [_ZWrite]
			
			Stencil
			{
		        Ref 12
		        Comp Always
			    Pass Replace
			}
			
			Blend [_SrcBlend] [_DstBlend]
			
			HLSLPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 3.5
				#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
				
				#pragma shader_feature _MATCAP_PIXELNORMAL_OFF
				#pragma shader_feature_local_fragment _ENUM_MCADD _ENUM_MCMULTIPLY
				
				struct Attributes
				{
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					float4 color : COLOR;
				};
				
				struct Varyings
				{
					float4 vertex	: SV_POSITION;
					float4 color : COLOR;
					float4 cap	: TEXCOORD0;
				};
				
				CBUFFER_START(UnityPerMaterial)
					half4 _BaseColor;
					half _MatCapIntensity;
				CBUFFER_END
				
				TEXTURE2D(_MatCap);
				SAMPLER(sampler_MatCap);

				float2 NormalToMatcapUV(float3 normal)
				{
					return TransformWorldToViewDir(normal).xy * 0.5 + 0.5;
				}
				
				Varyings vert (Attributes v)
				{
					Varyings o;
					o.vertex = TransformObjectToHClip(v.vertex.xyz);
					o.color = v.color;

					#if _MATCAP_PIXELNORMAL_OFF
						const float3 WSNormal = TransformObjectToWorldDir(v.normal);
						o.cap = float4(NormalToMatcapUV(WSNormal), 0.0, 0.0);
					#else
						o.cap = float4(v.normal, 0.0);
					#endif
					
					return o;
				}

				float4 frag (Varyings i) : SV_TARGET
				{
					#if _MATCAP_PIXELNORMAL_OFF
						float2 uv_mc = i.cap.xy;
					#else
						float2 uv_mc = TransformWorldToViewDir(TransformObjectToWorldDir(i.cap.xyz)).xy * 0.5 + 0.5;
					#endif
					
					float4 mc = SAMPLE_TEXTURE2D(_MatCap, sampler_MatCap, uv_mc) * _MatCapIntensity;
					float4 color = i.color * _BaseColor;
					
					#if defined(_ENUM_MCADD)
						color.rgb = (color.rgb + mc.rgb * i.color.a);
					#elif defined(_ENUM_MCMULTIPLY)
						color.rgb = lerp(color.rgb, color.rgb * mc.rgb, i.color.a);
					#endif
					color.a *= mc.a;
					
					return color;
				}
			ENDHLSL
		}
	}
}
