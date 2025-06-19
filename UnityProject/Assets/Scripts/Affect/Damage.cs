/*=====
<Damage.cs>
└作成者：takagi

＞内容
ダメージ機能を実装

＞注意事項
・一般的な(？)ダメージ計算より補正をかけられる機構を用意しています。
	プロパティから触れますが、初期状態なら補正がない状態(α版時点での仕様書通り)になります。
・Affectの変更により、使い方ごとにSOを作ることとなりました。

＞更新履歴
__Y25
_M05
D
12:プログラム仮作成:takagi
16:リファクタリング:takagi
_M06
13:継承元を MonoBehavior→ScriptableObject に変更:takagi
18:フラグを追加(処理分岐を減らすことで保守性を保つ):takagi
19:変数名変更(m_bNonInvincible→m_bIgnoreInvincible)
	・無敵判定修正
	・NonkillingDamageの融合に伴いprivate→private
	・SO化の融合
	・フラグのプロパティ化:takagi
=====*/

// 名前空間宣言
using UnityEngine;

// クラス定義
[CreateAssetMenu(menuName = AFFECT_MENU_TAB_NAME + AFFECT_NAME, fileName = AFFECT_NAME)]
public class CDamage : CAffect
{
	// 定数定義
	private const string AFFECT_NAME = "Damage";	// 効果名
	private const int MIN_DAMAGE = 1;	// 最低保証ダメージ

	// 変数宣言
	[Header("パラメータ")]
	[SerializeField, Tooltip("ダメージ値")] private float m_fDamage = 0.0f;
	private float m_fBaseCorrection = 0.0f;	// 基礎値補正
	private float m_fCorrectionRatio = 1.0f;	// 補正倍率
	[Header("状態")]
	[SerializeField, Tooltip("無敵貫通")] private bool m_bIgnoreInvincible = false;
	[SerializeField, Tooltip("ダメージ発生時無敵付与")] private bool m_bGrantInvincible = true;
	[SerializeField, Tooltip("致死性")] private bool m_bKillable = true;

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
	private float CorrectedDamage
	{
		get
		{
			// 提供
			return (m_fDamage + m_fBaseCorrection) * m_fCorrectionRatio;	// 補正後ダメージを提供
		}
	}

	/// <summary>
	/// 無敵貫通フラグプロパティ
	/// </summary>
	/// <value><see cref="m_bIgnoreInvincible"/></value>
	private bool IgnoreInvincible
	{
		get
		{
			// 提供
			return m_bIgnoreInvincible;	// 無敵貫通フラグを提供
		}
		set
		{
			// 更新
			m_bIgnoreInvincible = value;	// フラグ値更新
		}
	}

	/// <summary>
	/// 無敵付与フラグプロパティ
	/// </summary>
	/// <value><see cref="m_bGrantInvincible"/></value>
	private bool GrantInvincible
	{
		get
		{
			// 提供
			return m_bGrantInvincible;	// 無敵付与フラグを提供
		}
		set
		{
			// 更新
			m_bGrantInvincible = value;	// フラグ値更新
		}
	}

	/// <summary>
	/// 致死性フラグプロパティ
	/// </summary>
	/// <value><see cref="m_bKillable"/></value>
	private bool Killable
	{
		get
		{
			// 提供
			return m_bKillable;	// 致死性フラグを提供
		}
		set
		{
			// 更新
			m_bKillable = value;	// フラグ値更新
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
			// 変数宣言
			int _TemporalHP = _HitPoint.HP;	// 現在HPの退避

			// ダメージを与える
			if (m_bKillable || _HitPoint.HP - CulcDamage(CorrectedDamage, _HitPoint.Defence) > 0)	// 致死性がある・もしくはそもそも殺せていない
			{
				_HitPoint.HP -= CulcDamage(CorrectedDamage, _HitPoint.Defence);	// 通常のダメージ処理
			}
			else if(_HitPoint.HP > 0)	// 本来ならこのダメージ処理で死ぬが、非致死性ダメージとして扱う
			{
				_HitPoint.HP = 1;   // 非致死性効果で1耐えさせる
			}

			// 無敵付与
			if (m_bGrantInvincible && _TemporalHP > _HitPoint.HP)	// ダメージを与えたら無敵を付与する
			{
				// 変数宣言
				var _Granter = _Opponent.GetComponent<IDamagedInvincible>();	// 無敵付与方法を取得

				// 無敵起動
				if(_Granter != null)// 付与方法が明示されている
				{
					_Granter.GrantInvincible();	// 無敵状態にする
				}
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
	private int CulcDamage(float _fDamageValue, float _fDefence)
	{
		// 変数宣言
		int _Result = 0;	// 演算結果格納用

		// ダメージ計算
		_Result = (int)(_fDamageValue - _fDefence);	// 最終ダメージを求める

		// 補正
		if(_Result < MIN_DAMAGE)	// 最低保証が成立していない
		{
			_Result = MIN_DAMAGE;	// ダメージを保証
		}

		// 提供
		return _Result;	// 最終ダメージ確定
	}
}