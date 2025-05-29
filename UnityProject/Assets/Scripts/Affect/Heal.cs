/*=====
<Heal.cs>
���쐬�ҁFtakagi

�����e
�񕜋@�\������

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
public class CHeal : CAffect
{
	// �萔��`
	protected const int _MIN_DAMAGE = 1;	// �Œ�ۏ؉�

	// �ϐ��錾
	[Header("�p�����[�^")]
	[SerializeField, Tooltip("�񕜒l")] private float m_fHeal;
	private float m_fBaseCorrection = 0.0f;	// ��b�l�␳
	private float m_fCorrectionRatio = 1.0f;	// �␳�{��

	// �v���p�e�B��`

	/// <summary>
	/// ��b�񕜃v���p�e�B
	/// </summary>
	/// <value><see cref="m_fHeal"/></value>
	public float BaseHeal	// ���␳�񕜒l
	{
		get
		{
			// ��
			return m_fHeal;	// �v�Z�O�̊�b�񕜂��
		}
		set
		{
			// �X�V
			m_fHeal = value;	// �v�Z�O�̊�b�񕜂��X�V
		}
	}

	/// <summary>
	/// ��b�␳�v���p�e�B
	/// </summary>
	/// <value><see cref="m_fBaseCorrection"/></value>
	public float BaseCorrection
	{
		get
		{
			// ��
			return m_fBaseCorrection;	// �񕜂̊�b�l�␳���
		}
		set
		{
			// �X�V
			m_fBaseCorrection = value;	// �񕜂̊�b�l�␳���X�V
		}
	}

	/// <summary>
	/// �␳�{���v���p�e�B
	/// </summary>
	/// <value><see cref="m_fCorrectionRation"/></value>
	public float CorrectionRation
	{
		get
		{
			// ��
			return m_fCorrectionRatio;	// �񕜂̕␳�{�����
		}
		set
		{
			// �X�V
			m_fCorrectionRatio = value;	// �񕜂̕␳�{�����X�V
		}
	}

	/// <summary>
	/// �␳�񕜃v���p�e�B
	/// </summary>
	/// <value>
	/// <para>�␳��̉񕜂��</para>
	/// <para>�Q�l�F<see cref="m_fHeal">��b��</see></para>
	/// <para>�Q�l�F<see cref="m_fBaseCorrection">��b�l�␳</see></para>
	/// <para>�Q�l�F<see cref="m_fCorrectionRatio">�␳�{��</see></para>
	/// </value>
	protected float CorrectedHeal
	{
		get
		{
			// ��
			return (m_fHeal + m_fBaseCorrection) * m_fCorrectionRatio;	// �␳��񕜂��
		}
	}

	
	/// <summary>
	/// -�񕜌��ʊ֐�
	/// <para>�񕜂�^������ʂ��s���֐�</para>
	/// </summary>
	/// <param name="_Oneself">���ʂ̔�����</param>
	/// <param name="_Opponent">���ʂ̎󓮎�</param>
	public override void Affect(GameObject _Oneself, GameObject _Opponent)
	{
		// �ϐ��錾
		var _HitPoint = _Opponent.GetComponent<CHitPoint>();	// �񕜂��󂯂�HP

		// �񕜏���
		if(_HitPoint)	// �񕜂��󂯂���
		{
			// �񕜂�^����
			_HitPoint.HP += (int)CorrectedHeal;	// �ŏI�񕜂�HP�ɉe��������
		}
	}
}