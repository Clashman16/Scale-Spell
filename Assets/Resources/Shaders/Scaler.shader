Shader "Custom/DashedLine"
{
    Properties
    {
        _MainColor ("Line Color", Color) = (1, 1, 1, 1)
        _DashSize ("Dash Size", Float) = 0.03
        _GapSize ("Gap Size", Float) = 0.03
    }
    SubShader
    {
        Tags { "Queue"="Transparent" }
        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float _DashSize;
            float _GapSize;
            float4 _MainColor;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float dashLength = _DashSize + _GapSize;
                float modResult = fmod(i.uv.x, dashLength);

                if (modResult < _DashSize)
                {
                    return _MainColor; // Draw dash
                }
                else
                {
                    return float4(0, 0, 0, 0); // Transparent gap
                }
            }
            ENDCG
        }
    }
}