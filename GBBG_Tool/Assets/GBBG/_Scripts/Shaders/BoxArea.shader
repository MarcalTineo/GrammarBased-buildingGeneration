Shader "Unlit/BoxArea"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Distance ("Distance", Range(0, 0.5)) = 0
    }
    SubShader
    {
        Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _Color;
            float _Distance;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {

                float distanceToEdge = 0.5 - min(min(i.uv.x, 1-i.uv.x), min(i.uv.y, 1-i.uv.y));
                float alpha = saturate((distanceToEdge - _Distance)/(0.5 - _Distance));
                return fixed4(_Color.xyz, alpha * _Color.a);
            }

            ENDCG
        }
    }
}
