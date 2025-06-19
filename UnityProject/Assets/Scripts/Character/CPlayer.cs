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
12:HP関係の機能を外部に切り出し:takagi
16:Rayで遊んでみる:kato
20:ローリングの時に移動するように:kato
22:効果音の追加(WASDで移動時とEnterで攻撃時のみ):kato
28:SE関係の変数にツールチップ追加:takagi
30:無敵状態の更新処理を追加:kato
_M06
D
06:たたきつけ攻撃完成！！！:kato
11:ノックバック仮追加:sezaki
18:アニメーションをいじいじ:kato
19:Slamアニメーション実装:kato
19:ローリングアニメーションいじいじ:kato
=====*/

// 名前空間宣言
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

// クラス定義
public class CPlayer : MonoBehaviour, IDH
{
    // 変数宣言
    private Rigidbody m_Rb; // リジットボディ

	private float m_RayDistance = 10.0f;

    private float m_ftime = 0.0f;//たいまー
    private float m_fcount = 0.0f;//かうんと

	[Header("プレイヤーステータス")]
	[SerializeField]
	[Tooltip("初期HP")]
	private int m_nInitialHp = 100;
	[SerializeField]
	[Tooltip("初期防御力")]
	private int m_nInitialDef = 5;
	private CHitPoint m_HitPoint;	// HP機構

	[SerializeField]
	[Tooltip("攻撃力")]
	private int m_nAtk = 100;
	[SerializeField]
	[Tooltip("移動速度")]
	private float m_fSpeed = 2.0f;
	[SerializeField]
	[Tooltip("攻撃速度")]
	private float m_fAtkSpeed = 100.0f;

	[Header("攻撃ステータス")]

	[SerializeField]
	[Tooltip("攻撃範囲の横幅")]
	private float m_fAttackBoxWidth = 2f;     // 攻撃範囲の横幅
	[SerializeField]
	[Tooltip("攻撃範囲の奥行き")]
	private float m_fAttackBoxDepth = 3f;     // 攻撃範囲の奥行き
	[SerializeField]
	[Tooltip("攻撃範囲の高さ")]
	private float m_fAttackBoxHeight = 1.5f;    // 攻撃範囲の高さ
	[SerializeField]
	[Tooltip("攻撃範囲の縦オフセット")]
	private float m_fAttackBoxYOffset = 1.0f;
	[SerializeField]
	[Tooltip("攻撃範囲の横オフセット")]
	private float m_fAttackBoxXOffset = 1.0f; // 横（X軸）オフセット

    [SerializeField]
    [Tooltip("スマッシュ攻撃範囲の横幅")]
    private float m_fSmashAttackBoxWidth = 2f;     // 攻撃範囲の横幅
    [SerializeField]
    [Tooltip("スマッシュ攻撃範囲の奥行き")]
    private float m_fSmashAttackBoxDepth = 3f;     // 攻撃範囲の奥行き
    [SerializeField]
    [Tooltip("スマッシュ攻撃範囲の高さ")]
    private float m_fSmashAttackBoxHeight = 1.5f;    // 攻撃範囲の高さ
    [SerializeField]
    [Tooltip("スマッシュ攻撃範囲の縦オフセット")]
    private float m_fSmashAttackBoxYOffset = 1.0f;
    [SerializeField]
    [Tooltip("スマッシュ攻撃範囲の横オフセット")]
    private float m_fSmashAttackBoxXOffset = 1.0f; // 横（X軸）オフセット

    private float m_fLastAttackTime = -Mathf.Infinity;	// 最後に攻撃した時間
	private float m_fAttackCooldown;	// 攻撃のクールダウン時間
	//private bool m_bIsDead = false;	// プレイヤーが死んでいるかどうか
	private bool m_bIsPoison = false; //プレイヤーが毒カ
	private bool m_bAttackInput = false;
    private bool m_bPoisonUpdate = false;//毒更新用

	// 攻撃キーの変数
	[SerializeField]
	[Tooltip("攻撃キー")]
	private KeyCode m_AttackKey = KeyCode.Return;

	[SerializeField]
	[Tooltip("ローリングキー")]
	private KeyCode m_RollingKey = KeyCode.Space; // ローリングキー

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
	private float m_fRollingInvincibleTime = 0.5f; // 一旦ね
	private bool m_bIsRollingInvincible = false; // ローリング中の無敵フラグ

	private bool m_bIsInvicible = false; // 無敵フラグ
	private int m_nInvincibleTime = 90; // 無敵時間

	[Header("プレイヤーのSE関係")]
	[SerializeField]
	[Tooltip("プレイヤーの足音の間隔")]
	private float m_fFootStepInterval = 0.4f;	// 足音の間隔
	private float m_fFootStepTimer = 0.0f;
	[SerializeField]
	[Tooltip("移動音")]
	private  AudioClip m_MoveGroundSE;
	[SerializeField]
	[Tooltip("移動音量")]
	private float m_MoveGroundSEVolume = 0.05f;
	private AudioSource m_MoveGroundSESource;	// 移動SE用のオーディオソース
	[SerializeField]
	[Tooltip("攻撃(突き)音")]
	public AudioClip m_StabAttackSE;
	[SerializeField]
	[Tooltip("攻撃(突き)音量")]
	private float m_StabAttackSEVolume = 0.05f;
	private AudioSource m_StabAttackSESource;	// 突きSE用のオーディオソース


    CEnemy enemy;

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
		m_fAttackCooldown = 100.0f / m_fAtkSpeed;	// 攻撃速度に応じて攻撃間隔を設定(攻撃速度100なら1秒　200なら0.5秒)
		m_Animator = GetComponent<Animator>();  // アニメーターコンポーネント取得


        // 音源準備
        m_MoveGroundSESource = gameObject.AddComponent<AudioSource>();	// 移動用の音源コンポーネント作成
		m_MoveGroundSESource.volume = m_MoveGroundSEVolume;	// 音量を設定
		m_StabAttackSESource = gameObject.AddComponent<AudioSource>();	// 突き用の音源コンポーネント作成
		m_StabAttackSESource.volume = m_StabAttackSEVolume;	// 音量を設定
		
		// HPの実装
		if(m_HitPoint = GetComponent<CHitPoint>())
		{
#if UNITY_EDITOR
			// 出力
			Debug.Log(this + "にはHitPointが設定されていますが、この設定は初期化される可能性があります");
#endif	// !UNITY_EDITOR
		}
		else
		{
			m_HitPoint = gameObject.AddComponent<CHitPoint>();	// HPの機能追加
		}

		// 初期値設定
		m_HitPoint.MaxHP = m_nInitialHp;	// 初期HP設定
		m_HitPoint.Defence = m_nInitialDef;	// 初期防御設定

		// イベント接続
		m_HitPoint.OnDead += OnDead;	// 死亡時処理を接続
	}

	// 入力をまとめて取得する関数
	// 引数１：なし
	// ｘ
	// 戻値：なし
	// ｘ
	// 概要：プレイヤーの入力処理
	private Vector3 GetMoveInput()
	{
		Vector3 input = Vector3.zero; // 入力値を格納する変数

		if (Input.GetKey(KeyCode.W)) input += Vector3.forward;
		if (Input.GetKey(KeyCode.S)) input += Vector3.back;
		if (Input.GetKey(KeyCode.A)) input += Vector3.left;
		if (Input.GetKey(KeyCode.D)) input += Vector3.right;

		return input.normalized;
	}

	// 障害物との当たり判定処理関数
	// 引数１：なし
	// ｘ
	// 戻値：なし
	// ｘ
	// 概要：プレイヤーの障害物との当たり判定処理
	private bool AdjustDistanceByRaycast(Vector3 origin, Vector3 direction, float originalDistance, out float adjustedDistance)
	{
		adjustedDistance = originalDistance;
		RaycastHit hit;

		// Rayの高さを設定する
		Vector3 OffSetOrigin = origin + Vector3.up * m_fRayHeight;

		if (Physics.Raycast(OffSetOrigin, direction, out hit, originalDistance + m_RayDistance))
		{
			if (hit.distance <= m_fAvoidDistance)
			{
				adjustedDistance = 0f;
				return true;
			}
			else if (hit.distance < originalDistance + m_fAvoidDistance)
			{
				adjustedDistance = hit.distance - m_fAvoidDistance;
				return true;
			}
		}
		return false;
	}

	// 移動処理関数
	// 引数１：なし
	// ｘ
	// 戻値：なし
	// ｘ
	// 概要：プレイヤーの移動処理
	private void PlayerMove()
	{
		Vector3 input = GetMoveInput();

		if (input != Vector3.zero)
		{
			Vector3 moveDir = input;
			Vector3 rayOrigin = transform.position + Vector3.up * m_fRayHeight;
			float moveDistance = m_fSpeed;

			// 距離補正
			AdjustDistanceByRaycast(rayOrigin, moveDir, moveDistance, out moveDistance);

			// 足音再生
			if (!m_MoveGroundSESource.isPlaying)
			{
				m_MoveGroundSESource.clip = m_MoveGroundSE;
				m_MoveGroundSESource.loop = true;
				m_MoveGroundSESource.Play();
			}

			// 移動・回転
			m_Rb.transform.position += moveDir * moveDistance;
			m_Rb.transform.rotation = Quaternion.LookRotation(moveDir);

            m_Animator.SetBool("Run",true); // 歩行アニメーションを再生
            //m_Animator.SetBool("Run", false); // 歩行アニメーションを停止
        }
        else
		{

            // 入力なし時の足音停止
            if (m_MoveGroundSESource.isPlaying)
			{
				m_MoveGroundSESource.Stop();
			}
			m_Animator.SetBool("Run", false); // 歩行アニメーションを停止
			//m_Animator.SetBool("Run", true); // 歩行アニメーションを停止
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
		m_Animator.SetBool("Rolling", true); 
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
		Vector3 RollDir = m_vRollDirection.normalized; //ローリング方向 
		Vector3 RayOrigin = transform.position + Vector3.up * m_fRayHeight; // Rayの原点

		//障害物との距離をチェックして、移動距離を補正する
		AdjustDistanceByRaycast(RayOrigin, RollDir, fRollSpeed, out fRollSpeed);

		transform.position += RollDir * fRollSpeed; // ローリング移動

		if (m_fRollTimer >= m_fRollDuration)
		{
			m_bIsRolling = false; // ローリング終了
		}
	}

	// 攻撃関数
	// 引数１：なし
	// ｘ
	// 戻値：なし
	// ｘ
	// 概要：プレイヤーの攻撃(槍)
	private void Attack()
	{
		if (Time.time - m_fLastAttackTime >= m_fAttackCooldown)
		{
			m_fLastAttackTime = Time.time;

			Vector3 origin = transform.position;
			Vector3 forward = transform.forward;

			Vector3 boxHalfExtents = new Vector3(
				m_fAttackBoxWidth * 0.5f,
				m_fAttackBoxHeight * 0.5f,
				m_fAttackBoxDepth * 0.5f
			);

			Vector3 boxCenter = origin + forward  * (m_fAttackBoxDepth * 0.5f)
								+ transform.up * (boxHalfExtents.y + m_fAttackBoxYOffset)
								+ transform.right * m_fAttackBoxXOffset;
			//boxCenter.y += boxHalfExtents.y + m_fAttackBoxYOffset;

			// Debug表示
			//DebugDrawBox(boxCenter, boxHalfExtents, transform.rotation, Color.red, 500f);

			Collider[] hitColliders = Physics.OverlapBox(boxCenter, boxHalfExtents, transform.rotation);
			foreach (var hit in hitColliders)
			{
				if (hit.gameObject == this.gameObject) continue;

				var enemy = hit.GetComponent<CEnemy>();
				if (enemy != null)
				{
					enemy.Damage(m_nAtk,this.transform);
				}
			}

			m_Animator.SetBool("Attack", true); // 攻撃アニメーションを再生
        }

        // 攻撃音再生
        if (!m_StabAttackSESource.isPlaying)
		{
			m_StabAttackSESource.PlayOneShot(m_StabAttackSE);
		}
	}

    // 攻撃関数
    // 引数１：なし
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：プレイヤーの攻撃(叩きつけ)
	private void SmashAttack()
    {
        Debug.Log(m_fAttackCooldown);

        if (Time.time - m_fLastAttackTime >= m_fAttackCooldown)
        {
            m_fLastAttackTime = Time.time;

            Debug.Log("スマッシュ攻撃!!");

            Vector3 origin = transform.position;
            Vector3 forward = transform.forward;

            Vector3 boxHalfExtents = new Vector3(
                m_fSmashAttackBoxWidth * 0.5f,
                m_fSmashAttackBoxHeight * 0.5f,
                m_fSmashAttackBoxDepth * 0.5f
            );

            Vector3 boxCenter = origin + forward * (m_fSmashAttackBoxDepth * 0.5f)
                                + transform.up * (boxHalfExtents.y + m_fSmashAttackBoxYOffset)
                                + transform.right * m_fSmashAttackBoxXOffset;
            //boxCenter.y += boxHalfExtents.y + m_fAttackBoxYOffset;

            // Debug表示
            //DebugDrawBox(boxCenter, boxHalfExtents, transform.rotation, Color.red, 500f);

            Collider[] hitColliders = Physics.OverlapBox(boxCenter, boxHalfExtents, transform.rotation);
            foreach (var hit in hitColliders)
            {
                if (hit.gameObject == this.gameObject) continue;

                var enemy = hit.GetComponent<CEnemy>();
                if (enemy != null)
                {
                    enemy.Damage(m_nAtk, this.transform);
                }
            }
        }

		m_Animator.SetBool("Slam", true); // スマッシュアタックアニメーションを再生
										  // (アニメーションイベントでfalse読んでるからfalseの記述はなし
        // 攻撃音再生
        if (!m_StabAttackSESource.isPlaying)
        {
            m_StabAttackSESource.PlayOneShot(m_StabAttackSE);
        }
    }

    // ↓後で消すやつ
    private void DrawSmashAttackDebugBox()
    {
        Vector3 origin = transform.position;
        Vector3 forward = transform.forward;

        Vector3 boxHalfExtents = new Vector3(
            m_fSmashAttackBoxWidth * 0.5f,
            m_fSmashAttackBoxHeight * 0.5f,
            m_fSmashAttackBoxDepth * 0.5f
        );

        Vector3 boxCenter = origin
            + forward * (m_fSmashAttackBoxDepth * 0.5f)
            + transform.up * (boxHalfExtents.y + m_fSmashAttackBoxYOffset)
            + transform.right * m_fSmashAttackBoxXOffset;

        DebugDrawBox(boxCenter, boxHalfExtents, transform.rotation,Color.magenta, 0f); // ← duration 0でもOK
    }

    // ↓後で消すやつ
    private void DrawAttackDebugBox()
	{
		Vector3 origin = transform.position;
		Vector3 forward = transform.forward;

		Vector3 boxHalfExtents = new Vector3(
			m_fAttackBoxWidth * 0.5f,
			m_fAttackBoxHeight * 0.5f,
			m_fAttackBoxDepth * 0.5f
		);

		Vector3 boxCenter = origin
			+ forward * (m_fAttackBoxDepth * 0.5f)
			+ transform.up * (boxHalfExtents.y + m_fAttackBoxYOffset)
			+ transform.right * m_fAttackBoxXOffset;

		DebugDrawBox(boxCenter, boxHalfExtents, transform.rotation, Color.blue, 0f); // ← duration 0でもOK
	}

	// ボックスの可視化関数
	private void DebugDrawBox(Vector3 center, Vector3 halfExtents, Quaternion rotation, Color color, float duration)
	{
		Vector3[] points = new Vector3[8];
		Matrix4x4 matrix = Matrix4x4.TRS(center, rotation, Vector3.one);

		// ボックスの8点をローカルからワールドに変換
		Vector3 he = halfExtents;
		points[0] = matrix.MultiplyPoint3x4(new Vector3(-he.x, -he.y, -he.z));
		points[1] = matrix.MultiplyPoint3x4(new Vector3(he.x, -he.y, -he.z));
		points[2] = matrix.MultiplyPoint3x4(new Vector3(he.x, -he.y, he.z));
		points[3] = matrix.MultiplyPoint3x4(new Vector3(-he.x, -he.y, he.z));
		points[4] = matrix.MultiplyPoint3x4(new Vector3(-he.x, he.y, -he.z));
		points[5] = matrix.MultiplyPoint3x4(new Vector3(he.x, he.y, -he.z));
		points[6] = matrix.MultiplyPoint3x4(new Vector3(he.x, he.y, he.z));
		points[7] = matrix.MultiplyPoint3x4(new Vector3(-he.x, he.y, he.z));

		// 下部
		Debug.DrawLine(points[0], points[1], color, duration);
		Debug.DrawLine(points[1], points[2], color, duration);
		Debug.DrawLine(points[2], points[3], color, duration);
		Debug.DrawLine(points[3], points[0], color, duration);

		// 上部
		Debug.DrawLine(points[4], points[5], color, duration);
		Debug.DrawLine(points[5], points[6], color, duration);
		Debug.DrawLine(points[6], points[7], color, duration);
		Debug.DrawLine(points[7], points[4], color, duration);

		// 垂直
		Debug.DrawLine(points[0], points[4], color, duration);
		Debug.DrawLine(points[1], points[5], color, duration);
		Debug.DrawLine(points[2], points[6], color, duration);
		Debug.DrawLine(points[3], points[7], color, duration);
	}


	// 更新関数
	// 引数１：なし
	// ｘ
	// 戻値：なし
	// ｘ
	// 概要：プレイヤーが死んでいるかと死んだときの処理
	private void Update()
	{
        if (m_HitPoint.IsDead) return;   // プレイヤーが死んでいる場合は操作を無効にする

		if(Input.GetKeyDown(m_AttackKey))
		{
			m_bAttackInput = true; // 攻撃入力フラグを立てる
        }

		//DrawAttackDebugBox();

		//// プレイヤーのHPが0以下になったとき
		//if(m_nHp <= 0 && !m_bIsDead)
		//{
		//	Die(); // 死ぬ
		//}
		if (m_bIsPoison == true)//毒だったら
		{
			if (m_bPoisonUpdate == true)
			{
				m_fcount = 0.0f;
			}
			m_ftime += Time.deltaTime;
			m_fcount += Time.deltaTime;
			if (m_ftime >= 1.0f)//１びょうごと
			{
				//m_nHp -= 5;
				//Debug.Log("毒!5ダメージ現在のHP" + m_nHp);
				m_HitPoint.HP -= 5;
				Debug.Log("毒!5ダメージ現在のHP" + m_HitPoint.HP);
				m_ftime = 0.0f;
			}
			if (m_fcount >= 5.0f)
			{
				m_bIsPoison = false;
			}
			m_bPoisonUpdate = false;
		}

		if(!m_bIsRolling)
		{
			m_fRollingCoolTimer += Time.deltaTime; // ローリングのクールタイムを加算

			if(Input.GetKeyDown(m_RollingKey) && m_fRollingCoolTimer >= m_fRollCooldown)
			{
				StartRolling(); // ローリング開始
				StartCoroutine(RollingInvincibilityCoroutine()); // ローリング中の無敵時間開始
			}
		}

		Vector3 origin = transform.position + Vector3.up * m_fRayHeight;

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

       if(m_bAttackInput)
		{
			if(GetMoveInput().magnitude < 0.01f)
			{
				SmashAttack(); // スマッシュ攻撃
				
                m_bAttackInput = false; // 攻撃入力フラグをリセット
            }
            else
            {
                Attack(); // Playerが動いているときは通常攻撃
                m_bAttackInput = false; // 攻撃入力フラグをリセット
            }
           
        }
	}

	//// 死ぬ関数
	//// 引数１：なし
	//// ｘ
	//// 戻値：なし
	//// ｘ
	//// 概要：プレイヤーが死んだときに呼び出す処理
	//private void Die()
	//{
	//	m_bIsDead = true;
	//}

	void OnDrawGizmos()
	{
		//Gizmos.color = new Color(1, 0, 0, 0.5f);
		//Gizmos.DrawCube(transform.position + new Vector3(0,1,0), new Vector3(1, 2, 1));
		//DrawAttackDebugBox();
		DrawSmashAttackDebugBox();
        // ray表示
        Gizmos.color = Color.green;
		Gizmos.DrawLine(transform.position + Vector3.up * m_fRayHeight, transform.position + Vector3.up * m_fRayHeight + transform.forward * 1.0f);
	}

	// ＞ダメージ関数	//TODO:敵の「攻撃」動作にAffectとしてDamageをアタッチ
	// 引数：なし
	// ｘ
	// 戻値：なし
	// ｘ
	// 概要：ダメージを受ける
	public void Damage(int _nDamage, Transform attacker, int weight)
	{
		
		if (m_bIsInvicible) return; // 無敵状態ならダメージを受けない

		if (_nDamage <= m_HitPoint.Defence)// 防御が被ダメを上回ったら被ダメを1にする
		{
			_nDamage = 1;
		}
		else// ダメージを与える
		{
			_nDamage = _nDamage - m_HitPoint.Defence;
		}

		//m_nHp -= _nDamage; // ダメージ処理
		m_HitPoint.HP -= _nDamage; // ダメージ処理

        StartCoroutine(KnockbackCoroutine(attacker, weight));

        // 無敵状態開始
        StartCoroutine(InvincibilityCoroutine());
		
	}

    /// <summary>
    /// -ノックバック関数	
    /// <para>ノックバックする関数</para>
    /// <param name="Transform attacker">相手の向いてる方向</param>
    /// </summary>

    private IEnumerator KnockbackCoroutine(Transform attacker, int weight)
    {
        Debug.Log("ノックバック開始！");

       Vector3 knockbackDir = (transform.position - attacker.position).normalized;
        knockbackDir.y = 0f; // Y方向の動きをゼロにする
        knockbackDir = knockbackDir.normalized; // 正規化
        float knockbackPower = weight * 0.2f;      // ノックバックの力（距離 or スピード）
        float knockbackTime = 0.2f;        // ノックバック時間
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + knockbackDir * knockbackPower;
        float _fTimer = 0f;
        
        while (_fTimer < knockbackTime) //ノックバックの指定時間の間
        { //自分を後方へノックバック
            float t = _fTimer / knockbackTime;
            t = 1f - (1f - t) * (1f - t);
            Vector3 newPos = Vector3.Lerp(startPos, endPos, t);

            m_Rb.MovePosition(newPos);
            _fTimer += Time.deltaTime;
            yield return null;
        }


        Debug.Log("ノックバック終了！");
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
		for (int i = 0; i < m_nInvincibleTime; ++i)
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
		m_bIsRollingInvincible = true; // 無敵状態にする
		yield return new WaitForSeconds(m_fRollingInvincibleTime); // 一定時間待つ
		m_bIsRollingInvincible = false; // 無敵状態を解除する
	}

	private void OnDrawGizmosSelected() // オブジェクト選択時に表示
	{
#if UNITY_EDITOR
	// セレクトした時のDebug処理をここに追加
#endif
	}

		// 死亡時処理
	private void OnDead()
	{
		SceneManager.LoadScene("GAMEOVER");
		Destroy(gameObject);	// プレイヤーを消す
	}


	//---↓消す---
	//ダメージ処理
	public void Adddamege(int damage)
	{
		//m_nHp -= damage;
		//Debug.Log("プレイヤーは" + damage + "をくらった　現在のHP:" + m_nHp);
		//if (m_nHp < 0)//しんだら
		//{
		//	Debug.Log("死んだ");
		//}
		m_HitPoint.HP -= damage;
		Debug.Log("プレイヤーは" + damage + "をくらった　現在のHP:" + m_HitPoint.HP);
		if (m_HitPoint.HP < 0)//しんだら
		{
			Debug.Log("死んだ");
		}
	}


	public void Addheal(int heal)
	{
		//m_nHp += heal;
		//Debug.Log("プレイヤーは" + heal + "を回復した　現在のHP:" + m_nHp);
		m_HitPoint.HP += heal;
		Debug.Log("プレイヤーは" + heal + "を回復した　現在のHP:" + m_HitPoint.HP);
	}
	public void Addposion()
	{
		m_bIsPoison = true;
		m_bPoisonUpdate = true;
	}
	public void Addacid(int damage)
	{
		//if (m_nHp > damage)
		//{


		//	m_nHp -= damage;
		//	Debug.Log("プレイヤーは" + damage + "をくらった　現在のHP:" + m_nHp);
		//}
		if (m_HitPoint.HP > damage)
		{


			m_HitPoint.HP -= damage;
			Debug.Log("プレイヤーは" + damage + "をくらった　現在のHP:" + m_HitPoint.HP);
		}
		else
		{
			Debug.Log("酸だからしなん");
		}
	}

    public void OnAttackAnimationEnd()
	{
		m_Animator.SetBool("Attack", false);
	}

	public void OnSlamAttackAnimationEnd()
	{
        m_Animator.SetBool("Slam", false);
    }

	public void OnRollingAnimationEnd()
	{
		m_Animator.SetBool("Rolling", false);
	}

}