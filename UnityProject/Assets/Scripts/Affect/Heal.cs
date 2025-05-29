/*=====
<Heal.cs>
└作成者：takagi

＞内容
回復機能を実装

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
public class CHeal : CAffect
{
	// 定数定義
	protected const int _MIN_DAMAGE = 1;	// 最低保証回復

	// 変数宣言
	[Header("パラメータ")]
	[SerializeField, Tooltip("回復値")] private float m_fHeal;
	private float m_fBaseCorrection = 0.0f;	// 基礎値補正
	private float m_fCorrectionRatio = 1.0f;	// 補正倍率

	// プロパティ定義

	/// <summary>
	/// 基礎回復プロパティ
	/// </summary>
	/// <value><see cref="m_fHeal"/></value>
	public float BaseHeal	// 無補正回復値
	{
		get
		{
			// 提供
			return m_fHeal;	// 計算前の基礎回復を提供
		}
		set
		{
			// 更新
			m_fHeal = value;	// 計算前の基礎回復を更新
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
			return m_fBaseCorrection;	// 回復の基礎値補正を提供
		}
		set
		{
			// 更新
			m_fBaseCorrection = value;	// 回復の基礎値補正を更新
		}
	}

	/// <summary>
	/// 補正倍率プロパティ
	/// </summary>
	/// <value><see cref="m_fCorrectionRation"/></value>
	public float CorrectionRation
	{
		get
		{
			// 提供
			return m_fCorrectionRatio;	// 回復の補正倍率を提供
		}
		set
		{
			// 更新
			m_fCorrectionRatio = value;	// 回復の補正倍率を更新
		}
	}

	/// <summary>
	/// 補正回復プロパティ
	/// </summary>
	/// <value>
	/// <para>補正後の回復を提供</para>
	/// <para>参考：<see cref="m_fHeal">基礎回復</see></para>
	/// <para>参考：<see cref="m_fBaseCorrection">基礎値補正</see></para>
	/// <para>参考：<see cref="m_fCorrectionRatio">補正倍率</see></para>
	/// </value>
	protected float CorrectedHeal
	{
		get
		{
			// 提供
			return (m_fHeal + m_fBaseCorrection) * m_fCorrectionRatio;	// 補正後回復を提供
		}
	}

	
	/// <summary>
	/// -回復効果関数
	/// <para>回復を与える効果を行う関数</para>
	/// </summary>
	/// <param name="_Oneself">効果の発動者</param>
	/// <param name="_Opponent">効果の受動者</param>
	public override void Affect(GameObject _Oneself, GameObject _Opponent)
	{
		// 変数宣言
		var _HitPoint = _Opponent.GetComponent<CHitPoint>();	// 回復を受けるHP

		// 回復処理
		if(_HitPoint)	// 回復を受けられる
		{
			// 回復を与える
			_HitPoint.HP += (int)CorrectedHeal;	// 最終回復をHPに影響させる
		}
	}
}