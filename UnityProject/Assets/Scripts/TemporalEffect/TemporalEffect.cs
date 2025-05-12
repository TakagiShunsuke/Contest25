


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class TemporalEffect
{
	// 列挙定義
	public enum E_EFFECT_TYPE	// 効果分類
	{
		E_TYPE_BUFF,	// バフ効果
		E_TYPE_DEBUFF,	// デバフ効果
	}

	// 変数宣言
	[SerializeField, Tooltip("効果分類")] private E_EFFECT_TYPE m_eEffectType;
}