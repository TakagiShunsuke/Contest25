/*=====
<PrintKillCount.cs>
└作成者：takagi

＞内容
__Y25
_M05
D
30:仮作成(緊急):takagi
=====*/

using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CPrintKillCount : MonoBehaviour
{
	// 変数
	[SerializeField, Tooltip("てきすと")] private TextMeshProUGUI m_TMP;

	private string GetTextMsg()
	{
		return "kill: " + string.Format("{0}", CCountEnemy.m_DeathCount);
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		m_TMP = GetComponent<TextMeshProUGUI>();	//TODO:ヌルチェックtime_txt.SetText("{0}",(int)m_fTime);
		if(m_TMP == null) Debug.LogError(";;");
		m_TMP.text = GetTextMsg();
	}

	// Update is called once per frame
	void Update()
	{
		//時間制限を超えた時の処理
		if (m_TMP != null)
		{
			m_TMP.text = GetTextMsg();
		}

	}
}
