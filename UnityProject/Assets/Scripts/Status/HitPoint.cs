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
=====*/

// ���O��Ԑ錾
using UnityEngine;

// �N���X��`
public class CHitPoint : MonoBehaviour
{
	// �C�x���g��`
	public delegate void DeathEvent();	// �C�x���g�p�̊֐���`
	public event DeathEvent OnDead;	// ���S���̃C�x���g

	// �ϐ��錾
	[SerializeField]private int m_nHP;	// �̗͒l
	
	// �v���p�e�B��`

	/// <summary>
	/// ���S�t���O�v���p�e�B
	/// </summary>
	/// <value>���S��true, ����ȊO��false</value>
	public bool IsDead { get; private set; }	// ���S�t���O
	
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
			// �X�V
			m_nHP = value;	// HP�̒l���X�V

			// ���S����
			if (m_nHP < 1)	// HP�������Ȃ���
			{
				IsDead = true;	// ���S���������ɂ���
				if (OnDead != null)	// �k���`�F�b�N
				{
					OnDead.Invoke();	// ���S���C�x���g�𔭍s	//TODO:���ꂾ�Ǝ���h���Ȃ�(�Ƃ��ɏ����̓r���ňꎞ�I�ɎE�����Ȃ�)�����Ƃ��ɕs���̌Ăяo�����������邽�ߗv���P
				}
			}
		}
	}
}