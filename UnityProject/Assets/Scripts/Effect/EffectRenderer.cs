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

=====*/

// 名前空間宣言
using UnityEngine;
using UnityEngine.SceneManagement;

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


    // ＞初期化関数
    // 引数：なし
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：初期化処理
    private void Start()
    {
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

        // Shader へ反映（0個にする）
        m_RenderMaterial.SetInt("_nSphereCount", 0);
        m_RenderMaterial.SetVectorArray("_fSpheres", m_Spheres);
    }
}
