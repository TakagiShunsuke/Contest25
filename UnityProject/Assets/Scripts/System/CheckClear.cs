// 更新:takagi
//0530 緊急対応☆

using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckClear : MonoBehaviour
{
	[SerializeField, Tooltip("ウェーブ管理者")] private CWaveTimerManager m_Wavemanager;
	[SerializeField, Tooltip("クリア基準(討伐数)")] private int m_ClearNorm = 0;

	// 
	private void Start()
	{

		if (m_Wavemanager != null)
		{
		}
		
		// イベント接続
		m_Wavemanager.OnFinishedFinalWave +=OnFinishedFinalWave;	// 
	}

	private void OnFinishedFinalWave()
	{

		if (CBattleData.Instance != null && CBattleData.Instance.KillCount < m_ClearNorm)	// 失敗
		{
			SceneManager.LoadScene("GAMEOVER");
		}
		else	// 討伐数達成！
		{
			SceneManager.LoadScene("GAMECLEAR");
		}
	}
}
