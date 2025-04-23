/*=====
<CLiquidSpread.cs>
└作成者：tei

＞内容
液体拡散設定、処理

＞注意事項
　・プランナーがマテリアル調整する場合「拡散・表示・フェード設定」と「ランダム拡散幅設定」
　の変数数値を調整すればできます。
　・エフェクトの生成について「設置関数」使用すればできます。例：敵死亡時Setupを呼び出す。
　・当たり判定の初期化と処理は仮で入れましたが、追加の処理が書かないと当たり判定とかはいません。

＞更新履歴
　　__Y25
　_M04
D
18：プログラム作成：tei
19：処理微調整、コメント追加：tei
21：拡散サイズと拡散速度の変数が別々で渡す、
　　SpreadSpeedとSpreadDurationの誤用修正（一つの変数に変更）：tei
22：ハンガリアン記法で命名修正、ランダム拡散用変数名修正、処理を仕様に近く書き方へ変更、各変数コメント追加、
　　Start、Update関数プライベート化：tei
23：コーディング規約に従う修正、SetUp関数が使わないためコメントアウト

=====*/

// 名前空間宣言
using UnityEngine;

// クラス定義
public class CLiquidSpread : MonoBehaviour
{
    // 変数宣言

    [Header("拡散・表示・フェード設定")]
    [SerializeField, Tooltip("拡散にかかる時間")] private float m_fSpreadDuration = 1.0f;
    [SerializeField, Tooltip("拡散後にそのまま表示される時間")] private float m_fStayDuration = 10.0f;
    [SerializeField, Tooltip("フェードアウト時間")] private float m_fFadeDuration = 1.5f;
    [SerializeField, Tooltip("拡散サイズ")] private float m_fMaxSpread = 0.4f;
    [Tooltip("拡散開始時間")] private float fStartTime { get; set; }
    [Tooltip("拡散開始フラグ")] private bool bFadeStarted { get; set; } = false;

    [Header("ランダム拡散幅設定")]
    [Tooltip("直線 x = y 方向の拡散")] public Vector4 RandomSpreadBottomLeftToTopRight { private get; set; }
    [Tooltip("直線 -x = y 方向")] public Vector4 RandomSpreadTopLeftToBottomRight { private get; set; }
    [Tooltip("拡散前のベースサイズ")] private float m_fRandomSpreadBase { get; set; } = 1.0f;
    [Tooltip("拡散大きさの調整値(20%)")] public float m_fRandomSpreadAdjust { private get; set; } = 0.2f;

    private Material matMaterial;   // マテリアル
    //private Collider cldHitbox;

    // シェーダーのプロパティID取得
    private static readonly int nRandomSpreadBottomLeftToTopRight_ID = Shader.PropertyToID("_RandomSpreadBottomLeftToTopRight");    // ここの各ID変数コマンドをSH_LiquidSpread.shaderに参照
    private static readonly int nRandomSpreadTopLeftToBottomRight_ID = Shader.PropertyToID("_RandomSpreadTopLeftToBottomRight");
    private static readonly int nStartTime_ID = Shader.PropertyToID("_StartTime");
    private static readonly int nFadeStartTime_ID = Shader.PropertyToID("_FadeStartTime");
    private static readonly int nFadeDuration_ID = Shader.PropertyToID("_FadeDuration");
    private static readonly int nSpreadDuration_ID = Shader.PropertyToID("_SpreadDuration");
    private static readonly int nMaxSpread_ID = Shader.PropertyToID("_MaxSpread");
    private static readonly int nBaseColor_ID = Shader.PropertyToID("_BaseColor");

    // Setで受け取るよう
    [Tooltip("マテリアル色設定")] public Color clrLiquidColor { private get; set; } = Color.green;
    [Tooltip("拡散時間セット")] public float fSetUpSpreadDuration { private get; set; } = -1;


    // ＞初期化関数
    // 引数：無
    // ｘ
    // 戻値：無
    // ｘ
    // 概要：実行時の初期設定、処理
    private void Start()
    {
        // マテリアルゲット
        matMaterial = GetComponent<Renderer>().material;

        // スタート時間設定
        fStartTime = Time.time;

        // マテリアルパラメータ初期化
        matMaterial.SetFloat(nStartTime_ID, fStartTime);
        matMaterial.SetFloat(nFadeStartTime_ID, -1.0f);
        matMaterial.SetFloat(nFadeDuration_ID, m_fFadeDuration);

        // ランダムな拡がり幅を決める
        RandomSpreadBottomLeftToTopRight = new Vector4(
            Random.Range(m_fRandomSpreadBase - m_fRandomSpreadAdjust, m_fRandomSpreadBase + m_fRandomSpreadAdjust), // 右
            Random.Range(m_fRandomSpreadBase - m_fRandomSpreadAdjust, m_fRandomSpreadBase + m_fRandomSpreadAdjust), // 上
            Random.Range(m_fRandomSpreadBase - m_fRandomSpreadAdjust, m_fRandomSpreadBase + m_fRandomSpreadAdjust), // 左
            Random.Range(m_fRandomSpreadBase - m_fRandomSpreadAdjust, m_fRandomSpreadBase + m_fRandomSpreadAdjust)  // 下
        );

        // ランダムな拡がり幅を決める
        RandomSpreadTopLeftToBottomRight = new Vector4(
            Random.Range(m_fRandomSpreadBase - m_fRandomSpreadAdjust, m_fRandomSpreadBase + m_fRandomSpreadAdjust), // 右上
            Random.Range(m_fRandomSpreadBase - m_fRandomSpreadAdjust, m_fRandomSpreadBase + m_fRandomSpreadAdjust), // 左上
            Random.Range(m_fRandomSpreadBase - m_fRandomSpreadAdjust, m_fRandomSpreadBase + m_fRandomSpreadAdjust), // 左下
            Random.Range(m_fRandomSpreadBase - m_fRandomSpreadAdjust, m_fRandomSpreadBase + m_fRandomSpreadAdjust)  // 右下
        );

        matMaterial.SetVector(nRandomSpreadBottomLeftToTopRight_ID, RandomSpreadBottomLeftToTopRight);
        matMaterial.SetVector(nRandomSpreadTopLeftToBottomRight_ID, RandomSpreadTopLeftToBottomRight);
        
        // もし色やスピードが外部から設定されてたら反映する
        matMaterial.SetColor(nBaseColor_ID, clrLiquidColor);
        matMaterial.SetFloat(nSpreadDuration_ID, fSetUpSpreadDuration > 0 ? fSetUpSpreadDuration : m_fSpreadDuration);
        matMaterial.SetFloat(nMaxSpread_ID, m_fMaxSpread);

      
        // 当たり判定設定(仮)
        // cldHitbox = gameObject.AddComponent<CircleCollider2D>();
        // ((CircleCollider2D)hitbox).isTrigger = true;
        // ((CircleCollider2D)hitbox).radius = 0.5f;
    }

    // ＞更新関数
    // 引数：無
    // ｘ
    // 戻値：無
    // ｘ
    // 概要：液体に関しての更新処理
    private void Update()
    {
        // 経過時間計算
        float fElapsed = Time.time - fStartTime;

        // フェード開始計算
        if (!bFadeStarted && fElapsed >= m_fSpreadDuration + m_fStayDuration)
        {
            matMaterial.SetFloat(nFadeStartTime_ID, Time.time);
            bFadeStarted = true;
        }
        // 使用済み(一定時間)オブジェクト削除
        if (bFadeStarted && Time.time - (fStartTime + m_fSpreadDuration + m_fStayDuration) >= m_fFadeDuration)
        {
            Destroy(gameObject);
        }
    }

    // 当たり判定処理->プレイヤーが液体に触れたら反応(仮)
    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         Debug.Log("プレイヤーが液体に触れた！");
    //         
    //         // TODO:プレイヤーが液体を触ったら処理を追加
    //     }
    // }
}
