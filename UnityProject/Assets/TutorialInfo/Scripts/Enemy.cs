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
=====*/

// ���O��Ԑ錾
using System;
using UnityEngine;

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
	[SerializeField, Tooltip("�ǐՃt���O")] private bool m_bChase = true;	// �ǐ�

	private Transform m_Target;	// �v���C���[��Transform
	private bool m_bChasing = false;	// �ǐՒ����ǂ���


	// ���X�V�֐�
	// �����F�Ȃ�
	// ��
	// �ߒl�F�Ȃ�
	// ��
	// �T�v�F�X�V����
	private void Update()
	{
		if (m_bChasing) // �ǐՃt���O��true�̎�
		{
			Chaser(); // �ǐՊ֐����s
		}
	}

	// ���ǐՊ֐�
	// �����F�Ȃ�
	// ��
	// �ߒl�F�Ȃ�
	// ��
	// �T�v�F�v���C���[�̒ǐՂ�����
	public void Chaser()
	{
		if (m_bChase == true)	// �ǐՃt���O��true�̎��A�v���C���[�Ɍ������Ĉړ�
		{
			Vector3 direction = (m_Target.position - transform.position).normalized;
			transform.position = Vector3.MoveTowards(transform.position, m_Target.position, m_Status.m_fSpeed * Time.deltaTime);
		}
	}

	// ���_���[�W�֐�
	// �����F�Ȃ�
	// ��
	// �ߒl�F�Ȃ�
	// ��
	// �T�v�F�_���[�W��^����
	public void Damage(int _nDamage)�@
	{
		m_Status.m_nHp -= _nDamage;�@// �_���[�W����

		if (m_Status.m_nHp <= 0)	// HP��0�̎�
		{
			Destroy(gameObject);	// �G������
		}
	}

	// ���ǐՊJ�n�֐�
	// �����FCollider _Collision
	// ��
	// �ߒl�F�Ȃ�
	// ��
	// �T�v�F�v���C���[���͈͓��ɓ�������ǐՂ��J�n����
	private void OnTriggerEnter(Collider _Collision)
	{
		//�ڐG����(�ǐՔ͈�)
		if (_Collision.CompareTag("Player"))	// �v���C���[���͈͓��ɓ�������
		{
			m_Target = _Collision.transform;	// �v���C���[��Transform��ۑ�
			m_bChasing = true;	// �ǂ������J�n
		}
	}

	// ���ǐՏI���֐�
	// �����FCollider _Collision
	// ��
	// �ߒl�F�Ȃ�
	// ��
	// �T�v�F�v���C���[���͈͊O�ɏo����ǐՂ��I������
	private void OnTriggerExit(Collider _Collision)	// �ڐG����(�ǐՔ͈�)
	{
		if (_Collision.CompareTag("Player"))	// �v���C���[���͈͊O�ɏo����
		{
			m_bChasing = false;	// �ǂ������I��
			m_Target = null;	// �^�[�Q�b�g������
		}
	}
}