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
=====*/

// 名前空間宣言
using UnityEngine;

// クラス定義
public class CHitPoint : MonoBehaviour
{
	// イベント定義
	public delegate void DeathEvent();	// イベント用の関数定義
	public event DeathEvent OnDead;	// 死亡時のイベント

	// 変数宣言
	[SerializeField]private int m_nHP;	// 体力値
	
	// プロパティ定義

	/// <summary>
	/// 死亡フラグプロパティ
	/// </summary>
	/// <value>死亡時true, それ以外でfalse</value>
	public bool IsDead { get; private set; }	// 死亡フラグ
	
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
			m_nHP = value;	// HPの値を更新

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
}