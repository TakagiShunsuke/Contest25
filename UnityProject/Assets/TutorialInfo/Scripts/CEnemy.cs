/*=====
<CodingRule.cs>	// �X�N���v�g��
���쐬�ҁFsezaki

�����e
�G�i�v���g�^�C�v�j����

�����ӎ���	// �Ȃ��Ƃ��͏ȗ�OK
���̋K�񏑂ɋL�q�̂Ȃ����͔̂�������A�K�X�ǉ�����

���X�V����
__Y25	// '25�N
_M04	// 4��
D		// ��
16:�v���O�����쐬:sezaki	// ���t:�ύX���e:�{�s��
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
        [SerializeField, Tooltip("HP")] public int m_nHp;                  // HP
        [SerializeField, Tooltip("�U����")] public int m_nAttack;       // �U����
        [SerializeField, Tooltip("���x")] public float m_fSpeed;             // ����
        [SerializeField, Tooltip("�U�����x")] public int m_nAttackSpeed;   // �U�����x
        [SerializeField, Tooltip("�h���")] public int m_nDefense;         // �h��
        [SerializeField, Tooltip("����")] public int m_nGrowth;            // ����
    }

    // �ϐ��錾

    [Header("�X�e�[�^�X")]
    [SerializeField, Tooltip("�X�e�[�^�X")] private Status m_Status;

    [Header("�ǐ�")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform target;


    // ���X�V�֐�
    // �����F�Ȃ�   
    // ��
    // �ߒl�F�Ȃ�
    // ��
    // �T�v�F�X�V����
    private void Update()
    {
        agent.SetDestination(target.position);
    }

    // ���_���[�W�֐�
    // �����F�Ȃ�   
    // ��
    // �ߒl�F�Ȃ�
    // ��
    // �T�v�F�_���[�W��^����
    public void Damage(int _nDamage)�@
    {
        if(_nDamage <= m_Status.m_nDefense)// �h�䂪��_�������������_����1�ɂ���
        {
            _nDamage = 1;
        }
        else
        {
            _nDamage = _nDamage - m_Status.m_nDefense;
        }

        m_Status.m_nHp -= _nDamage;

        if (m_Status.m_nHp <= 0) // HP��0�̎�
        {
            Destroy(gameObject); // �G������
        }
    }
}