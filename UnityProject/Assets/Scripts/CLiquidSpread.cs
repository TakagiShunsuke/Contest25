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

=====*/

// 名前空間宣言
using UnityEngine;

// クラス定義
public class CLiquidSpread : MonoBehaviour
{
    // 変数宣言

    [Header("拡散・表示・フェード設定")]
    [SerializeField] private float spreadDuration = 1.0f; // 拡散にかかる時間
    [SerializeField] private float stayDuration = 5.0f;   // 拡散後そのまま表示される時間
    [SerializeField] private float fadeDuration = 1.5f;   // フェードアウト時間
    private float startTime;
    private bool fadeStarted = false;

    [Header("ランダム拡散幅設定")]
    private Vector4 randomSpreadPlus;  // x方向, y方向
    private Vector4 randomSpreadMinus;  // -x方向, -y方向
    [SerializeField] private float randomSpreadPlusMin = 0.8f;
    [SerializeField] private float randomSpreadPlusMax = 1.2f;
    [SerializeField] private float randomSpreadMinusMin = 0.8f;
    [SerializeField] private float randomSpreadMinusMax = 1.2f;

    [Header("他のマテリアル設定")]
    private Material mat;
    private Collider hitbox;

    // シェーダーから変数名を取得
    private static readonly int RandomSpreadPlus_ID = Shader.PropertyToID("_RandomSpreadPlus");
    private static readonly int RandomSpreadMinus_ID = Shader.PropertyToID("_RandomSpreadMinus");

    // 外部から設定を受け取るため
    private Color liquidColor = Color.green;
    private float externalSpreadSpeed = 1.0f;


    // ＞設置関数
    // 引数１：Color color：色
    // 引数２：float spreadDuration：数値  // 拡散にかかる時間
    // 引数３：float stayDuration：数値    // 液体残る時間
    // 引数４：float fadeDuration：数値    // 液体消える時間
    // ｘ
    // 戻値：無
    // ｘ
    // 概要：拡散マテリアル設定する用
    public void Setup(Color color, float spreadDuration, float stayDuration, float fadeDuration)
    {
        this.liquidColor = color;
        this.externalSpreadSpeed = spreadDuration;
        this.stayDuration = stayDuration;
        this.fadeDuration = fadeDuration;
    }

    // ＞初期化関数
    // 引数：無
    // ｘ
    // 戻値：無
    // ｘ
    // 概要：実行時の初期設定、処理
    void Start()
    {
        mat = GetComponent<Renderer>().material;
        startTime = Time.time;

        // マテリアルパラメータ初期化
        mat.SetFloat("_StartTime", startTime);
        mat.SetFloat("_FadeStartTime", -1.0f);
        mat.SetFloat("_FadeDuration", fadeDuration);

        // ここでランダムな拡がり幅を決める！
        randomSpreadPlus = new Vector4(
            Random.Range(randomSpreadPlusMin, randomSpreadPlusMax), // 右
            Random.Range(randomSpreadPlusMin, randomSpreadPlusMax), // 上
            Random.Range(randomSpreadPlusMin, randomSpreadPlusMax), // 左
            Random.Range(randomSpreadPlusMin, randomSpreadPlusMax)  // 下
        );

        randomSpreadMinus = new Vector4(
            Random.Range(randomSpreadMinusMin, randomSpreadMinusMax), // 右上
            Random.Range(randomSpreadMinusMin, randomSpreadMinusMax), // 左上
            Random.Range(randomSpreadMinusMin, randomSpreadMinusMax), // 左下
            Random.Range(randomSpreadMinusMin, randomSpreadMinusMax)  // 右下
        );

        mat.SetVector(RandomSpreadPlus_ID, randomSpreadPlus);
        mat.SetVector(RandomSpreadMinus_ID, randomSpreadMinus);
        // 追加：もし色やスピードが外部から設定されてたら反映する
        mat.SetColor("_BaseColor", liquidColor);
        mat.SetFloat("_SpreadSpeed", externalSpreadSpeed);

        // 当たり判定設定(仮)
        // hitbox = gameObject.AddComponent<CircleCollider2D>();
        // ((CircleCollider2D)hitbox).isTrigger = true;
        // ((CircleCollider2D)hitbox).radius = 0.5f;
    }

    // ＞更新関数
    // 引数：無
    // ｘ
    // 戻値：無
    // ｘ
    // 概要：液体に関しての更新処理
    void Update()
    {
        float elapsed = Time.time - startTime;

        // フェード開始計算
        if (!fadeStarted && elapsed >= spreadDuration + stayDuration)
        {
            mat.SetFloat("_FadeStartTime", Time.time);
            fadeStarted = true;
        }
        // 使用済み(一定時間)オブジェクト削除
        if (fadeStarted && Time.time - (startTime + spreadDuration + stayDuration) >= fadeDuration)
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
