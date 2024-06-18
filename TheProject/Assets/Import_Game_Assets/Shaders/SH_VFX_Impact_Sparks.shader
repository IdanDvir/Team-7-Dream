Shader "FX/Impact/SH_VFX_Impact Sparks"
{
    Properties
    {
        [MainColor] _BaseColor ("Main Color", Color) = (0.5,0.5,0.5,1.0)
                
        [TransparentBlendmodes] _BlendMode ("Transparency Blend Mode", Float) = 0
        [Enum(UnityEngine.Rendering.CullMode)] _Cull ("Cull", Float) = 0
        [Enum(ZWriteMode)] _ZWrite("ZWrite", Float) = 1
        
        [HideInInspector] _SrcBlend ("Source Blend Mode", Float) = 1 // UnityEngine.Rendering.BlendMode.One
		[HideInInspector] _DstBlend ("Destination Blend Mode", Float) = 2 // UnityEngine.Rendering.BlendMode.Zero
    }
    SubShader
    {
        Tags { "RenderPipeline"="SRPDefaultUnlit" "RenderType"="Transparent" "RenderQueue"="Transparent" }
        LOD 100

        Pass
        {
            Blend [_SrcBlend] [_DstBlend]
            Cull [_Cull]
            ZWrite [_ZWrite]
            ZTest LEqual
            
            Stencil
            {
                Ref 12
                Comp NotEqual
            }
            
            HLSLPROGRAM
            #pragma vertex vert
			#pragma fragment frag
			#pragma target 3.5
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float4 mainColor : TEXCOORD0;
                float4 coreColor : TEXCOORD1;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            CBUFFER_START(UnityPerMaterial)
				half4 _BaseColor;
            CBUFFER_END
                        
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);

                half mainColorMask = saturate(v.color.r + v.color.b);
                half edgeAlphaMask = saturate(v.color.b - v.color.a) * v.color.b;
                edgeAlphaMask = saturate(edgeAlphaMask + (1. - v.color.b));
                
                o.color.rgb = saturate(v.mainColor.rgb * mainColorMask + v.coreColor.rgb * v.color.g) * edgeAlphaMask;
                o.color.a = saturate(mainColorMask * v.mainColor.a + v.color.g * v.coreColor.a) * edgeAlphaMask;

                o.color *= _BaseColor;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                return i.color;
            }
            ENDHLSL
        }
    }
}
