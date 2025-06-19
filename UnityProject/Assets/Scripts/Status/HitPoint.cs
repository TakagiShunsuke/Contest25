/*=====
<HitPoint.cs>
���쐬�ҁFtakagi

�����e
�����A�̗͂̋@�\������

���X�V����
__Y25
_M05
D
12:�v���O�������쐬:takagi
16:���t�@�N�^�����O:takagi
28:�h��𓝍�(�̗͂̋@�\�ɂ����֘A���Ȃ������Ȃ���)�E
	�ő�̗͂̊T�O��ǉ�:takagi
_M06
D
18:�C�x���g�̒�`��ǉ��A�֐��^��Action�ɕύX:takagi
=====*/

// ���O��Ԑ錾
using System;
using UnityEngine;

// �N���X��`
public class CHitPoint : MonoBehaviour
{
	// �萔��`
	private const int MIN_GUARANTEE_MAX_VALUE = 1;	// �ő�HP�̍Œ�ۏؒl

	// �C�x���g��`
	public event Action OnDamaged;	// ��_���[�W���̃C�x���g
	public event Action OnHealed;	// �񕜎��̃C�x���g
	public event Action OnDead;	// ���S���̃C�x���g

	// �ϐ��錾
	[SerializeField, Tooltip("�ő�̗�")] private int m_nMaxHP = MIN_GUARANTEE_MAX_VALUE;
	private int m_nHP = MIN_GUARANTEE_MAX_VALUE;	// �̗͎c��
	[SerializeField, Tooltip("�h��")]private int m_nDefence = 0;

	// �v���p�e�B��`

	/// <summary>
	/// ���S�t���O�v���p�e�B
	/// </summary>
	/// <value>���S��true, ����ȊO��false</value>
	public bool IsDead { get; private set; }	// ���S�t���O	/// <summary>

	/// �ő�HP�v���p�e�B
	/// </summary>
	/// <value><see cref="m_nMaxHP"/></value>
	public int MaxHP	// �\���̓I�ɋ@�\��؂�o���Ă���̂ŃQ�b�^�E�Z�b�^���K�v
	{
		get
		{
			// ��
			return m_nMaxHP;	// ����HP��
		}
		set
		{
			// �ޔ�
			var _nTemp = m_nMaxHP;	// �X�V�O�̍ő�l��ޔ�

			// �X�V
			m_nMaxHP = value;   // HP�̒l���X�V

			// ��������
			if (_nTemp < m_nMaxHP)	// �ő�l������
			{
				m_nHP += m_nMaxHP - _nTemp;	// ��������
			}

			// �␳
			if (m_nMaxHP < MIN_GUARANTEE_MAX_VALUE)	// �Œ�ۏ؂�˔j
			{
				m_nMaxHP = MIN_GUARANTEE_MAX_VALUE;	// �Œ�ۏ؂��@�\������
			}

			// �����W
			if(m_nMaxHP < m_nHP)	// HP�c�ʂ��ő�l�𒴉�
			{
				m_nHP = m_nMaxHP;	// �ő�l�ɗ}����
			}
		}
	}
	
	/// <summary>
	/// HP�v���p�e�B
	/// </summary>
	/// <value><see cref="m_nHP"/></value>
	public int HP	// �\���̓I�ɋ@�\��؂�o���Ă���̂ŃQ�b�^�E�Z�b�^���K�v
	{
		get
		{
			// ��
			return m_nHP;	// ����HP��
		}
		set
		{
			// �ޔ�
			var _nTemp = m_nHP;	// �X�V�O�̍ő�l��ޔ�

			// �X�V
			m_nHP = value;  // HP�̒l���X�V

			// �␳
			if (m_nMaxHP < m_nHP)	// �ő�l�𒴉�
			{
				m_nHP = m_nMaxHP;	// �ő�l�ɕ␳
			}

			// �C�x���g����
			if (m_nHP < 1)	// HP�������Ȃ���
			{
				IsDead = true;	// ���S���������ɂ���
				if (OnDead != null)	// �k���`�F�b�N
				{
					OnDead.Invoke();	// ���S���C�x���g�𔭍s	//TODO:���ꂾ�Ǝ���h���Ȃ�(�Ƃ��ɏ����̓r���ňꎞ�I�ɎE�����Ȃ�)�����Ƃ��ɕs���̌Ăяo�����������邽�ߗv���P
				}
			}
			else if(m_nHP < _nTemp)	// �_���[�W���󂯂�
			{
				if(OnDamaged != null)	// �k���`�F�b�N
				{
					OnDamaged.Invoke();	// ��_���[�W���C�x���g�𔭍s
				}
			}
			else if(m_nHP > _nTemp)	// �񕜂���
			{
				if(OnHealed != null)	// �k���`�F�b�N
				{
					OnHealed.Invoke();	// �񕜎��C�x���g�𔭍s
				}
			}
		}
	}

	/// <summary>
	/// �h��v���p�e�B
	/// </summary>
	/// <value><see cref="m_nDefence"/></value>
	public int Defence	// �\���̓I�ɋ@�\��؂�o���Ă���̂ŃQ�b�^�E�Z�b�^���K�v
	{
		get
		{
			// ��
			return m_nDefence;	// ����HP��
		}
		set
		{
			// �X�V
			m_nDefence = value;  // HP�̒l���X�V
		}
	}
}