/*=====
<EffectRenderer.cs>
└作成者：tei

＞内容
エフェクトプレハブの各球体のデータを管理

＞注意事項

＞更新履歴
__Y25
_M04
D
25：プログラム作成：tei
26：微調整とコメント追加：tei

_M05
D
01：スクリプト名、変数名修正：tei
04：コーディングルールの沿ってコード修正：tei
22：球体取得方式大改良、球体のデータを管理：tei
23：エフェクトクリア処理不具合修正：tei

=====*/

// 名前空間宣言
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Unity.VisualScripting;

// クラス定義
public class CEffectRenderer : MonoBehaviour
{
    // 定数定義
    private const int MAX_SPHERE_COUNT = 256;	// 同時に扱える最大球数（シェーダーの配列制限に合わせて）

    // 変数宣言
    [Header("マテリアル設定")]
    [SerializeField, Tooltip("マテリアル")] private Material m_RenderMaterial;

    private Vector4[] m_Spheres = new Vector4[MAX_SPHERE_COUNT]; // シェーダーに渡す「位置＋半径」の配列
    private SphereCollider[] m_Colliders; // 子オブジェクトにある SphereCollider を取得するための配列

    // === 散らばる球体の状態管理 ===
    // 球体1つ分のデータを格納するクラス（位置・速度・初期スケールなど）
    private class CSphereData
    {
        public Transform tf;          // 球体のTransform
        public Vector3 velocity;      // 初期散乱の速度
        public Vector3 initialScale;  // 最初の大きさ（縮小に使う）
    }

    [Header("プレハブ自動化ランダム生成用変数")]
    [SerializeField, Tooltip("エフェクトプレハブ")] private GameObject m_SpherePrefab;
    [SerializeField, Tooltip("球体最小数")] private int m_nMinSpheres = 5;
    [SerializeField, Tooltip("球体最大数")] private int m_nMaxSpheres = 10;
    [SerializeField, Tooltip("沈むスピード")] private float m_fFallSpeed = 0.5f;
    [SerializeField, Tooltip("縮小比率")] private float m_fShrinkAmount = 0.3f;
    [SerializeField, Tooltip("初速度の大きさ")] private float m_fInitialSpeed = 1.5f;
    [SerializeField, Tooltip("減速の強さ")] private float m_fDrag = 2.0f;
    [SerializeField, Tooltip("エフェクト生存時間")] private float m_fLifeTime = 5.0f;
    [SerializeField, Tooltip("生成した球体の大きさ(半径)")] private float m_SpawnRadius = 0.5f;
    [SerializeField, Tooltip("生成した球体の大きさのランダムスケール最小値")] private float m_MinScale = 0.2f;
    [SerializeField, Tooltip("生成した球体の大きさのランダムスケール最大値")] private float m_MaxScale = 0.6f;

    private List<CSphereData> m_SphereList = new();   // 球体トランスフォーム格納リスト
    private float m_fElapsedTime = 0.0f;            // 経過時間


    // ＞初期化関数
    // 引数：なし
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：初期化処理
    private void Start()
    {
        if (m_SpherePrefab == null)
        {
            Debug.LogError("SpherePrefab が設定されていません！");
            return;
        }

        if (m_SpherePrefab.GetComponent<CEffectRenderer>() != null)
        {
            Debug.LogError("SpherePrefab に CEffectRenderer が含まれており、無限ループになります！");
            return;
        }

        //プレハブ球体数乱数生成
        int count = Random.Range(m_nMinSpheres, m_nMaxSpheres + 1);

        for (int i = 0; i < count; i++)
        {
            // プレハブから球体生成（親は自分）
            GameObject sphere = Instantiate(m_SpherePrefab, transform);

            // ★ 散らばる方向にランダム配置（半径範囲内）
            Vector3 offset = Random.insideUnitSphere * m_SpawnRadius;
            sphere.transform.localPosition = offset;

            // ★ 大きさランダム
            float scale = Random.Range(m_MinScale, m_MaxScale);
            sphere.transform.localScale = Vector3.one * scale;

            // ★ ランダム初速度（XZ中心 + 少しY方向）
            Vector3 velocity = (Random.insideUnitSphere + Vector3.up * 0.5f).normalized * m_fInitialSpeed;

            // データ保存
            m_SphereList.Add(new CSphereData
            {
                tf = sphere.transform,
                velocity = velocity,
                initialScale = sphere.transform.localScale
            });
        }

        // 子オブジェクトのSphereColliderを取得
        m_Colliders = GetComponentsInChildren<SphereCollider>();

        // CDeathEffectManagerに自分を登録
        if (CDeathEffectManager.Instance != null)
        {
            CDeathEffectManager.Instance.Register(this);
        }
        else
        {
            Debug.LogWarning("[EffectRenderer] DeathEffectManagerがシーン上に存在しません。");
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
        m_fElapsedTime += Time.deltaTime;

        // 時間に応じて縮小量を計算（0～1）
        float shrinkT = Mathf.Clamp01(m_fElapsedTime / m_fLifeTime);

        foreach (var data in m_SphereList)
        {
            Transform tf = data.tf;

            // === ① 散らばる動き（初速度あり）===
            if (data.velocity.magnitude > 0.01f)
            {
                // 速度に基づいて移動
                tf.localPosition += data.velocity * Time.deltaTime;

                // 摩擦（減速）
                data.velocity = Vector3.Lerp(data.velocity, Vector3.zero, m_fDrag * Time.deltaTime);
            }
            else
            {
                // === ② 停止後に沈む処理 ===
                Vector3 pos = tf.localPosition;
                pos.y -= m_fFallSpeed * Time.deltaTime;
                tf.localPosition = pos;
            }

            // === ③ 時間で少しずつ縮小 ===
            float scaleFactor = Mathf.Lerp(1f, 1f - m_fShrinkAmount, shrinkT);
            tf.localScale = data.initialScale * scaleFactor;
        }
    }

    // ＞ゲット関数
    // 引数：なし
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：球体データを取得
    public Vector4[] GetSphereData()
    {
        // nullチェック：コライダー未取得または空の場合は空配列を返す
        if (m_Colliders == null || m_Colliders.Length == 0)
        {
            Debug.LogWarning($"[EffectRenderer] SphereCollider が未設定です（{gameObject.name}）");
            return new Vector4[0]; // 空の配列を返す
        }

        Vector4[] result = new Vector4[m_Colliders.Length];
        
        for (int i = 0; i < m_Colliders.Length; i++)
        {

            var col = m_Colliders[i];
            
            if (col == null) continue; // 個別のコライダーが null の場合もスキップ

            var pos = col.transform.position;
            var rad = col.radius * col.transform.lossyScale.x;
            result[i] = new Vector4(pos.x, pos.y, pos.z, rad);
        }
        return result;
    }

    
    // ＞エフェクト削除関数
    // 引数：なし
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：使用済みのエフェクト描画をクリアして削除します
    public void ClearEffect()
    {
        // 配列を空に
        for (int i = 0; i < m_Spheres.Length; i++)
        {
            m_Spheres[i] = Vector4.zero;
        }
    }
}
