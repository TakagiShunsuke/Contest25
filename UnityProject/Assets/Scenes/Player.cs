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

using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField]
    [Tooltip("HP")]
    private int Hp = 100;
    [SerializeField]
    [Tooltip("�U����")]
    private int Atk = 100;
    [SerializeField]
    [Tooltip("�ړ����x")]
    private float speed = 200.0f;
    [SerializeField]
    [Tooltip("�U�����x")]
    private float AtkSpeed = 100.0f; // �U�����x
    [SerializeField]
    [Tooltip("�h���")]
    private int Def = 5;

    [SerializeField]
    [Tooltip("�U���̔��a")]
    private float attackRange = 0.02f; // �U���̔��a

    [SerializeField]
    [Tooltip("�U���̊p�x")]
    private float attackAngle = 45f; // 45�x�͈̔�

    private float lastAttackTime = 0f; // �Ō�ɍU����������
    private float attackCooldown; // �U���̃N�[���_�E������

    private bool isDead = false; // �v���C���[������ł��邩�ǂ���

    

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        attackCooldown = 1f / AtkSpeed; // �U�����x�ɉ����čU���Ԋu��ݒ�
    }

    private void PlayerMove()
    {
        // �v���C���[�̈ړ�
        if (Input.GetKey(KeyCode.W))
        {
            rb.transform.position += Vector3.forward * speed / 150;
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.transform.position += Vector3.back * speed / 150;
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.transform.position += Vector3.left * speed / 150;
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.transform.position += Vector3.right * speed / 150;
        }
    }

    private void Attack()
    {
        // �U�����邽�߂̃N�[���_�E�����ԃ`�F�b�N
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;

            // �U���̌���
            Vector3 forward = transform.forward;

            // �U���͈͂̌v�Z
            Collider[] hitColliders = Physics.OverlapSphere(transform.position + forward * attackRange, attackRange);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject == this.gameObject) continue;

                Vector3 directionToTarget = hitCollider.transform.position - transform.position;
                float angle = Vector3.Angle(forward, directionToTarget);
                if (angle <= attackAngle / 2)
               {
                    // �͈͓��ɓ������G�ɍU������
                    Debug.Log("�U���q�b�g�I");
                    

                }
            }

            Debug.Log("�U���I"); // �U���̃��O
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isDead) return; // �v���C���[������ł���ꍇ�͑���𖳌��ɂ���

        PlayerMove();

        // �v���C���[�̍U��(Enter)
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Attack(); // �U���������Ăяo��
            
        }

        if(Hp <= 0 && !isDead)
        {
            Die();
        }
    }

    private void Die() // �v���C���[�����񂾂Ƃ��ɌĂяo������
    {
        isDead = true;
        Debug.Log("�v���C���[�͎��ɂ܂����I");
    }

    
    
}
