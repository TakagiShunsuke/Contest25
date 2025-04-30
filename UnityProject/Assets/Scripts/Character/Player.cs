/*=====
<Player.cs>
���쐬�ҁFkato

�����e
�v���C���[�̈ړ��ƍU���𐧌䂷��X�N���v�g

���X�V����
__Y25
_M04
D
18:�v���C���[�̈ړ��ƍU�������B:kato
21:�\�L���̏C��:takagi
22:�L�[�̕ϐ���:kato
23:�U���L�[�̕ϐ������ύX:kato
25:�v���C���[�̈ړ������ǉ�:kato yuma
26:�X�y�[�X�̏C��:takagi
27:�v���C���[�̈ړ��錾��public�`private�ɕύX:kato
=====*/

// ���O��Ԑ錾
using UnityEngine;

// �N���X��`
public class CPlayer : MonoBehaviour
{
	// �ϐ��錾
	private Rigidbody m_Rb; // ���W�b�g�{�f�B

	[Header("�v���C���[�X�e�[�^�X")]
	[SerializeField]
	[Tooltip("HP")]
	private int m_nHp = 100;
	[SerializeField]
	[Tooltip("�U����")]
	private int m_nAtk = 100;
	[SerializeField]
	[Tooltip("�ړ����x")]
	private float m_fSpeed = 2.0f;
	[SerializeField]
	[Tooltip("�U�����x")]
	private float m_fAtkSpeed = 100.0f;	// �U�����x
	[SerializeField]
	[Tooltip("�h���")]
	private int m_nDef = 5;

	[Header("�U���X�e�[�^�X")]
	[SerializeField]
	[Tooltip("�U���̔��a")]
	private float m_fAttackRange = 0.02f;	// �U���̔��a
	[SerializeField]
	[Tooltip("�U���̊p�x")]
	private float m_fAttackAngle = 45.0f;	// 45�x�͈̔�
	private float m_fLastAttackTime = 0.0f;	// �Ō�ɍU����������
	private float m_fAttackCooldown;	// �U���̃N�[���_�E������
	private bool m_bIsDead = false;	// �v���C���[������ł��邩�ǂ���

	// �U���L�[�̕ϐ�
	[SerializeField]
	[Tooltip("�U���L�[")]
	private KeyCode m_AttackKey = KeyCode.Return;

	// �A�j���[�^�[�֘A�̕ϐ�
	public Animator m_Animator;	// �A�j���[�^�[�ϐ��ێ��p
	private bool m_bWalkInput	= false;	// �ړ����̓t���O
	private bool m_bAttack		= false;	// �U���t���O
	public bool m_bOnGround	= true;	// �n�ʂɂ��邩�ǂ����̃t���O

	[Header("�v���C���[�ړ�����")]
	// �v���C���[�ړ������p�̕ϐ�

	[SerializeField]
	[Tooltip("�v���C���[�̈ړ������͈͂̌��_")]
	private Vector3 m_vMoveLimitOrigin = Vector3.zero; // �v���C���[�̈ړ������͈͂̌��_
    [SerializeField]
	[Tooltip("�v���C���[�̈ړ������͈�X")]
	private float m_fMoveLimit_x = 10.0f;	// �v���C���[�̈ړ������͈�
	[SerializeField]
	[Tooltip("�v���C���[�̈ړ������͈�Z")]
	private float m_fMoveLimit_z = 10.0f;	// �v���C���[�̈ړ������͈�
	

	// �������֐�
	// �����P�F�Ȃ�
	// ��
	// �ߒl�F�Ȃ�
	// ��
	// �T�v�F����������
	private void Start()
	{
		// �v���C���[�̏�����
		m_Rb = GetComponent<Rigidbody>();
		m_fAttackCooldown = 1.0f / m_fAtkSpeed;	// �U�����x�ɉ����čU���Ԋu��ݒ�
	}

	// �ړ������֐�
	// �����P�F�Ȃ�
	// ��
	// �ߒl�F�Ȃ�
	// ��
	// �T�v�F�v���C���[�̈ړ�����
	private void PlayerMove()
	{
		// �v���C���[�̈ړ�
		if (Input.GetKey(KeyCode.W))
		{
			m_Rb.transform.position += Vector3.forward * m_fSpeed;	// �O
		}
		if (Input.GetKey(KeyCode.S))
		{
			m_Rb.transform.position += Vector3.back * m_fSpeed;	// ���
		}
		if (Input.GetKey(KeyCode.A))
		{
			m_Rb.transform.position += Vector3.left * m_fSpeed;	// ��
		}
		if (Input.GetKey(KeyCode.D))
		{
			m_Rb.transform.position += Vector3.right * m_fSpeed;	// �E
		}

		/*  ���炩�ɓ������ǉ�������p�^�[���̈ړ�
		//if (Input.GetKey(KeyCode.S))
		//{
		//    rb.AddForce(Vector3.back * speed);
		//}
		//if (Input.GetKey(KeyCode.A))
		//{
		//    rb.AddForce(Vector3.left * speed);
		//}
		//if (Input.GetKey(KeyCode.D))
		//{
		//    rb.AddForce(Vector3.right * speed);
		//}

		*/
	}

	// �U���֐�
	// �����P�F�Ȃ�
	// ��
	// �ߒl�F�Ȃ�
	// ��
	// �T�v�F�v���C���[�̍U��
	private void Attack()
	{
		// �U�����邽�߂̃N�[���_�E�����ԃ`�F�b�N
		if (Time.time - m_fLastAttackTime >= m_fAttackCooldown)
		{
			// �U���̃N�[���_�E�����Ԃ��X�V
			m_fLastAttackTime = Time.time;

			// �U���̌���
			Vector3 _Forward = transform.forward;

			// �U���͈͂̌v�Z
			Collider[] _HitColliders = Physics.OverlapSphere(transform.position + _Forward * m_fAttackRange, m_fAttackRange);
			foreach (var _HitCollider in _HitColliders)
			{
				if (_HitCollider.gameObject == this.gameObject) continue;

				Vector3 _DirectionToTarget = _HitCollider.transform.position - transform.position;
				float fAngle = Vector3.Angle(_Forward, _DirectionToTarget);
				if (fAngle <= m_fAttackAngle / 2)
				{	
					// �G�ɍU�������������Ƃ��̏�����ǉ��������ꍇ�����ɏ���
				}
			}
		}
	}

	// �X�V�֐�
	// �����P�F�Ȃ�
	// ��
	// �ߒl�F�Ȃ�
	// ��
	// �T�v�F�v���C���[������ł��邩�Ǝ��񂾂Ƃ��̏���
	private void Update()
	{
		if(m_bIsDead) return;	// �v���C���[������ł���ꍇ�͑���𖳌��ɂ���

		// �v���C���[��HP��0�ȉ��ɂȂ����Ƃ�
		if(m_nHp <= 0 && !m_bIsDead)
		{
			Die(); // ����
		}

		Vector3 _NowPosition = transform.position;	// ���݂̈ʒu���擾

		_NowPosition.x = Mathf.Clamp(_NowPosition.x, m_vMoveLimitOrigin.x - m_fMoveLimit_x, m_vMoveLimitOrigin.x + m_fMoveLimit_x);
		_NowPosition.z = Mathf.Clamp(_NowPosition.z, m_vMoveLimitOrigin.z - m_fMoveLimit_z, m_vMoveLimitOrigin.z + m_fMoveLimit_z);

		transform.position = _NowPosition;	// �v���C���[�̈ʒu�𐧌��͈͓��Ɏ��߂�
	}

	// �����X�V�֐�
	// �����P�F�Ȃ�
	// ��
	// �ߒl�F�Ȃ�
	// ��
	// �T�v�F�v���C���[�̈ړ������ƍU������
	private void FixedUpdate()
	{
		// �ړ�����
		PlayerMove();

		// �v���C���[�̍U��(Enter)
		if (Input.GetKeyDown(m_AttackKey))
		{
			Attack();	// �U���������Ăяo��

		}
	}

	// ���ʊ֐�
	// �����P�F�Ȃ�
	// ��
	// �ߒl�F�Ȃ�
	// ��
	// �T�v�F�v���C���[�����񂾂Ƃ��ɌĂяo������
	private void Die()
	{
		m_bIsDead = true;
	}
}
