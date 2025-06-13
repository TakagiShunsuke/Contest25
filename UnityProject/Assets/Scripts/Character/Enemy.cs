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
25:navMesh�ǉ�:sezaki
25�F���S�G�t�F�N�g�p�̃I�u�W�F�N�g�錾�ƓG���S������̃G�t�F�N�g���������ǉ��Ftei
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
28:�G�t�F�N�g���}�[�W:takagi
30:HP�̎d�l�ύX�ɔ����A�����������ő�HP�ɔ��f:takagi
_M06
D
11:�A�G���_���[�W���󂯂��Ƃ��Ԃ�����A�U���͈͂𐬒�����悤��:sezaki 
=====*/

// ���O��Ԑ錾
using System;
using System.Collections;
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
		[SerializeField, Tooltip("�d��")] public int m_nWeight;
		[SerializeField, Tooltip("�����x")] public int m_nGrowth;
		[SerializeField, Tooltip("�������")] public int m_nGrowthLimit;
		[SerializeField, Tooltip("�������x")] public int m_nGrowthSpeed;
		[SerializeField, Tooltip("������")] public int m_nGrowthPower;
		[SerializeField, Tooltip("�U������")] public float m_fAtkRange;
		[SerializeField, Tooltip("�U���p�x")] public float m_fAtkAngle;
        [SerializeField, Tooltip("�m�b�N�o�b�N�����l")] public float m_fBack;
        [SerializeField, Tooltip("��~����")] public float m_fStop;
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
        [SerializeField, Tooltip("�U��������������")] public float m_fAtkRange;
        [SerializeField, Tooltip("�U���p�x��������")] public float m_fAtkAngle;
        [SerializeField, Tooltip("��~����")] public float m_fStop;
    }

    // �ϐ��錾
    private CHitPoint m_HitPoint;   // HP
    [SerializeField] private Material flashMaterial; //�_���[�W�󂯂��Ƃ��̃}�e���A��
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
	[SerializeField, Tooltip("����HP")]�@private int m_nInitialHP;
	[SerializeField, Tooltip("�h���")] public int m_nInitialDef;
	private float m_fScale;	//�T�C�Y�ύX
	private NavMeshAgent m_Agent;	// �ǐՑΏ�
	[SerializeField, Tooltip("�̉t")] GameObject m_Blood;

    private Rigidbody m_Rigid;                     // Rigidbody�Q��
    private bool m_IsKnockback = false; //�m�b�N�o�b�N�t���O

    private float m_fStop;

    private Material defaultMaterial; //�ʏ�̃}�e���A��
    private Renderer rend; //�����_���[

    [Header("�G�t�F�N�g")]
	[SerializeField, Tooltip("�G�t�F�N�g�v���n�u")] private GameObject deathEffectPrefab;


	/// <summary>
	/// -�������֐�
	/// <para>�����������֐�</para>
	/// </summary>
	private void Start()
	{
		// NavMeshAgent���擾
		m_Agent = GetComponent<NavMeshAgent>();
        m_Rigid = GetComponent<Rigidbody>();
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

        rend = GetComponentInChildren<Renderer>();
        if (defaultMaterial == null && rend != null)
        {
            defaultMaterial = rend.sharedMaterial;
        }

        // HP�̎���
        if (m_HitPoint = GetComponent<CHitPoint>())
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
		m_HitPoint.MaxHP = m_nInitialHP;	// ����HP�ݒ�
		m_HitPoint.Defence = m_nInitialDef;	// �����h��ݒ�

		//�����X�e�[�^�X���m��
		m_StatusInitial = m_Status;
		m_fSpeedInitial = m_Agent.speed;
		m_nInitialHP = m_HitPoint.HP;
        m_fStop = m_Agent.stoppingDistance;

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
					//float angle = Vector3.Angle(transform.forward, dirToTarget);
					float angle = Vector2.Angle(new Vector2(transform.forward.x, transform.forward.z), new Vector2(dirToTarget.x, dirToTarget.z));	// ���ʏ�Ŕ���
					
					if (angle < m_Status.m_fAtkAngle / 2f)	// �U���͈͓���
					{
						//Player�����鎞
						CPlayer player = hit.GetComponent<CPlayer>();
						if (player != null)
						{
							player.Damage(m_Status.m_nAtk,this.transform, m_Status.m_nWeight);
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
			//m_HitPoint.HP = m_HitPoint.HP + (int)(m_nInitialHP * m_Growth.m_fHP);
			m_HitPoint.MaxHP += (int)(m_nInitialHP * m_Growth.m_fHP);
			m_Status.m_nAtk = m_Status.m_nAtk + (int)(m_StatusInitial.m_nAtk * m_Growth.m_fAtk);
			m_Agent.speed = m_Agent.speed + (m_fSpeedInitial * m_Growth.m_fMoveSpeed);
			m_Status.m_fAtkSpeed = m_Status.m_fAtkSpeed + (m_StatusInitial.m_fAtkSpeed * m_Growth.m_fAtkSpeed);
			//m_Status.m_nDef = m_Status.m_nDef + (int)(m_StatusInitial.m_nDef * m_Growth.m_fDef);
			m_HitPoint.Defence = m_HitPoint.Defence + (int)(m_nInitialDef * m_Growth.m_fDef);
			m_Status.m_nWeight = m_Status.m_nWeight + (int)(m_StatusInitial.m_nWeight * m_Growth.m_fWeight);
			m_Status.m_nGrowth = m_Status.m_nGrowth + m_Status.m_nGrowthPower;
			m_fScale = m_Status.m_nGrowth / m_StatusInitial.m_nGrowth;
            m_Status.m_fAtkAngle = m_Status.m_fAtkAngle + (int)(m_StatusInitial.m_fAtkAngle * m_Growth.m_fAtkAngle);
            m_Status.m_fAtkRange = m_Status.m_fAtkRange + (int)(m_StatusInitial.m_fAtkRange * m_Growth.m_fAtkRange);
            transform.localScale += new Vector3(m_fScale, m_fScale, m_fScale);
            m_Agent.stoppingDistance = m_Agent.stoppingDistance + (int)(m_fStop * m_Growth.m_fStop);
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
    /// <param name="Transform attacker">����̌����Ă����</param>
    /// </summary>
    public void Damage(int _nDamage, Transform attacker)
	{
		if (_nDamage <= m_HitPoint.Defence)	// �h�䂪��_�������������_����1�ɂ���
		{
			_nDamage = 1;
		}
		else	// �_���[�W��^����
		{
			_nDamage = _nDamage - m_HitPoint.Defence;
		}

		//m_Status.m_nHp -= _nDamage;�@// �_���[�W����
		m_HitPoint.HP -= _nDamage;  // �_���[�W����

        StartCoroutine(KnockbackCoroutine(attacker));
        StartCoroutine(FlashRedCoroutine());  
        //if (m_Status.m_nHp <= 0)	// HP��0�̎�
        //if (m_HitPoint.HP <= 0)	// HP��0�̎�
        //{
        //	Destroy(gameObject);	// �G������
        //}
    }

    /// <summary>
    /// -�m�b�N�o�b�N�֐�	
    /// <para>�m�b�N�o�b�N����֐�</para>
    /// <param name="Transform attacker">����̌����Ă����</param>
    /// </summary>

    private IEnumerator KnockbackCoroutine(Transform attacker)
    {
        Debug.Log("�m�b�N�o�b�N�J�n�I");
        if(m_Agent.enabled && m_Agent.isOnNavMesh)
        {
            m_Agent.isStopped = true;

        }
        m_IsKnockback = true;

        Vector3 knockbackDir = (transform.position - attacker.position).normalized;
        float knockbackPower = 100 / m_Status.m_nWeight * m_Status.m_fBack;      // �m�b�N�o�b�N�̗́i���� or �X�s�[�h�j
        float knockbackTime = 0.2f;        // �m�b�N�o�b�N����
        float _fTimer = 0f;

        while (_fTimer < knockbackTime) //�m�b�N�o�b�N�̎w�莞�Ԃ̊�
        { //�G������փm�b�N�o�b�N
            transform.position += knockbackDir * knockbackPower * Time.deltaTime;
            _fTimer += Time.deltaTime;
            yield return null;
        }

        // �ǐՂ��ꎞ��~���鎞��
        yield return new WaitForSeconds(m_Status.m_fStop);

        m_IsKnockback = false;
        m_Agent.isStopped = false;
        Debug.Log("�m�b�N�o�b�N�I���I");
    }

    /// <summary>
    /// -�t���b�V���֐�
    /// <para>�_���[�W���󂯂��Ƃ��ɐԂ�����֐�</para>
    /// </summary>

    IEnumerator FlashRedCoroutine()
    {
        int flashCount = 4; //�����
        float flashInterval = 0.1f; //�_�ł̑��x

        rend = GetComponentInChildren<Renderer>();
        if (rend == null) yield break;

        for (int i = 0; i < flashCount; i++) //�Ԃ��_�ł���
        {
            rend.material = flashMaterial;
            yield return new WaitForSeconds(flashInterval);
            rend.material = defaultMaterial;
            yield return new WaitForSeconds(flashInterval);
        }
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

		// ���S�G�t�F�N�g��G�̈ʒu�ɐ���
		if (deathEffectPrefab != null)
		{
			Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
		}

		Destroy(gameObject);	// �G������

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