/*=====
<HitPoint.cs>
└作成者：takagi

＞内容
生死、体力の機能を実装

＞更新履歴
__Y25
_M05
D
12:プログラム仮作成:takagi
16:リファクタリング:takagi
28:防御を統合(体力の機能にしか関連しない処理なため)・
	最大体力の概念を追加:takagi
=====*/

// 名前空間宣言
using Unity.VisualScripting;
using UnityEngine;

// クラス定義
public class CHitPoint : MonoBehaviour
{
	// 定数定義
	private const int MIN_GUARANTEE_MAX_VALUE = 1;	// 最大HPの最低保証値

	// イベント定義
	public delegate void DeathEvent();	// イベント用の関数定義
	public event DeathEvent OnDead;	// 死亡時のイベント

	// 変数宣言
	[SerializeField, Tooltip("最大体力")] private int m_nMaxHP = MIN_GUARANTEE_MAX_VALUE;
	private int m_nHP = MIN_GUARANTEE_MAX_VALUE;	// 体力残量
	[SerializeField, Tooltip("防御")]private int m_nDefence = 0;

	// プロパティ定義

	/// <summary>
	/// 死亡フラグプロパティ
	/// </summary>
	/// <value>死亡時true, それ以外でfalse</value>
	public bool IsDead { get; private set; }	// 死亡フラグ	/// <summary>

	/// 最大HPプロパティ
	/// </summary>
	/// <value><see cref="m_nMaxHP"/></value>
	public int MaxHP	// 構造体的に機能を切り出しているのでゲッタ・セッタが必要
	{
		get
		{
			// 提供
			return m_nMaxHP;	// 現在HP提供
		}
		set
		{
			// 退避
			var _nTemp = m_nMaxHP;	// 更新前の最大値を退避

			// 更新
			m_nMaxHP = value;   // HPの値を更新

			// 増加分回復
			if (_nTemp < m_nMaxHP)	// 最大値が増加
			{
				m_nHP += m_nMaxHP - _nTemp;	// 増加分回復
			}

			// 補正
			if (m_nMaxHP < MIN_GUARANTEE_MAX_VALUE)	// 最低保証を突破
			{
				m_nMaxHP = MIN_GUARANTEE_MAX_VALUE;	// 最低保証を機能させる
			}

			// レンジ
			if(m_nMaxHP < m_nHP)	// HP残量が最大値を超過
			{
				m_nHP = m_nMaxHP;	// 最大値に抑える
			}
		}
	}
	
	/// <summary>
	/// HPプロパティ
	/// </summary>
	/// <value><see cref="m_nHP"/></value>
	public int HP	// 構造体的に機能を切り出しているのでゲッタ・セッタが必要
	{
		get
		{
			// 提供
			return m_nHP;	// 現在HP提供
		}
		set
		{
			// 更新
			m_nHP = value;  // HPの値を更新

			// 補正
			if (m_nMaxHP < m_nHP)	// 最大値を超過
			{
				m_nHP = m_nMaxHP;	// 最大値に補正
			}

			// 死亡判定
			if (m_nHP < 1)	// HPが無くなった
			{
				IsDead = true;	// 死亡した扱いにする
				if (OnDead != null)	// ヌルチェック
				{
					OnDead.Invoke();	// 死亡時イベントを発行	//TODO:これだと死後蘇生など(とくに処理の途中で一時的に殺したなど)したときに不慮の呼び出しが発生するため要改善
				}
			}
		}
	}

	/// <summary>
	/// 防御プロパティ
	/// </summary>
	/// <value><see cref="m_nDefence"/></value>
	public int Defence	// 構造体的に機能を切り出しているのでゲッタ・セッタが必要
	{
		get
		{
			// 提供
			return m_nDefence;	// 現在HP提供
		}
		set
		{
			// 更新
			m_nDefence = value;  // HPの値を更新
		}
	}
}