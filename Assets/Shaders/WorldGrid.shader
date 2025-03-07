Shader "Custom/WorldGrid"
{
    Properties
    {
        _GridColor("Grid Color", Color) = (0.5, 0.5, 0.5, 1)
        _BackgroundColor("Background Color", Color) = (0.2, 0.2, 0.2, 1)
        _LineWidth("Line Width", Range(0.001, 0.1)) = 0.02
        [Toggle]_AntiAlias("Anti-Aliasing", Float) = 1
        _AmbientColor("Ambient Color", Color) = (0.1, 0.1, 0.1, 1)
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" "LightMode"="ForwardBase" }
        LOD 100
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                SHADOW_COORDS(2)
                UNITY_FOG_COORDS(3)
            };

            fixed4 _GridColor;
            fixed4 _BackgroundColor;
            float _LineWidth;
            float _AntiAlias;
            fixed4 _AmbientColor;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                TRANSFER_SHADOW(o);
                UNITY_TRANSFER_FOG(o, o.pos);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // 以世界空间 xz 坐标计算网格
                float2 gridPos = i.worldPos.xz;
                // 计算每个 cell 内部距离中心的偏移，范围在 [0,0.5]
                float2 gridDist = abs(frac(gridPos) - 0.5);
                // 根据是否开启抗锯齿计算 fwidth
                float2 aa = fwidth(gridPos) * (_AntiAlias > 0.5 ? 1.0 : 0.0);
                float aaFactor = min(aa.x, aa.y);
                // 取 x 和 z 两方向距离最近网格线的距离
                float lineDist = min(gridDist.x, gridDist.y);
                // 用 smoothstep 实现抗锯齿效果：当 lineDist 小于 _LineWidth 时显示网格色
                float gridLine = 1.0 - smoothstep(_LineWidth, _LineWidth + aaFactor, lineDist);
                
                // 将背景色和网格色进行混合，gridLine 越大（靠近线条）越显示 _GridColor
                fixed4 color = lerp(_BackgroundColor, _GridColor, gridLine);

                // 计算基本定向光漫反射
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                float diff = max(0, dot(normalize(i.worldNormal), lightDir));

                // 只让直接光受到阴影影响，环境光保持不变
    float shadow = SHADOW_ATTENUATION(i);
    float3 lighting = _AmbientColor.rgb + (_LightColor0.rgb * diff * shadow);
    
    fixed4 litColor = color * fixed4(lighting,0);

                // 应用阴影和雾效
                
                UNITY_APPLY_FOG(i.fogCoord, litColor);
                return litColor;
            }
            ENDCG
        }
        
        // 阴影投射 Pass
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
    
    Fallback "Diffuse"
}
