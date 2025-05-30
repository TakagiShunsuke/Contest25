/*=====
<PrintKillCount.cs>
���쐬�ҁFtakagi

�����e
__Y25
_M05
D
30:���쐬(�ً}):takagi
=====*/

using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CPrintKillCount : MonoBehaviour
{
	// �ϐ�
	[SerializeField, Tooltip("�Ă�����")] private TextMeshProUGUI m_TMP;

	private string GetTextMsg()
	{
		return "kill: " + string.Format("{0}", CCountEnemy.m_DeathCount);
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		m_TMP = GetComponent<TextMeshProUGUI>();	//TODO:�k���`�F�b�Ntime_txt.SetText("{0}",(int)m_fTime);
		if(m_TMP == null) Debug.LogError(";;");
		m_TMP.text = GetTextMsg();
	}

	// Update is called once per frame
	void Update()
	{
		//���Ԑ����𒴂������̏���
		if (m_TMP != null)
		{
			m_TMP.text = GetTextMsg();
		}

	}
}
