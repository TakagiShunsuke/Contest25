//TODO�F��ŃR�����g


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class CDamageNonKilling : CDamage
{
	// �萔��`
	[SerializeField, Tooltip("�_���[�W�l")]public static int _min_damage = 0;
	// �ϐ��錾
	[Header("�p�����[�^")]
	[SerializeField, Tooltip("�_���[�W�l")] private float m_fDamage;

	// �v���p�e�B��`
	public float BaseDamage	// ���␳�_���[�W�l
	{
		get
		{
			return m_fDamage;
		}
		set
		{
			m_fDamage = value;
		}
	}
	public float FinalDamage	// �␳��ŏI�_���[�W�l
	{
		get
		{
			return m_fDamage;
		}
	}

	public override sealed void Affect(GameObject _Oneself, GameObject _Opponent)
	{
		// �ϐ��錾
		var _HitPoint = _Opponent.GetComponent<CHitPoint>();
		var _Deffence = _Opponent.GetComponent<CDefence>();

		if(_HitPoint)
		{
			float def = 0;
			if(_Deffence)
			{
				def = _Deffence.Defence;
			}

			if (_HitPoint.HP - CulcDamage(FinalDamage, def) > 0)	// ���ȂȂ�
			{
				_HitPoint.HP -= CulcDamage(FinalDamage, def);
			}
			else if(_HitPoint.HP > 0)	// ���̃_���[�W�����Ŏ���
			{
				_HitPoint.HP = 1;	// 1�ς�����
			}

		}
	}


	// �_���[�W�v�Z��	//TODO:�e�֐��Ɉړ�
	protected int CulcDamage(float _fDamageValue, float _fDefence)
	{	
		// �ϐ��錾
		int _Result = 0;

		// �_���[�W�v�Z
		_Result = (int)(_fDamageValue - _fDefence);

		// �␳
		if(_Result < 1)
		{
			_Result = 1;	// 1�_���[�W�͕ۏ�	//TODO:�}�W�b�N�i���o�[����
		}

		// ��
		return _Result;
	}
}