Shader "Hidden/SH_BlitColorFill"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BaseColor ("Color", Color) = (1, 1, 1, 1) 
    }
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

            float4 _Color;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv     : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                return _Color; // íPêFìhÇË
            }

            ENDHLSL
        }
    }
}
