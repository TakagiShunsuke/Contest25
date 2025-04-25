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
22：各変数名調整：tei

=====*/
Shader "Custom/SH_LiquidSpread_Unlit"
{
    // 液体生成に必要なプロパティ
      Properties
    {
        _BaseColor ("Base Color", Color) = (0, 1, 0, 1)                     // 色
         _MaxSpread ("Max Spread Distance", Float) = 1.0                    // 拡散最大距離（広さ）
        _SpreadDuration ("Spread Duration", Float) = 1.5                    // 拡散にかかる時間
        _Cutoff ("Cutoff Threshold", Range(0,1)) = 0.05                     // 閾値設定(α値用)
        _FadeDuration ("Fade Duration", Float) = 1.5                        // フェイド時間
        _FadeStartTime ("Fade Start Time", Float) = -1.0                    // フェイド開始時間
        _StartTime ("Start Time", Float) = 0.0                              // 拡散開始時間
        _LiquidTex ("Liquid Noise Texture", 2D) = "white" {}                // テクスチャ
        _RandomSpreadBottomLeftToTopRight ("Random Spread BLToTR", Vector) = (1,1,1,1)      // ランダム拡散幅（直線  x = y 方向）
        _RandomSpreadTopLeftToBottomRight ("Random Spread TLToBR", Vector) = (1,1,1,1)      // ランダム拡散幅（直線 -x = y 方向）

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

            float4 _BaseColor;      // 各変数コマンド上記"Priorities"のところに参照
            float _MaxSpread;
            float _SpreadDuration;
            float _Cutoff;
            float _StartTime;
            float _FadeStartTime;
            float _FadeDuration;
            TEXTURE2D(_LiquidTex);          
            SAMPLER(sampler_LiquidTex);                 // サンプラーテクスチャ
            float4 _RandomSpreadBottomLeftToTopRight;
            float4 _RandomSpreadTopLeftToBottomRight;

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
                float2 fCenteredUV = (i.uv - 0.5f);

                // 中心からの方向
                float2 fDirection = normalize(fCenteredUV);

                // 元の距離
                float fDistance = length(fCenteredUV);

                // 時間経過による拡散速度調整
                 float fElapsed = _Time.y - _StartTime;
                 float t = saturate(fElapsed / _SpreadDuration);    // 0~1正規化 (イージング変更値)

                // なめらかな拡散計算：easeInOut
                float fSpreadCurve = t * t * (3.0f - 2.0f * t);

                // 最終的な拡散範囲
                float fSpread = fSpreadCurve * _MaxSpread;

                // Ver.① Directionで制御
                {
                    // // 方向ごとに拡散補正
                    // float strench = 1.0f;

                    // // x軸優先
                    // if(abs(fDirection.x) > abs(fDirection.y))
                    // {
                    //     if(fDirection.x > 0)
                    //     {
                    //         strench = _RandomSpreadBottomLeftToTopRight.x;
                    //     }
                    //     else
                    //     {
                    //         strench = _RandomSpreadBottomLeftToTopRight.z;
                    //     }
                    // }

                    // // y軸優先
                    // else
                    // {
                    //     if(fDirection.y > 0)
                    //     {
                    //         strench = _RandomSpreadBottomLeftToTopRight.y;
                    //     }
                    //     else
                    //     {
                    //         strench = _RandomSpreadBottomLeftToTopRight.w;
                    //     }
                    // }

                    // // 斜め方向補正
                    // if(abs(fDirection.x - fDirection.y) < 0.2)
                    // {
                    //     if(fDirection.x > 0)
                    //     {
                    //         strench = _RandomSpreadTopLeftToBottomRight.x;
                    //     }
                    //     else
                    //     {
                    //         strench = _RandomSpreadTopLeftToBottomRight.z;
                    //     }
                    // }
                    // else if(abs(fDirection.x + fDirection.y) < 0.2)
                    // {
                    //     if(fDirection.x < 0)
                    //     {
                    //         strench = _RandomSpreadTopLeftToBottomRight.y;
                    //     }
                    //     else
                    //     {
                    //         strench = _RandomSpreadTopLeftToBottomRight.w;
                    //     }
                    // }

                    // // 最後に距離を補正
                    // fDistance /= strench;
                }
                
                // Ver.② Rotationで制御
                {
                    // 角度取得(-π～π)
                    float fAngle = atan2(fDirection.y, fDirection.x);

                    // 角度を0~1正規化
                    fAngle = (fAngle + 3.1415926) / (2.0 * 3.1415926);

                    // 8方向に分割
                    float fSector = fAngle * 8.0;           // 8方向のセクター
                    float fSectorIndex = floor(fSector);    // セクターインデクス
                    float fSectorLerp = fSector - fSectorIndex;    // 0~1の間

                    // 8方向のランダムパラメータ
                    float fStretchArray[8];
                    fStretchArray[0] = _RandomSpreadBottomLeftToTopRight.x;   // 右
                    fStretchArray[1] = _RandomSpreadTopLeftToBottomRight.x;  // 右上
                    fStretchArray[2] = _RandomSpreadBottomLeftToTopRight.y;   // 上
                    fStretchArray[3] = _RandomSpreadTopLeftToBottomRight.y;  // 左上
                    fStretchArray[4] = _RandomSpreadBottomLeftToTopRight.z;   // 左
                    fStretchArray[5] = _RandomSpreadTopLeftToBottomRight.z;  // 左下
                    fStretchArray[6] = _RandomSpreadBottomLeftToTopRight.w;   // 下
                    fStretchArray[7] = _RandomSpreadTopLeftToBottomRight.w;  // 右下

                    // セクター間を補間して拡散率を決める
                    float fStretch = lerp(
                        fStretchArray[(int)fSectorIndex % 8],
                        fStretchArray[((int)fSectorIndex + 1) % 8],
                        fSectorLerp
                        );

                    // 最終的な距離に反映
                    fDistance /= fStretch;
                }
                

                // 拡散ベースのアルファ
                float fBaseAlpha = smoothstep(fSpread, fSpread - _Cutoff, fDistance);

                // 液体模様テクスチャを取得（必要ならテクスチャ設定）
                float fNoise = SAMPLE_TEXTURE2D(_LiquidTex, sampler_LiquidTex, i.uv).r;

                // 液体模様を掛け算
                float fAlpha = fBaseAlpha * fNoise;

                // フェードアウト処理
                if (_FadeStartTime > 0)
                {
                    float fFadeProgress = saturate((_Time.y - _FadeStartTime) / _FadeDuration);
                    fAlpha *= (1.0 - fFadeProgress);
                }

                // 透明すぎたら破棄
                if (fAlpha <= 0.001)
                    discard;

                return float4(_BaseColor.rgb, fAlpha * _BaseColor.a);
            }
            ENDHLSL
        }
    }
}
