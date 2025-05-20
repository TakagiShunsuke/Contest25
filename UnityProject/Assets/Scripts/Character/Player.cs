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
25:プレイヤーの移動制限追加:kato yuma
26:スペースの修正:takagi
27:プレイヤーの移動宣言をpublic～privateに変更:kato
_MO5
D
07:攻撃の判定がおかしかったので修正:kato
08:攻撃のクールダウン時間を修正:kato
08:ダメージ発生を仮置き:takagi
16:Rayで遊んでみる:kato
20:ローリングの時に移動するように:kato
=====*/

// 名前空間宣言
using Unity.VisualScripting;
using UnityEngine;
using System.Collections; // 無敵状態用

// クラス定義
public class CPlayer : MonoBehaviour
{
	// 変数宣言
	private Rigidbody m_Rb; // リジットボディ

	private float m_RayDistance = 10.0f;

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
	private float m_fAtkSpeed = 100.0f;	// 攻撃速度
	[SerializeField]
	[Tooltip("防御力")]
	private int m_nDef = 5;

	[Header("攻撃ステータス")]
	[SerializeField]
	[Tooltip("攻撃の半径")]
	private float m_fAttackRange = 0.02f;	// 攻撃の半径
	[SerializeField]
	[Tooltip("攻撃の角度")]
	private float m_fAttackAngle = 45.0f;	// 45度の範囲
	private float m_fLastAttackTime = -Mathf.Infinity;	// 最後に攻撃した時間
	private float m_fAttackCooldown;	// 攻撃のクールダウン時間
	private bool m_bIsDead = false;	// プレイヤーが死んでいるかどうか

	// 攻撃キーの変数
	[SerializeField]
	[Tooltip("攻撃キー")]
	private KeyCode m_AttackKey = KeyCode.Return;

	// アニメーター関連の変数
	public Animator m_Animator;	// アニメーター変数維持用
	private bool m_bWalkInput	= false;	// 移動入力フラグ
	private bool m_bAttack		= false;	// 攻撃フラグ
	public bool m_bOnGround	= true;	// 地面にいるかどうかのフラグ

	[Header("プレイヤー移動制限")]
	// プレイヤー移動制限用の変数

	[SerializeField]
	[Tooltip("プレイヤーの移動制限範囲の原点")]
	private Vector3 m_vMoveLimitOrigin = Vector3.zero; // プレイヤーの移動制限範囲の原点
	[SerializeField]
	[Tooltip("プレイヤーの移動制限範囲X")]
	private float m_fMoveLimit_x = 10.0f;	// プレイヤーの移動制限範囲
	[SerializeField]
	[Tooltip("プレイヤーの移動制限範囲Z")]
	private float m_fMoveLimit_z = 10.0f;   // プレイヤーの移動制限範囲

	[SerializeField]
	[Tooltip("Rayによる障害物回避距離")]
	private float m_fAvoidDistance = 1.0f;  // 障害物との最低距離

    [SerializeField]
	[Tooltip("PlayerRayの高さ")]
	private float m_fRayHeight = 1.5f; // Rayの高さ

	[Header("プレイヤーのローリング関係")]

    [SerializeField]
    [Tooltip("Playerがローリングするときの除算処理固定値")]
    private float m_fRollSpeed = 0.05f; // 移動速度*0.05用
    [SerializeField]
    [Tooltip("Playerのリーリングのクールタイム")]
    private float m_fRollCooldown = 3.0f; // ローリングのクールタイム(秒)

	private bool m_bIsRolling = false; // ローリングフラグ
    private float m_fRollingCoolTimer = 0.0f; // ローリングが最後に行われてからの経過時間
	private float m_fRollTimer = 0.0f; // ローリング中の経過時間
	private Vector3 m_vRollDirection; // ローリングの方向
	private float m_fRollDuration = 0.3f; // ローリングの持続時間

	// ローリング中の無敵時間
	[SerializeField]
	[Tooltip("Playerのローリング中の無敵時間")]
	private float m_fRollingInvicibleTime = 0.5f; // 一旦ね
	private bool m_bIsRollingInvicible = false; // ローリング中の無敵フラグ

    private bool m_bIsInvicible = false; // 無敵フラグ
	private int m_nInvicibleTime = 90; // 無敵時間

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
		m_fAttackCooldown = 1.0f / m_fAtkSpeed;	// 攻撃速度に応じて攻撃間隔を設定
	}

	// 移動処理関数
	// 引数１：なし
	// ｘ
	// 戻値：なし
	// ｘ
	// 概要：プレイヤーの移動処理
	private void PlayerMove()
	{
		Ray ray = new Ray(transform.position, transform.forward); // プレイヤーの前方にRayを飛ばす
        RaycastHit hit; // Rayの当たり判定を格納する変数

        Vector3 input = new Vector3(); // 入力値を格納する変数

		if (Input.GetKey(KeyCode.W))
		{
            input += Vector3.forward;

        }
        if (Input.GetKey(KeyCode.S))
		{
            input += Vector3.back;

        }
        if (Input.GetKey(KeyCode.A))
		{
            input += Vector3.left;

        }
        if (Input.GetKey(KeyCode.D))
		{
            input += Vector3.right;

        }

        if (input != Vector3.zero)
        {
            input.Normalize();
            Vector3 moveDir = input;
			Vector3 RayPosition = transform.position + Vector3.up * m_fRayHeight; // Rayの位置をプレイヤーの位置に設定
            float moveDistance = m_fSpeed;


            // Rayを前方に飛ばして障害物との距離をチェック
            if (Physics.Raycast(RayPosition, moveDir, out hit, m_RayDistance))
            {
                

                if (hit.distance <= m_fAvoidDistance)
                {
                    // 近すぎて移動しない
                    moveDistance = 0;
                }
                else if (hit.distance < moveDistance + m_fAvoidDistance)
                {
                    // 距離を調整して止まる
                    moveDistance = hit.distance - m_fAvoidDistance;
                }
            }

            m_Rb.transform.position += moveDir * moveDistance;

            

            // 回転
            m_Rb.transform.rotation = Quaternion.LookRotation(moveDir);

        }

    // プレイヤーの移動 正面向けるけどw+dが変になる移動
    /*
    if (Input.GetKey(KeyCode.W))
    {
        m_Rb.transform.position += Vector3.forward * m_fSpeed;	// 前
        m_Rb.transform.rotation = Quaternion.Euler(0, 0, 0);    // 前を向く
    }
    if (Input.GetKey(KeyCode.S))
    {
        m_Rb.transform.position += Vector3.back * m_fSpeed;	// 後ろ
        m_Rb.transform.rotation = Quaternion.Euler(0, 180, 0);  // 後ろを向く
    }
    if (Input.GetKey(KeyCode.A))
    {
        m_Rb.transform.position += Vector3.left * m_fSpeed;	// 左
        m_Rb.transform.rotation = Quaternion.Euler(0, 270, 0);  // 左を向く
    }
    if (Input.GetKey(KeyCode.D))
    {
        m_Rb.transform.position += Vector3.right * m_fSpeed;    // 右
        m_Rb.transform.rotation = Quaternion.Euler(0, 90, 0);   // 右を向く
    }
    */

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

	// ローリング関数
	// 引数１：なし
	// ｘ
	// 戻値：なし
	// ｘ
	// 概要：プレイヤーのローリングの初期化処理関数
	private void StartRolling()
	{
		m_bIsRolling = true;
		m_fRollTimer = 0.0f;
        m_fRollingCoolTimer = 0.0f; // ローリングのクールタイムをリセット
        m_vRollDirection = transform.forward; // ローリングの方向を設定

		// アニメーションの再生があればここで再生する
    }

    // ローリング関数
    // 引数１：なし
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：プレイヤーのローリング処理
	private void RollMovement()
	{
		m_fRollTimer += Time.fixedDeltaTime;

		float fRollSpeed = m_fSpeed * m_fRollSpeed; // ローリングの移動速度

		m_Rb.MovePosition(m_Rb.position + m_vRollDirection * fRollSpeed);

		if(m_fRollTimer >= m_fRollDuration)
		{
			m_bIsRolling = false; // ローリング終了
        }
    }

    // 攻撃関数
    // 引数１：なし
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：プレイヤーの攻撃
    private void Attack()
	{
		if (Time.time - m_fLastAttackTime >= m_fAttackCooldown)
		{
			m_fLastAttackTime = Time.time;

			Vector3 forward = transform.forward;
			Vector3 origin = transform.position;

			// 周囲のコライダーを一定範囲で取得（円形）
			Collider[] hitColliders = Physics.OverlapSphere(origin, m_fAttackRange);
			foreach (var hit in hitColliders)
			{
				if (hit.gameObject == this.gameObject) continue;

				Vector3 toTarget = hit.transform.position - origin;
				toTarget.y = 0f; // 高さ無視（XZ平面のみで計算）

				// 距離チェック（このチェックはOverlapsphereがやってるけど一応）
				if (toTarget.magnitude > m_fAttackRange) continue;

				// 扇型の角度内か判定
				float angle = Vector3.Angle(forward, toTarget.normalized);
				if (angle <= m_fAttackAngle * 0.5f)
				{
					// デバッグ用
					Debug.Log("Hit target: " + hit.name);

					// TODO: 敵に攻撃処理を追加
					var _EnemyScript = hit.gameObject.GetComponent<CEnemy>();
					if(_EnemyScript != null)
					{
						_EnemyScript.Damage(m_nAtk);	// 一時的なダメージ処理
						Debug.Log("AttackHit!");
					}
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
		if(m_bIsDead) return;	// プレイヤーが死んでいる場合は操作を無効にする

		// プレイヤーのHPが0以下になったとき
		if(m_nHp <= 0 && !m_bIsDead)
		{
			Die(); // 死ぬ
			Destroy(this.gameObject); // プレイヤーを消す
        }

		if(!m_bIsRolling)
		{
			m_fRollingCoolTimer += Time.deltaTime; // ローリングのクールタイムを加算

			if(Input.GetKeyDown(KeyCode.Space) && m_fRollingCoolTimer >= m_fRollCooldown)
			{
				StartRolling(); // ローリング開始
				StartCoroutine(RollingInvincibilityCoroutine()); // ローリング中の無敵時間開始
            }
        }

        Vector3 origin = transform.position + Vector3.up * m_fRayHeight;
        Debug.DrawRay(origin, transform.forward * 1.0f, Color.green);

    }

	// 物理更新関数
	// 引数１：なし
	// ｘ
	// 戻値：なし
	// ｘ
	// 概要：プレイヤーの移動処理と攻撃処理
	private void FixedUpdate()
	{
		if(m_bIsRolling)
		{
			RollMovement();
		}
		else
		{
            // 移動処理
            PlayerMove();
        }
        // プレイヤーの移動制限
        Vector3 _NowPosition = transform.position;  // 現在の位置を取得

        _NowPosition.x = Mathf.Clamp(_NowPosition.x, m_vMoveLimitOrigin.x - m_fMoveLimit_x, m_vMoveLimitOrigin.x + m_fMoveLimit_x);
        _NowPosition.z = Mathf.Clamp(_NowPosition.z, m_vMoveLimitOrigin.z - m_fMoveLimit_z, m_vMoveLimitOrigin.z + m_fMoveLimit_z);

        transform.position = _NowPosition;  // プレイヤーの位置を制限範囲内に収める

        // プレイヤーの攻撃(Enter)
        if (Input.GetKeyDown(m_AttackKey))
		{
			Attack();	// 攻撃処理を呼び出す

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

	void OnDrawGizmos()
	{
		Gizmos.color = new Color(1, 0, 0, 0.5f);
		Gizmos.DrawCube(transform.position + new Vector3(0,1,0), new Vector3(1, 2, 1));
		
	}

	// ＞ダメージ関数
	// 引数：なし
	// ｘ
	// 戻値：なし
	// ｘ
	// 概要：ダメージを受ける
	public void Damage(int _nDamage)
	{
		if (m_bIsInvicible) return; // 無敵状態ならダメージを受けない

        if (_nDamage <= m_nDef)// 防御が被ダメを上回ったら被ダメを1にする
		{
			_nDamage = 1;
		}
		else// ダメージを与える
		{
			_nDamage = _nDamage - m_nDef;
		}

		m_nHp -= _nDamage; // ダメージ処理

		
		// 無敵状態開始
		StartCoroutine(InvincibilityCoroutine());
        
    }

    // ＞無敵状態関数
    // 引数：なし
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：ダメージを受けたときに90フレーム無敵状態になる
    private IEnumerator InvincibilityCoroutine()
    {
        m_bIsInvicible = true; // 無敵状態にする
        Debug.Log("無敵状態!!!");
        for (int i = 0; i < m_nInvicibleTime; ++i)
		{
			yield return null; // 1フレーム待つ
        }

        m_bIsInvicible = false; // 無敵状態を解除する
		Debug.Log("無敵状態解除!!");
    }

    // ＞無敵状態関数(ローリング中)
    // 引数：なし
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：ローリングしている間一定時間無敵になる
	private IEnumerator RollingInvincibilityCoroutine()
	{
        m_bIsRollingInvicible = true; // 無敵状態にする
		yield return new WaitForSeconds(m_fRollingInvicibleTime); // 一定時間待つ
        m_bIsRollingInvicible = false; // 無敵状態を解除する
    }

    private void OnDrawGizmosSelected() // オブジェクト選択時に表示
	{
#if UNITY_EDITOR
		Gizmos.color = new Color(1, 1, 0, 0.4f);

		Vector3 offSet = new Vector3(0, 1, 0);
		Vector3 origin = transform.position + offSet;
		Vector3 forward = transform.forward;
		int segments = 30; // 表示する線の数（多いほどなめらか）

		for (int i = 0; i <= segments; i++)
		{
			float angle = -m_fAttackAngle / 2 + m_fAttackAngle * i / segments;
			Quaternion rot = Quaternion.Euler(0, angle, 0);
			Vector3 dir = rot * forward;
			Gizmos.DrawLine(origin, origin + dir.normalized * m_fAttackRange);
		}

		Gizmos.color = new Color(0, 0, 1, 1.0f);
		Vector3 start = transform.position + offSet;
        Vector3 end = transform.position + transform.forward * 20.0f + offSet;
		Gizmos.DrawLine(start, end);
#endif
	}
}
