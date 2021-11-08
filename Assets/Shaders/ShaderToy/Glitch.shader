Shader "Unlit/Glitch"
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

            fixed4 frag (v2f i) : SV_Target
            {
                    float2 p = (i.vertex.xy - _ScreenParams.xy * .5) / _ScreenParams.y;

                    float t = (_Time.y + 1.3) * 2.;
                    float c = .0, d = .0, s = .0;

                    for (float f = -5.; f < 1.; f += .6)
                    {
                        s = 20.;

                        if (p.x > .0)
                            s = 20. + sin(tan((p.x + p.y) / (p.x / p.y)) * 10.) * 2.;
                        //s = 20. + sin(f+ sin(p.x/p.y)*5. )*10.;

                    d = distance(
                            float3(tan(p.x * s + t), tan(p.y * s + t), f),
                            float3(p.x * 20., .0, -5.)
                        );

                    c += d * sin(t + .5 / p.x + cos(p.y) * 5.) * .01;
                }

                return float4(.5 - c, .6 - c * .25,1. - c * .5, 1.);//float4(1.-c);
            }
            ENDCG
        }
    }
}
