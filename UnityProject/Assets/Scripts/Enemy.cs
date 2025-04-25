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
//TODO:navMeshを追加したことを記述
25:
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
		[SerializeField, Tooltip("HP")] public int m_nHp;					// HP
		[SerializeField, Tooltip("攻撃力")] public int m_nAttack;			// 攻撃力
		[SerializeField, Tooltip("速度")] public float m_fSpeed;			// 速さ
		[SerializeField, Tooltip("攻撃速度")] public int m_nAttackSpeed;	// 攻撃速度
		[SerializeField, Tooltip("防御力")] public int m_nDefense;			// 防御
		[SerializeField, Tooltip("成長")] public int m_nGrowth;				// 成長
	}

	// 変数宣言

	[Header("ステータス")]
	[SerializeField, Tooltip("ステータス")] private Status m_Status; // ステータス

	private NavMeshAgent m_Agent;  // 追跡対象

    [SerializeField, Tooltip("ターゲット")] private Transform m_Target;  // プレイヤーのTransform


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
    }

    // ＞更新関数
    // 引数：なし
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：更新処理
    private void Update()
	{
		//追跡
		m_Agent.SetDestination(m_Target.position);
	}

	// ＞ダメージ関数
	// 引数：なし
	// ｘ
	// 戻値：なし
	// ｘ
	// 概要：ダメージを与える
	public void Damage(int _nDamage)
	{
		if (_nDamage <= m_Status.m_nDefense)// 防御が被ダメを上回ったら被ダメを1にする
		{
			_nDamage = 1;
		}
		else// ダメージを与える
		{
			_nDamage = _nDamage - m_Status.m_nDefense;
		}

		m_Status.m_nHp -= _nDamage;　// ダメージ処理

		if (m_Status.m_nHp <= 0)	// HPが0の時
		{
			Destroy(gameObject);	// 敵を消す
		}
	}
}