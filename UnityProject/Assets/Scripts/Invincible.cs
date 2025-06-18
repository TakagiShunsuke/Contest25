/*=====
<Invincible.cs>
���쐬�ҁFkato

�����e
���G��Ԃ�����

�����ӎ���
�E���G�̏�ԃt���O�̂悤�Ȋ��o�ŗp���܂��B
�E���ۂ̖��G�����͂����ɂ͂Ȃ����ߒ��ӁB

���X�V����
__Y25
_M05
D
30:�v���O�����쐬:kato
_M06
D
17:���t�@�N�^�����O:takagi
=====*/

// ���O��Ԑ錾
using UnityEngine;

// �N���X��`
public class CInvincible : MonoBehaviour
{
	// �ϐ��錾
	[Header("�X�e�[�^�X")]
	[SerializeField, Tooltip("�@�\����")] private float m_fAliveTime = 0.0f;

	// �v���p�e�B��`
	
	/// <summary>
	/// �@�\���ԃv���p�e�B
	/// </summary>
	/// <value><see cref="m_fAliveTime"/></value>
	public float AliveTime
	{
		get
		{
			// ��
			return m_fAliveTime;	// �c�@�\���Ԓ�
		}
		set
		{
			// �X�V
			if (m_fAliveTime < value)	// �p�����ԍX�V(����)
			{
				m_fAliveTime = value;	// �������󂯕t��
			}
		}
	}


#if UNITY_EDITOR // �G�f�B�^�g�p��
	/// <summary>
	/// -�������֐�
	/// <para>����X�V���̏���</para>
	/// </summary>
	private void Start()
	{
		Debug.Log("���G�J�n");
		if(m_fAliveTime < 0.0f)	// �@�\���Ԃ��Ȃ�
		{
			// �o��
			Debug.LogError("�@�\���Ԃ��ُ�ł�");
		}
	}
#endif

	/// <summary>
	/// �X�V�֐�
	/// <para>�X�V����</para>
	/// </summary>
	private void Update()
	{
		// �^�C�}�[�X�V
		m_fAliveTime -= Time.deltaTime;	// �c���Ԃ̎��R����

		// ����
		if (m_fAliveTime < 0.0f)	// �@�\��~
		{
			Debug.Log("���G����");
			Destroy(this);	// ���G����
		}
	}
}
