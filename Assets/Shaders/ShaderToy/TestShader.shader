Shader "Unlit/TestShader"
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

            fixed4 frag(v2f i) : SV_Target
            {
                    // Normalized pixel coordinates (from 0 to 1)
                float2 uv = 2. * (i.vertex.xy / _ScreenParams.xy) - 1.;
                //vec2 uv = ( 2.*fragCoord - iResolution.xy ) / iResolution.y; 
                uv.x *= _ScreenParams.x / _ScreenParams.y;
                float d = dot(uv,float2(cos(_Time.y),sin(_Time.y)));


                float dd = length(uv);

                float3 col = float3(1,1,1) / (50. * abs(d - .5));

                // Time varying pixel color
                col += float3(abs(cos(_Time.y)),1.,abs(sin(_Time.y))) / (50. * abs(dd - .5));

                // Output to screen
                return float4(col,1.0);
            }
            ENDCG
        }
    }
}
