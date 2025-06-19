// �X�V:takagi
//0530 �ً}�Ή���

using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckClear : MonoBehaviour
{
	[SerializeField, Tooltip("�E�F�[�u�Ǘ���")] private CWaveTimerManager m_Wavemanager;
	[SerializeField, Tooltip("�N���A�(������)")] private int m_ClearNorm = 0;

	// 
	private void Start()
	{

		if (m_Wavemanager != null)
		{
		}
		
		// �C�x���g�ڑ�
		m_Wavemanager.OnFinishedFinalWave +=OnFinishedFinalWave;	// 
	}

	private void OnFinishedFinalWave()
	{

		if (CBattleData.Instance != null && CBattleData.Instance.KillCount < m_ClearNorm)	// ���s
		{
			SceneManager.LoadScene("GAMEOVER");
		}
		else	// �������B���I
		{
			SceneManager.LoadScene("GAMECLEAR");
		}
	}
}
