/*=====
<TrackCamera.cs>
└作成者：takagi

＞内容
ターゲットを追従するカメラのスクリプト

＞注意事項
・追跡対象が設定されていないと動きません(警告吐きます)。
・編集モード時にも動作するためトランスフォームの変更等も無効化し、常に追跡を完遂します。

＞更新履歴
__Y25
_M05
D
07:プログラム作成開始:takagi
08:プログラム作成完了:takagi
=====*/

// 名前空間宣言
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// クラス定義
[ExecuteAlways]	// 非再生時にも処理
public class CTrackCamera : MonoBehaviour
{
	// 変数宣言
	[Header("追跡情報")]
	[SerializeField, Tooltip("追跡対象")] private GameObject m_Target;
	[SerializeField, Tooltip("相対位置")] private Vector3 m_RelativePosition;
	[SerializeField, Tooltip("注視点補正")] private Vector3 m_CorrectLookAt;


	// ＞カメラ姿勢関数
	// 引数１：なし
	// ｘ
	// 戻値：なし
	// ｘ
	// 概要：追跡処理に伴う向きや移動の演算処理
	private void Tracking()
	{
		// 検査
		if (!m_Target)	// 必要要件の不足時
		{
#if UNITY_EDITOR	//エディタ使用中             
			// エラー出力
			Debug.LogWarning("必要な要素が不足しています");	// 警告ログ出力
#endif

			// 中断
			return;	// 更新処理中断
		}

		// 変数宣言
		Vector3 _ToLooking = m_Target.gameObject.transform.position + m_CorrectLookAt - transform.position;	// 注視点に補正を乗せて二点間ベクトルを算出


		// 移動
		transform.position = m_Target.gameObject.transform.position + m_RelativePosition;	// カメラの座標を計算

		// 向きを演算
		transform.LookAt(m_Target.gameObject.transform.position + m_CorrectLookAt);	// 今回のようなただの追従ケース(補間などを調整しない場合)にはこれの方が適切
	}

	// ＞物理更新関数
	// 引数１：なし
	// ｘ
	// 戻値：なし
	// ｘ
	// 概要：物理更新処理
	private void FixedUpdate()
	{
		// 追跡処理
		Tracking();	// プレイヤーの物理更新に伴って更新する
	}

#if UNITY_EDITOR	//エディタ使用中
	// ＞更新関数
	// 引数１：なし
	// ｘ
	// 戻値：なし
	// ｘ
	// 概要：編集時も追跡処理を稼働させるための更新処理
	void Update()
	{
		// 編集/再生のモードによって状態切り替え
		if (!Application.isPlaying)	// 編集モード
		{
			// 追跡機能の恒常化
			if (transform.hasChanged)	// トランスフォームの変更を確認
			{
				transform.hasChanged = false;	// 変更フラグをクリア
				Tracking();	// 変更を無効化
			}
		}
	}
	
	// ＞インスペクター値変更時処理関数
	// 引数１：なし
	// ｘ
	// 戻値：なし
	// ｘ
	// 概要：インスペクタで参照元や相対などの更新があったときに即座に反映する
	private void OnValidate()
	{
		// 追跡処理
		Tracking();	// 変更された値に合わせて位置等を変える
	}

	// ＞ギズモ描画関数
	// 引数１：なし
	// ｘ
	// 戻値：なし
	// ｘ
	// 概要：追跡対象との関係が見えやすいようにシーンビューでギズモ表示
	private void OnDrawGizmos()
	{
		// 検査
		if (!m_Target)	// 必要要件の不足時
		{
			// エラー出力
			Debug.LogWarning("必要な要素が不足しています");	// 警告ログ出力

			// 中断
			return;	// 更新処理中断
		}

		// カメラ-対象物間の線分
		Gizmos.color = Color.blue;	// 青で表示
		Gizmos.DrawLine(transform.position, m_Target.gameObject.transform.position);	// 線分描画

		// カメラ-注視点間の線分
		Gizmos.color = Color.red;	// 赤で表示
		//Gizmos.DrawIcon()
		Gizmos.DrawLine(transform.position, m_Target.gameObject.transform.position + m_CorrectLookAt);	// 線分描画
	}
#endif
}