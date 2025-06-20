/*=====
<EventAffects.cs>
└作成者：takagi

＞内容
列挙イベントに紐づく効果一覧を定義

＞注意事項
・列挙と構造体を紐づけた配列まで定義したかったものの、ジェネリック型ではType指定できず、どうしてもインスペクタ表示まわりで詰まるので
	使うたびに定義してもらう形を取らざるを得ませんでした。
	：定義法：[SerializeField, CIndexWithEnum(typeof(E_EVENT)), Tooltip("効果イベント")] private EventAffects[] m_InnerAffectEventor;
・構造体自体は入れ子構造用の定義となり共通の定義となるのでここで定義しています

＞更新履歴
__Y25
_M06
D
20:プログラム作成:takagi
=====*/

// 名前空間宣言
using System;
using System.Collections.Generic;
using UnityEngine;

// 構造体定義
[Serializable]
public struct EventAffects
{
	// 変数宣言
	[Header("発動効果一覧　※他のものと使いまわす場合データが上書きされないか気を付けてください※")]
	[SerializeField, Tooltip("効果データ")] public List<CAffect> m_Affects;


	/// <summary>
	/// -効果発動関数
	/// <para>紐づいたすべての効果を発動する処理</para>
	/// </summary>
	/// <param name="_Oneself">効果の発動者</param>
	/// <param name="_Opponent">効果の受動者</param>
	public void BootAffects(GameObject _Oneself, GameObject _Opponent)
	{
		foreach (var _Affect in m_Affects )	// 紐づいているすべての効果を起動
		{
			if(_Affect) // ヌルチェック
			{
				_Affect.Affect(_Oneself, _Opponent);	// 効果発動
			}
		}
	}
}
