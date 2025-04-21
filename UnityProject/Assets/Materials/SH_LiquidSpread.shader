/*=====
<SH_LiquidSpread.Shader>
└作成者：tei

＞内容
液体拡散処理の設定と計算

＞注意事項

＞更新履歴
　　__Y25
　_M04
D
18：プログラム作成：tei
19：処理微調整、コメント追加：tei
21：拡散範囲と拡散速度を別々で計算に変更：tei

=====*/
Shader "Custom/SH_LiquidSpread_Unlit"
{
    // 液体生成に必要なプロパティ
      Properties
    {
        _BaseColor ("Base Color", Color) = (0, 1, 0, 1)                     // 色
         _MaxSpread ("Max Spread Distance", Float) = 1.0                    // 拡散最大距離
        _SpreadDuration ("Spread Duration", Float) = 1.5                    // 拡散にかかる時間
        _Cutoff ("Cutoff Threshold", Range(0,1)) = 0.05                     // 閾値設定(α値用)
        _FadeDuration ("Fade Duration", Float) = 1.5                        // フェイド時間
        _FadeStartTime ("Fade Start Time", Float) = -1.0                    // フェイド開始時間
        _StartTime ("Start Time", Float) = 0.0                              // 拡散開始時間
        _LiquidTex ("Liquid Noise Texture", 2D) = "white" {}                // テクスチャ
        _RandomSpreadPlus ("Random Spread Plus", Vector) = (1,1,1,1)        // ランダム拡散幅プラス
        _RandomSpreadMinus ("Random Spread Minus", Vector) = (1,1,1,1)      // ランダム拡散幅マイナス

    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            Name "Unlit"
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            // 変数宣言

            float4 _BaseColor;
            float _MaxSpread;
            float _SpreadDuration;
            float _Cutoff;
            float _StartTime;
            float _FadeStartTime;
            float _FadeDuration;
            TEXTURE2D(_LiquidTex);          
            SAMPLER(sampler_LiquidTex);
            float4 _RandomSpreadPlus;
            float4 _RandomSpreadMinus;

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = v.uv;
                return o;
            }

            half4 frag (Varyings i) : SV_Target
            {
                // UVを中心基準に変換
                float2 centeredUV = (i.uv - 0.5f);

                // 中心からの方向
                float2 dir = normalize(centeredUV);

                // 元の距離
                float dist = length(centeredUV);

                // 時間経過による拡散速度調整
                 float elapsed = _Time.y - _StartTime;
                 float t = saturate(elapsed / _SpreadDuration);    // 0~1正規化

                // なめらかな拡散：easeInOut
                float spreadCurve = t * t * (3.0f - 2.0f * t);

                // 最終的な拡散範囲
                float spread = spreadCurve * _MaxSpread;

                // Ver.① Directionで制御
                {
                    // // 方向ごとに拡散補正
                    // float strench = 1.0f;

                    // // x軸優先
                    // if(abs(dir.x) > abs(dir.y))
                    // {
                    //     if(dir.x > 0)
                    //     {
                    //         strench = _RandomSpreadPlus.x;
                    //     }
                    //     else
                    //     {
                    //         strench = _RandomSpreadPlus.z;
                    //     }
                    // }

                    // // y軸優先
                    // else
                    // {
                    //     if(dir.y > 0)
                    //     {
                    //         strench = _RandomSpreadPlus.y;
                    //     }
                    //     else
                    //     {
                    //         strench = _RandomSpreadPlus.w;
                    //     }
                    // }

                    // // 斜め方向補正
                    // if(abs(dir.x - dir.y) < 0.2)
                    // {
                    //     if(dir.x > 0)
                    //     {
                    //         strench = _RandomSpreadMinus.x;
                    //     }
                    //     else
                    //     {
                    //         strench = _RandomSpreadMinus.z;
                    //     }
                    // }
                    // else if(abs(dir.x + dir.y) < 0.2)
                    // {
                    //     if(dir.x < 0)
                    //     {
                    //         strench = _RandomSpreadMinus.y;
                    //     }
                    //     else
                    //     {
                    //         strench = _RandomSpreadMinus.w;
                    //     }
                    // }

                    // // 最後に距離を補正
                    // dist /= strench;
                }
                
                // Ver.② Rotationで制御
                {
                    // 角度取得(-π～π)
                    float angle = atan2(dir.y, dir.x);

                    // 角度を0~1正規化
                    angle = (angle + 3.1415926) / (2.0 * 3.1415926);

                    // 8方向に分割
                    float sector = angle * 8.0;
                    float sectorIndex = floor(sector);
                    float sectorLerp = sector - sectorIndex;    // 0~1の間

                    // 8方向のランダムパラメータ
                    float stretchArray[8];
                    stretchArray[0] = _RandomSpreadPlus.x;   // 右
                    stretchArray[1] = _RandomSpreadMinus.x;  // 右上
                    stretchArray[2] = _RandomSpreadPlus.y;   // 上
                    stretchArray[3] = _RandomSpreadMinus.y;  // 左上
                    stretchArray[4] = _RandomSpreadPlus.z;   // 左
                    stretchArray[5] = _RandomSpreadMinus.z;  // 左下
                    stretchArray[6] = _RandomSpreadPlus.w;   // 下
                    stretchArray[7] = _RandomSpreadMinus.w;  // 右下

                    // セクター間を補間して拡散率を決める
                    float stretch = lerp(
                        stretchArray[(int)sectorIndex % 8],
                        stretchArray[((int)sectorIndex + 1) % 8],
                        sectorLerp
                        );

                    // 最終的な距離に反映
                    dist /= stretch;
                }
                

                // 拡散ベースのアルファ
                float baseAlpha = smoothstep(spread, spread - _Cutoff, dist);

                // 液体模様テクスチャを取得（必要ならテクスチャ設定）
                float noise = SAMPLE_TEXTURE2D(_LiquidTex, sampler_LiquidTex, i.uv).r;

                // 液体模様を掛け算
                float alpha = baseAlpha * noise;

                // フェードアウト処理（fade開始後、徐々に透明にする）
                if (_FadeStartTime > 0)
                {
                    float fadeProgress = saturate((_Time.y - _FadeStartTime) / _FadeDuration);
                    alpha *= (1.0 - fadeProgress);
                }

                // 透明すぎたら破棄
                if (alpha <= 0.001)
                    discard;

                return float4(_BaseColor.rgb, alpha * _BaseColor.a);
            }
            ENDHLSL
        }
    }
}
