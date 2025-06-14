/*=====
<Singleton.cs>
└作成者：takagi

＞内容
純粋なシングルトンの実装

＞更新履歴
__Y25
_M06
D
14:プログラム作成完了:takagi
=====*/

// クラス定義

public sealed class CBattleData : CPureSingleton<CBattleData>
{
	// プロパティ定義

	/// <summary>
	/// 討伐数プロパティ
	/// </summary>
	/// <value>敵を倒した数</value>
	public uint KillCount { get; set; }


	/// <summary>
	/// -コンストラクタ
	/// <para>インスタンス生成時の処理</para>
	/// </summary>
	public CBattleData()
	{
	}

	/// <summary>
	/// -リセット関数
	/// <para>データを0で初期化</para>
	/// </summary>
	public void Clear()
	{
		// 初期化
		KillCount = 0;	// 倒した数を初期化
	}
}