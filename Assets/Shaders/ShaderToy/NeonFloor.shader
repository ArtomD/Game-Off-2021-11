Shader "Unlit/NeonFloor"
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

            float3 revProject(float2 camPos, float worldY, float fov) {
                static const float PI = 3.14159265f;
                float worldZ = worldY / (camPos.y * tan(fov * PI / 180 * .5));
                float worldX = worldY * camPos.x / camPos.y;
                return float3(worldX, worldY, worldZ);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                static const float PI = 3.14159265f;

                float2 uv = i.vertex / _ScreenParams.xy;
                float2 p = (i.vertex.xy - _ScreenParams.xy *.5) / _ScreenParams.y;
    
                // Define supersample sizes
                float fragsize = 1. / _ScreenParams.y;
                float supersize = fragsize / float(8);

                // Define the screenspace horizon [-0.5, 0.5]
                float horizonY = 0.2;

                float4 fragColor;
    
                // Clip above horizon (optional)
                if (p.y < horizonY) {
    	            fragColor = float4(float3(0,0,0), 1.0);
                }
                else {
                    // Initialize current fragment intensity
                    float intensity = 0.;
                    // Define the current grid displacement
                    float3 displace = float3(3.*sin(2.*PI*0.1* _Time.y), 4.* _Time.y, 1.5);
                    // Define the FOV
                    float fov = 90.0;
        
                    // Retrieve supersamples
                    for (int i = 0; i < 8; i++) {
                        for (int j = 0; j < 8; j++) {
                            float2 superoffset = float2(i,j) * supersize;
                            // Get worldspace position of grid
                            float2 gridPos = revProject(p + superoffset - float2(0., horizonY), displace.z, fov).xz;                
                            // Create grid
                            float2 grid = frac(gridPos - displace.xy) - 0.5;
                            // Make wavy pattern
                            float pattern = 0.7+0.6*sin(gridPos.y - 6.* _Time.y);
                
                            // Compute distance from grid edge
                            float dist = max(grid.x*grid.x, grid.y*grid.y);
                            // Compute grid fade distance
                            float fade = min(1.5, pow(1.2, -length(gridPos) + 15.0));
                            // Set brightness
                            float bright = 0.015 / (0.26 - dist);
                            intensity += fade * pattern * bright;
                        }
                    }
        
                    // Define current fragment color
                    float3 col = 0.5 + 0.5*cos(_Time.y +p.yxy+float3(0,10,20));
                    // Normalize intensity
                    intensity /= float(8*8);
        
    	            fragColor = float4(intensity * col, 1.0);
                }
    
                return pow(fragColor, float4(.4545, .4545, .4545, .4545));
            }
            ENDCG
        }
    }
}
