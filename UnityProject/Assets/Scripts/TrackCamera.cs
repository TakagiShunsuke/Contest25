/*=====
<TrackCamera.cs>
���쐬�ҁFtakagi

�����e
�^�[�Q�b�g��Ǐ]����J�����̃X�N���v�g

�����ӎ���
�E�ǐՑΏۂ��ݒ肳��Ă��Ȃ��Ɠ����܂���(�x���f���܂�)�B
�E�ҏW���[�h���ɂ����삷�邽�߃g�����X�t�H�[���̕ύX�������������A��ɒǐՂ��������܂��B

���X�V����
__Y25
_M05
D
07:�v���O�����쐬�J�n:takagi
08:�v���O�����쐬����:takagi
=====*/

// ���O��Ԑ錾
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// �N���X��`
[ExecuteAlways]	// ��Đ����ɂ�����
public class CTrackCamera : MonoBehaviour
{
	// �ϐ��錾
	[Header("�ǐՏ��")]
	[SerializeField, Tooltip("�ǐՑΏ�")] private GameObject m_Target;
	[SerializeField, Tooltip("���Έʒu")] private Vector3 m_RelativePosition;
	[SerializeField, Tooltip("�����_�␳")] private Vector3 m_CorrectLookAt;


	// ���J�����p���֐�
	// �����P�F�Ȃ�
	// ��
	// �ߒl�F�Ȃ�
	// ��
	// �T�v�F�ǐՏ����ɔ���������ړ��̉��Z����
	private void Tracking()
	{
		// ����
		if (!m_Target)	// �K�v�v���̕s����
		{
#if UNITY_EDITOR	//�G�f�B�^�g�p��             
			// �G���[�o��
			Debug.LogWarning("�K�v�ȗv�f���s�����Ă��܂�");	// �x�����O�o��
#endif

			// ���f
			return;	// �X�V�������f
		}

		// �ϐ��錾
		Vector3 _ToLooking = m_Target.gameObject.transform.position + m_CorrectLookAt - transform.position;	// �����_�ɕ␳���悹�ē�_�ԃx�N�g�����Z�o


		// �ړ�
		transform.position = m_Target.gameObject.transform.position + m_RelativePosition;	// �J�����̍��W���v�Z

		// ���������Z
		transform.LookAt(m_Target.gameObject.transform.position + m_CorrectLookAt);	// ����̂悤�Ȃ����̒Ǐ]�P�[�X(��ԂȂǂ𒲐����Ȃ��ꍇ)�ɂ͂���̕����K��
	}

	// �������X�V�֐�
	// �����P�F�Ȃ�
	// ��
	// �ߒl�F�Ȃ�
	// ��
	// �T�v�F�����X�V����
	private void FixedUpdate()
	{
		// �ǐՏ���
		Tracking();	// �v���C���[�̕����X�V�ɔ����čX�V����
	}

#if UNITY_EDITOR	//�G�f�B�^�g�p��
	// ���X�V�֐�
	// �����P�F�Ȃ�
	// ��
	// �ߒl�F�Ȃ�
	// ��
	// �T�v�F�ҏW�����ǐՏ������ғ������邽�߂̍X�V����
	void Update()
	{
		// �ҏW/�Đ��̃��[�h�ɂ���ď�Ԑ؂�ւ�
		if (!Application.isPlaying)	// �ҏW���[�h
		{
			// �ǐՋ@�\�̍P�퉻
			if (transform.hasChanged)	// �g�����X�t�H�[���̕ύX���m�F
			{
				transform.hasChanged = false;	// �ύX�t���O���N���A
				Tracking();	// �ύX�𖳌���
			}
		}
	}
	
	// ���C���X�y�N�^�[�l�ύX�������֐�
	// �����P�F�Ȃ�
	// ��
	// �ߒl�F�Ȃ�
	// ��
	// �T�v�F�C���X�y�N�^�ŎQ�ƌ��⑊�΂Ȃǂ̍X�V���������Ƃ��ɑ����ɔ��f����
	private void OnValidate()
	{
		// �ǐՏ���
		Tracking();	// �ύX���ꂽ�l�ɍ��킹�Ĉʒu����ς���
	}

	// ���M�Y���`��֐�
	// �����P�F�Ȃ�
	// ��
	// �ߒl�F�Ȃ�
	// ��
	// �T�v�F�ǐՑΏۂƂ̊֌W�������₷���悤�ɃV�[���r���[�ŃM�Y���\��
	private void OnDrawGizmos()
	{
		// ����
		if (!m_Target)	// �K�v�v���̕s����
		{
			// �G���[�o��
			Debug.LogWarning("�K�v�ȗv�f���s�����Ă��܂�");	// �x�����O�o��

			// ���f
			return;	// �X�V�������f
		}

		// �J����-�Ώە��Ԃ̐���
		Gizmos.color = Color.blue;	// �ŕ\��
		Gizmos.DrawLine(transform.position, m_Target.gameObject.transform.position);	// �����`��

		// �J����-�����_�Ԃ̐���
		Gizmos.color = Color.red;	// �Ԃŕ\��
		//Gizmos.DrawIcon()
		Gizmos.DrawLine(transform.position, m_Target.gameObject.transform.position + m_CorrectLookAt);	// �����`��
	}
#endif
}