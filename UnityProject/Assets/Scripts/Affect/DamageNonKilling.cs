/*=====
<DamageNonKilling.cs>
└作成者：takagi

＞内容
非致死性ダメージ機能を実装

＞注意事項
・一般的な(？)ダメージ計算より補正をかけられる機構を用意しています。
	プロパティから触れますが、初期状態なら補正がない状態(α版時点での仕様書通り)になります。

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
public class CDamageNonKilling : CDamage
{
	/// <summary>
	/// -非致死性ダメージ効果関数
	/// <para>不殺のダメージ効果を行う関数</para>
	/// </summary>
	/// <param name="_Oneself">効果の発動者</param>
	/// <param name="_Opponent">効果の受動者</param>
	public override sealed void Affect(GameObject _Oneself, GameObject _Opponent)
	{
		// 保全
		if(_Opponent == null)	// 相手がいない
		{
#if UNITY_EDITOR
			Debug.Log("効果発動対象が見つかりません");
#endif	// !UNITY_EDITOR
			return;	// 処理中断
		}

		// 変数宣言
		var _HitPoint = _Opponent.GetComponent<CHitPoint>();	// ダメージを受けるHP
		
		// ダメージ処理
		if(_HitPoint)	// ダメージを受けられる
		{
			if (_HitPoint.HP - CulcDamage(CorrectedDamage, _HitPoint.Defence) > 0)	// 死なない
			{
				_HitPoint.HP -= CulcDamage(CorrectedDamage, _HitPoint.Defence);	// 通常のダメージ処理
			}
			else if(_HitPoint.HP > 0)	// このダメージ処理で死ぬ
			{
				_HitPoint.HP = 1;	// 非致死性効果を発動
			}
		}
	}
}