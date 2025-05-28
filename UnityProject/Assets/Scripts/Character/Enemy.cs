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
25:navMesh�ǉ�
_M05
D
1:�U���ǉ�
9:�Վ��I�ɃX�|�i�[�Ή��̒ǐՋ@�\�g��:takagi
9:Enemy�𐶐����ɋ߂��̃i�r���b�V���Ƀ��[�v����
	�v���C���[�������Ń^�[�Q�b�g����悤�ɏC��:sezaki
9:�^�[�Q�b�g�������擾���Ă����̂Ŕ�V���A���C�Y��:takagi
12:CHitPoint��K�p:takagi
14:������HP�v�Z�����:takagi
21:�X�e�[�^�X�A�����C��:sezaki
21:�����͂�؂�o���A���̑��ׂ������t�@�N�^�����O���:takagi
=====*/

// ���O��Ԑ錾
using System;
using UnityEngine;
using UnityEngine.AI;

// �N���X��`
public class CEnemy : MonoBehaviour, IDH
{
	// �\���̒�`
	[Serializable]
	public struct Status	// �G�X�e�[�^�X
	{
		[SerializeField, Tooltip("�U����")] public int m_nAtk;
		[SerializeField, Tooltip("�U�����x")] public float m_fAtkSpeed;
		[SerializeField, Tooltip("�h���")] public int m_nDef;
		[SerializeField, Tooltip("�d��")] public int m_nWeight;
		[SerializeField, Tooltip("�����x")] public int m_nGrowth;
		[SerializeField, Tooltip("�������")] public int m_nGrowthLimit;
		[SerializeField, Tooltip("�������x")] public int m_nGrowthSpeed;
		[SerializeField, Tooltip("������")] public int m_nGrowthPower;
		[SerializeField, Tooltip("�U������")] public float m_fAtkRange;
		[SerializeField, Tooltip("�U���p�x")] public float m_fAtkAngle;
	}
	[Serializable]
	public struct GrowthRate	// ��������
	{
		[SerializeField, Tooltip("�̗͐�������")] public float m_fHP;
		[SerializeField, Tooltip("�U����������")] public float m_fAtk;
		[SerializeField, Tooltip("�ړ���������")] public float m_fMoveSpeed;
		[SerializeField, Tooltip("�U�����x��������")] public float m_fAtkSpeed;
		[SerializeField, Tooltip("�h�䐬������")] public float m_fDef;
		[SerializeField, Tooltip("�d�ʐ�������")] public float m_fWeight;
	}
	
	// �ϐ��錾
	private CHitPoint m_HitPoint;   // HP

    private float m_ftime = 0.0f;//�����܁[
    private float m_fcount = 0.0f;//�������
    private bool m_bIsPoison = false; //�v���C���[���ŃJ
    private bool m_bPoisonUpdate = false;//�ōX�V�p
    [Header("�X�e�[�^�X")]
	[SerializeField, Tooltip("�X�e�[�^�X")] private Status m_Status;
	[SerializeField, Tooltip("������")] private GrowthRate m_Growth;
	private Status m_StatusInitial;	// �X�e�[�^�X�����l
	private Transform m_Target;	// �v���C���[��Transform
	private float m_fGrowthInterval;	// �����Ԋu�i�b�j
	private float m_fGrowthTimer = 0f;	// �����^�C�}�[
	private float m_fAtkCooldown = 0f;	// �U���̃N�[���^�C��
	private float m_fSpeedInitial;	// ���x�����l
	private int m_nHPInitial;	//�̗͏����l
	private float m_fScale;	//�T�C�Y�ύX
	private NavMeshAgent m_Agent;	// �ǐՑΏ�
	[SerializeField, Tooltip("�̉t")] GameObject m_Blood;


	/// <summary>
	/// -�������֐�
	/// <para>�����������֐�</para>
	/// </summary>
	private void Start()
	{
		// NavMeshAgent���擾
		m_Agent = GetComponent<NavMeshAgent>();

		// Player�������ŒT���ă^�[�Q�b�g�ɐݒ�
		GameObject playerObj = GameObject.FindWithTag("Player");
		if (playerObj != null)
		{
			m_Target = playerObj.transform;
		}

		// �n�ʂ̈ʒu��T��
		if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 10f, NavMesh.AllAreas))
		{
			m_Agent.Warp(hit.position);	// NavMesh�̒n�ʂɃ��[�v������
		}

		// HP�̎���
		m_HitPoint = GetComponent<CHitPoint>();	// HP���擾
		if (!m_HitPoint)	// �R���|�[�l���g���Ȃ�
		{
			// �@�\�̊m��
			m_HitPoint = gameObject.AddComponent<CHitPoint>();	// ����ɍ쐬
			
			// �o��
			Debug.LogWarning("HP���s�����Ă��܂��F�����ō쐬��");

			// �����l�ݒ�
			m_HitPoint.HP = 100;	// �ݒ肳��ĂȂ��Ƃ������Ƃ͖������Ȑ����̂͂�...	//TODO:���P
		}

		//�����X�e�[�^�X���m��
		m_StatusInitial = m_Status;
		m_fSpeedInitial = m_Agent.speed;
		m_nHPInitial = m_HitPoint.HP;

		// �C�x���g�ڑ�
		m_HitPoint.OnDead += OnDead;	// ���S��������ڑ�
	}

	/// <summary>
	/// -�X�V�֐�
	/// <para>�X�V�����֐�</para>
	/// </summary>
	private void Update()
	{
		m_fAtkCooldown -= Time.deltaTime;	// �o�ߎ��ԂŌ��炷

		//�ǐ�
		m_Agent.SetDestination(m_Target.position);

		// �U��
		Attack();


        //�f�o�t
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
        // ����
        Growth();
	}


	/// <summary>
	/// -�U���֐�
	/// <para>Player�ɋ߂Â�����U������֐�</para>
	/// </summary>
	public void Attack()
	{
		if (!m_Agent.pathPending && m_Agent.remainingDistance <= m_Agent.stoppingDistance)// �G�ɍő���߂Â�����
		{
			if (m_fAtkCooldown > 0f) return;	// �N�[���^�C�����I�������

			// ���͈͓��̃R���C�_�[�����o
			Collider[] hits = Physics.OverlapSphere(transform.position, m_Status.m_fAtkRange);

			foreach (Collider hit in hits)
			{
				if (hit.CompareTag("Player"))	// �^�O��Player�̃I�u�W�F�N�g��T��
				{
					Vector3 dirToTarget = (hit.transform.position - transform.position).normalized; 
					float angle = Vector3.Angle(transform.forward, dirToTarget);

					if (angle < m_Status.m_fAtkAngle / 2f)	// �U���͈͓���
					{
						//Player�����鎞
						CPlayer player = hit.GetComponent<CPlayer>();
						if (player != null)
						{
							player.Damage(m_Status.m_nAtk);
							m_fAtkCooldown = 10.0f / m_Status.m_fAtkSpeed;
						}
					}
				}
			}
		}
	}


	/// <summary>
	/// -�U���͈͊֐�
	/// <para>�U���͈͂����o������֐�</para>
	/// </summary>
	private void OnDrawGizmos()
	{
		int _nsegments = 30;	// ��`���\��������̖{��

		Gizmos.color = new Color(1, 0, 0, 0.3f);	// �ԁE�������ɐݒ�

		Vector3 origin = transform.position;	// �G�̌��݈ʒu
		Quaternion startRotation = Quaternion.Euler(0, -m_Status.m_fAtkAngle / 2, 0);	// ���[�̊p�x�ɉ�
		Vector3 startDirection = startRotation * transform.forward;	// ���[�������o��

		Vector3 prevPoint = origin + startDirection * m_Status.m_fAtkRange;

		for (int i = 1; i <= _nsegments; i++)
		{
			float angle = -m_Status.m_fAtkAngle / 2 + (m_Status.m_fAtkAngle / _nsegments) * i;
			Quaternion rot = Quaternion.Euler(0, angle, 0);	// �e���̊p�x
			Vector3 direction = rot * transform.forward;	// ��]�����ĕ����擾
			Vector3 point = origin + direction * m_Status.m_fAtkRange;

			Gizmos.DrawLine(origin, point);	// ���_���_�̐�
			Gizmos.DrawLine(prevPoint, point);	// �O��̓_������̓_�Ő�`�̕�
			prevPoint = point;	// ���̃��[�v�p�ɕۑ�
		}
	}

	/// <summary>
	/// -�����֐�
	/// <para>���Ԃ��ƂɓG�̃X�e�[�^�X���グ��֐�</para>
	/// </summary>
	public void Growth()
	{
		//�������Ȃ��Ȃ�X�L�b�v
		if (m_Status.m_nGrowthSpeed == 0)
		{
			return;
		}
		//�������x�����߂�
		m_fGrowthInterval = 100 / m_Status.m_nGrowthSpeed;
		m_fGrowthTimer += Time.deltaTime;
		//��������
		if (m_fGrowthTimer >= m_fGrowthInterval)
		{
			// �����x������ɒB���Ă����牽�����Ȃ�
			if (m_Status.m_nGrowth >= m_Status.m_nGrowthLimit)
			{
				m_Status.m_nGrowth = m_Status.m_nGrowthLimit; 
				return;
			}
			//m_Status.m_nHp += m_Status.m_nGrowth + m_Status.m_nGrowthSpeed;
			//m_HitPoint.HP += (int)(m_Status.m_nGrowthSpeed * 0.1f);
			m_HitPoint.HP = m_HitPoint.HP + (int)(m_nHPInitial * m_Growth.m_fHP);
			m_Status.m_nAtk = m_Status.m_nAtk + (int)(m_StatusInitial.m_nAtk * m_Growth.m_fAtk);
			m_Agent.speed = m_Agent.speed + (m_fSpeedInitial * m_Growth.m_fMoveSpeed);
			m_Status.m_fAtkSpeed = m_Status.m_fAtkSpeed + (m_StatusInitial.m_fAtkSpeed * m_Growth.m_fAtkSpeed);
			m_Status.m_nDef = m_Status.m_nDef + (int)(m_StatusInitial.m_nDef * m_Growth.m_fDef);
			m_Status.m_nWeight = m_Status.m_nWeight + (int)(m_StatusInitial.m_nWeight * m_Growth.m_fWeight);
			m_Status.m_nGrowth = m_Status.m_nGrowth + m_Status.m_nGrowthPower;
			m_fScale = m_Status.m_nGrowth / m_StatusInitial.m_nGrowth;
			transform.localScale += new Vector3(m_fScale, m_fScale, m_fScale);
			// �����x������𒴂�����A����ɑ����Ă���
			if (m_Status.m_nGrowth > m_Status.m_nGrowthLimit)
			{
				m_Status.m_nGrowth = m_Status.m_nGrowthLimit;
			}

			m_fGrowthTimer = 0f;
		}
	}

	/// <summary>
	/// -�_���[�W�֐�	//TODO:�v���C���[�́u�U���v�����Affect�Ƃ���Damage���A�^�b�`
	/// <para>�_���[�W���󂯂�֐�</para>
	/// <param name="_nDamage">����̍U����</param>
	/// </summary>
	public void Damage(int _nDamage)
	{
		if (_nDamage <= m_Status.m_nDef)	// �h�䂪��_�������������_����1�ɂ���
		{
			_nDamage = 1;
		}
		else	// �_���[�W��^����
		{
			_nDamage = _nDamage - m_Status.m_nDef;
		}

		//m_Status.m_nHp -= _nDamage;�@// �_���[�W����
		m_HitPoint.HP -= _nDamage;	// �_���[�W����

		//if (m_Status.m_nHp <= 0)	// HP��0�̎�
		//if (m_HitPoint.HP <= 0)	// HP��0�̎�
		//{
		//	Destroy(gameObject);	// �G������
		//}
	}

	/// <summary>
	/// -���S�������֐�
	/// <para>HP��0�ɂȂ����Ƃ��ɌĂяo�����֐�</para>
	/// </summary>
	private void OnDead()
	{
		// �̉t�̔r�o
		if (m_Blood != null)	
		{
			float _temp_y = 0.0f;

			Ray ray = new Ray(transform.position, Vector3.down);
			RaycastHit hitten;
			if (Physics.Raycast(ray, out hitten, 200.0f))
			{
				_temp_y = hitten.transform.gameObject.transform.position.y;
				//Debug.Log(hitten.transform.gameObject.name);
			}

			//Debug.Log(_temp_y);
			//Instantiate(m_Blood, new Vector3(transform.position.x, _temp_y, transform.position.z), Quaternion.identity);
			Instantiate(m_Blood, transform.position, Quaternion.identity);
			//Debug.LogError("�̉t����");
		}
		else
		{
			Debug.LogError("�̉t���ݒ肳��Ă��܂���");
		}

		Destroy(gameObject);	// �G������
	}

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

}