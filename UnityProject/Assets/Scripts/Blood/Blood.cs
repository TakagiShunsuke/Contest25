/*=====
<Blood.cs>
└作成者：takagi

＞内容
痕跡の機能実装

＞注意事項
・CAffectの派生コンポーネントがないと機能しません。
・//TODO:コードを規約に寄せる必要あり。
・UIDに対してタイマーをここに用意できている点は自然でgood
・Destroyしたなど、明らかに捨てていいUIDに紐づけたタイマーを消せずメモリを蝕み続ける点はbad

＞更新履歴
__Y25
_M05
D
11:プログラム作成:takagi
30:誤字修正:takagi
_M06
D
20:効果をSOで設定できるように変更
	・不要な関数を削除:takagi
=====*/

// 名前空間宣言
using System.Collections.Generic;
using UnityEngine;

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
	[SerializeField, CIndexWithEnum(typeof(E_BLOOD_EVENT)), Tooltip("効果イベント")] private EventAffects[] m_InnerAffectEventor;	// 効果用のイベント管理


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
			//if(m_Affect)
			//	m_Affect.Affect(gameObject, _Entered.gameObject);	// 自分が相手に効果を発動
			
			//m_InnerAffectEventor.InvokeEvent(E_BLOOD_EVENT.ON_STAY, gameObject, _Entered.gameObject);
			foreach (var af in m_InnerAffectEventor[(int)E_BLOOD_EVENT.ON_STAY].m_Affects )
			{
				af.Affect(gameObject, _Entered.gameObject);
			}

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
				//m_Affect.Affect(gameObject, _Staying.gameObject);   // 自分が相手に効果を発動
				//m_InnerAffectEventor.InvokeEvent(E_BLOOD_EVENT.ON_STAY, gameObject, _Staying.gameObject);
				foreach (var af in m_InnerAffectEventor[(int)E_BLOOD_EVENT.ON_STAY].m_Affects )
				{
					af.Affect(gameObject, _Staying.gameObject);
				}

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
}