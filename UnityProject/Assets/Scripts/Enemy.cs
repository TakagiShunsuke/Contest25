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
	[SerializeField, Tooltip("�X�e�[�^�X")] private Status m_Status; // �X�e�[�^�X

	private NavMeshAgent m_Agent;  // �ǐՑΏ�

    [SerializeField, Tooltip("�^�[�Q�b�g")] private Transform m_Target;  // �v���C���[��Transform


    // ���������֐�
    // �����F�Ȃ�
    // ��
    // �ߒl�F�Ȃ�
    // ��
    // �T�v�F����������
    private void Start()
    {
        // NavMeshAgent���擾
        m_Agent = GetComponent<NavMeshAgent>();
    }

    // ���X�V�֐�
    // �����F�Ȃ�
    // ��
    // �ߒl�F�Ȃ�
    // ��
    // �T�v�F�X�V����
    private void Update()
	{
		//�ǐ�
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
		else// �_���[�W��^����
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