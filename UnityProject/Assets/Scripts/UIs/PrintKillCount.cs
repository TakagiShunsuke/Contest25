/*=====
<PrintKillCount.cs>
���쐬�ҁFtakagi

�����e
�������\���̎���

�����ӎ���
�����I��TMP��t�^����̂Őݒ肵�Ă��Ȃ��ƕςȈʒu�ɏo�邱�Ƃɂ��Ȃ肦�܂��B

���X�V����
__Y25
_M05
D
30:���쐬(�ً}):takagi
_M06
D
14:�o�g���f�[�^�̓o��ɔ����y�����t�@�N�^�����O:takagi
=====*/

// ���O��Ԑ錾
using TMPro;
using UnityEngine;

// �N���X��`
[RequireComponent(typeof(TextMeshProUGUI))]
public class CPrintKillCount : MonoBehaviour
{
	// �ϐ��錾
	[SerializeField, Tooltip("�������\���ꏊ")] private TextMeshProUGUI m_TMP;


	/// <summary>
	/// -�������\�����擾�֐�
	/// <para>�������o�̓e�L�X�g�̋��ʉ���}�鏈��</para>
	/// </summary>
	/// <returns>�������̏o�̓e�L�X�g</returns>
	private string GetKillPrint()
	{
		// ��	
		return "kill: " + CBattleData.Instance.KillCount;	// �������̕\�����@
	}
	
	/// <summary>
	/// -�������֐�
	/// <para>����������</para>
	/// </summary>
	private void Start()
	{
		// �e�L�X�g������
		m_TMP = GetComponent<TextMeshProUGUI>();	// �@�\�擾
		m_TMP.text = GetKillPrint();	// �\�����e������
	}
	
	/// <summary>
	/// -�X�V�֐�	//TODO:�C�x���g�œG���|���ꂽ�Ƃ������Ăяo�����悤��
	/// <para>�X�V����</para>
	/// </summary>
	private void Update()
	{
		// �X�V
		if (m_TMP != null)	// �k���`�F�b�N
		{
			m_TMP.text = GetKillPrint();	// �\���X�V
		}
	}
}
