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
08:�U���̃N�[���_�E�����Ԃ��C��:kato
08:�_���[�W���������u��:takagi
16:Ray�ŗV��ł݂�:kato
20:���[�����O�̎��Ɉړ�����悤��:kato
=====*/

// ���O��Ԑ錾
using Unity.VisualScripting;
using UnityEngine;
using System.Collections; // ���G��ԗp

// �N���X��`
public class CPlayer : MonoBehaviour
{
	// �ϐ��錾
	private Rigidbody m_Rb; // ���W�b�g�{�f�B

	private float m_RayDistance = 10.0f;

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
	private float m_fLastAttackTime = -Mathf.Infinity;	// �Ō�ɍU����������
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
	private float m_fMoveLimit_z = 10.0f;   // �v���C���[�̈ړ������͈�

	[SerializeField]
	[Tooltip("Ray�ɂ���Q���������")]
	private float m_fAvoidDistance = 1.0f;  // ��Q���Ƃ̍Œ዗��

    [SerializeField]
	[Tooltip("PlayerRay�̍���")]
	private float m_fRayHeight = 1.5f; // Ray�̍���

	[Header("�v���C���[�̃��[�����O�֌W")]

    [SerializeField]
    [Tooltip("Player�����[�����O����Ƃ��̏��Z�����Œ�l")]
    private float m_fRollSpeed = 0.05f; // �ړ����x*0.05�p
    [SerializeField]
    [Tooltip("Player�̃��[�����O�̃N�[���^�C��")]
    private float m_fRollCooldown = 3.0f; // ���[�����O�̃N�[���^�C��(�b)

	private bool m_bIsRolling = false; // ���[�����O�t���O
    private float m_fRollingCoolTimer = 0.0f; // ���[�����O���Ō�ɍs���Ă���̌o�ߎ���
	private float m_fRollTimer = 0.0f; // ���[�����O���̌o�ߎ���
	private Vector3 m_vRollDirection; // ���[�����O�̕���
	private float m_fRollDuration = 0.3f; // ���[�����O�̎�������

	// ���[�����O���̖��G����
	[SerializeField]
	[Tooltip("Player�̃��[�����O���̖��G����")]
	private float m_fRollingInvicibleTime = 0.5f; // ��U��
	private bool m_bIsRollingInvicible = false; // ���[�����O���̖��G�t���O

    private bool m_bIsInvicible = false; // ���G�t���O
	private int m_nInvicibleTime = 90; // ���G����

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
		Ray ray = new Ray(transform.position, transform.forward); // �v���C���[�̑O����Ray���΂�
        RaycastHit hit; // Ray�̓����蔻����i�[����ϐ�

        Vector3 input = new Vector3(); // ���͒l���i�[����ϐ�

		if (Input.GetKey(KeyCode.W))
		{
            input += Vector3.forward;

        }
        if (Input.GetKey(KeyCode.S))
		{
            input += Vector3.back;

        }
        if (Input.GetKey(KeyCode.A))
		{
            input += Vector3.left;

        }
        if (Input.GetKey(KeyCode.D))
		{
            input += Vector3.right;

        }

        if (input != Vector3.zero)
        {
            input.Normalize();
            Vector3 moveDir = input;
			Vector3 RayPosition = transform.position + Vector3.up * m_fRayHeight; // Ray�̈ʒu���v���C���[�̈ʒu�ɐݒ�
            float moveDistance = m_fSpeed;


            // Ray��O���ɔ�΂��ď�Q���Ƃ̋������`�F�b�N
            if (Physics.Raycast(RayPosition, moveDir, out hit, m_RayDistance))
            {
                

                if (hit.distance <= m_fAvoidDistance)
                {
                    // �߂����Ĉړ����Ȃ�
                    moveDistance = 0;
                }
                else if (hit.distance < moveDistance + m_fAvoidDistance)
                {
                    // �����𒲐����Ď~�܂�
                    moveDistance = hit.distance - m_fAvoidDistance;
                }
            }

            m_Rb.transform.position += moveDir * moveDistance;

            

            // ��]
            m_Rb.transform.rotation = Quaternion.LookRotation(moveDir);

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

	// ���[�����O�֐�
	// �����P�F�Ȃ�
	// ��
	// �ߒl�F�Ȃ�
	// ��
	// �T�v�F�v���C���[�̃��[�����O�̏����������֐�
	private void StartRolling()
	{
		m_bIsRolling = true;
		m_fRollTimer = 0.0f;
        m_fRollingCoolTimer = 0.0f; // ���[�����O�̃N�[���^�C�������Z�b�g
        m_vRollDirection = transform.forward; // ���[�����O�̕�����ݒ�

		// �A�j���[�V�����̍Đ�������΂����ōĐ�����
    }

    // ���[�����O�֐�
    // �����P�F�Ȃ�
    // ��
    // �ߒl�F�Ȃ�
    // ��
    // �T�v�F�v���C���[�̃��[�����O����
	private void RollMovement()
	{
		m_fRollTimer += Time.fixedDeltaTime;

		float fRollSpeed = m_fSpeed * m_fRollSpeed; // ���[�����O�̈ړ����x

		m_Rb.MovePosition(m_Rb.position + m_vRollDirection * fRollSpeed);

		if(m_fRollTimer >= m_fRollDuration)
		{
			m_bIsRolling = false; // ���[�����O�I��
        }
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
					var _EnemyScript = hit.gameObject.GetComponent<CEnemy>();
					if(_EnemyScript != null)
					{
						_EnemyScript.Damage(m_nAtk);	// �ꎞ�I�ȃ_���[�W����
						Debug.Log("AttackHit!");
					}
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
			Destroy(this.gameObject); // �v���C���[������
        }

		if(!m_bIsRolling)
		{
			m_fRollingCoolTimer += Time.deltaTime; // ���[�����O�̃N�[���^�C�������Z

			if(Input.GetKeyDown(KeyCode.Space) && m_fRollingCoolTimer >= m_fRollCooldown)
			{
				StartRolling(); // ���[�����O�J�n
				StartCoroutine(RollingInvincibilityCoroutine()); // ���[�����O���̖��G���ԊJ�n
            }
        }

        Vector3 origin = transform.position + Vector3.up * m_fRayHeight;
        Debug.DrawRay(origin, transform.forward * 1.0f, Color.green);

    }

	// �����X�V�֐�
	// �����P�F�Ȃ�
	// ��
	// �ߒl�F�Ȃ�
	// ��
	// �T�v�F�v���C���[�̈ړ������ƍU������
	private void FixedUpdate()
	{
		if(m_bIsRolling)
		{
			RollMovement();
		}
		else
		{
            // �ړ�����
            PlayerMove();
        }
        // �v���C���[�̈ړ�����
        Vector3 _NowPosition = transform.position;  // ���݂̈ʒu���擾

        _NowPosition.x = Mathf.Clamp(_NowPosition.x, m_vMoveLimitOrigin.x - m_fMoveLimit_x, m_vMoveLimitOrigin.x + m_fMoveLimit_x);
        _NowPosition.z = Mathf.Clamp(_NowPosition.z, m_vMoveLimitOrigin.z - m_fMoveLimit_z, m_vMoveLimitOrigin.z + m_fMoveLimit_z);

        transform.position = _NowPosition;  // �v���C���[�̈ʒu�𐧌��͈͓��Ɏ��߂�

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
		Gizmos.DrawCube(transform.position + new Vector3(0,1,0), new Vector3(1, 2, 1));
		
	}

	// ���_���[�W�֐�
	// �����F�Ȃ�
	// ��
	// �ߒl�F�Ȃ�
	// ��
	// �T�v�F�_���[�W���󂯂�
	public void Damage(int _nDamage)
	{
		if (m_bIsInvicible) return; // ���G��ԂȂ�_���[�W���󂯂Ȃ�

        if (_nDamage <= m_nDef)// �h�䂪��_�������������_����1�ɂ���
		{
			_nDamage = 1;
		}
		else// �_���[�W��^����
		{
			_nDamage = _nDamage - m_nDef;
		}

		m_nHp -= _nDamage; // �_���[�W����

		
		// ���G��ԊJ�n
		StartCoroutine(InvincibilityCoroutine());
        
    }

    // �����G��Ԋ֐�
    // �����F�Ȃ�
    // ��
    // �ߒl�F�Ȃ�
    // ��
    // �T�v�F�_���[�W���󂯂��Ƃ���90�t���[�����G��ԂɂȂ�
    private IEnumerator InvincibilityCoroutine()
    {
        m_bIsInvicible = true; // ���G��Ԃɂ���
        Debug.Log("���G���!!!");
        for (int i = 0; i < m_nInvicibleTime; ++i)
		{
			yield return null; // 1�t���[���҂�
        }

        m_bIsInvicible = false; // ���G��Ԃ���������
		Debug.Log("���G��ԉ���!!");
    }

    // �����G��Ԋ֐�(���[�����O��)
    // �����F�Ȃ�
    // ��
    // �ߒl�F�Ȃ�
    // ��
    // �T�v�F���[�����O���Ă���Ԉ�莞�Ԗ��G�ɂȂ�
	private IEnumerator RollingInvincibilityCoroutine()
	{
        m_bIsRollingInvicible = true; // ���G��Ԃɂ���
		yield return new WaitForSeconds(m_fRollingInvicibleTime); // ��莞�ԑ҂�
        m_bIsRollingInvicible = false; // ���G��Ԃ���������
    }

    private void OnDrawGizmosSelected() // �I�u�W�F�N�g�I�����ɕ\��
	{
#if UNITY_EDITOR
		Gizmos.color = new Color(1, 1, 0, 0.4f);

		Vector3 offSet = new Vector3(0, 1, 0);
		Vector3 origin = transform.position + offSet;
		Vector3 forward = transform.forward;
		int segments = 30; // �\��������̐��i�����قǂȂ߂炩�j

		for (int i = 0; i <= segments; i++)
		{
			float angle = -m_fAttackAngle / 2 + m_fAttackAngle * i / segments;
			Quaternion rot = Quaternion.Euler(0, angle, 0);
			Vector3 dir = rot * forward;
			Gizmos.DrawLine(origin, origin + dir.normalized * m_fAttackRange);
		}

		Gizmos.color = new Color(0, 0, 1, 1.0f);
		Vector3 start = transform.position + offSet;
        Vector3 end = transform.position + transform.forward * 20.0f + offSet;
		Gizmos.DrawLine(start, end);
#endif
	}
}
