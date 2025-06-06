/*=====
<DeathEffectManager.cs>
└作成者：tei

＞内容
EffectRenderer.csから球データをまとめて、シェーダーに送信

＞注意事項
球体データの上限は256個、上限超えた場合は超えた分が切り捨てられるので、表示がバグると思います。

＞更新履歴
__Y25
_M05
D
22：プログラム作成：tei

=====*/

// 名前空間宣言
using System.Collections.Generic;
using UnityEngine;

// クラス定義
public class CDeathEffectManager : MonoBehaviour
{
    // 定数定義
    public static CDeathEffectManager Instance { get; private set; }    // シングルトンパターン（シーンに1つだけ存在）

    // 変数宣言
    [Header("共通マテリアル（全エフェクトに適用）")]
    [SerializeField, Tooltip("SH_DeathEffect用のマテリアルを指定")] private Material m_SharedMaterial;

    private List<CEffectRenderer> EffectRenderers = new List<CEffectRenderer>();    // 登録された全ての EffectRenderer を保持

    private Vector4[] SphereBuffer = new Vector4[256];  // 全エフェクトから集めたスフィア情報を一時的に保存する配列

    // クラス定義
    // 効果用RTと位置を記録するクラス
    private class EffectDataEntry
    {
        public RenderTexture RenderTexture;
        public Vector3 WorldPosition;

        public EffectDataEntry(RenderTexture rt, Vector3 pos)
        {
            RenderTexture = rt;
            WorldPosition = pos;
        }
    }

    private List<EffectDataEntry> m_EffectDataList = new List<EffectDataEntry>();

    // ＞初期化関数
    // 引数：なし
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：初期化処理
    private void Awake()
    {
        // シングルトンインスタンスの設定
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // ＞登録関数
    // 引数：CEffectRenderer renderer：スクリプト
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：EffectRendererからのデータを登録
    public void Register(CEffectRenderer renderer)
    {
        if (!EffectRenderers.Contains(renderer))
        {
            EffectRenderers.Add(renderer);
        }
    }

    // ＞登録解除関数
    // 引数：CEffectRenderer renderer：スクリプト
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：マネージャから登録したデータを解除する
    public void Unregister(CEffectRenderer renderer)
    {
        if (EffectRenderers.Contains(renderer))
        {
            EffectRenderers.Remove(renderer);
        }
    }

    // ＞更新関数
    // 引数：なし
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：更新処理
    private void Update()
    {
        int _Index = 0;

        // 登録されたすべてのEffectRendererからスフィア情報を収集
        foreach (var renderer in EffectRenderers)
        {
            var spheres = renderer.GetSphereData();
            foreach (var sphere in spheres)
            {
                if (_Index >= 256) break; // 256個を超える場合は切り捨て
                SphereBuffer[_Index++] = sphere;
            }
        }

        // マテリアルに現在のスフィア数と配列をシェーダーに渡す
        if (m_SharedMaterial != null)
        {
            m_SharedMaterial.SetInt("_nSphereCount", _Index);
            m_SharedMaterial.SetVectorArray("_fSpheres", SphereBuffer);
        }
    }

    

    // ＞登録関数
    // 引数：RenderTexture rt：データ用RenderTexture, Vector3 worldPos：テクスチャのワールドポス
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：データ格納用のレンダーテクスチャのデータを登録
    public void RegisterEffectTexture(RenderTexture rt, Vector3 worldPos)
    {
        if (rt != null)
        {
            m_EffectDataList.Add(new EffectDataEntry(rt, worldPos));
            // Debug.Log($"デカール効果データ登録: {worldPos}");
        }
    }

    // ＞ゲッター関数
    // 引数：なし
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：他のスクリプトでアクセスしたい時使う
    public IReadOnlyList<RenderTexture> GetAllEffectTextures()
    {
        return m_EffectDataList.ConvertAll(e => e.RenderTexture);
    }

    // ＞登録解除関数
    // 引数：なし
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：レンダーテクスチャの登録解除
    public void UnregisterEffectTexture(RenderTexture rt)
    {
        m_EffectDataList.RemoveAll(e => e.RenderTexture == rt);
    }
}
