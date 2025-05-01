/*=====
<CDeathEffect.cs>
└作成者：tei

＞内容
死亡エフェクトの設定、処理

＞注意事項

＞更新履歴
__Y25
_M04
D
25：プログラム作成：tei
26：微調整、コメント追加：tei
30：エフェクト再生問題修正：tei

_M05
D
01：スクリプト名、変数名修正：tei

=====*/
using UnityEngine;

public class CDeathEffect : MonoBehaviour
{
    [Header("設定")]
    [SerializeField, Tooltip("表示される時間")] private float m_fLifeTime = 5f;
    [SerializeField, Tooltip("フェード時間")] private float m_fFadeDuration = 2f;
    [SerializeField, Tooltip("初期アルファ")] private float m_fStartAlpha = 0.9f;
    [SerializeField, Tooltip("フェード対象のマテリアル")] private Material FadeMaterial;

    private float m_fTimecount { get; set; } = 0f;    // エフェクト表示時間カウンター
    private float m_fFadeCount { get; set; } = 0f;    // エフェクトフェイド時間カウンター
    private bool m_bIsFading { get; set; } = false;   // フェイドフラグ
    private CEffectRenderer EffectRenderer { get; set; }    // エフェクトクラス参照      

    // ＞初期化関数
    // 引数：なし
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：初期化処理

    void Start()
    {
        // Materialをインスタンス化して独立させる
        var renderer = GetComponentInChildren<Renderer>();

        if (renderer != null)
        {
            FadeMaterial = new Material(renderer.sharedMaterial); // ← sharedMaterialを複製！
            renderer.material = FadeMaterial;
        }

        // α値設定
        if (FadeMaterial != null)
        {
            Color color = FadeMaterial.color;
            color.a = m_fStartAlpha;
            FadeMaterial.color = color;
        }
        else
        {
            Debug.LogWarning("[CDeadEffect] FadeMaterial が設定されていません！");
        }

        // 各パラメータ初期化
        m_fTimecount = 0f;
        m_fFadeDuration = 0f;
        m_bIsFading = false;
        // レンダラー取得
        EffectRenderer = GetComponent<CEffectRenderer>();
    }

    // ＞更新関数
    // 引数：なし
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：更新処理

    void Update()
    {
        // フェイド計算
        m_fTimecount += Time.deltaTime;

        if (m_fTimecount >= m_fLifeTime && !m_bIsFading)
        {
            m_bIsFading = true;
        }

        // α値を計算して、フェイド処理に反映
        if (m_bIsFading)
        {
            // α値を時間によって薄くなる
            m_fFadeCount += Time.deltaTime;
            float t = Mathf.Clamp01(m_fFadeCount / m_fFadeDuration);
            float currentAlpha = Mathf.Lerp(m_fStartAlpha, 0f, t);

            if (FadeMaterial != null)
            {
                Color color = FadeMaterial.color;
                color.a = currentAlpha;
                FadeMaterial.color = color;
            }
            // α値が0になったらエフェクトの描画とオブジェクト自体を削除
            if (t >= 1f)
            {
                if (EffectRenderer != null)
                {
                    EffectRenderer.ClearEffect();
                }

                Destroy(gameObject);
            }
        }
    }
}
