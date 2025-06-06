Shader "Hidden/SH_BiltEffectData"
{
     SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {

            ZTest Always Cull Off ZWrite Off

            HLSLPROGRAM
            #include "UnityCG.cginc"

            #pragma vertex vert
            #pragma fragment frag

            float _EffectType;   // Rチャンネル
            float _EffectLevel;  // Gチャンネル

            struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
            struct v2f { float2 uv : TEXCOORD0; float4 vertex : SV_POSITION; };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                return float4(_EffectType, _EffectLevel, 0, 1); // R=種別, G=段階
            }

            ENDHLSL
        }
    }
}
