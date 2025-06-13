/*=====
<InputAffectEventor.cs>
└作成者：takagi

＞内容
効果用疑似イベントを実装

＞更新履歴
__Y25
_M06
13:プログラム作成完了:takagi
=====*/

// 名前空間宣言
using System;
using System.Collections.Generic;
using UnityEngine;

// クラス定義
public class CInnerAffectEventor<TTiming> where TTiming : Enum
{
	// 変数宣言
	private Dictionary<TTiming, List<CAffect>> m_AffectEvents;	// 疑似イベントに紐づけた効果インスタンスリスト

	// プロパティ定義

	/// <summary>
	/// 効果用疑似イベントプロパティ
	/// </summary>
	/// <value><see cref="m_AffectEvents"/></value>
	public Dictionary<TTiming, List<CAffect>> AffectEvents	// 効果用疑似イベント
	{
		get
		{
			// 提供
			return m_AffectEvents;	// インスタンス提供
		}
		private set
		{
			// 更新
			m_AffectEvents = value; // インスタンス更新
		}
	}

	/// <summary>
	/// -疑似イベント実行関数
	/// <para>疑似イベントに紐づいた効果をすべて起動</para>
	/// </summary>
	/// <param name="_Timing">発動イベント</param>
	/// <param name="_Oneself">効果の発動者</param>
	/// <param name="_Opponent">効果の受動者</param>
	public void InvokeEvent(TTiming _Timing, GameObject _Oneself, GameObject _Opponent)
	{
		// 保全
		if (m_AffectEvents == null || !m_AffectEvents.ContainsKey(_Timing))	// 疑似イベントが非適用
		{
#if UNITY_EDITOR
			Debug.Log("疑似イベントが機能していません");
#endif	// !UNITY_EDITOR
			return;	// 処理中断
		}

		// イベント機能
		foreach (var _Affect in m_AffectEvents[_Timing])	// 対象疑似イベントに接続している効果
		{
			_Affect.Affect(_Oneself, _Opponent);	// 接続された効果を発動
		}
	}
}