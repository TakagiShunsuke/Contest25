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
	[SerializeField, Tooltip("ゲーム終了キー")] private KeyCode m_FinishKey = KeyCode.Escape;


	/// <summary>
	/// -初期化関数
	/// <para>初期化処理</para>
	/// </summary>
	private void Start()
	{
		// カーソル設定
		Cursor.visible = false;	// 不可視
	}

	/// <summary>
	/// -更新関数
	/// <para>キー受付判定処理</para>	//TODO:入力イベント関数があったはず...置き換えること
	/// </summary>
	private void Update()
	{
		// 終了判定
#if !UNITY_EDITOR
		if(Input.GetKeyUp(m_FinishKey))	// 終了コマンド
		{
			Application.Quit();	// アプリケーション終了
		}
#endif	// UNITY_EDITOR
	}
}
