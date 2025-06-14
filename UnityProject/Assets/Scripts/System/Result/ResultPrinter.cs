/*=====
<ResultPrinter.cs>
└作成者：takagi

＞内容
リザルト表示の実装

＞更新履歴
__Y25
_M06
D
14:プログラム作成完了:takagi
=====*/

// 名前空間宣言
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// クラス定義
[RequireComponent(typeof(Canvas))]
public class CResultPrinter : CMonoSingleton<CResultPrinter>
{
	// 構造体定義
	[Serializable]
	private struct RankData
	{
		[Header("データ")]
		[SerializeField, Tooltip("討伐数下限")] public uint m_MinKilled;
		[SerializeField, Tooltip("表示画像")] public Sprite m_Texture;
	}

	// 定数定義
	private const string RANK_IMAGE_NAME = "RankImage";	// ランク表示インスタンス名(代替生産時のみ使用)
	private const string KILL_IMAGE_NAME = "KillIcon";	// 討伐アイコン表示インスタンス名(代替生産時のみ使用)
	private const string KILL_PRINT_NAME = "KillPrinter";	// 討伐数表示インスタンス名(代替生産時のみ使用)

	// 変数宣言
	[Header("ランク設定")]
	[SerializeField, Tooltip("ランク外アイコン ※仕様外(ゲームオーバーの領域)")] private Sprite m_FailedTex = null;	// ゲームオーバーの領域なので基本無いはず、仕様変更時対応用
	[SerializeField, Tooltip("評価 ※低次のものから高次のものへと順に並べてください")] private RankData[] m_Ranks = null;
	[SerializeField, Tooltip("ランクアイコン表示場所")] private Image m_RankImage = null;
	private static Vector2 m_AnchoredRankPos = new(720f, -360f);	// ランクアイコン表示場所(代替生産時のみ使用)
	[Header("討伐表示")]
	[SerializeField, Tooltip("討伐アイコン")] private Sprite m_KillMark = null;
	[SerializeField, Tooltip("討伐アイコン表示場所")] private Image m_KillImage = null;
	private static Vector2 m_AnchoredKillImgPos = new(-880f, -360f);	// 討伐アイコン表示場所(代替生産時のみ使用)
	[SerializeField, Tooltip("討伐数表示場所")] private TextMeshProUGUI m_KillPrint = null;
	private static Vector2 m_AnchoredKillPrtPos = new(-620f, -360f);	// 討伐数表示場所(代替生産時のみ使用)


	/// <summary>
	/// -起動関数
	/// <para>インスタンス生成直後に行う処理</para>
	/// </summary>
	protected override void CustomAwake()
	{
		// 変数宣言
		uint _uKilled = CBattleData.Instance.KillCount;	// インゲームで倒した数を取得
		Sprite _UseRankTexture = null;	// ランクに応じて決定するテクスチャ
		
		// ランク決定(TODO:ステージが分かれるなら、この処理はインゲームで完結しているべき)
		if (m_Ranks == null || _uKilled < m_Ranks[0].m_MinKilled)	// 該当ランクが存在しない
		{
#if UNITY_EDITOR
			Debug.LogWarning("ランクが存在しません！");
#endif	// !UNITY_EDITOR
			_UseRankTexture = m_FailedTex;	// ランク外として設定
		}
		else
		{
			for (int _nIdx = 1; _nIdx < m_Ranks.Length || ((_UseRankTexture = m_Ranks[_nIdx - 1].m_Texture) && false); _nIdx++)	// ランク単位でのループ	※抜けるときに最大ランクとして設定
			{
				if (_uKilled < m_Ranks[_nIdx].m_MinKilled)	// 次ランクには該当しない
				{
					_UseRankTexture = m_Ranks[_nIdx - 1].m_Texture;	// 現在ランクとして確定
					break;	// 演算終了
				}
			}
		}

		// 保全		※あくまで生成位置を調整したい場合を考慮して変数化しているだけで、ここからの生成・設定でも問題はない
		if (!m_RankImage)	// ヌルチェック
		{
			// 生成
			var _ImageObj = new GameObject();	// 画像用のオブジェクト生成

			// 親子付け
			_ImageObj.transform.parent = transform;	// 自身の子にする

			// 名付け
			_ImageObj.name = RANK_IMAGE_NAME;	// 画像オブジェクト名設定

			// 機能設定
			m_RankImage = _ImageObj.AddComponent<Image>();	// 画像表示設定

			// 位置決め
			var _RectTransform = _ImageObj.GetComponent<RectTransform>();	// 専用トランスフォーム取得
			if (_RectTransform != null)	// ヌルチェック
			{
				_RectTransform.anchoredPosition = m_AnchoredRankPos;	// 初期位置設定
			}
		}
		if (!m_KillImage)	// ヌルチェック
		{
			// 生成
			var _ImageObj = new GameObject();	// 画像用のオブジェクト生成

			// 親子付け
			_ImageObj.transform.parent = transform;	// 自身の子にする

			// 名付け
			_ImageObj.name = KILL_IMAGE_NAME;	// 画像オブジェクト名設定

			// 機能設定
			m_KillImage = _ImageObj.AddComponent<Image>();	// 画像表示設定

			// 位置決め
			var _RectTransform = _ImageObj.GetComponent<RectTransform>();	// 専用トランスフォーム取得
			if (_RectTransform != null)	// ヌルチェック
			{
				_RectTransform.anchoredPosition = m_AnchoredKillImgPos;	// 初期位置設定
			}
		}
		if (!m_KillPrint)	// ヌルチェック
		{
			// 生成
			var _PrintObj = new GameObject();	// 討伐数用のオブジェクト生成

			// 親子付け
			_PrintObj.transform.parent = transform;	// 自身の子にする

			// 名付け
			_PrintObj.name = KILL_PRINT_NAME;	// 討伐数オブジェクト名設定

			// 機能設定
			m_KillPrint = _PrintObj.AddComponent<TextMeshProUGUI>();	// 討伐数表示設定

			// 位置決め
			var _RectTransform = _PrintObj.GetComponent<RectTransform>();	// 専用トランスフォーム取得
			if (_RectTransform != null)	// ヌルチェック
			{
				_RectTransform.anchoredPosition = m_AnchoredKillPrtPos;	// 初期位置設定
			}
		}

		// パラメータ設定
		m_RankImage.sprite = _UseRankTexture;	// ランク表示画像設定
		m_KillImage.sprite = m_KillMark;	// 討伐アイコン画像設定
		m_KillPrint.text = $"x {_uKilled}";	// 討伐数テキスト設定
	}

#if UNITY_EDITOR
	/// <summary>
	/// -インスペクタ調整関数
	/// <para>変な数値更新をリアルタイムで補正しフールプルーフを低減させる処理</para>
	/// </summary>
	private void OnValidate()
	{
		// 定義域検査
		if (m_Ranks != null)	// 検証対象が存在
		{
			for (int _nIdx = 1; _nIdx < m_Ranks.Length; _nIdx++)	// ランク単位でのループ
			{
				if (m_Ranks[_nIdx].m_MinKilled <= m_Ranks[_nIdx - 1].m_MinKilled)	// 前項と並び順が矛盾
				{
					m_Ranks[_nIdx].m_MinKilled = m_Ranks[_nIdx - 1].m_MinKilled + 1;	// 最小値を制限
				}
			}
		}
	}
#endif	// !UNITY_EDITOR
}