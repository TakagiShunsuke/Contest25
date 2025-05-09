/*=====
<Player.cs>
└作成者：kato

＞内容
プレイヤーの移動と攻撃を制御するスクリプト

＞更新履歴
__Y25
_M04
D
18:プレイヤーの移動と攻撃完成。:kato
21:表記ゆれの修正:takagi
22:キーの変数化:kato
23:攻撃キーの変数命名変更:kato
=====*/

// 名前空間宣言
using UnityEngine;

// クラス定義
public class a : MonoBehaviour, IDH
{
    // 変数宣言
    private Rigidbody m_Rb; // リジットボディ

    private float time = 0.0f;//たいまー
    private float count = 0.0f;//かうんと
    [Header("プレイヤーステータス")]
    [SerializeField]
    [Tooltip("HP")]
    private int m_nHp = 100;
    [SerializeField]
    [Tooltip("攻撃力")]
    private int m_nAtk = 100;
    [SerializeField]
    [Tooltip("移動速度")]
    private float m_fSpeed = 2.0f;
    [SerializeField]
    [Tooltip("攻撃速度")]
    private float m_fAtkSpeed = 100.0f; // 攻撃速度
    [SerializeField]
    [Tooltip("防御力")]
    private int m_nDef = 5;
    [SerializeField]
    [Tooltip("毒状態")]
    private bool m_bPoison = false;

    private bool b = false;
    [Header("攻撃ステータス")]
    [SerializeField]
    [Tooltip("攻撃の半径")]
    private float m_fAttackRange = 0.02f; // 攻撃の半径
    [SerializeField]
    [Tooltip("攻撃の角度")]
    private float m_fAttackAngle = 45.0f; // 45度の範囲
    private float m_fLastAttackTime = 0.0f; // 最後に攻撃した時間
    private float m_fAttackCooldown; // 攻撃のクールダウン時間
    private bool m_bIsDead = false; // プレイヤーが死んでいるかどうか

    // 攻撃キーの変数
    [SerializeField]
    [Tooltip("攻撃キー")]
    private KeyCode m_AttackKey = KeyCode.Return;

    // 初期化関数
    // 引数１：なし
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：初期化処理
    private void Start()
    {
        // プレイヤーの初期化
        m_Rb = GetComponent<Rigidbody>();
        m_fAttackCooldown = 1.0f / m_fAtkSpeed; // 攻撃速度に応じて攻撃間隔を設定
    }

    // 移動処理関数
    // 引数１：なし
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：プレイヤーの移動処理
    private void PlayerMove()
    {
        // プレイヤーの移動
        if (Input.GetKey(KeyCode.W))
        {
            m_Rb.transform.position += Vector3.forward * m_fSpeed; // 前
        }
        if (Input.GetKey(KeyCode.S))
        {
            m_Rb.transform.position += Vector3.back * m_fSpeed; // 後ろ
        }
        if (Input.GetKey(KeyCode.A))
        {
            m_Rb.transform.position += Vector3.left * m_fSpeed; // 左
        }
        if (Input.GetKey(KeyCode.D))
        {
            m_Rb.transform.position += Vector3.right * m_fSpeed;// 右
        }

        /*  滑らかに動くけど加速するパターンの移動
		//if (Input.GetKey(KeyCode.S))
		//{
		//    rb.AddForce(Vector3.back * speed);
		//}
		//if (Input.GetKey(KeyCode.A))
		//{
		//    rb.AddForce(Vector3.left * speed);
		//}
		//if (Input.GetKey(KeyCode.D))
		//{
		//    rb.AddForce(Vector3.right * speed);
		//}

		*/
    }

    // 攻撃関数
    // 引数１：なし
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：プレイヤーの攻撃
    private void Attack()
    {
        // 攻撃するためのクールダウン時間チェック
        if (Time.time - m_fLastAttackTime >= m_fAttackCooldown)
        {
            // 攻撃のクールダウン時間を更新
            m_fLastAttackTime = Time.time;

            // 攻撃の向き
            Vector3 _Forward = transform.forward;

            // 攻撃範囲の計算
            Collider[] _HitColliders = Physics.OverlapSphere(transform.position + _Forward * m_fAttackRange, m_fAttackRange);
            foreach (var _HitCollider in _HitColliders)
            {
                if (_HitCollider.gameObject == this.gameObject) continue;

                Vector3 _DirectionToTarget = _HitCollider.transform.position - transform.position;
                float fAngle = Vector3.Angle(_Forward, _DirectionToTarget);
                if (fAngle <= m_fAttackAngle / 2)
                {
                    // 敵に攻撃が当たったときの処理を追加したい場合ここに書く
                }
            }
        }
    }

    // 更新関数
    // 引数１：なし
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：プレイヤーが死んでいるかと死んだときの処理
    private void Update()
    {
        if (m_bIsDead) return; // プレイヤーが死んでいる場合は操作を無効にする

        // プレイヤーのHPが0以下になったとき
        if (m_nHp <= 0 && !m_bIsDead)
        {
            Die(); // 死ぬ
        }
        if (m_bPoison == true)//毒だったら
        {
            if (b == true)
            {
                count = 0.0f;
            }
            time += Time.deltaTime;
            count += Time.deltaTime;
            if (time >= 1.0f)//１びょうごと
            {
                m_nHp -= 5;
                Debug.Log("毒!5ダメージ現在のHP" + m_nHp);
                time = 0.0f;
            }
            if (count >= 5.0f)
            {
                m_bPoison = false;
            }
        }
        b = false;
    }

    // 物理更新関数
    // 引数１：なし
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：プレイヤーの移動処理と攻撃処理
    private void FixedUpdate()
    {
        // 移動処理
        PlayerMove();

        // プレイヤーの攻撃(Enter)
        if (Input.GetKeyDown(m_AttackKey))
        {
            Attack(); // 攻撃処理を呼び出す

        }
    }

    // 死ぬ関数
    // 引数１：なし
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：プレイヤーが死んだときに呼び出す処理
    private void Die()
    {
        m_bIsDead = true;
    }

    public void Adddamege(int damage)
    {
        m_nHp -= damage;
        Debug.Log("プレイヤーは" + damage + "をくらった　現在のHP:" + m_nHp);
        if (m_nHp < 0)//しんだら
        {
            Debug.Log("死んだ");
        }
    }
    public void Addheal(int heal)
    {
        m_nHp += heal;
        Debug.Log("プレイヤーは" + heal + "を回復した　現在のHP:" + m_nHp);
    }
    public void Addposion()
    {
        m_bPoison = true;
        b = true;
    }
    public void Addacid(int damage)
    {
        if (m_nHp > damage)
        {


            m_nHp -= damage;
            Debug.Log("プレイヤーは" + damage + "をくらった　現在のHP:" + m_nHp);
        }
        else
        {
            Debug.Log("酸だからしなん");
        }
    }
}
