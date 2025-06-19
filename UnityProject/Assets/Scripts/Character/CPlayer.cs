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
12:HP�֌W�̋@�\���O���ɐ؂�o��:takagi
16:Ray�ŗV��ł݂�:kato
20:���[�����O�̎��Ɉړ�����悤��:kato
22:���ʉ��̒ǉ�(WASD�ňړ�����Enter�ōU�����̂�):kato
28:SE�֌W�̕ϐ��Ƀc�[���`�b�v�ǉ�:takagi
30:���G��Ԃ̍X�V������ǉ�:kato
_M06
D
06:���������U�������I�I�I:kato
11:�m�b�N�o�b�N���ǉ�:sezaki
18:�A�j���[�V��������������:kato
19:Slam�A�j���[�V��������:kato
19:���[�����O�A�j���[�V������������:kato
=====*/

// ���O��Ԑ錾
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

// �N���X��`
public class CPlayer : MonoBehaviour, IDH
{
    // �ϐ��錾
    private Rigidbody m_Rb; // ���W�b�g�{�f�B

	private float m_RayDistance = 10.0f;

    private float m_ftime = 0.0f;//�����܁[
    private float m_fcount = 0.0f;//�������

	[Header("�v���C���[�X�e�[�^�X")]
	[SerializeField]
	[Tooltip("����HP")]
	private int m_nInitialHp = 100;
	[SerializeField]
	[Tooltip("�����h���")]
	private int m_nInitialDef = 5;
	private CHitPoint m_HitPoint;	// HP�@�\

	[SerializeField]
	[Tooltip("�U����")]
	private int m_nAtk = 100;
	[SerializeField]
	[Tooltip("�ړ����x")]
	private float m_fSpeed = 2.0f;
	[SerializeField]
	[Tooltip("�U�����x")]
	private float m_fAtkSpeed = 100.0f;

	[Header("�U���X�e�[�^�X")]

	[SerializeField]
	[Tooltip("�U���͈͂̉���")]
	private float m_fAttackBoxWidth = 2f;     // �U���͈͂̉���
	[SerializeField]
	[Tooltip("�U���͈͂̉��s��")]
	private float m_fAttackBoxDepth = 3f;     // �U���͈͂̉��s��
	[SerializeField]
	[Tooltip("�U���͈͂̍���")]
	private float m_fAttackBoxHeight = 1.5f;    // �U���͈͂̍���
	[SerializeField]
	[Tooltip("�U���͈͂̏c�I�t�Z�b�g")]
	private float m_fAttackBoxYOffset = 1.0f;
	[SerializeField]
	[Tooltip("�U���͈͂̉��I�t�Z�b�g")]
	private float m_fAttackBoxXOffset = 1.0f; // ���iX���j�I�t�Z�b�g

    [SerializeField]
    [Tooltip("�X�}�b�V���U���͈͂̉���")]
    private float m_fSmashAttackBoxWidth = 2f;     // �U���͈͂̉���
    [SerializeField]
    [Tooltip("�X�}�b�V���U���͈͂̉��s��")]
    private float m_fSmashAttackBoxDepth = 3f;     // �U���͈͂̉��s��
    [SerializeField]
    [Tooltip("�X�}�b�V���U���͈͂̍���")]
    private float m_fSmashAttackBoxHeight = 1.5f;    // �U���͈͂̍���
    [SerializeField]
    [Tooltip("�X�}�b�V���U���͈͂̏c�I�t�Z�b�g")]
    private float m_fSmashAttackBoxYOffset = 1.0f;
    [SerializeField]
    [Tooltip("�X�}�b�V���U���͈͂̉��I�t�Z�b�g")]
    private float m_fSmashAttackBoxXOffset = 1.0f; // ���iX���j�I�t�Z�b�g

    private float m_fLastAttackTime = -Mathf.Infinity;	// �Ō�ɍU����������
	private float m_fAttackCooldown;	// �U���̃N�[���_�E������
	//private bool m_bIsDead = false;	// �v���C���[������ł��邩�ǂ���
	private bool m_bIsPoison = false; //�v���C���[���ŃJ
	private bool m_bAttackInput = false;
    private bool m_bPoisonUpdate = false;//�ōX�V�p

	// �U���L�[�̕ϐ�
	[SerializeField]
	[Tooltip("�U���L�[")]
	private KeyCode m_AttackKey = KeyCode.Return;

	[SerializeField]
	[Tooltip("���[�����O�L�[")]
	private KeyCode m_RollingKey = KeyCode.Space; // ���[�����O�L�[

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
	private float m_fRollingInvincibleTime = 0.5f; // ��U��
	private bool m_bIsRollingInvincible = false; // ���[�����O���̖��G�t���O

	private bool m_bIsInvicible = false; // ���G�t���O
	private int m_nInvincibleTime = 90; // ���G����

	[Header("�v���C���[��SE�֌W")]
	[SerializeField]
	[Tooltip("�v���C���[�̑����̊Ԋu")]
	private float m_fFootStepInterval = 0.4f;	// �����̊Ԋu
	private float m_fFootStepTimer = 0.0f;
	[SerializeField]
	[Tooltip("�ړ���")]
	private  AudioClip m_MoveGroundSE;
	[SerializeField]
	[Tooltip("�ړ�����")]
	private float m_MoveGroundSEVolume = 0.05f;
	private AudioSource m_MoveGroundSESource;	// �ړ�SE�p�̃I�[�f�B�I�\�[�X
	[SerializeField]
	[Tooltip("�U��(�˂�)��")]
	public AudioClip m_StabAttackSE;
	[SerializeField]
	[Tooltip("�U��(�˂�)����")]
	private float m_StabAttackSEVolume = 0.05f;
	private AudioSource m_StabAttackSESource;	// �˂�SE�p�̃I�[�f�B�I�\�[�X


    CEnemy enemy;

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
		m_fAttackCooldown = 100.0f / m_fAtkSpeed;	// �U�����x�ɉ����čU���Ԋu��ݒ�(�U�����x100�Ȃ�1�b�@200�Ȃ�0.5�b)
		m_Animator = GetComponent<Animator>();  // �A�j���[�^�[�R���|�[�l���g�擾


        // ��������
        m_MoveGroundSESource = gameObject.AddComponent<AudioSource>();	// �ړ��p�̉����R���|�[�l���g�쐬
		m_MoveGroundSESource.volume = m_MoveGroundSEVolume;	// ���ʂ�ݒ�
		m_StabAttackSESource = gameObject.AddComponent<AudioSource>();	// �˂��p�̉����R���|�[�l���g�쐬
		m_StabAttackSESource.volume = m_StabAttackSEVolume;	// ���ʂ�ݒ�
		
		// HP�̎���
		if(m_HitPoint = GetComponent<CHitPoint>())
		{
#if UNITY_EDITOR
			// �o��
			Debug.Log(this + "�ɂ�HitPoint���ݒ肳��Ă��܂����A���̐ݒ�͏����������\��������܂�");
#endif	// !UNITY_EDITOR
		}
		else
		{
			m_HitPoint = gameObject.AddComponent<CHitPoint>();	// HP�̋@�\�ǉ�
		}

		// �����l�ݒ�
		m_HitPoint.MaxHP = m_nInitialHp;	// ����HP�ݒ�
		m_HitPoint.Defence = m_nInitialDef;	// �����h��ݒ�

		// �C�x���g�ڑ�
		m_HitPoint.OnDead += OnDead;	// ���S��������ڑ�
	}

	// ���͂��܂Ƃ߂Ď擾����֐�
	// �����P�F�Ȃ�
	// ��
	// �ߒl�F�Ȃ�
	// ��
	// �T�v�F�v���C���[�̓��͏���
	private Vector3 GetMoveInput()
	{
		Vector3 input = Vector3.zero; // ���͒l���i�[����ϐ�

		if (Input.GetKey(KeyCode.W)) input += Vector3.forward;
		if (Input.GetKey(KeyCode.S)) input += Vector3.back;
		if (Input.GetKey(KeyCode.A)) input += Vector3.left;
		if (Input.GetKey(KeyCode.D)) input += Vector3.right;

		return input.normalized;
	}

	// ��Q���Ƃ̓����蔻�菈���֐�
	// �����P�F�Ȃ�
	// ��
	// �ߒl�F�Ȃ�
	// ��
	// �T�v�F�v���C���[�̏�Q���Ƃ̓����蔻�菈��
	private bool AdjustDistanceByRaycast(Vector3 origin, Vector3 direction, float originalDistance, out float adjustedDistance)
	{
		adjustedDistance = originalDistance;
		RaycastHit hit;

		// Ray�̍�����ݒ肷��
		Vector3 OffSetOrigin = origin + Vector3.up * m_fRayHeight;

		if (Physics.Raycast(OffSetOrigin, direction, out hit, originalDistance + m_RayDistance))
		{
			if (hit.distance <= m_fAvoidDistance)
			{
				adjustedDistance = 0f;
				return true;
			}
			else if (hit.distance < originalDistance + m_fAvoidDistance)
			{
				adjustedDistance = hit.distance - m_fAvoidDistance;
				return true;
			}
		}
		return false;
	}

	// �ړ������֐�
	// �����P�F�Ȃ�
	// ��
	// �ߒl�F�Ȃ�
	// ��
	// �T�v�F�v���C���[�̈ړ�����
	private void PlayerMove()
	{
		Vector3 input = GetMoveInput();

		if (input != Vector3.zero)
		{
			Vector3 moveDir = input;
			Vector3 rayOrigin = transform.position + Vector3.up * m_fRayHeight;
			float moveDistance = m_fSpeed;

			// �����␳
			AdjustDistanceByRaycast(rayOrigin, moveDir, moveDistance, out moveDistance);

			// �����Đ�
			if (!m_MoveGroundSESource.isPlaying)
			{
				m_MoveGroundSESource.clip = m_MoveGroundSE;
				m_MoveGroundSESource.loop = true;
				m_MoveGroundSESource.Play();
			}

			// �ړ��E��]
			m_Rb.transform.position += moveDir * moveDistance;
			m_Rb.transform.rotation = Quaternion.LookRotation(moveDir);

            m_Animator.SetBool("Run",true); // ���s�A�j���[�V�������Đ�
            //m_Animator.SetBool("Run", false); // ���s�A�j���[�V�������~
        }
        else
		{

            // ���͂Ȃ����̑�����~
            if (m_MoveGroundSESource.isPlaying)
			{
				m_MoveGroundSESource.Stop();
			}
			m_Animator.SetBool("Run", false); // ���s�A�j���[�V�������~
			//m_Animator.SetBool("Run", true); // ���s�A�j���[�V�������~
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
		m_Animator.SetBool("Rolling", true); 
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
		Vector3 RollDir = m_vRollDirection.normalized; //���[�����O���� 
		Vector3 RayOrigin = transform.position + Vector3.up * m_fRayHeight; // Ray�̌��_

		//��Q���Ƃ̋������`�F�b�N���āA�ړ�������␳����
		AdjustDistanceByRaycast(RayOrigin, RollDir, fRollSpeed, out fRollSpeed);

		transform.position += RollDir * fRollSpeed; // ���[�����O�ړ�

		if (m_fRollTimer >= m_fRollDuration)
		{
			m_bIsRolling = false; // ���[�����O�I��
		}
	}

	// �U���֐�
	// �����P�F�Ȃ�
	// ��
	// �ߒl�F�Ȃ�
	// ��
	// �T�v�F�v���C���[�̍U��(��)
	private void Attack()
	{
		if (Time.time - m_fLastAttackTime >= m_fAttackCooldown)
		{
			m_fLastAttackTime = Time.time;

			Vector3 origin = transform.position;
			Vector3 forward = transform.forward;

			Vector3 boxHalfExtents = new Vector3(
				m_fAttackBoxWidth * 0.5f,
				m_fAttackBoxHeight * 0.5f,
				m_fAttackBoxDepth * 0.5f
			);

			Vector3 boxCenter = origin + forward  * (m_fAttackBoxDepth * 0.5f)
								+ transform.up * (boxHalfExtents.y + m_fAttackBoxYOffset)
								+ transform.right * m_fAttackBoxXOffset;
			//boxCenter.y += boxHalfExtents.y + m_fAttackBoxYOffset;

			// Debug�\��
			//DebugDrawBox(boxCenter, boxHalfExtents, transform.rotation, Color.red, 500f);

			Collider[] hitColliders = Physics.OverlapBox(boxCenter, boxHalfExtents, transform.rotation);
			foreach (var hit in hitColliders)
			{
				if (hit.gameObject == this.gameObject) continue;

				var enemy = hit.GetComponent<CEnemy>();
				if (enemy != null)
				{
					enemy.Damage(m_nAtk,this.transform);
				}
			}

			m_Animator.SetBool("Attack", true); // �U���A�j���[�V�������Đ�
        }

        // �U�����Đ�
        if (!m_StabAttackSESource.isPlaying)
		{
			m_StabAttackSESource.PlayOneShot(m_StabAttackSE);
		}
	}

    // �U���֐�
    // �����P�F�Ȃ�
    // ��
    // �ߒl�F�Ȃ�
    // ��
    // �T�v�F�v���C���[�̍U��(�@����)
	private void SmashAttack()
    {
        Debug.Log(m_fAttackCooldown);

        if (Time.time - m_fLastAttackTime >= m_fAttackCooldown)
        {
            m_fLastAttackTime = Time.time;

            Debug.Log("�X�}�b�V���U��!!");

            Vector3 origin = transform.position;
            Vector3 forward = transform.forward;

            Vector3 boxHalfExtents = new Vector3(
                m_fSmashAttackBoxWidth * 0.5f,
                m_fSmashAttackBoxHeight * 0.5f,
                m_fSmashAttackBoxDepth * 0.5f
            );

            Vector3 boxCenter = origin + forward * (m_fSmashAttackBoxDepth * 0.5f)
                                + transform.up * (boxHalfExtents.y + m_fSmashAttackBoxYOffset)
                                + transform.right * m_fSmashAttackBoxXOffset;
            //boxCenter.y += boxHalfExtents.y + m_fAttackBoxYOffset;

            // Debug�\��
            //DebugDrawBox(boxCenter, boxHalfExtents, transform.rotation, Color.red, 500f);

            Collider[] hitColliders = Physics.OverlapBox(boxCenter, boxHalfExtents, transform.rotation);
            foreach (var hit in hitColliders)
            {
                if (hit.gameObject == this.gameObject) continue;

                var enemy = hit.GetComponent<CEnemy>();
                if (enemy != null)
                {
                    enemy.Damage(m_nAtk, this.transform);
                }
            }
        }

		m_Animator.SetBool("Slam", true); // �X�}�b�V���A�^�b�N�A�j���[�V�������Đ�
										  // (�A�j���[�V�����C�x���g��false�ǂ�ł邩��false�̋L�q�͂Ȃ�
        // �U�����Đ�
        if (!m_StabAttackSESource.isPlaying)
        {
            m_StabAttackSESource.PlayOneShot(m_StabAttackSE);
        }
    }

    // ����ŏ������
    private void DrawSmashAttackDebugBox()
    {
        Vector3 origin = transform.position;
        Vector3 forward = transform.forward;

        Vector3 boxHalfExtents = new Vector3(
            m_fSmashAttackBoxWidth * 0.5f,
            m_fSmashAttackBoxHeight * 0.5f,
            m_fSmashAttackBoxDepth * 0.5f
        );

        Vector3 boxCenter = origin
            + forward * (m_fSmashAttackBoxDepth * 0.5f)
            + transform.up * (boxHalfExtents.y + m_fSmashAttackBoxYOffset)
            + transform.right * m_fSmashAttackBoxXOffset;

        DebugDrawBox(boxCenter, boxHalfExtents, transform.rotation,Color.magenta, 0f); // �� duration 0�ł�OK
    }

    // ����ŏ������
    private void DrawAttackDebugBox()
	{
		Vector3 origin = transform.position;
		Vector3 forward = transform.forward;

		Vector3 boxHalfExtents = new Vector3(
			m_fAttackBoxWidth * 0.5f,
			m_fAttackBoxHeight * 0.5f,
			m_fAttackBoxDepth * 0.5f
		);

		Vector3 boxCenter = origin
			+ forward * (m_fAttackBoxDepth * 0.5f)
			+ transform.up * (boxHalfExtents.y + m_fAttackBoxYOffset)
			+ transform.right * m_fAttackBoxXOffset;

		DebugDrawBox(boxCenter, boxHalfExtents, transform.rotation, Color.blue, 0f); // �� duration 0�ł�OK
	}

	// �{�b�N�X�̉����֐�
	private void DebugDrawBox(Vector3 center, Vector3 halfExtents, Quaternion rotation, Color color, float duration)
	{
		Vector3[] points = new Vector3[8];
		Matrix4x4 matrix = Matrix4x4.TRS(center, rotation, Vector3.one);

		// �{�b�N�X��8�_�����[�J�����烏�[���h�ɕϊ�
		Vector3 he = halfExtents;
		points[0] = matrix.MultiplyPoint3x4(new Vector3(-he.x, -he.y, -he.z));
		points[1] = matrix.MultiplyPoint3x4(new Vector3(he.x, -he.y, -he.z));
		points[2] = matrix.MultiplyPoint3x4(new Vector3(he.x, -he.y, he.z));
		points[3] = matrix.MultiplyPoint3x4(new Vector3(-he.x, -he.y, he.z));
		points[4] = matrix.MultiplyPoint3x4(new Vector3(-he.x, he.y, -he.z));
		points[5] = matrix.MultiplyPoint3x4(new Vector3(he.x, he.y, -he.z));
		points[6] = matrix.MultiplyPoint3x4(new Vector3(he.x, he.y, he.z));
		points[7] = matrix.MultiplyPoint3x4(new Vector3(-he.x, he.y, he.z));

		// ����
		Debug.DrawLine(points[0], points[1], color, duration);
		Debug.DrawLine(points[1], points[2], color, duration);
		Debug.DrawLine(points[2], points[3], color, duration);
		Debug.DrawLine(points[3], points[0], color, duration);

		// �㕔
		Debug.DrawLine(points[4], points[5], color, duration);
		Debug.DrawLine(points[5], points[6], color, duration);
		Debug.DrawLine(points[6], points[7], color, duration);
		Debug.DrawLine(points[7], points[4], color, duration);

		// ����
		Debug.DrawLine(points[0], points[4], color, duration);
		Debug.DrawLine(points[1], points[5], color, duration);
		Debug.DrawLine(points[2], points[6], color, duration);
		Debug.DrawLine(points[3], points[7], color, duration);
	}


	// �X�V�֐�
	// �����P�F�Ȃ�
	// ��
	// �ߒl�F�Ȃ�
	// ��
	// �T�v�F�v���C���[������ł��邩�Ǝ��񂾂Ƃ��̏���
	private void Update()
	{
        if (m_HitPoint.IsDead) return;   // �v���C���[������ł���ꍇ�͑���𖳌��ɂ���

		if(Input.GetKeyDown(m_AttackKey))
		{
			m_bAttackInput = true; // �U�����̓t���O�𗧂Ă�
        }

		//DrawAttackDebugBox();

		//// �v���C���[��HP��0�ȉ��ɂȂ����Ƃ�
		//if(m_nHp <= 0 && !m_bIsDead)
		//{
		//	Die(); // ����
		//}
		if (m_bIsPoison == true)//�ł�������
		{
			if (m_bPoisonUpdate == true)
			{
				m_fcount = 0.0f;
			}
			m_ftime += Time.deltaTime;
			m_fcount += Time.deltaTime;
			if (m_ftime >= 1.0f)//�P�т傤����
			{
				//m_nHp -= 5;
				//Debug.Log("��!5�_���[�W���݂�HP" + m_nHp);
				m_HitPoint.HP -= 5;
				Debug.Log("��!5�_���[�W���݂�HP" + m_HitPoint.HP);
				m_ftime = 0.0f;
			}
			if (m_fcount >= 5.0f)
			{
				m_bIsPoison = false;
			}
			m_bPoisonUpdate = false;
		}

		if(!m_bIsRolling)
		{
			m_fRollingCoolTimer += Time.deltaTime; // ���[�����O�̃N�[���^�C�������Z

			if(Input.GetKeyDown(m_RollingKey) && m_fRollingCoolTimer >= m_fRollCooldown)
			{
				StartRolling(); // ���[�����O�J�n
				StartCoroutine(RollingInvincibilityCoroutine()); // ���[�����O���̖��G���ԊJ�n
			}
		}

		Vector3 origin = transform.position + Vector3.up * m_fRayHeight;

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

       if(m_bAttackInput)
		{
			if(GetMoveInput().magnitude < 0.01f)
			{
				SmashAttack(); // �X�}�b�V���U��
				
                m_bAttackInput = false; // �U�����̓t���O�����Z�b�g
            }
            else
            {
                Attack(); // Player�������Ă���Ƃ��͒ʏ�U��
                m_bAttackInput = false; // �U�����̓t���O�����Z�b�g
            }
           
        }
	}

	//// ���ʊ֐�
	//// �����P�F�Ȃ�
	//// ��
	//// �ߒl�F�Ȃ�
	//// ��
	//// �T�v�F�v���C���[�����񂾂Ƃ��ɌĂяo������
	//private void Die()
	//{
	//	m_bIsDead = true;
	//}

	void OnDrawGizmos()
	{
		//Gizmos.color = new Color(1, 0, 0, 0.5f);
		//Gizmos.DrawCube(transform.position + new Vector3(0,1,0), new Vector3(1, 2, 1));
		//DrawAttackDebugBox();
		DrawSmashAttackDebugBox();
        // ray�\��
        Gizmos.color = Color.green;
		Gizmos.DrawLine(transform.position + Vector3.up * m_fRayHeight, transform.position + Vector3.up * m_fRayHeight + transform.forward * 1.0f);
	}

	// ���_���[�W�֐�	//TODO:�G�́u�U���v�����Affect�Ƃ���Damage���A�^�b�`
	// �����F�Ȃ�
	// ��
	// �ߒl�F�Ȃ�
	// ��
	// �T�v�F�_���[�W���󂯂�
	public void Damage(int _nDamage, Transform attacker, int weight)
	{
		
		if (m_bIsInvicible) return; // ���G��ԂȂ�_���[�W���󂯂Ȃ�

		if (_nDamage <= m_HitPoint.Defence)// �h�䂪��_�������������_����1�ɂ���
		{
			_nDamage = 1;
		}
		else// �_���[�W��^����
		{
			_nDamage = _nDamage - m_HitPoint.Defence;
		}

		//m_nHp -= _nDamage; // �_���[�W����
		m_HitPoint.HP -= _nDamage; // �_���[�W����

        StartCoroutine(KnockbackCoroutine(attacker, weight));

        // ���G��ԊJ�n
        StartCoroutine(InvincibilityCoroutine());
		
	}

    /// <summary>
    /// -�m�b�N�o�b�N�֐�	
    /// <para>�m�b�N�o�b�N����֐�</para>
    /// <param name="Transform attacker">����̌����Ă����</param>
    /// </summary>

    private IEnumerator KnockbackCoroutine(Transform attacker, int weight)
    {
        Debug.Log("�m�b�N�o�b�N�J�n�I");

       Vector3 knockbackDir = (transform.position - attacker.position).normalized;
        knockbackDir.y = 0f; // Y�����̓������[���ɂ���
        knockbackDir = knockbackDir.normalized; // ���K��
        float knockbackPower = weight * 0.2f;      // �m�b�N�o�b�N�̗́i���� or �X�s�[�h�j
        float knockbackTime = 0.2f;        // �m�b�N�o�b�N����
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + knockbackDir * knockbackPower;
        float _fTimer = 0f;
        
        while (_fTimer < knockbackTime) //�m�b�N�o�b�N�̎w�莞�Ԃ̊�
        { //����������փm�b�N�o�b�N
            float t = _fTimer / knockbackTime;
            t = 1f - (1f - t) * (1f - t);
            Vector3 newPos = Vector3.Lerp(startPos, endPos, t);

            m_Rb.MovePosition(newPos);
            _fTimer += Time.deltaTime;
            yield return null;
        }


        Debug.Log("�m�b�N�o�b�N�I���I");
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
		for (int i = 0; i < m_nInvincibleTime; ++i)
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
		m_bIsRollingInvincible = true; // ���G��Ԃɂ���
		yield return new WaitForSeconds(m_fRollingInvincibleTime); // ��莞�ԑ҂�
		m_bIsRollingInvincible = false; // ���G��Ԃ���������
	}

	private void OnDrawGizmosSelected() // �I�u�W�F�N�g�I�����ɕ\��
	{
#if UNITY_EDITOR
	// �Z���N�g��������Debug�����������ɒǉ�
#endif
	}

		// ���S������
	private void OnDead()
	{
		SceneManager.LoadScene("GAMEOVER");
		Destroy(gameObject);	// �v���C���[������
	}


	//---������---
	//�_���[�W����
	public void Adddamege(int damage)
	{
		//m_nHp -= damage;
		//Debug.Log("�v���C���[��" + damage + "����������@���݂�HP:" + m_nHp);
		//if (m_nHp < 0)//���񂾂�
		//{
		//	Debug.Log("����");
		//}
		m_HitPoint.HP -= damage;
		Debug.Log("�v���C���[��" + damage + "����������@���݂�HP:" + m_HitPoint.HP);
		if (m_HitPoint.HP < 0)//���񂾂�
		{
			Debug.Log("����");
		}
	}


	public void Addheal(int heal)
	{
		//m_nHp += heal;
		//Debug.Log("�v���C���[��" + heal + "���񕜂����@���݂�HP:" + m_nHp);
		m_HitPoint.HP += heal;
		Debug.Log("�v���C���[��" + heal + "���񕜂����@���݂�HP:" + m_HitPoint.HP);
	}
	public void Addposion()
	{
		m_bIsPoison = true;
		m_bPoisonUpdate = true;
	}
	public void Addacid(int damage)
	{
		//if (m_nHp > damage)
		//{


		//	m_nHp -= damage;
		//	Debug.Log("�v���C���[��" + damage + "����������@���݂�HP:" + m_nHp);
		//}
		if (m_HitPoint.HP > damage)
		{


			m_HitPoint.HP -= damage;
			Debug.Log("�v���C���[��" + damage + "����������@���݂�HP:" + m_HitPoint.HP);
		}
		else
		{
			Debug.Log("�_�����炵�Ȃ�");
		}
	}

    public void OnAttackAnimationEnd()
	{
		m_Animator.SetBool("Attack", false);
	}

	public void OnSlamAttackAnimationEnd()
	{
        m_Animator.SetBool("Slam", false);
    }

	public void OnRollingAnimationEnd()
	{
		m_Animator.SetBool("Rolling", false);
	}

}