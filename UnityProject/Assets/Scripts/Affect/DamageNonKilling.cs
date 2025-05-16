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
	public override sealed void Affect(GameObject _Oneself, GameObject _Opponent)
	{
		// �ϐ��錾
		var _HitPoint = _Opponent.GetComponent<CHitPoint>();	// �_���[�W���󂯂�HP
		var _Deffence = _Opponent.GetComponent<CDefence>();	// �_���[�W�ɑ΂���h��l
		
		// �_���[�W����
		if(_HitPoint)	// �_���[�W���󂯂���
		{
			// �ϐ��錾
			float _DeffenceValue = 0;	// �h��l

			// �h����擾
			if(_Deffence)	// �h�䂪����
			{
				_DeffenceValue = _Deffence.Defence;	// �h��l���X�V
			}

			if (_HitPoint.HP - CulcDamage(CorrectedDamage, _DeffenceValue) > 0)	// ���ȂȂ�
			{
				_HitPoint.HP -= CulcDamage(CorrectedDamage, _DeffenceValue);	// �ʏ�̃_���[�W����
			}
			else if(_HitPoint.HP > 0)	// ���̃_���[�W�����Ŏ���
			{
				_HitPoint.HP = 1;	// ��v�������ʂ𔭓�
			}
		}
	}
}