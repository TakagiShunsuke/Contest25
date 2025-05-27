/*=====
<temporalAffect.cs>
└作成者：takagi

＞内容
一時的な効果要素を実装

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
public abstract class TemporalAffect
{
	// 列挙定義
	public enum E_AFFECT_TYPE	// 効果分類
	{
		BUFF,	// バフ効果
		DEBUFF,	// デバフ効果
	}

	// 変数宣言
	[SerializeField, Tooltip("効果分類")] private E_AFFECT_TYPE m_eAffectType;
}