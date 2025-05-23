/*=====
<Affect.cs>
└作成者：takagi

＞内容
効果要素を実装

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
public abstract class CAffect : MonoBehaviour
{
	/// <summary>
	/// -効果関数
	/// <para>各効果の呼び出し共通化のための抽象関数</para>
	/// </summary>
	/// <param name="_Oneself">効果の発動者</param>
	/// <param name="_Opponent">効果の受動者</param>
	public abstract void Affect(GameObject _Oneself, GameObject _Opponent);
}
