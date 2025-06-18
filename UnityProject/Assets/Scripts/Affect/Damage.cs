/*=====
<Damage.cs>
└作成者：takagi

＞内容
ダメージ機能を実装

＞注意事項
・一般的な(？)ダメージ計算より補正をかけられる機構を用意しています。
	プロパティから触れますが、初期状態なら補正がない状態(α版時点での仕様書通り)になります。

＞更新履歴
__Y25
_M05
D
12:プログラム仮作成:takagi
16:リファクタリング:takagi
18:フラグを追加(処理分岐を減らすことで保守性を保つ):takagi
19:変数名変更(m_bNonInvincible→m_bIgnoreInvincible)
	・無敵判定修正:takagi
=====*/

// 名前空間宣言
using UnityEngine;

// クラス定義
public class CDamage : CAffect
{
	// 定数定義
	protected const int _MIN_DAMAGE = 1;	// 最低保証ダメージ

	// 変数宣言
	[Header("パラメータ")]
	[SerializeField, Tooltip("ダメージ値")] private float m_fDamage;
	private float m_fBaseCorrection = 0.0f;	// 基礎値補正
	private float m_fCorrectionRatio = 1.0f;	// 補正倍率
	[Header("状態")]
	[SerializeField, Tooltip("無敵貫通")] protected bool m_bIgnoreInvincible = false;
	[SerializeField, Tooltip("致死性")] protected bool m_bKillable = true;

	// プロパティ定義

	/// <summary>
	/// 基礎ダメージプロパティ
	/// </summary>
	/// <value><see cref="m_fDamage"/></value>
	public float BaseDamage	// 無補正ダメージ値
	{
		get
		{
			// 提供
			return m_fDamage;	// 計算前の基礎ダメージを提供
		}
		set
		{
			// 更新
			m_fDamage = value;	// 計算前の基礎ダメージを更新
		}
	}

	/// <summary>
	/// 基礎補正プロパティ
	/// </summary>
	/// <value><see cref="m_fBaseCorrection"/></value>
	public float BaseCorrection
	{
		get
		{
			// 提供
			return m_fBaseCorrection;	// ダメージの基礎値補正を提供
		}
		set
		{
			// 更新
			m_fBaseCorrection = value;	// ダメージの基礎値補正を更新
		}
	}

	/// <summary>
	/// 補正倍率プロパティ
	/// </summary>
	/// <value><see cref="m_fCorrectionRatio"/></value>
	public float CorrectionRation
	{
		get
		{
			// 提供
			return m_fCorrectionRatio;	// ダメージの補正倍率を提供
		}
		set
		{
			// 更新
			m_fCorrectionRatio = value;	// ダメージの補正倍率を更新
		}
	}

	/// <summary>
	/// 補正ダメージプロパティ
	/// </summary>
	/// <value>
	/// <para>補正後のダメージを提供</para>
	/// <para>参考：<see cref="m_fDamage">基礎ダメージ</see></para>
	/// <para>参考：<see cref="m_fBaseCorrection">基礎値補正</see></para>
	/// <para>参考：<see cref="m_fCorrectionRatio">補正倍率</see></para>
	/// </value>
	protected float CorrectedDamage
	{
		get
		{
			// 提供
			return (m_fDamage + m_fBaseCorrection) * m_fCorrectionRatio;	// 補正後ダメージを提供
		}
	}

	
	/// <summary>
	/// -ダメージ効果関数
	/// <para>ダメージを与える効果を行う関数</para>
	/// </summary>
	/// <param name="_Oneself">効果の発動者</param>
	/// <param name="_Opponent">効果の受動者</param>
	public override void Affect(GameObject _Oneself, GameObject _Opponent)
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
		var _Invincible = _Opponent.GetComponent<CInvincible>();	// 無敵状態

		// 無敵
		if (_Invincible && !m_bIgnoreInvincible)	// 無敵を適用
		{
			// 中断
			return;	// ダメージ処理が発生しない
		}

		// ダメージ処理
		if (_HitPoint)	// ダメージを受けられる
		{
			// ダメージを与える
			if (m_bKillable || _HitPoint.HP - CulcDamage(CorrectedDamage, _HitPoint.Defence) > 0)	// 致死性がある・もしくはそもそも殺せていない
			{
				_HitPoint.HP -= CulcDamage(CorrectedDamage, _HitPoint.Defence);	// 通常のダメージ処理
			}
			else if(_HitPoint.HP > 0)	// 本来ならこのダメージ処理で死ぬが、非致死性ダメージとして扱う
			{
				_HitPoint.HP = 1;	// 非致死性効果で1耐えさせる
			}
		}
	}
	
	/// <summary>
	/// -ダメージ計算関数
	/// <para>ダメージ処理に必要な情報をすべて揃え、演算する</para>
	/// </summary>
	/// <param name="_fDamageValue">与えるダメージ値</param>
	/// <param name="_fDefence">ダメージ抵抗値</param>
	/// <returns>最終ダメージ</returns>
	protected int CulcDamage(float _fDamageValue, float _fDefence)
	{
		// 変数宣言
		int _Result = 0;	// 演算結果格納用

		// ダメージ計算
		_Result = (int)(_fDamageValue - _fDefence);	// 最終ダメージを求める

		// 補正
		if(_Result < _MIN_DAMAGE)	// 最低保証が成立していない
		{
			_Result = _MIN_DAMAGE;	// ダメージを保証
		}

		// 提供
		return _Result;	// 最終ダメージ確定
	}
}