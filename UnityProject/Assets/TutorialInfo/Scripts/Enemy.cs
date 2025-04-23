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
=====*/

// 名前空間宣言
using System;
using UnityEngine;

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
	[SerializeField, Tooltip("ステータス")] private Status m_Status;

	[Header("追跡")]
	[SerializeField, Tooltip("追跡フラグ")] private bool m_bChase = true;	// 追跡

	private Transform m_Target;	// プレイヤーのTransform
	private bool m_bChasing = false;	// 追跡中かどうか


	// ＞更新関数
	// 引数：なし
	// ｘ
	// 戻値：なし
	// ｘ
	// 概要：更新処理
	private void Update()
	{
		if (m_bChasing) // 追跡フラグがtrueの時
		{
			Chaser(); // 追跡関数実行
		}
	}

	// ＞追跡関数
	// 引数：なし
	// ｘ
	// 戻値：なし
	// ｘ
	// 概要：プレイヤーの追跡をする
	public void Chaser()
	{
		if (m_bChase == true)	// 追跡フラグがtrueの時、プレイヤーに向かって移動
		{
			Vector3 direction = (m_Target.position - transform.position).normalized;
			transform.position = Vector3.MoveTowards(transform.position, m_Target.position, m_Status.m_fSpeed * Time.deltaTime);
		}
	}

	// ＞ダメージ関数
	// 引数：なし
	// ｘ
	// 戻値：なし
	// ｘ
	// 概要：ダメージを与える
	public void Damage(int _nDamage)　
	{
		m_Status.m_nHp -= _nDamage;　// ダメージ処理

		if (m_Status.m_nHp <= 0)	// HPが0の時
		{
			Destroy(gameObject);	// 敵を消す
		}
	}

	// ＞追跡開始関数
	// 引数：Collider _Collision
	// ｘ
	// 戻値：なし
	// ｘ
	// 概要：プレイヤーが範囲内に入ったら追跡を開始する
	private void OnTriggerEnter(Collider _Collision)
	{
		//接触判定(追跡範囲)
		if (_Collision.CompareTag("Player"))	// プレイヤーが範囲内に入ったら
		{
			m_Target = _Collision.transform;	// プレイヤーのTransformを保存
			m_bChasing = true;	// 追いかけ開始
		}
	}

	// ＞追跡終了関数
	// 引数：Collider _Collision
	// ｘ
	// 戻値：なし
	// ｘ
	// 概要：プレイヤーが範囲外に出たら追跡を終了する
	private void OnTriggerExit(Collider _Collision)	// 接触判定(追跡範囲)
	{
		if (_Collision.CompareTag("Player"))	// プレイヤーが範囲外に出たら
		{
			m_bChasing = false;	// 追いかけ終了
			m_Target = null;	// ターゲットを解除
		}
	}
}