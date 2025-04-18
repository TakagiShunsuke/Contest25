/*  滑らかに動くけど加速するパターンの移動
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
    [Tooltip("攻撃力")]
    private int Atk = 100;
    [SerializeField]
    [Tooltip("移動速度")]
    private float speed = 200.0f;
    [SerializeField]
    [Tooltip("攻撃速度")]
    private float AtkSpeed = 100.0f; // 攻撃速度
    [SerializeField]
    [Tooltip("防御力")]
    private int Def = 5;

    [SerializeField]
    [Tooltip("攻撃の半径")]
    private float attackRange = 0.02f; // 攻撃の半径

    [SerializeField]
    [Tooltip("攻撃の角度")]
    private float attackAngle = 45f; // 45度の範囲

    private float lastAttackTime = 0f; // 最後に攻撃した時間
    private float attackCooldown; // 攻撃のクールダウン時間

    private bool isDead = false; // プレイヤーが死んでいるかどうか

    

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        attackCooldown = 1f / AtkSpeed; // 攻撃速度に応じて攻撃間隔を設定
    }

    private void PlayerMove()
    {
        // プレイヤーの移動
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
        // 攻撃するためのクールダウン時間チェック
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;

            // 攻撃の向き
            Vector3 forward = transform.forward;

            // 攻撃範囲の計算
            Collider[] hitColliders = Physics.OverlapSphere(transform.position + forward * attackRange, attackRange);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject == this.gameObject) continue;

                Vector3 directionToTarget = hitCollider.transform.position - transform.position;
                float angle = Vector3.Angle(forward, directionToTarget);
                if (angle <= attackAngle / 2)
               {
                    // 範囲内に入った敵に攻撃処理
                    Debug.Log("攻撃ヒット！");
                    

                }
            }

            Debug.Log("攻撃！"); // 攻撃のログ
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isDead) return; // プレイヤーが死んでいる場合は操作を無効にする

        PlayerMove();

        // プレイヤーの攻撃(Enter)
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Attack(); // 攻撃処理を呼び出す
            
        }

        if(Hp <= 0 && !isDead)
        {
            Die();
        }
    }

    private void Die() // プレイヤーが死んだときに呼び出す処理
    {
        isDead = true;
        Debug.Log("プレイヤーは死にました！");
    }

    
    
}
