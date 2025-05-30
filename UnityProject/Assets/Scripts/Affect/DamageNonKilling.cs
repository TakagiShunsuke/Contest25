/*=====
<DamageNonKilling.cs>
���쐬�ҁFtakagi

�����e
��v�����_���[�W�@�\������

�����ӎ���
�E��ʓI��(�H)�_���[�W�v�Z���␳����������@�\��p�ӂ��Ă��܂��B
	�v���p�e�B����G��܂����A������ԂȂ�␳���Ȃ����(���Ŏ��_�ł̎d�l���ʂ�)�ɂȂ�܂��B

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
public class CDamageNonKilling : CDamage
{
	/// <summary>
	/// -��v�����_���[�W���ʊ֐�
	/// <para>�s�E�̃_���[�W���ʂ��s���֐�</para>
	/// </summary>
	/// <param name="_Oneself">���ʂ̔�����</param>
	/// <param name="_Opponent">���ʂ̎󓮎�</param>
	public override sealed void Affect(GameObject _Oneself, GameObject _Opponent)
	{
		// �ۑS
		if(_Opponent == null)	// ���肪���Ȃ�
		{
#if UNITY_EDITOR
			Debug.Log("���ʔ����Ώۂ�������܂���");
#endif	// !UNITY_EDITOR
			return;	// �������f
		}

		// �ϐ��錾
		var _HitPoint = _Opponent.GetComponent<CHitPoint>();	// �_���[�W���󂯂�HP
		
		// �_���[�W����
		if(_HitPoint)	// �_���[�W���󂯂���
		{
			if (_HitPoint.HP - CulcDamage(CorrectedDamage, _HitPoint.Defence) > 0)	// ���ȂȂ�
			{
				_HitPoint.HP -= CulcDamage(CorrectedDamage, _HitPoint.Defence);	// �ʏ�̃_���[�W����
			}
			else if(_HitPoint.HP > 0)	// ���̃_���[�W�����Ŏ���
			{
				_HitPoint.HP = 1;	// ��v�������ʂ𔭓�
			}
		}
	}
}