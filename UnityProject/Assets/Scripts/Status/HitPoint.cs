


using UnityEngine;

public class CHitPoint : MonoBehaviour
{
	// �C�x���g��`
	public delegate void DeathEvent();	// �C�x���g�p�̊֐���`
	public event DeathEvent OnDead;	// ���S���̏���

	// �ϐ��錾
	[SerializeField]private int m_nHP;	// �̗͒l
	
	// �v���p�e�B��`
	public bool IsDead { get; private set; }	// ���S�t���O

	public int HP	// �\���̓I�ɋ@�\��؂�o���Ă���̂ŃQ�b�^�E�Z�b�^���K�v
	{
		get
		{
			return m_nHP;
		}
		set
		{
			m_nHP = value;
			if (m_nHP < 1)  // HP�������Ȃ���
			{
				IsDead = true;  // ���S����������
				if (OnDead != null)
				{
					OnDead.Invoke();	//TODO:���ꂾ�Ǝ���h���Ȃ�(�Ƃ��ɏ����̓r���ňꎞ�I�ɎE�����Ȃ�)�����Ƃ��ɕs���̌Ăяo�����������邽�ߗv���P
				}
			}

		}
	}
}
