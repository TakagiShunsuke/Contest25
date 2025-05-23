/*=====
<SH_DeathEffect.Shader>
└作成者：tei

＞内容
死亡エフェクト処理計算

＞注意事項

＞更新履歴
　　__Y25
　_M04
D
25：プログラム作成：tei

=====*/
Shader "Custom/SH_DeathEffect"
{
    // エフェクト生成に必要なプロパティ
   Properties
    {
        _Color ("Main Color", Color) = (0.2, 1.0, 0.5, 0.8) // カラー
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="transparent" }
        Pass
        {
            Name "RaymarchWithDepth"
            Tags { "LightMode" = "UniversalForward" }

            ZWrite On
            ZTest LEqual
            Blend SrcAlpha OneMinusSrcAlpha     // 半透明用のブレンド設定

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            // 構造体定義
            // 頂点/フラグメント構造体
            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            struct FragOutput
            {
                float4 color : SV_Target;
                float depth : SV_Depth;
            };

            // // 頂点シェーダー：平面上の各点のワールド位置を取得
            // VS_OUT vert(VS_IN v)
            // {
            //     VS_OUT o;
            //     o.pos = UnityObjectToClipPos(v.vertex);
            //     o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
            //     return o;
            // }

            // ＞結合補間関数
            // 引数１：float fFirstNum：数値
            // 引数２：float fSecondNum：数値
            // 引数３：float fConstK：数値
            // ｘ
            // 戻値：補間の最小値
            // ｘ
            // 概要：オブジェクト同士を補間して結合する最小値を計算

            float SmoothMin(float fFirstNum, float fSecondNum, float fConstK)
            {
                float h = clamp(0.5 + 0.5 * (fSecondNum - fFirstNum) / fConstK, 0.0, 1.0);
                return lerp(fSecondNum, fFirstNum, h) - fConstK * h * (1.0 - h);
            }

            // 変数宣言
            float4 _Color;  // カラー

            // 最大256個の球を扱えるように定義
            uniform int _nSphereCount;      // 球体の数を計算
            uniform float4 _fSpheres[256];  // 球体のデータ （xyz: 中心, w: 半径）

            // ＞距離ゲット関数
            // 引数：float3 fPos：座標
            // ｘ
            // 戻値：計算した距離
            // ｘ
            // 概要：最も近い球までの距離を探す

            float GetDistance(float3 fPos)
            {
                float d = 1000.0;
                for (int i = 0; i < _nSphereCount; i++)
                {
                    float dist = length(fPos - _fSpheres[i].xyz) - _fSpheres[i].w;
                    d = SmoothMin(d, dist, 0.9);
                }
                return d;
            }

            // ＞法線計算関数
            // 引数：float3 fPos：座標
            // ｘ
            // 戻値：法線ベクトル
            // ｘ
            // 概要：法線（ノーマル）をSDFから計算

            float3 GetNormal(float3 fPos)
            {
                float eps = 0.001;
                return normalize(float3(
                    GetDistance(fPos + float3(eps, 0, 0)) - GetDistance(fPos - float3(eps, 0, 0)),
                    GetDistance(fPos + float3(0, eps, 0)) - GetDistance(fPos - float3(0, eps, 0)),
                    GetDistance(fPos + float3(0, 0, eps)) - GetDistance(fPos - float3(0, 0, eps))
                ));
            }

            // 頂点シェーダー
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
                OUT.worldPos = TransformObjectToWorld(IN.positionOS).xyz;
                return OUT;
            }

            // フラグメントシェーダー：ピクセルごとに球にレイを飛ばして色を決定
            FragOutput frag(Varyings IN)
            {
                
                FragOutput OUT;

                // レイの起点と方向
                float3 rayOrigin = GetCameraPositionWS(); // Unity 6 URPの安全なカメラ位置取得
                float3 rayDir = normalize(IN.worldPos - rayOrigin);
                float3 pos = rayOrigin;

                float dist;
                const int maxSteps = 64;
                for (int i = 0; i < maxSteps; i++)
                {
                    dist = GetDistance(pos);
                    if (dist < 0.001) break;
                    pos += rayDir * dist;
                }

                if (dist >= 0.001)
                {
                    OUT.color = float4(0, 0, 0, 0);
                    OUT.depth = IN.positionHCS.z / IN.positionHCS.w;
                    return OUT;
                }

                // ライティング
                float3 normal = GetNormal(pos);
                Light mainLight = GetMainLight();
                float3 lightDir = normalize(mainLight.direction);
                float diff = saturate(dot(normal, lightDir));
                float3 col = lerp(float3(0.05, 0.2, 0.1), float3(0.3, 1.0, 0.6), diff);

                // 最終カラー
                OUT.color = float4(col * _Color.rgb, _Color.a);

                // 深度書き込み（カメラ空間から取得）
                float4 clipPos = TransformWorldToHClip(pos);
                OUT.depth = clipPos.z / clipPos.w;

                return OUT;
            }

            ENDHLSL
        }
    }
}
