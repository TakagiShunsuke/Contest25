/*=====
<DeathEffect.cs>
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
04：複数敵の場合エフェクトが共通される問題を修正、コーディングルールの沿ってコード修正：tei

=====*/

// 名前空間宣言
using UnityEngine;

// クラス定義
public class CDeathEffect : MonoBehaviour
{
    // 変数宣言
    [Header("設定")]
    [SerializeField, Tooltip("表示される時間")] private float m_fLifeTime = 5f;
    [SerializeField, Tooltip("フェード時間")] private float m_fFadeDuration = 2f;
    [SerializeField, Tooltip("初期アルファ")] private float m_fStartAlpha = 0.9f;
    [SerializeField, Tooltip("フェード対象のマテリアル配列")] private Material[] m_FadeMaterials;
    [SerializeField, Tooltip("フェード対象マテリアル")]
    private Material m_OriginalMaterial;    // マテリアル本体
    private Material m_InstanceMaterial;    // 複製のマテリアル

    // プロパティ定義
    private float TimeCount { get; set; } = 0f;    // エフェクト表示時間カウンター
    private float FadeCount { get; set; } = 0f;    // エフェクトフェイド時間カウンター
    private bool IsFading { get; set; } = false;   // フェイドフラグ
    private CEffectRenderer EffectRenderer { get; set; }    // エフェクトクラス参照      


    // ＞初期化関数
    // 引数：なし
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：初期化処理

    private void Start()
    {
        if (m_OriginalMaterial != null)
        {
            // Renderer がない場合でもマテリアルを手動で複製・設定
            m_InstanceMaterial = new Material(m_OriginalMaterial);

            // 初期アルファ設定
            Color color = m_InstanceMaterial.GetColor("_Color");
            color.a = m_fStartAlpha;
            m_InstanceMaterial.SetColor("_Color", color);

            // 配列に入れてフェード処理対応
            m_FadeMaterials = new Material[1];
            m_FadeMaterials[0] = m_InstanceMaterial;
        }
        else
        {
            // 通常通り Renderer を探す処理
            Renderer[] _Renderers = GetComponentsInChildren<Renderer>();
            m_FadeMaterials = new Material[_Renderers.Length];

            for (int i = 0; i < _Renderers.Length; i++)
            {
                Material _NewMaterial = new Material(_Renderers[i].sharedMaterial);
                _Renderers[i].material = _NewMaterial;
                m_FadeMaterials[i] = _NewMaterial;

                Color color = _NewMaterial.GetColor("_Color");
                color.a = m_fStartAlpha;
                _NewMaterial.SetColor("_Color", color);
            }
        }

        // レンダラー取得
        EffectRenderer = GetComponent<CEffectRenderer>();
        if (EffectRenderer == null)
        {
            Debug.LogWarning("[DeathEffect] EffectRenderer がアタッチされていません。エフェクトの終了処理はスキップされます。");
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
        // フェイド計算
        TimeCount += Time.deltaTime;

        // 表示時間を過ぎたらフェード開始
        if (TimeCount >= m_fLifeTime && !IsFading)
        {
            IsFading = true;
            FadeCount = 0f;
        }

        // フェード中処理
        if (IsFading)
        {
            // フェード時間をカウント
            FadeCount += Time.deltaTime;

            // 進行割合（0～1）を計算
            float _T = Mathf.Clamp01(FadeCount / m_fFadeDuration);
            float _CurrentAlpha = Mathf.Lerp(m_fStartAlpha, 0f, _T);

            // 全マテリアルに対してアルファ値を適用
            foreach (Material mat in m_FadeMaterials)
            {
                if (mat != null)
                {
                    Color color = mat.GetColor("_Color");
                    color.a = _CurrentAlpha;
                    mat.SetColor("_Color", color);
                }
            }

            // フェード完了後の処理
            if (_T >= 1f)
            {
                StopEffect();
            }
        }
    }

    // ＞エフェクト停止関数
    // 引数：なし
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：エフェクトを停止し、関連処理を実行して削除する

    private void StopEffect()
    {
        if (EffectRenderer != null)
        {
            EffectRenderer.ClearEffect();
        }
        if (m_InstanceMaterial != null)
        {
            Destroy(m_InstanceMaterial);
        }

        Destroy(gameObject);
    }
}
