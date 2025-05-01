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

=====*/
using UnityEngine;

public class CDeathEffect : MonoBehaviour
{
    [Header("設定")]
    [SerializeField, Tooltip("表示される時間")] private float fLifeTime = 5f;
    [SerializeField, Tooltip("フェード時間")] private float fFadeDuration = 2f;
    [SerializeField, Tooltip("フェード対象のマテリアル")] private Material mFadeMaterial;
    [SerializeField, Tooltip("初期アルファ")] private float fStartAlpha = 0.9f;

    private float fTimeCount { get; set; } = 0f;    // エフェクト表示時間カウンター
    private float fFadeCount { get; set; } = 0f;    // エフェクトフェイド時間カウンター
    private bool bIsFading { get; set; } = false;   // フェイドフラグ
    private CEffectRenderer effectRenderer { get; set; }    // エフェクトクラス参照      

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
            mFadeMaterial = new Material(renderer.sharedMaterial); // ← sharedMaterialを複製！
            renderer.material = mFadeMaterial;
        }

        // α値設定
        if (mFadeMaterial != null)
        {
            Color color = mFadeMaterial.color;
            color.a = fStartAlpha;
            mFadeMaterial.color = color;
        }
        else
        {
            Debug.LogWarning("[CDeadEffect] mFadeMaterial が設定されていません！");
        }

        // 各パラメータ初期化
        fTimeCount = 0f;
        fFadeDuration = 0f;
        bIsFading = false;
        // レンダラー取得
        effectRenderer = GetComponent<CEffectRenderer>();
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
        fTimeCount += Time.deltaTime;

        if (fTimeCount >= fLifeTime && !bIsFading)
        {
            bIsFading = true;
        }

        // α値を計算して、フェイド処理に反映
        if (bIsFading)
        {
            // α値を時間によって薄くなる
            fFadeCount += Time.deltaTime;
            float t = Mathf.Clamp01(fFadeCount / fFadeDuration);
            float currentAlpha = Mathf.Lerp(fStartAlpha, 0f, t);

            if (mFadeMaterial != null)
            {
                Color color = mFadeMaterial.color;
                color.a = currentAlpha;
                mFadeMaterial.color = color;
            }
            // α値が0になったらエフェクトの描画とオブジェクト自体を削除
            if (t >= 1f)
            {
                if (effectRenderer != null)
                {
                    effectRenderer.ClearEffect();
                }

                Destroy(gameObject);
            }
        }
    }
}
