Shader "PixelPie/UIRadialGradient"
{
    Properties
    {
        _Color1 ("Color 1", Color) = (1,1,1,1)
        _Color2 ("Color 2", Color) = (0,0,0,1)
        _Hardness ("Hardness", Range(0, 1)) = 1
        _CenterX ("CenterX", float) = 0.5
        _CenterY ("CenterY", float) = 0.5
        _ScaleX ("ScaleX", Range(0, 3)) = 1
        _ScaleY ("ScaleY", Range(0, 3)) = 1
        
        [HideInInspector] _Stencil("Stencil ID", Float) = 0
        [HideInInspector] _StencilComp("StencilComp", Float) = 8
        [HideInInspector] _StencilOp("StencilOp", Float) = 0
        [HideInInspector] _StencilReadMask("StencilReadMask", Float) = 255
        [HideInInspector] _StencilWriteMask("StencilWriteMask", Float) = 255
        [HideInInspector] _ColorMask("ColorMask", Float) = 15
        
        // This is here coz Unity is fussy about it
        [HideInInspector] _MainTex("MainTex", 2D) = "black" {}
    }
    
    SubShader
    {
        Blend SrcAlpha OneMinusSrcAlpha
        Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            
            fixed4 _Color1;
            fixed4 _Color2;
            float _Hardness;
            
            float _CenterX;
            float _CenterY;
            float _ScaleX;
            float _ScaleY;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Scale the uvs
                const float scaled_x = i.uv.x / _ScaleX;
                const float scaled_y = i.uv.y / _ScaleY;
                
                // Calculate a clamped hardness value because when its 1 it causes artifacts
                const float hardness = min(_Hardness, .9999f);
                
                // Sample the color and alpha from the assigned texture
                fixed4 tex_color = tex2D(_MainTex, i.uv);

                // Calculate distance from the center
                const float2 center = float2(_CenterX - (1 - 1 / _ScaleX) / 2 , _CenterY - (1 - 1 / _ScaleY) / 2 );
                const float2 scaled_uv = float2(scaled_x, scaled_y);
                const float dist = saturate(distance(scaled_uv, center));

                // Calculate gradient between two colors considering hardness
                float t = saturate(dist / .5f);
                t = max(t - hardness, 0) / (1 - hardness);
                fixed4 gradient_color = lerp(_Color1, _Color2, t);

                // Combined with texture alpha
                gradient_color.a = gradient_color.a * tex_color.a;
                
                return gradient_color;
            }
            ENDCG
        }
    }
}