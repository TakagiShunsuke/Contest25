/*=====
<Blood.cs>
└作成者：takagi

＞内容
体液？血？の機能実装

＞注意事項
・CAffectの派生コンポーネントがないと機能しません。
・//TODO:コードを規約に寄せる必要あり。
・UIDに対してタイマーをここに用意できている点は自然でgood
・Destroyしたなど、明らかに捨てていいUIDに紐づけたタイマーを消せずメモリを蝕み続ける点はbad
・

＞更新履歴
__Y25
_M05
D
11:プログラム作成:takagi
30:誤字修正:takagi
=====*/

// 名前空間宣言
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

// クラス定義
public class CBlood : MonoBehaviour
{


	// 列挙定義
	private enum E_BLOOD_EVENT
	{
		ON_STAY,	// 体液上に発動
	}


	// 変数宣言
	[Header("性質")]
	[SerializeField, Tooltip("効果発動間隔")] private float m_fCoolTime;
	private Dictionary<int, float> m_fCoolDownTimers = new Dictionary<int, float>();	// 時間計測用
	private CAffect m_Affect;	// 体液の効果	※自身のコンポーネントから取得	//TODO:Playerなどで別効果のCAffectを使い同居する場合、非想定の結果になる点を改善
	private CInnerAffectEventor<E_BLOOD_EVENT> m_InnerAffectEventor;	// 効果用のイベント管理

	// ＞初期化関数
	// 引数：なし
	// ｘ
	// 戻値：なし
	// ｘ
	// 概要：初期化処理
	void Start()
	{
		//TODO:m_InnerAffectEventor.AffectEvents.Add(E_BLOOD_EVENT.ON_STAY, );

		// 初期化
//		m_Affect = GetComponent<CAffect>();	// 自身の特徴取得
//#if UNITY_EDITOR	// エディタ使用中
//		if (!m_Affect)	// 取得に失敗した時
//		{
//			// エラー出力
//			Debug.LogError("効果コンポーネントが設定されていません");	// ログ出力
//		}
//		else if (GetComponents<CAffect>().Length > 1)	// 使わない効果コンポーネントがあるとき
//		{
//			// エラー出力
//			Debug.LogWarning("無効な効果が設定されています");	// ログ出力
//		}
//		if(m_fCoolTime < 0.0f)
//		{
//			// エラー出力
//			Debug.LogError("クールタイムが機能していません");	// ログ出力
//		}
//#endif
	}

	// ＞物理更新関数
	// 引数：なし
	// ｘ
	// 戻値：なし
	// ｘ
	// 概要：物理周期でタイマー更新
	void FixedUpdate()
	{
		// 変数宣言
		List<int> _nTimerKeys = new List<int>(m_fCoolDownTimers.Keys);	// ループで辞書を一律更新するためのキーコピー

		// タイマー更新
		foreach (var _nKey in _nTimerKeys)	// 各タイマー単位
		{
			if(m_fCoolDownTimers[_nKey] > 0.0f)
			{
				m_fCoolDownTimers[_nKey] -= Time.fixedDeltaTime;	// 更新フレーム間時間を累加

				// 補正
				if(m_fCoolDownTimers[_nKey] < 0.0f)	// カウント完了
				{
					m_fCoolDownTimers[_nKey] = 0.0f;	// 0で打ち止め
				}
			}
		}
	}

	private void OnTriggerEnter(Collider _Entered)
	{
		// 
		if (!m_fCoolDownTimers.ContainsKey(_Entered.gameObject.GetInstanceID()))	// 初登録
		{
			// 効果発動
			if(m_Affect)
				m_Affect.Affect(gameObject, _Entered.gameObject);	// 自分が相手に効果を発動

			// クールタイム開始
			m_fCoolDownTimers.Add(_Entered.gameObject.GetInstanceID(), m_fCoolTime);	// タイマーを登録
		}
	}

	private void OnTriggerStay(Collider _Staying)
	{
		// 
		if (m_fCoolDownTimers.ContainsKey(_Staying.gameObject.GetInstanceID()))	// 登録済
		{
			if (m_fCoolDownTimers[_Staying.gameObject.GetInstanceID()] == 0.0f)	// 
			{
				// 効果発動
				m_Affect.Affect(gameObject, _Staying.gameObject);	// 自分が相手に効果を発動

				// クールタイム開始
				m_fCoolDownTimers[_Staying.gameObject.GetInstanceID()] = m_fCoolTime;	// タイマーをリセット
			}
		}
#if UNITY_EDITOR	// エディタ使用中
		else
		{
			// エラー出力
			Debug.LogError("クールタイムが登録されていません");	// ログ出力
		}
#endif
	}
//	private void OnCollisionEnter(Collision _Entered)
//	{
//		// 
//		if (!m_fCoolDownTimers.ContainsKey(_Entered.gameObject.GetInstanceID()))	// 初登録
//		{
//			// 効果発動
//			m_Affect.Affect(gameObject, _Entered.gameObject);	// 自分が相手に効果を発動

//			// クールタイム開始
//			m_fCoolDownTimers.Add(_Entered.gameObject.GetInstanceID(), m_fCoolTime);	// タイマーを登録
//		}
//	}

//	private void OnCollisionStay(Collision _Staying)
//	{
//		// 
//		if (m_fCoolDownTimers.ContainsKey(_Staying.gameObject.GetInstanceID()))	// 初登録
//		{
//			if (m_fCoolDownTimers[_Staying.gameObject.GetInstanceID()] == 0.0f)	// 
//			{
//				// 効果発動
//				m_Affect.Affect(gameObject, _Staying.gameObject);	// 自分が相手に効果を発動

//				// クールタイム開始
//				m_fCoolDownTimers[_Staying.gameObject.GetInstanceID()] = m_fCoolTime;	// タイマーをリセット
//			}
//		}
//#if UNITY_EDITOR	// エディタ使用中
//		else
//		{
//			// エラー出力
//			Debug.LogError("エラー");	// ログ出力
//		}
//#endif
//	}

	//private void OnCollisionExit(Collision _Exited)
	//{
	//	//※淵で入出を繰り返したら意図しているよりも多く効果を受ける
	//	if (m_fCoolDownTimers.ContainsKey(_Exited.gameObject.GetInstanceID()))
	//	{
	//	}
	//}
}