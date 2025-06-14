/*=====
<ClearManage.cs>
└作成者：takagi

＞内容
クリア判定を実装

＞更新履歴
__Y25
_M06
D
13:プログラム作成完了:takagi
14:バトルデータのクリア処理を追加:takagi
=====*/

// 名前空間宣言
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

// クラス定義
public class CResultSceneChanger : MonoBehaviour
{
	// 変数宣言
	[SerializeField, Tooltip("遷移先シーン")] private SceneDropDown m_Scene;
	[SerializeField, Tooltip("遷移キー")] private KeyCode m_LoadKey = KeyCode.Return;	
	[SerializeField, Tooltip("選択音")] public AudioClip m_DecideSE;
	[SerializeField, Tooltip("選択音量")] private float m_DecideSEVolume = 0.05f;
	private AudioSource m_DecideSESource;	// 選択SE用のオーディオソース
	

	/// <summary>
	/// -初期化関数
	/// <para>初期化処理</para>
	/// </summary>
	private void Start()
	{
		// 音源準備
		m_DecideSESource = gameObject.AddComponent<AudioSource>();	// 選択用の音源コンポーネント作成
		m_DecideSESource.volume = m_DecideSEVolume;	// 音量を設定
	}

	/// <summary>
	/// -更新関数
	/// <para>更新処理</para>
	/// </summary>
	void Update()
	{
		// 保全
		if (m_Scene.SceneName == string.Empty)	// ヌルチェック
		{
#if UNITY_EDITOR
			Debug.LogError("遷移先シーンが見つかりません");
#endif	// !UNITY_EDITOR
			return;	// 処理中断
		}

		// シーン遷移
		if (Input.GetKey(m_LoadKey))	// シーン遷移入力時
		{
			// 専用データクリア
			if (CBattleData.Instance != null)	// ヌルチェック
			{
				CBattleData.Instance.Clear();	// リザルト用のデータなのでシーン遷移時には消しておく
			}

			// 効果音再生
			if (!m_DecideSESource.isPlaying)	// 再生が重ならない
			{
				m_DecideSESource.PlayOneShot(m_DecideSE);	// 選択音再生
			}

			// 遷移実行
			SceneManager.LoadScene(m_Scene.SceneName);	// 設定されている次シーンへ遷移
		}
	}
}