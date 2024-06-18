Shader "PixelPie/JapaneseSun"
{
	Properties
	{
	    _MainTex ("Texture (RGB)", 2D) = "white" {}
        _Size ("Size", Range(0, 100)) = 5
        _Shape ("Shape", Range(-1, 4)) = 1
        _ShapeEdge ("Shape Edge", Range(-1, 6)) = 2
        _ShapeAmount ("Shape Amount", Range(-10, 10)) = 1
        _AnimSpin ("Animation Spin", Range(-100, 100)) = 0
        _CircleMask ("Circle Mask", Range(0, 2)) = 2
        _BurnColor ("Burn Color", Color) = (1, 1, 1, 1)
        _BurnColorThreshold ("Burn Color Threshold", Range(0, 1)) = 1
	}

	SubShader
	{
		Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }

		LOD 100

		Pass
		{

            Cull Off
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 color    : COLOR;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 color    : COLOR;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _BurnColor;
            float _Size,_Shape,_ShapeEdge,_ShapeAmount,_AnimSpin,_CircleMask,_BurnColorThreshold;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);

                // Pivot
                float2 pivot = float2(0.5, 0.5);
                // Rotation Matrix
                float rotatation = _Time * _AnimSpin;
                float cosAngle = cos(rotatation);
                float sinAngle = sin(rotatation);
                float2x2 rot = float2x2(cosAngle, -sinAngle, sinAngle, cosAngle);
 
                // Rotation consedering pivot
                float2 uv = v.uv.xy - pivot;
                o.uv = mul(rot, uv);
                o.uv += pivot;

                o.color = v.color;
				return o;
			}
            
            float star(float2 uv, float size)
            {
                uv = abs(uv);
                float2 pos = min(uv.xy/uv.yx, size);
                float p = (_ShapeEdge - pos.x - pos.y) * _ShapeAmount;
                return (2.0+p*(p*p-1.5)) / (uv.x+uv.y);      
            }

            fixed4 frag (v2f i) : SV_Target
			{
                float2 uv = i.uv - float2(0.5, 0.5);
                float leng = length(uv);

                uv *= _Size;
                            
                float3 col = float3(star(uv,_Shape) * i.color.rgb);
                col = clamp(col , 0, 1);

                float darkAmount = (col.r + col.g + col.b)/3;
                
                if(darkAmount > _BurnColorThreshold ) 
                {
                    col *= _BurnColor;
                }
                
                float alpha = darkAmount * lerp(0, 1, smoothstep(1,0,abs(leng/_CircleMask)));
                return float4(col,alpha);
            }

			
			ENDCG
		}
	}
}