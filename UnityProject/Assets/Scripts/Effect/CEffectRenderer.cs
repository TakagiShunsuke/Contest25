/*=====
<CEffectRenderer.cs>
└作成者：tei

＞内容
コーディング規約を記述

＞注意事項

＞更新履歴
__Y25
_M04
D
25：プログラム作成：tei
26：微調整とコメント追加：tei

=====*/using UnityEngine;
using UnityEngine.SceneManagement;

public class CEffectRenderer : MonoBehaviour
{
    [Header("マテリアル設定")]
    [SerializeField, Tooltip("マテリアル")] private Material material;

    private const int m_nMaxSphereCount = 256; // 同時に扱える最大球数（シェーダーの配列制限に合わせて）
    private Vector4[] Spheres = new Vector4[m_nMaxSphereCount]; // シェーダーに渡す「位置＋半径」の配列
    private SphereCollider[] Colliders; // 子オブジェクトにある SphereCollider を取得するための配列

     // ＞初期化関数
    // 引数：なし
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：初期化処理
    void Start()
    {

        // 初回だけコライダーを取得しておく
        Colliders = GetComponentsInChildren<SphereCollider>();

        // 球の数をマテリアルに渡す
        material.SetInt("_nSphereCount", Colliders.Length);
    }

    // ＞更新関数
    // 引数：なし
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：更新処理
    void Update()
    {
        // 毎フレーム、各球の情報を更新
        for (int i = 0; i < Colliders.Length; i++)
        {
            var col = Colliders[i];
            var t = col.transform;

            // 中心位置（ワールド座標）
            Vector3 center = t.position;

            // 実際の半径（スケールが変わってたら考慮）
            float radius = t.lossyScale.x * col.radius;

            // Vector4 で格納（x,y,z = 中心位置、w = 半径）
            Spheres[i] = new Vector4(center.x, center.y, center.z, radius);
        }

        // シェーダーに現在の球の数と配列を送信
        material.SetInt("_nSphereCount", Colliders.Length);
        material.SetVectorArray("_fSpheres", Spheres);

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
        for (int i = 0; i < Spheres.Length; i++)
        {
            Spheres[i] = Vector4.zero;
        }

        // Shader へ反映（0個にする）
        material.SetInt("_nSphereCount", 0);
        material.SetVectorArray("_fSpheres", Spheres);
    }
}
