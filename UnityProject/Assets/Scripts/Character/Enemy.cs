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
25:navMesh追加
_M05
D
1:攻撃追加
=====*/

// 名前空間宣言
using System;
using UnityEngine;
using UnityEngine.AI;

// クラス定義
public class CEnemy : MonoBehaviour
{
	// 構造体定義
	[Serializable]
	public struct Status //敵ステータス
	{
		[SerializeField, Tooltip("HP")] public int m_nHp;					   // HP
		[SerializeField, Tooltip("攻撃力")] public int m_nAtk;			       // 攻撃力
		//[SerializeField, Tooltip("速度")] public float m_fSpeed;			   // 速さ
		[SerializeField, Tooltip("攻撃速度")] public float m_fAtkSpeed;	       // 攻撃速度
		[SerializeField, Tooltip("防御力")] public int m_nDef;			       // 防御
		[SerializeField, Tooltip("成長度")] public int m_nGrowth;              // 成長度
        [SerializeField, Tooltip("成長速度")] public int m_nGrowthSpeed;       // 成長速度
        [SerializeField, Tooltip("重量")] public int m_nWeight;                // 重量
        [SerializeField, Tooltip("攻撃距離")] public float m_fAtkRange;        // 攻撃範囲         
        [SerializeField, Tooltip("攻撃角度")] public float m_fAtkAngle;	       // 攻撃角度		
    }

    // 変数宣言

    [Header("ステータス")]
	[SerializeField, Tooltip("ステータス")] private Status m_Status; // ステータス

    [SerializeField, Tooltip("ターゲット")] private Transform m_Target;  // プレイヤーのTransform
    [SerializeField, Tooltip("成長間隔")] private float m_fGrowthInterval = 5f; // 成長間隔（秒）

    private float m_fGrowthTimer = 0f; // 成長タイマー
    private float m_fAtkCooldown = 0f; // 攻撃のクールタイム
    private NavMeshAgent m_Agent;  // 追跡対象

    // ＞初期化関数
    // 引数：なし
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：初期化処理
    private void Start()
    {
        // NavMeshAgentを取得
        m_Agent = GetComponent<NavMeshAgent>();

        // Playerを自動で探してターゲットに設定
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            m_Target = playerObj.transform;
        }

        // 地面の位置を探す
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            m_Agent.Warp(hit.position); // NavMeshの地面にワープさせる
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
        m_fAtkCooldown -= Time.deltaTime; // 経過時間で減らす

        //追跡
        m_Agent.SetDestination(m_Target.position);

        // 攻撃
        Attack();

        // 成長
        Growth();
    }

    // ＞攻撃関数
    // 引数：なし
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：Playerに近づいたら攻撃する
    public void Attack()
    {
        if (!m_Agent.pathPending && m_Agent.remainingDistance <= m_Agent.stoppingDistance)// 敵に最大限近づいたら
        {
            if (m_fAtkCooldown > 0f) return; // クールタイムが終わったら

            // 一定範囲内のコライダーを検出
            Collider[] hits = Physics.OverlapSphere(transform.position, m_Status.m_fAtkRange);

            foreach (Collider hit in hits)
            {
                if (hit.CompareTag("Player")) // タグがPlayerのオブジェクトを探す
                {
                    Vector3 dirToTarget = (hit.transform.position - transform.position).normalized; 
                    float angle = Vector3.Angle(transform.forward, dirToTarget);

                    if (angle < m_Status.m_fAtkAngle / 2f) // 攻撃範囲内に
                    {
                        //Playerがいる時
                        CPlayer player = hit.GetComponent<CPlayer>();
                        if (player != null)
                        {
                            player.Damage(m_Status.m_nAtk);
                            m_fAtkCooldown = 10.0f / m_Status.m_fAtkSpeed;
                        }
                    }
                }
            }
        }
    }


    private void OnDrawGizmos()
    {
        int _nsegments = 30;               // 扇形を構成する線の本数

        Gizmos.color = new Color(1, 0, 0, 0.3f); // 赤・半透明に設定

        Vector3 origin = transform.position; // 敵の現在位置
        Quaternion startRotation = Quaternion.Euler(0, -m_Status.m_fAtkAngle / 2, 0); // 左端の角度に回す
        Vector3 startDirection = startRotation * transform.forward; // 左端方向を出す

        Vector3 prevPoint = origin + startDirection * m_Status.m_fAtkRange;

        for (int i = 1; i <= _nsegments; i++)
        {
            float angle = -m_Status.m_fAtkAngle / 2 + (m_Status.m_fAtkAngle / _nsegments) * i;
            Quaternion rot = Quaternion.Euler(0, angle, 0); // 各線の角度
            Vector3 direction = rot * transform.forward;    // 回転させて方向取得
            Vector3 point = origin + direction * m_Status.m_fAtkRange;

            Gizmos.DrawLine(origin, point);      // 原点→点の線
            Gizmos.DrawLine(prevPoint, point);   // 前回の点→今回の点で扇形の辺
            prevPoint = point;                   // 次のループ用に保存
        }
    }

    // ＞成長関数
    // 引数：なし
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：時間ごとに敵のステータスを上げる
    public void Growth()
    {
        m_fGrowthTimer += Time.deltaTime;

        if (m_fGrowthTimer >= m_fGrowthInterval)
        {
            m_Status.m_nHp += m_Status.m_nGrowth + m_Status.m_nGrowthSpeed;
            m_fGrowthTimer = 0f;
        }
    }

    // ＞ダメージ関数
    // 引数：なし
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：ダメージを受ける
    public void Damage(int _nDamage)
	{
		if (_nDamage <= m_Status.m_nDef)// 防御が被ダメを上回ったら被ダメを1にする
		{
			_nDamage = 1;
		}
		else// ダメージを与える
		{
			_nDamage = _nDamage - m_Status.m_nDef;
		}

		m_Status.m_nHp -= _nDamage;　// ダメージ処理

		if (m_Status.m_nHp <= 0)	// HPが0の時
		{
			Destroy(gameObject);	// 敵を消す
		}
	}
}