/*=====
<MonoSingleton.cs>
���쐬�ҁFtakagi

�����e
MonoBehaviour�p�V���O���g���̎���

�����ӎ���
�E�W�F�l���b�N�֐��ł��B�p�����Ɍ^�w���Y��Ȃ����ƁB
�E�C���X�^���X��null�ȏꍇ������܂��B�擾���Ƀk���`�F�b�N���邱�ƁB

���X�V����
__Y24
_M06
D
05:�v���O�����쐬:takagi
06:�����Ftakagi
21:���t�@�N�^�����O:takagi
24:���t�@�N�^�����O:takagi
__Y25
_M06
D
14:���t�@�N�^�����O:takagi
=====*/

// ���O��Ԓ�`
using UnityEngine;

// �N���X��`

public abstract class CMonoSingleton<MonoType> : CVirtualizeMono where MonoType : CMonoSingleton<MonoType>	// where���Ōp���c���[�𖾎��FMonoType��CMonoSingleton<MonoType>��CVirtualizeMono��MonoBehaviour
{
	// �ϐ��錾
	static private MonoType m_Instance;	// �C���X�^���X�i�[�p

	// �v���p�e�B��`
	
	/// <summary>
	/// �C���X�^���X�v���p�e�B
	/// </summary>
	/// <value><see cref="m_Instance"/></value>
	public static MonoType Instance	// �p����I�u�W�F�N�g�̃C���X�^���X
	{
		get
		{
			if (m_Instance == null)	// �k���`�F�b�N
			{
				GameObject _GameObject = new GameObject();	// �C���X�^���X�쐬
				m_Instance = _GameObject.AddComponent<MonoType>();	// ���g�̃R���|�[�l���g�o�^
			}
			return m_Instance;	// �C���X�^���X��
		}
	}


	/// <summary>
	/// -�������֐�
	/// <para>�C���X�^���X��������ɍs������</para>
	/// </summary>
	protected override sealed void Awake()
	{
		// ���g�������ڂ�
		if(m_Instance != null && m_Instance.gameObject != null)	// ���łɎ��g�Ɠ���̂��̂�����
		{
			// �����L�����Z��
			Destroy(this.gameObject);	// ���g�̐������Ȃ��������Ƃɂ���
			return;	// �����͏�������Ȃ�
		}

		// �C���X�^���X�o�^
		m_Instance = (MonoType)this;	// ���g���C���X�^���X�Ƃ��ēo�^

		// �ǉ��̏���
		CustomAwake();	// �q�N���X�����̃^�C�~���O�ōs����������
	}

	/// <summary>
	/// -�j���֐�
	/// <para>�C���X�^���X�j�����ɍs������</para>
	/// </summary>
	protected override sealed void OnDestroy()
	{
		// �����L�����Z������
		if(this != m_Instance)	// �����L�����Z���̂��߂ɍs��ꂽ�j���ł���
		{
			// �I��
			return;	// ����ȍ~�̏����͋����ɍs������̂ł͂Ȃ�
		}

		// �C���X�^���X�j��
		if(m_Instance != null)	// �C���X�^���X�Ƃ��ēo�^����Ă���
		{
			m_Instance = null;	// �C���X�^���X���k���ɏ�����
		}

		// �ǉ��̏���
		CustomOnDestroy();	// �q�N���X�����̃^�C�~���O�ōs����������
	}
}