/*=====
<Invincible.cs>
└作成者：kato

＞内容
無敵状態を実装

＞注意事項
・無敵の状態フラグのような感覚で用います。
・実際の無敵処理はここにはないため注意。

＞更新履歴
__Y25
_M05
D
30:プログラム作成:kato
_M06
D
17:リファクタリング:takagi
=====*/

// 名前空間宣言
using UnityEngine;

// クラス定義
public class CInvincible : MonoBehaviour
{
	// 変数宣言
	[Header("ステータス")]
	[SerializeField, Tooltip("機能時間")] private float m_fAliveTime = 0.0f;

	// プロパティ定義
	
	/// <summary>
	/// 機能時間プロパティ
	/// </summary>
	/// <value><see cref="m_fAliveTime"/></value>
	public float AliveTime
	{
		get
		{
			// 提供
			return m_fAliveTime;	// 残機能時間提供
		}
		set
		{
			// 更新
			if (m_fAliveTime < value)	// 継続時間更新(延長)
			{
				m_fAliveTime = value;	// 延長を受け付け
			}
		}
	}


#if UNITY_EDITOR // エディタ使用中
	/// <summary>
	/// -初期化関数
	/// <para>初回更新時の処理</para>
	/// </summary>
	private void Start()
	{
		Debug.Log("無敵開始");
		if(m_fAliveTime < 0.0f)	// 機能時間がない
		{
			// 出力
			Debug.LogError("機能時間が異常です");
		}
	}
#endif

	/// <summary>
	/// 更新関数
	/// <para>更新処理</para>
	/// </summary>
	private void Update()
	{
		// タイマー更新
		m_fAliveTime -= Time.deltaTime;	// 残時間の自然減少

		// 検査
		if (m_fAliveTime < 0.0f)	// 機能停止
		{
			Debug.Log("無敵解除");
			Destroy(this);	// 無敵解除
		}
	}
}
