Shader "Unlit/SampleGrid2"
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

            float2 rand2(in float2 p)
            {
                return frac(float2(sin(p.x * 591.32 + p.y * 154.077), cos(p.x * 391.32 + p.y * 49.077)));
            }


            float voronoi(in float2 x)
            {
                float2 p = floor(x);
                float2 f = frac(x);
                float minDistance = 1.;

                for (int j = -1; j <= 1; j++)
                {
                    for (int i = -1; i <= 1; i++)
                    {
                        float2 b = float2(i, j);
                        float2 rand = .5 + .5 * sin(_Time.y * 1. + 12. * rand2(p + b));
                        float2 r = float2(b) - f + rand;
                        minDistance = min(minDistance, length(r));
                    }
                }
                return minDistance;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;
                uv.x = i.uv.x;
                float val = pow(voronoi(uv * 16.) * 1.25, 7.) * 2.;
                float gridLineThickness = 1. / _ScreenParams.y;
                float2 grid = step(mod(uv, .05), float2(gridLineThickness, gridLineThickness));

                return float4(.0, val * (grid.x + grid.y), val * (grid.x + grid.y), 1.);

            }

            
            ENDCG
        }
    }
}
