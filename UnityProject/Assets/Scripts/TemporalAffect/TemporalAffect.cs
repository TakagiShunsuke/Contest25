/*=====
<TemporalAffect.cs>
└作成者：takagi

＞内容
一時的な効果要素を実装

＞注意事項
子オブジェクトとして所有されることで親に効果を発動することを前提としています

＞更新履歴
__Y25
_M05
D
12:プログラム仮作成:takagi
16:リファクタリング:takagi
30:実装:takagi
=====*/

// 名前空間宣言
using UnityEngine;

// クラス定義
public class CTemporalAffect : MonoBehaviour
{
	// 列挙定義
	public enum E_AFFECT_TYPE	// 効果分類
	{
		BUFF,	// バフ効果
		DEBUFF,	// デバフ効果
	}
	private enum E_TIMING_EVENT	// 効果イベント
	{
		ON_CICLE,	// 周期が回った時に発動
	}

	// 変数宣言
	[Header("ステータス")]
	[SerializeField, Tooltip("機能時間")] private float m_fAliveTime = 0.0f;
	private float m_fAliveTimer = 0.0f;	// 機能時間用のタイマー
	[Header("効果")]
	[SerializeField, Tooltip("効果発動周期")] private float m_fAffectCycle = 0.0f;
	private float m_fAffectTimer = 0.0f;	// 効果発動時間用のタイマー
	[SerializeField, CIndexWithEnum(typeof(E_TIMING_EVENT)), Tooltip("効果イベント")] private EventAffects[] m_InnerAffectEventor;	// 効果用のイベント管理
	[Header("メタデータ")]
	[SerializeField, Tooltip("効果分類")] private E_AFFECT_TYPE m_eAffectType = E_AFFECT_TYPE.BUFF;


	/// <summary>
	/// -初期化関数
	/// <para>初回更新時の処理</para>
	/// </summary>
	private void Start()
	{
#if UNITY_EDITOR // エディタ使用中
		if (!transform.parent.gameObject)	// 親オブジェクトがない
		{
			// 出力
			Debug.LogError("効果の発動対象が見つかりません");
		}
		if(m_fAliveTime < 0.0f)	// 機能時間がない
		{
			// 出力
			Debug.LogError("機能時間が異常です");
		}
		if(m_fAffectCycle < 0.0f)	// 周期時間がない
		{
			// 出力
			Debug.LogError("効果発動周期が異常です");
		}
#endif

		// 効果発動
		m_InnerAffectEventor[(int)E_TIMING_EVENT.ON_CICLE].BootAffects(gameObject, transform.parent.gameObject);	// 初回効果発動
	}
	
	// ＞更新関数
	// 引数：なし
	// ｘ
	// 戻値：なし
	// ｘ
	// 概要：周期的に効果発動
	private void Update()
	{
		// タイマー更新
		m_fAliveTimer += Time.deltaTime;	// 機能時間をカウント
		m_fAffectTimer += Time.deltaTime;	// 効果発動間隔をカウント

		// 補正
		if (m_fAliveTimer > m_fAliveTime)	// 機能停止
		{
			m_fAffectTimer -= m_fAliveTimer;	// 機能停止後のカウントを除外
		}

		// 効果発動
		if (m_fAffectTimer > m_fAffectCycle)	// 効果発動タイミング
		{
			m_InnerAffectEventor[(int)E_TIMING_EVENT.ON_CICLE].BootAffects(gameObject, transform.parent.gameObject);	// 初回効果発動
			m_fAffectTimer -= m_fAffectCycle;	// 周期分カウントリセット
		}

		// 生存決め
		if (m_fAliveTimer > m_fAliveTime)	// 機能停止中	※処理順のため同条件の処理が二回走っている
		{
			Destroy(gameObject);	// 機能停止したため消しておく
		}
	}
}