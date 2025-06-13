/*=====
<Enemy.cs>
└作成者：sezaki

＞内容
敵（プロトタイプ）制作

＞更新履歴
__Y25
_M04
D
16:プログラム作成:sezaki
23:変数名修正・
	Damage()関数のダメージ値を引数化:sezaki
23:ファイルモードをspc→タブに変更:takagi
25:navMesh追加:sezaki
25：死亡エフェクト用のオブジェクト宣言と敵死亡した後のエフェクト生成処理追加：tei
_M05
D
1:攻撃追加
9:臨時的にスポナー対応の追跡機能拡張:takagi
9:Enemyを生成時に近くのナビメッシュにワープする
	プレイヤーを自動でターゲットするように修正:sezaki
9:ターゲットを自動取得していたので非シリアライズ化:takagi
12:CHitPointを適用:takagi
14:成長のHP計算を訂正:takagi
21:ステータス、成長修正:sezaki
21:成長力を切り出し、その他細かいリファクタリング作業:takagi
28:エフェクトをマージ:takagi
30:HPの仕様変更に伴い、成長処理を最大HPに反映:takagi
_M06
D
11:、敵がダメージを受けたとき赤く光る、攻撃範囲を成長するように:sezaki 
=====*/

// 名前空間宣言
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// クラス定義
public class CEnemy : MonoBehaviour, IDH
{
	// 構造体定義
	[Serializable]
	public struct Status	// 敵ステータス
	{
		[SerializeField, Tooltip("攻撃力")] public int m_nAtk;
		[SerializeField, Tooltip("攻撃速度")] public float m_fAtkSpeed;
		[SerializeField, Tooltip("重量")] public int m_nWeight;
		[SerializeField, Tooltip("成長度")] public int m_nGrowth;
		[SerializeField, Tooltip("成長上限")] public int m_nGrowthLimit;
		[SerializeField, Tooltip("成長速度")] public int m_nGrowthSpeed;
		[SerializeField, Tooltip("成長力")] public int m_nGrowthPower;
		[SerializeField, Tooltip("攻撃距離")] public float m_fAtkRange;
		[SerializeField, Tooltip("攻撃角度")] public float m_fAtkAngle;
        [SerializeField, Tooltip("ノックバック調整値")] public float m_fBack;
        [SerializeField, Tooltip("停止時間")] public float m_fStop;
    }
    [Serializable]
	public struct GrowthRate	// 成長割合
	{
		[SerializeField, Tooltip("体力成長割合")] public float m_fHP;
		[SerializeField, Tooltip("攻撃成長割合")] public float m_fAtk;
		[SerializeField, Tooltip("移動成長割合")] public float m_fMoveSpeed;
		[SerializeField, Tooltip("攻撃速度成長割合")] public float m_fAtkSpeed;
		[SerializeField, Tooltip("防御成長割合")] public float m_fDef;
		[SerializeField, Tooltip("重量成長割合")] public float m_fWeight;
        [SerializeField, Tooltip("攻撃距離成長割合")] public float m_fAtkRange;
        [SerializeField, Tooltip("攻撃角度成長割合")] public float m_fAtkAngle;
        [SerializeField, Tooltip("停止距離")] public float m_fStop;
    }

    // 変数宣言
    private CHitPoint m_HitPoint;   // HP
    [SerializeField] private Material flashMaterial; //ダメージ受けたときのマテリアル
    private float m_ftime = 0.0f;//たいまー
    private float m_fcount = 0.0f;//かうんと
    private bool m_bIsPoison = false; //プレイヤーが毒カ
    private bool m_bPoisonUpdate = false;//毒更新用
    [Header("ステータス")]
	[SerializeField, Tooltip("ステータス")] private Status m_Status;
	[SerializeField, Tooltip("成長力")] private GrowthRate m_Growth;
	private Status m_StatusInitial;	// ステータス初期値
	private Transform m_Target;	// プレイヤーのTransform
	private float m_fGrowthInterval;	// 成長間隔（秒）
	private float m_fGrowthTimer = 0f;	// 成長タイマー
	private float m_fAtkCooldown = 0f;	// 攻撃のクールタイム
	private float m_fSpeedInitial;	// 速度初期値
	[SerializeField, Tooltip("初期HP")]　private int m_nInitialHP;
	[SerializeField, Tooltip("防御力")] public int m_nInitialDef;
	private float m_fScale;	//サイズ変更
	private NavMeshAgent m_Agent;	// 追跡対象
	[SerializeField, Tooltip("体液")] GameObject m_Blood;

    private Rigidbody m_Rigid;                     // Rigidbody参照
    private bool m_IsKnockback = false; //ノックバックフラグ

    private float m_fStop;

    private Material defaultMaterial; //通常のマテリアル
    private Renderer rend; //レンダラー

    [Header("エフェクト")]
	[SerializeField, Tooltip("エフェクトプレハブ")] private GameObject deathEffectPrefab;


	/// <summary>
	/// -初期化関数
	/// <para>初期化処理関数</para>
	/// </summary>
	private void Start()
	{
		// NavMeshAgentを取得
		m_Agent = GetComponent<NavMeshAgent>();
        m_Rigid = GetComponent<Rigidbody>();
        // Playerを自動で探してターゲットに設定
        GameObject playerObj = GameObject.FindWithTag("Player");
		if (playerObj != null)
		{
			m_Target = playerObj.transform;
		}

		// 地面の位置を探す
		if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 10f, NavMesh.AllAreas))
		{
			m_Agent.Warp(hit.position);	// NavMeshの地面にワープさせる
		}

        rend = GetComponentInChildren<Renderer>();
        if (defaultMaterial == null && rend != null)
        {
            defaultMaterial = rend.sharedMaterial;
        }

        // HPの実装
        if (m_HitPoint = GetComponent<CHitPoint>())
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
		m_HitPoint.MaxHP = m_nInitialHP;	// 初期HP設定
		m_HitPoint.Defence = m_nInitialDef;	// 初期防御設定

		//初期ステータスを確保
		m_StatusInitial = m_Status;
		m_fSpeedInitial = m_Agent.speed;
		m_nInitialHP = m_HitPoint.HP;
        m_fStop = m_Agent.stoppingDistance;

		// イベント接続
		m_HitPoint.OnDead += OnDead;	// 死亡時処理を接続
	}

	/// <summary>
	/// -更新関数
	/// <para>更新処理関数</para>
	/// </summary>
	private void Update()
	{
		m_fAtkCooldown -= Time.deltaTime;	// 経過時間で減らす

            //追跡
            m_Agent.SetDestination(m_Target.position);

            // 攻撃
            Attack();

        //デバフ
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
        // 成長
        Growth();
	}


	/// <summary>
	/// -攻撃関数
	/// <para>Playerに近づいたら攻撃する関数</para>
	/// </summary>
	public void Attack()
	{
		if (!m_Agent.pathPending && m_Agent.remainingDistance <= m_Agent.stoppingDistance)// 敵に最大限近づいたら
		{
			if (m_fAtkCooldown > 0f) return;	// クールタイムが終わったら

			// 一定範囲内のコライダーを検出
			Collider[] hits = Physics.OverlapSphere(transform.position, m_Status.m_fAtkRange);

			foreach (Collider hit in hits)
			{
				if (hit.CompareTag("Player"))	// タグがPlayerのオブジェクトを探す
				{
					Vector3 dirToTarget = (hit.transform.position - transform.position).normalized; 
					//float angle = Vector3.Angle(transform.forward, dirToTarget);
					float angle = Vector2.Angle(new Vector2(transform.forward.x, transform.forward.z), new Vector2(dirToTarget.x, dirToTarget.z));	// 平面上で判定
					
					if (angle < m_Status.m_fAtkAngle / 2f)	// 攻撃範囲内に
					{
						//Playerがいる時
						CPlayer player = hit.GetComponent<CPlayer>();
						if (player != null)
						{
							player.Damage(m_Status.m_nAtk,this.transform, m_Status.m_nWeight);
							m_fAtkCooldown = 10.0f / m_Status.m_fAtkSpeed;
						}
					}
				}
			}
		}
	}


	/// <summary>
	/// -攻撃範囲関数
	/// <para>攻撃範囲を視覚化する関数</para>
	/// </summary>
	private void OnDrawGizmos()
	{
		int _nsegments = 30;	// 扇形を構成する線の本数

		Gizmos.color = new Color(1, 0, 0, 0.3f);	// 赤・半透明に設定

		Vector3 origin = transform.position;	// 敵の現在位置
		Quaternion startRotation = Quaternion.Euler(0, -m_Status.m_fAtkAngle / 2, 0);	// 左端の角度に回す
		Vector3 startDirection = startRotation * transform.forward;	// 左端方向を出す

		Vector3 prevPoint = origin + startDirection * m_Status.m_fAtkRange;

		for (int i = 1; i <= _nsegments; i++)
		{
			float angle = -m_Status.m_fAtkAngle / 2 + (m_Status.m_fAtkAngle / _nsegments) * i;
			Quaternion rot = Quaternion.Euler(0, angle, 0);	// 各線の角度
			Vector3 direction = rot * transform.forward;	// 回転させて方向取得
			Vector3 point = origin + direction * m_Status.m_fAtkRange;

			Gizmos.DrawLine(origin, point);	// 原点→点の線
			Gizmos.DrawLine(prevPoint, point);	// 前回の点→今回の点で扇形の辺
			prevPoint = point;	// 次のループ用に保存
		}
	}

	/// <summary>
	/// -成長関数
	/// <para>時間ごとに敵のステータスを上げる関数</para>
	/// </summary>
	public void Growth()
	{
		//成長しないならスキップ
		if (m_Status.m_nGrowthSpeed == 0)
		{
			return;
		}
		//成長速度を求める
		m_fGrowthInterval = 100 / m_Status.m_nGrowthSpeed;
		m_fGrowthTimer += Time.deltaTime;
		//成長する
		if (m_fGrowthTimer >= m_fGrowthInterval)
		{
			// 成長度が上限に達していたら何もしない
			if (m_Status.m_nGrowth >= m_Status.m_nGrowthLimit)
			{
				m_Status.m_nGrowth = m_Status.m_nGrowthLimit; 
				return;
			}
			//m_Status.m_nHp += m_Status.m_nGrowth + m_Status.m_nGrowthSpeed;
			//m_HitPoint.HP += (int)(m_Status.m_nGrowthSpeed * 0.1f);
			//m_HitPoint.HP = m_HitPoint.HP + (int)(m_nInitialHP * m_Growth.m_fHP);
			m_HitPoint.MaxHP += (int)(m_nInitialHP * m_Growth.m_fHP);
			m_Status.m_nAtk = m_Status.m_nAtk + (int)(m_StatusInitial.m_nAtk * m_Growth.m_fAtk);
			m_Agent.speed = m_Agent.speed + (m_fSpeedInitial * m_Growth.m_fMoveSpeed);
			m_Status.m_fAtkSpeed = m_Status.m_fAtkSpeed + (m_StatusInitial.m_fAtkSpeed * m_Growth.m_fAtkSpeed);
			//m_Status.m_nDef = m_Status.m_nDef + (int)(m_StatusInitial.m_nDef * m_Growth.m_fDef);
			m_HitPoint.Defence = m_HitPoint.Defence + (int)(m_nInitialDef * m_Growth.m_fDef);
			m_Status.m_nWeight = m_Status.m_nWeight + (int)(m_StatusInitial.m_nWeight * m_Growth.m_fWeight);
			m_Status.m_nGrowth = m_Status.m_nGrowth + m_Status.m_nGrowthPower;
			m_fScale = m_Status.m_nGrowth / m_StatusInitial.m_nGrowth;
            m_Status.m_fAtkAngle = m_Status.m_fAtkAngle + (int)(m_StatusInitial.m_fAtkAngle * m_Growth.m_fAtkAngle);
            m_Status.m_fAtkRange = m_Status.m_fAtkRange + (int)(m_StatusInitial.m_fAtkRange * m_Growth.m_fAtkRange);
            transform.localScale += new Vector3(m_fScale, m_fScale, m_fScale);
            m_Agent.stoppingDistance = m_Agent.stoppingDistance + (int)(m_fStop * m_Growth.m_fStop);
			// 成長度が上限を超えたら、上限に揃えておく
			if (m_Status.m_nGrowth > m_Status.m_nGrowthLimit)
			{
				m_Status.m_nGrowth = m_Status.m_nGrowthLimit;
			}

			m_fGrowthTimer = 0f;
		}
	}

    /// <summary>
    /// -ダメージ関数	//TODO:プレイヤーの「攻撃」動作にAffectとしてDamageをアタッチ
    /// <para>ダメージを受ける関数</para>
    /// <param name="_nDamage">相手の攻撃力</param>
    /// <param name="Transform attacker">相手の向いてる方向</param>
    /// </summary>
    public void Damage(int _nDamage, Transform attacker)
	{
		if (_nDamage <= m_HitPoint.Defence)	// 防御が被ダメを上回ったら被ダメを1にする
		{
			_nDamage = 1;
		}
		else	// ダメージを与える
		{
			_nDamage = _nDamage - m_HitPoint.Defence;
		}

		//m_Status.m_nHp -= _nDamage;　// ダメージ処理
		m_HitPoint.HP -= _nDamage;  // ダメージ処理

        StartCoroutine(KnockbackCoroutine(attacker));
        StartCoroutine(FlashRedCoroutine());  
        //if (m_Status.m_nHp <= 0)	// HPが0の時
        //if (m_HitPoint.HP <= 0)	// HPが0の時
        //{
        //	Destroy(gameObject);	// 敵を消す
        //}
    }

    /// <summary>
    /// -ノックバック関数	
    /// <para>ノックバックする関数</para>
    /// <param name="Transform attacker">相手の向いてる方向</param>
    /// </summary>

    private IEnumerator KnockbackCoroutine(Transform attacker)
    {
        Debug.Log("ノックバック開始！");
        if(m_Agent.enabled && m_Agent.isOnNavMesh)
        {
            m_Agent.isStopped = true;

        }
        m_IsKnockback = true;

        Vector3 knockbackDir = (transform.position - attacker.position).normalized;
        float knockbackPower = 100 / m_Status.m_nWeight * m_Status.m_fBack;      // ノックバックの力（距離 or スピード）
        float knockbackTime = 0.2f;        // ノックバック時間
        float _fTimer = 0f;

        while (_fTimer < knockbackTime) //ノックバックの指定時間の間
        { //敵を後方へノックバック
            transform.position += knockbackDir * knockbackPower * Time.deltaTime;
            _fTimer += Time.deltaTime;
            yield return null;
        }

        // 追跡を一時停止する時間
        yield return new WaitForSeconds(m_Status.m_fStop);

        m_IsKnockback = false;
        m_Agent.isStopped = false;
        Debug.Log("ノックバック終了！");
    }

    /// <summary>
    /// -フラッシュ関数
    /// <para>ダメージを受けたときに赤く光る関数</para>
    /// </summary>

    IEnumerator FlashRedCoroutine()
    {
        int flashCount = 4; //光る回数
        float flashInterval = 0.1f; //点滅の速度

        rend = GetComponentInChildren<Renderer>();
        if (rend == null) yield break;

        for (int i = 0; i < flashCount; i++) //赤く点滅する
        {
            rend.material = flashMaterial;
            yield return new WaitForSeconds(flashInterval);
            rend.material = defaultMaterial;
            yield return new WaitForSeconds(flashInterval);
        }
    }

    /// <summary>
    /// -死亡時処理関数
    /// <para>HPが0になったときに呼び出される関数</para>
    /// </summary>
    private void OnDead()
	{
		// 体液の排出
		if (m_Blood != null)	
		{
			float _temp_y = 0.0f;

			Ray ray = new Ray(transform.position, Vector3.down);
			RaycastHit hitten;
			if (Physics.Raycast(ray, out hitten, 200.0f))
			{
				_temp_y = hitten.transform.gameObject.transform.position.y;
				//Debug.Log(hitten.transform.gameObject.name);
			}

			//Debug.Log(_temp_y);
			//Instantiate(m_Blood, new Vector3(transform.position.x, _temp_y, transform.position.z), Quaternion.identity);
			Instantiate(m_Blood, transform.position, Quaternion.identity);
			//Debug.LogError("体液生成");
		}
		else
		{
			Debug.LogError("体液が設定されていません");
		}

		// 死亡エフェクトを敵の位置に生成
		if (deathEffectPrefab != null)
		{
			Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
		}

		Destroy(gameObject);	// 敵を消す

		Destroy(gameObject);	// 敵を消す
	}

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

}