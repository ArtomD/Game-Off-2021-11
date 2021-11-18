Shader "Unlit/SampleGrid"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float2 mod(float2 x, float2 y) 
            {
 
                return x - y * floor(x / y);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;

                //uv.xy -= iMouse.xy / iResolution.xy;

                float tile_size = 1.0 / 8.0;

                float edge = tile_size / 24.0;

                uv = (mod(uv, tile_size) - mod(uv - edge, tile_size) - edge) * (1.0 / tile_size);
                float4 grid_frag = 1.0 - length(uv) * 0.5; // 0.9 for the face of the tile, 0.5 for the edge

                // Normalized pixel coordinates (from 0 to 1)
                uv = i.uv;

                // Time varying pixel color
                float3 col = 0.75 + 0.50 * cos(
                    _Time.y + uv.xyx + float3(2, 0, 0)
                );

                // Output to screen
                return float4(col, 1.0) - grid_frag;

            }

            
            ENDCG
        }
    }
}
