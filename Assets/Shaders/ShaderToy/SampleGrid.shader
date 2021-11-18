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
            #define PI 3.14
            #define TARGET_COUNT 15
            #define GRID_CELL_SIZE 0.1
            #define RED float3(1.0,0.0,0.0)
            #define GREEN float3(0.0,1.0,0.0)
            #define BLUE float3(0.0,0.0,1.0)
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

            float2 getGridPosition(in float2 uv)
            {
                return float2((uv.x / GRID_CELL_SIZE), (uv.y / GRID_CELL_SIZE));
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = (i.vertex.xy - _ScreenParams.xy * .5) / _ScreenParams.y;                
                
                float2 gridBoundUV = getGridPosition(uv);

                float2 cellBoundUV = gridBoundUV - round(gridBoundUV);

                float redIntensity = 0.0;
                float blueIntensity = 0.0;

                for (int targetIndex = 0; targetIndex < TARGET_COUNT; ++targetIndex)
                {
                    float f_targetIndex = float(targetIndex);

                    float trigOffset = (PI / float(TARGET_COUNT)) * f_targetIndex;
                    float2 targetPosition = float2(sin(_Time.y + trigOffset) * 0.51 + tan(f_targetIndex + trigOffset), cos(_Time.y + trigOffset) * 0.1 + sin(f_targetIndex + trigOffset));
                    float2 gridBoundTargetPosition = getGridPosition(targetPosition);
                    float2 edgeBoundPosition = float2(gridBoundTargetPosition.x, gridBoundTargetPosition.y);

                    // change the op between the lengths to subtraction for some extreme strobe effects
                    float distanceToTarget = length(gridBoundUV - round(gridBoundTargetPosition)) + length((gridBoundUV)-(edgeBoundPosition));

                    redIntensity += length(GRID_CELL_SIZE / (distanceToTarget * 9.5) / cellBoundUV) * GRID_CELL_SIZE;
                }

                for (int targetIndex = 0; targetIndex < TARGET_COUNT; ++targetIndex)
                {
                    float f_targetIndex = float(targetIndex);

                    float trigOffset = (PI / float(TARGET_COUNT)) * f_targetIndex;

                    float2 targetPosition = float2(sin(_Time.y + trigOffset) * 0.51 + sin(f_targetIndex + trigOffset), tan(_Time.y + trigOffset) * 0.1 + sin(f_targetIndex + trigOffset));
                    float2 gridBoundTargetPosition = getGridPosition(targetPosition);
                    float2 edgeBoundPosition = float2(gridBoundTargetPosition.x, gridBoundTargetPosition.y);

                    float distanceToTarget = length(gridBoundUV - round(gridBoundTargetPosition)) + distance(gridBoundUV, edgeBoundPosition);

                    blueIntensity += length(GRID_CELL_SIZE / (distanceToTarget * 15.5) / cellBoundUV) * GRID_CELL_SIZE;

                }

                float3 col = smoothstep(0.2, 1.0, redIntensity + blueIntensity);

                col += redIntensity * GREEN;
                col += blueIntensity * BLUE;

                // Output to screen
                return float4(col, 1.0);

            }

            
            ENDCG
        }
    }
}
