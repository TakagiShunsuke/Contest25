using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static CResultPrinter;

// クラス定義
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
	private const string CANVAS_NAME = "ResultCanvas";	// キャンバスのインスタンス名
	private const string RANK_IMAGE_NAME = "RankImage";	// ランク表示インスタンス名

	// 変数宣言
	[SerializeField, Tooltip("ランク外アイコン ※仕様外(ゲームオーバーの領域)")] Sprite m_FailedTex = null;	// ゲームオーバーの領域なので基本無いはず、仕様変更時対応用
	[SerializeField, Tooltip("評価 ※低次のものから高次のものへと順に並べてください")] RankData[] m_Ranks = null;


	/// <summary>
	/// -起動関数
	/// <para>インスタンス生成直後に行う処理</para>
	protected override void CustomAwake()
	{
		// 変数宣言
		uint _Killed = CBattleData.Instance.KillCount;	// インゲームで倒した数を取得
		Sprite _UseTexture = null;	// ランクに応じて決定するテクスチャ
		 
		// ランク決定(TODO:ステージが分かれるなら、この処理はインゲームで完結しているべき)
		if (m_Ranks == null || _Killed < m_Ranks[0].m_MinKilled)	// 該当ランクが存在しない
		{
#if UNITY_EDITOR
			Debug.LogWarning("ランクが存在しません！");
#endif	// !UNITY_EDITOR
			_UseTexture = m_FailedTex;	// ランク外として設定
		}
		else
		{
			for (int _Idx = 1; _Idx < m_Ranks.Length; _Idx++)   // ランク単位でのループ
			{
				if (_Killed < m_Ranks[_Idx].m_MinKilled)	// 次ランクには該当しない
				{
					_UseTexture = m_Ranks[_Idx - 1].m_Texture;	// 現在ランクとして確定
					break;	// 演算終了
				}
			}
		}
		
		// 生成
		var _CanvasObj = new GameObject();	// キャンバス用のオブジェクト生成
		var _ImageObj = new GameObject();	// 画像用のオブジェクト生成

		// 親子付け
		_ImageObj.transform.parent = _CanvasObj.transform;	// 画像をキャンバスに持たせる

		// 名付け
		_CanvasObj.name = CANVAS_NAME;	// キャンバス名設定
		_ImageObj.name = RANK_IMAGE_NAME;	// 画像オブジェクト名設定

		// 機能設定
			// -- キャンバス
			var _Canvas = _CanvasObj.AddComponent<Canvas>();	// キャンバス設定
			_Canvas.renderMode = RenderMode.ScreenSpaceOverlay;	// UIを最前面に出す
			_Canvas.AddComponent<CanvasScaler>();	// UIのスケール制御
			_Canvas.AddComponent<GraphicRaycaster>();	// キャンバスへのレイ判定
			_Canvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord1;	// シェーダーセマンティクス：テクスチャ座標
			_Canvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.Normal;	// シェーダーセマンティクス：法線
			_Canvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.Tangent; // シェーダーセマンティクス：接線
		
			// -- キャンバス
			var _Image = _ImageObj.AddComponent<Image>();	// 画像表示設定
			_Image.sprite = _UseTexture;	// 表示画像設定
	}

	private void OnValidate()
	{
		// 定義域検査
		if (m_Ranks != null)	// 検証対象が存在
		{
			for (int _Idx = 1; _Idx < m_Ranks.Length; _Idx++)	// ランク単位でのループ
			{
				if (m_Ranks[_Idx].m_MinKilled <= m_Ranks[_Idx - 1].m_MinKilled)	// 前項と並び順が矛盾
				{
					m_Ranks[_Idx].m_MinKilled = m_Ranks[_Idx - 1].m_MinKilled + 1;	// 最小値を制限
				}
			}
		}
	}
}