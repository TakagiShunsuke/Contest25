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
	public override sealed void Affect(GameObject _Oneself, GameObject _Opponent)
	{
		// 変数宣言
		var _HitPoint = _Opponent.GetComponent<CHitPoint>();	// ダメージを受けるHP
		var _Deffence = _Opponent.GetComponent<CDefence>();	// ダメージに対する防御値
		
		// ダメージ処理
		if(_HitPoint)	// ダメージを受けられる
		{
			// 変数宣言
			float _DeffenceValue = 0;	// 防御値

			// 防御を取得
			if(_Deffence)	// 防御がある
			{
				_DeffenceValue = _Deffence.Defence;	// 防御値を更新
			}

			if (_HitPoint.HP - CulcDamage(CorrectedDamage, _DeffenceValue) > 0)	// 死なない
			{
				_HitPoint.HP -= CulcDamage(CorrectedDamage, _DeffenceValue);	// 通常のダメージ処理
			}
			else if(_HitPoint.HP > 0)	// このダメージ処理で死ぬ
			{
				_HitPoint.HP = 1;	// 非致死性効果を発動
			}
		}
	}
}