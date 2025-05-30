/*=====
<AddTemporalAffect.cs>
└作成者：takagi

＞内容
一時的効果付与を実装

＞更新履歴
__Y25
_M05
D
30:プログラム作成:takagi
=====*/

// 名前空間宣言
using UnityEngine;

// クラス定義
public class CAddTemporalAffect : CAffect
{
	// 変数宣言
	[Header("パラメータ")]
	[SerializeField, Tooltip("付与する効果")]private GameObject m_TemporalAffect;

	
	/// <summary>
	/// -効果付与関数
	/// <para>バフ・デバフを与える効果を行う関数</para>
	/// </summary>
	/// <param name="_Oneself">効果の発動者</param>
	/// <param name="_Opponent">効果の受動者</param>
	public override void Affect(GameObject _Oneself, GameObject _Opponent)
	{
		// 保全
		if(m_TemporalAffect == null)	// 効果がない
		{
#if UNITY_EDITOR
			Debug.Log("付与効果が存在しません");
#endif	// !UNITY_EDITOR
			return;	// 処理中断
		}
		if(_Opponent == null)	// 相手がいない
		{
#if UNITY_EDITOR
			Debug.Log("効果発動対象が見つかりません");
#endif	// !UNITY_EDITOR
			return;	// 処理中断
		}

		// 生成
		Instantiate(m_TemporalAffect, _Opponent.transform);	// 相手に効果付与
	}
}