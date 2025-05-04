/*=====
<EffectRenderer.cs>
└作成者：tei

＞内容
エフェクトプレハブの各球体のプロパティを取得して、シェーダーに渡す

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
        // 初回だけコライダーを取得しておく
        m_Colliders = GetComponentsInChildren<SphereCollider>();

        // 球の数をマテリアルに渡す
        m_RenderMaterial.SetInt("_nSphereCount", m_Colliders.Length);
    }

    // ＞更新関数
    // 引数：なし
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：更新処理
    private void Update()
    {
        // 毎フレーム、各球の情報を更新
        for (int i = 0; i < m_Colliders.Length; i++)
        {
            var _Col = m_Colliders[i];
            var _T = _Col.transform;

            // 中心位置（ワールド座標）
            Vector3 _Center = _T.position;

            // 実際の半径（スケールが変わってたら考慮）
            float _Radius = _T.lossyScale.x * _Col.radius;

            // Vector4 で格納（x,y,z = 中心位置、w = 半径）
            m_Spheres[i] = new Vector4(_Center.x, _Center.y, _Center.z, _Radius);
        }

        // シェーダーに現在の球の数と配列を送信
        m_RenderMaterial.SetInt("_nSphereCount", m_Colliders.Length);
        m_RenderMaterial.SetVectorArray("_fSpheres", m_Spheres);
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
