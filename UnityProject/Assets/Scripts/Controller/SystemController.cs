/*=====
<SystemController.cs>	// �X�N���v�g��
���쐬�ҁFtakagi

�����e
�V�X�e���p�̓��͑Ή�

�����ӎ���
�S�V�[���œ������Ă���	//TODO:������
//TODO:�V���O���g��

���X�V����
__Y25
_M05
D
09:�v���O�������쐬:takagi
=====*/

// ���O��Ԑ錾
using UnityEngine;

// �N���X��`
public class CSystemController : MonoBehaviour
{
	// �ϐ��錾
	[Header("����")]
	[SerializeField, Tooltip("�Q�[���I���L�[")] private KeyCode m_FinishKey;

	// �X�V�֐�
	// �����P�F�Ȃ�
	// ��
	// �ߒl�F�Ȃ�
	// ��
	// �T�v�F�L�[��t���菈��	//TODO:���̓C�x���g�֐����������͂�...�u�������邱��
	void Update()
	{
		// �I������
#if !UNITY_EDITOR
		if(Input.GetKeyUp(KeyCode.Escape))	// �I���R�}���h
		{
			Application.Quit();	// �A�v���P�[�V�����I��
		}
#endif
		
	}
}
