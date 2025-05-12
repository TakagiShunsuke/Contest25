


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class CDamage : CAffect
{
	// 変数宣言
	[Header("パラメータ")]
	[SerializeField, Tooltip("ダメージ値")] private float m_fDamage;

	// プロパティ定義
	public float BaseDamage	// 無補正ダメージ値
	{
		get
		{
			return m_fDamage;
		}
		set
		{
			m_fDamage = value;
		}
	}
	public float FinalDamage	// 補正後最終ダメージ値
	{
		get
		{
			return m_fDamage;
		}
	}

	public override sealed void Affect(GameObject _Oneself, GameObject _Opponent)
	{
		// 変数宣言
		var _HitPoint = _Opponent.GetComponent<CHitPoint>();
		var _Deffence = _Opponent.GetComponent<CDefence>();

		if(_HitPoint)
		{
			float def = 0;
			if(_Deffence)
			{
				def = _Deffence.Defence;
			}

			_HitPoint.HP -= CulcDamage(FinalDamage, def);
		}
	}


	// ダメージ計算式	//TODO:親関数に移動
	protected int CulcDamage(float _fDamageValue, float _fDefence)
	{	
		// 変数宣言
		int _Result = 0;

		// ダメージ計算
		_Result = (int)(_fDamageValue - _fDefence);

		// 補正
		if(_Result < 1)
		{
			_Result = 1;	// 1ダメージは保証	//TODO:マジックナンバー除去
		}

		// 提供
		return _Result;
	}
}