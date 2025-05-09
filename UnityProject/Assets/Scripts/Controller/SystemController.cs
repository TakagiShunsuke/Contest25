/*=====
<SystemController.cs>	// スクリプト名
└作成者：takagi

＞内容
システム用の入力対応

＞注意事項
全シーンで導入してくれ	//TODO:自動化
//TODO:シングルトン

＞更新履歴
__Y25
_M05
D
09:プログラム仮作成:takagi
=====*/

// 名前空間宣言
using UnityEngine;

// クラス定義
public class CSystemController : MonoBehaviour
{
	// 変数宣言
	[Header("入力")]
	[SerializeField, Tooltip("ゲーム終了キー")] private KeyCode m_FinishKey;

	// 更新関数
	// 引数１：なし
	// ｘ
	// 戻値：なし
	// ｘ
	// 概要：キー受付判定処理	//TODO:入力イベント関数があったはず...置き換えること
	void Update()
	{
		// 終了判定
#if !UNITY_EDITOR
		if(Input.GetKeyUp(KeyCode.Escape))	// 終了コマンド
		{
			Application.Quit();	// アプリケーション終了
		}
#endif
		
	}
}
