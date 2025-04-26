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
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Pass
        {
            ZWrite Off                          // 背景のものが透けるようにZWriteオフ
            Blend SrcAlpha OneMinusSrcAlpha     // 半透明用のブレンド設定

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // 構造体定義
            // 頂点データを受け取る
            struct VS_IN
            {
                float4 vertex : POSITION;
            };

            // 頂点シェーダーの出力
            struct VS_OUT
            {
                float4 pos : SV_POSITION;       // 空間座標
                float3 worldPos : TEXCOORD0;    // ワールド座標
            };

            // 頂点シェーダー：平面上の各点のワールド位置を取得
            VS_OUT vert(VS_IN v)
            {
                VS_OUT o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

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
                float fDistance = 1000; // 距離を格納用
                for (int i = 0; i < _nSphereCount; i++)
                {
                    float dis = length(fPos - _fSpheres[i].xyz) - _fSpheres[i].w;
                    fDistance = SmoothMin(fDistance, dis, 0.8); // 第3引数が滑らかさの強さ（大きくすると接合が広がる）
                }
                return fDistance;
            }

            // ＞法線計算関数
            // 引数：float3 fPos：座標
            // ｘ
            // 戻値：法線ベクトル
            // ｘ
            // 概要：法線（ノーマル）をSDFから計算

            float3 getNormal(float3 fPos)
            {
                float eps = 0.001; // 微小差分
                float3 dx = float3(eps, 0, 0);
                float3 dy = float3(0, eps, 0);
                float3 dz = float3(0, 0, eps);

                return normalize(float3(
                    GetDistance(fPos + dx) - GetDistance(fPos - dx),
                    GetDistance(fPos + dy) - GetDistance(fPos - dy),
                    GetDistance(fPos + dz) - GetDistance(fPos - dz)
                ));
            }
           
            fixed4 _Color;  // カラー

            // フラグメントシェーダー：ピクセルごとに球にレイを飛ばして色を決定
            fixed4 frag(VS_OUT i) : SV_Target
            {
                
                float3 rayOrigin = _WorldSpaceCameraPos; // レイの出発点（カメラ）
                float3 rayDir = normalize(i.worldPos - rayOrigin); // 平面のピクセルへ向かう方向

                float3 pos = rayOrigin; // 現在のレイの位置
                float dist = 0;
                
                // 最大64ステップまで進めて球との交差を探す
                for (int j = 0; j < 64; j++)
                {
                    // 原点からの距離を取得
                    dist = GetDistance(pos); // ここで複数球の距離を使う
                    
                    if (dist < 0.001) break; // 距離が小さい＝ヒット
                    pos += rayDir * dist; // ヒットしなければ距離ぶん進む
                }

                if (dist < 0.001)
                {
                    // ライトの向き＆法線計算
                    float3 normal = getNormal(pos); // スライム表面の法線

                    // シーンのDirectional Lightの向き
                    float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);

                    // Lamabert拡散光(0~1)
                    float diff = saturate(dot(normal, lightDir));


                    // トゥーン風段階
                    // 色定義
                    float3 shadowColor = float3(0.05, 0.2, 0.1); // 影
                    float3 lightColor  = float3(0.3, 1.0, 0.6);  // 明るい

                    // 陰影の滑らかさを調整
                    float shade = smoothstep(0.3, 0.7, diff);

                    // 明るさに応じて色を補間
                    float3 col = lerp(shadowColor, lightColor, shade);
                    
                    // 出力＆α値調整
                    return float4(col, _Color.a);
                }
                else
                {
                    return float4(0, 0, 0, 0);
                }
            }

            ENDCG
        }
    }
}
