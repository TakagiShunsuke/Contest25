/*=====
<Blood.cs>
���쐬�ҁFtakagi

�����e
���Ղ̋@�\����

�����ӎ���
�ECAffect�̔h���R���|�[�l���g���Ȃ��Ƌ@�\���܂���B
�E//TODO:�R�[�h���K��Ɋ񂹂�K�v����B
�EUID�ɑ΂��ă^�C�}�[�������ɗp�ӂł��Ă���_�͎��R��good
�EDestroy�����ȂǁA���炩�Ɏ̂ĂĂ���UID�ɕR�Â����^�C�}�[����������������I�ݑ�����_��bad

���X�V����
__Y25
_M05
D
11:�v���O�����쐬:takagi
30:�뎚�C��:takagi
_M06
D
20:���ʂ�SO�Őݒ�ł���悤�ɕύX
	�E�s�v�Ȋ֐����폜:takagi
=====*/

// ���O��Ԑ錾
using System.Collections.Generic;
using UnityEngine;

// �N���X��`
public class CBlood : MonoBehaviour
{
	// �񋓒�`
	private enum E_BLOOD_EVENT
	{
		ON_STAY,	// �̉t��ɔ���
	}

	// �ϐ��錾
	[Header("����")]
	[SerializeField, Tooltip("���ʔ����Ԋu")] private float m_fCoolTime;
	private Dictionary<int, float> m_fCoolDownTimers = new Dictionary<int, float>();	// ���Ԍv���p
	[SerializeField, CIndexWithEnum(typeof(E_BLOOD_EVENT)), Tooltip("���ʃC�x���g")] private EventAffects[] m_InnerAffectEventor;	// ���ʗp�̃C�x���g�Ǘ�


	// �������X�V�֐�
	// �����F�Ȃ�
	// ��
	// �ߒl�F�Ȃ�
	// ��
	// �T�v�F���������Ń^�C�}�[�X�V
	void FixedUpdate()
	{
		// �ϐ��錾
		List<int> _nTimerKeys = new List<int>(m_fCoolDownTimers.Keys);	// ���[�v�Ŏ������ꗥ�X�V���邽�߂̃L�[�R�s�[

		// �^�C�}�[�X�V
		foreach (var _nKey in _nTimerKeys)	// �e�^�C�}�[�P��
		{
			if(m_fCoolDownTimers[_nKey] > 0.0f)
			{
				m_fCoolDownTimers[_nKey] -= Time.fixedDeltaTime;	// �X�V�t���[���Ԏ��Ԃ�݉�

				// �␳
				if(m_fCoolDownTimers[_nKey] < 0.0f)	// �J�E���g����
				{
					m_fCoolDownTimers[_nKey] = 0.0f;	// 0�őł��~��
				}
			}
		}
	}

	private void OnTriggerEnter(Collider _Entered)
	{
		// 
		if (!m_fCoolDownTimers.ContainsKey(_Entered.gameObject.GetInstanceID()))	// ���o�^
		{
			// ���ʔ���
			//if(m_Affect)
			//	m_Affect.Affect(gameObject, _Entered.gameObject);	// ����������Ɍ��ʂ𔭓�
			
			//m_InnerAffectEventor.InvokeEvent(E_BLOOD_EVENT.ON_STAY, gameObject, _Entered.gameObject);
			foreach (var af in m_InnerAffectEventor[(int)E_BLOOD_EVENT.ON_STAY].m_Affects )
			{
				af.Affect(gameObject, _Entered.gameObject);
			}

			// �N�[���^�C���J�n
			m_fCoolDownTimers.Add(_Entered.gameObject.GetInstanceID(), m_fCoolTime);	// �^�C�}�[��o�^
		}
	}

	private void OnTriggerStay(Collider _Staying)
	{
		// 
		if (m_fCoolDownTimers.ContainsKey(_Staying.gameObject.GetInstanceID()))	// �o�^��
		{
			if (m_fCoolDownTimers[_Staying.gameObject.GetInstanceID()] == 0.0f)	// 
			{
				// ���ʔ���
				//m_Affect.Affect(gameObject, _Staying.gameObject);   // ����������Ɍ��ʂ𔭓�
				//m_InnerAffectEventor.InvokeEvent(E_BLOOD_EVENT.ON_STAY, gameObject, _Staying.gameObject);
				foreach (var af in m_InnerAffectEventor[(int)E_BLOOD_EVENT.ON_STAY].m_Affects )
				{
					af.Affect(gameObject, _Staying.gameObject);
				}

				// �N�[���^�C���J�n
				m_fCoolDownTimers[_Staying.gameObject.GetInstanceID()] = m_fCoolTime;	// �^�C�}�[�����Z�b�g
			}
		}
#if UNITY_EDITOR	// �G�f�B�^�g�p��
		else
		{
			// �G���[�o��
			Debug.LogError("�N�[���^�C�����o�^����Ă��܂���");	// ���O�o��
		}
#endif
	}
}