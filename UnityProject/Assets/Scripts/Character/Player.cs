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
_MO5
D
07:�U���̔��肪�������������̂ŏC��:kato
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
		Vector3 input = new Vector3(); // ���͒l���i�[����ϐ�
        if (Input.GetKey(KeyCode.W)) input += Vector3.forward;
        if (Input.GetKey(KeyCode.S)) input += Vector3.back;
        if (Input.GetKey(KeyCode.A)) input += Vector3.left;
        if (Input.GetKey(KeyCode.D)) input += Vector3.right;

        if (input != Vector3.zero)
        {
            input.Normalize();
            m_Rb.transform.position += input * m_fSpeed;

            // ��]������i�X���[�Y�ɂ������ꍇ��Lerp�ł�OK�j
            m_Rb.transform.rotation = Quaternion.LookRotation(input);
        }

        // �v���C���[�̈ړ� ���ʌ����邯��w+d���ςɂȂ�ړ�
        /*
		if (Input.GetKey(KeyCode.W))
		{
			m_Rb.transform.position += Vector3.forward * m_fSpeed;	// �O
			m_Rb.transform.rotation = Quaternion.Euler(0, 0, 0);    // �O������
        }
		if (Input.GetKey(KeyCode.S))
		{
			m_Rb.transform.position += Vector3.back * m_fSpeed;	// ���
			m_Rb.transform.rotation = Quaternion.Euler(0, 180, 0);  // ��������
        }
		if (Input.GetKey(KeyCode.A))
		{
			m_Rb.transform.position += Vector3.left * m_fSpeed;	// ��
			m_Rb.transform.rotation = Quaternion.Euler(0, 270, 0);  // ��������
        }
		if (Input.GetKey(KeyCode.D))
		{
			m_Rb.transform.position += Vector3.right * m_fSpeed;    // �E
            m_Rb.transform.rotation = Quaternion.Euler(0, 90, 0);   // �E������
        }
		*/

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
        if (Time.time - m_fLastAttackTime >= m_fAttackCooldown)
        {
            m_fLastAttackTime = Time.time;

            Vector3 forward = transform.forward;
            Vector3 origin = transform.position;

            // ���͂̃R���C�_�[�����͈͂Ŏ擾�i�~�`�j
            Collider[] hitColliders = Physics.OverlapSphere(origin, m_fAttackRange);
            foreach (var hit in hitColliders)
            {
                if (hit.gameObject == this.gameObject) continue;

                Vector3 toTarget = hit.transform.position - origin;
                toTarget.y = 0f; // ���������iXZ���ʂ݂̂Ōv�Z�j

                // �����`�F�b�N�i���̃`�F�b�N��Overlapsphere������Ă邯�ǈꉞ�j
                if (toTarget.magnitude > m_fAttackRange) continue;

                // ��^�̊p�x��������
                float angle = Vector3.Angle(forward, toTarget.normalized);
                if (angle <= m_fAttackAngle * 0.5f)
                {
					// �f�o�b�O�p
                    Debug.Log("Hit target: " + hit.name);

                    // TODO: �G�ɍU��������ǉ�
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

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, new Vector3(1, 1, 1));
		Gizmos.color = new Color(0, 1, 0, 1.0f);
		Gizmos.DrawWireSphere(transform.position + transform.forward * m_fAttackRange, m_fAttackRange);
		Gizmos.color = new Color(0, 0, 1, 1.0f);
		Gizmos.DrawLine(transform.position ,transform.position + transform.forward * 20.0f);
    }

	private void OnDrawGizmosSelected() // �I�u�W�F�N�g���󎞂ɕ\��
	{
#if UNITY_EDITOR
		Gizmos.color = new Color(1, 1, 0, 0.4f);
		
		Vector3 origin = transform.position;
		Vector3 forward = transform.forward;
		int segments = 30; // �\��������̐��i�����قǂȂ߂炩�j

		for (int i = 0; i <= segments; i++)
		{
			float angle = -m_fAttackAngle / 2 + m_fAttackAngle * i / segments;
			Quaternion rot = Quaternion.Euler(0, angle, 0);
			Vector3 dir = rot * forward;
			Gizmos.DrawLine(origin, origin + dir.normalized * m_fAttackRange);
		}
#endif
	}
}
