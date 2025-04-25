/*=====
<Enemy.cs>
���쐬�ҁFsezaki

�����e
�G�i�v���g�^�C�v�j����

���X�V����
__Y25
_M04
D
16:�v���O�����쐬:sezaki
23:�ϐ����C���E
	Damage()�֐��̃_���[�W�l��������:sezaki
23:�t�@�C�����[�h��spc���^�u�ɕύX:takagi
//TODO:navMesh��ǉ��������Ƃ��L�q
25:
=====*/

// ���O��Ԑ錾
using System;
using UnityEngine;
using UnityEngine.AI;

// �N���X��`
public class CEnemy : MonoBehaviour
{
	// �\���̒�`
	[Serializable]
	public struct Status //�G�X�e�[�^�X
	{
		[SerializeField, Tooltip("HP")] public int m_nHp;					// HP
		[SerializeField, Tooltip("�U����")] public int m_nAttack;			// �U����
		[SerializeField, Tooltip("���x")] public float m_fSpeed;			// ����
		[SerializeField, Tooltip("�U�����x")] public int m_nAttackSpeed;	// �U�����x
		[SerializeField, Tooltip("�h���")] public int m_nDefense;			// �h��
		[SerializeField, Tooltip("����")] public int m_nGrowth;				// ����
	}

	// �ϐ��錾

	[Header("�X�e�[�^�X")]
	[SerializeField, Tooltip("�X�e�[�^�X")] private Status m_Status;

	[Header("�ǐ�")]
	[SerializeField] private NavMeshAgent m_Agent;  //TODO:�R�����g
													//TODO:�u���g�̎��G�[�W�F���g���g���v�Ȃ珉���������Ŏ����擾����ׂ��ł��B
													//TODO:���̏ꍇ�A[SerializeField]�Őݒ�ł���悤�ɂ���K�v�͂���܂���
	[SerializeField] private Transform m_Target;	// �v���C���[��Transform
													//TODO:Tooltip�̐ݒ肪�K�v

	// ���X�V�֐�
	// �����F�Ȃ�
	// ��
	// �ߒl�F�Ȃ�
	// ��
	// �T�v�F�X�V����
	private void Update()
	{
		//TODO:�R�����g
		m_Agent.SetDestination(m_Target.position);
	}

	// ���_���[�W�֐�
	// �����F�Ȃ�
	// ��
	// �ߒl�F�Ȃ�
	// ��
	// �T�v�F�_���[�W��^����
	public void Damage(int _nDamage)
	{
		if (_nDamage <= m_Status.m_nDefense)// �h�䂪��_�������������_����1�ɂ���
		{
			_nDamage = 1;
		}
		else
		{
			_nDamage = _nDamage - m_Status.m_nDefense;
		}

		m_Status.m_nHp -= _nDamage;�@// �_���[�W����

		if (m_Status.m_nHp <= 0)	// HP��0�̎�
		{
			Destroy(gameObject);	// �G������
		}
	}
}