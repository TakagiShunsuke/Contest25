


using UnityEngine;

public class CHitPoint : MonoBehaviour
{
	// イベント定義
	public delegate void DeathEvent();	// イベント用の関数定義
	public event DeathEvent OnDead;	// 死亡時の処理

	// 変数宣言
	[SerializeField]private int m_nHP;	// 体力値
	
	// プロパティ定義
	public bool IsDead { get; private set; }	// 死亡フラグ

	public int HP	// 構造体的に機能を切り出しているのでゲッタ・セッタが必要
	{
		get
		{
			return m_nHP;
		}
		set
		{
			m_nHP = value;
			if (m_nHP < 1)  // HPが無くなった
			{
				IsDead = true;  // 死亡した扱いに
				if (OnDead != null)
				{
					OnDead.Invoke();	//TODO:これだと死後蘇生など(とくに処理の途中で一時的に殺したなど)したときに不慮の呼び出しが発生するため要改善
				}
			}

		}
	}
}
