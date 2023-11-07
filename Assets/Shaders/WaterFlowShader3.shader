Shader "Custom/WaterFlowShader3" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _FlowSpeed ("Flow Speed", Range(0, 10)) = 1.0
        _Tiling ("Tiling", Range(1, 10)) = 1.0
        _Offset ("Offset", Range(0, 1)) = 0.0
    }
 
    SubShader {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
 
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
 
            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
 
            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
 
            sampler2D _MainTex;
            float _FlowSpeed;
            float _Tiling;
            float _Offset;
 
            v2f vert (appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv * _Tiling + float2(_Offset, 0);
                return o;
            }
 
            half4 frag (v2f i) : SV_Target {
                // Calculate new UV coordinates with looping flow effect
                float2 flowUV = i.uv;

                // Limit time to [0, 1] range
                float time = _Time.y % 1.0;
                flowUV.y += time * _FlowSpeed * 0.01; // Flow along Y-axis over time

                // Ensure UV coordinates loop within [0, 1] range
                if (flowUV.y > 1.0) {
                    flowUV.y -= 1.0;
                }

                // Apply tiling to ensure texture wraps and repeats
                flowUV *= _Tiling;

                // Sample the main texture with the looping flow effect
                half4 flowColor = tex2D(_MainTex, flowUV);

                // Change the color to blue
                half4 blueColor = half4(1, 1, 1, 1); // Blue color

                // Combine the flowColor and blueColor
                half4 finalColor = flowColor * blueColor;

                return finalColor;
            }
            ENDCG
        }
    }
}
