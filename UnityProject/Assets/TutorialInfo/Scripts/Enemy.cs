/*=====
<CodingRule.cs>	// スクリプト名
└作成者：sezaki

＞内容
敵（プロトタイプ）制作

＞注意事項	// ないときは省略OK
この規約書に記述のないものは判明次第、適宜追加する

＞更新履歴
__Y25	// '25年
_M04	// 4月
D		// 日
16:プログラム作成:sezaki	// 日付:変更内容:施行者
=====*/

// 名前空間宣言
using System;
using UnityEngine;	


// クラス定義
public class CEnemy : MonoBehaviour
{

    // 構造体定義
    [Serializable]
    public struct Status //敵ステータス
    {
        [SerializeField, Tooltip("HP")] public int Hp;                  // HP
        [SerializeField, Tooltip("攻撃力")] public int m_nAttack;       // 攻撃力
        [SerializeField, Tooltip("速度")] public int Speed;             // 速さ
        [SerializeField, Tooltip("攻撃速度")] public int AttackSpeed;   // 攻撃速度
        [SerializeField, Tooltip("防御力")] public int Defense;         // 防御
        [SerializeField, Tooltip("成長")] public int Growth;            // 成長
    }

    // 変数宣言
    [Header("ステータス")]
    [SerializeField, Tooltip("ステータス")] private Status m_Status;
    [Header("追跡")]
    [SerializeField, Tooltip("追跡フラグ")] private bool m_bChase = true; // 追跡
    private Transform m_Target; // プレイヤーのTransform
    private bool m_bChasing = false; // 追跡中かどうか
    [Header("ダメージ")]
    [SerializeField, Tooltip("自分がくらうダメージ")] private int m_nDamage;


    // ＞更新関数
    // 引数：なし   
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：更新処理
    private void Update()
    {
        if (m_bChasing) // 追跡フラグがtrueの時
        {
            Chaser(); // 追跡関数実行
        }
    }

    // ＞追跡関数
    // 引数：なし  
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：プレイヤーの追跡をする
    public void Chaser() 
    {
        if (m_bChase == true) // 追跡フラグがtrueの時、プレイヤーに向かって移動
        {
            Vector3 direction = (m_Target.position - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, m_Target.position, m_Status.Speed * Time.deltaTime);
        }
    }

    // ＞ダメージ関数
    // 引数：なし   
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：ダメージを与える
    public void Damage()　
    {
        m_Status.Hp -= m_nDamage;　// ダメージ処理

        if (m_Status.Hp <= 0) // HPが0の時
        {
            Destroy(gameObject); // 敵を消す
        }
    }

    // ＞ダメージ判定関数
    // 引数：Collision _Collision : 当たった相手
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：プレイヤーに当たったときダメージ関数を実行
    private void OnCollisionEnter(Collision _Collision) 
    {
        if (_Collision.gameObject.CompareTag("Player")) // Playerタグのオブジェクトに当たったとき
        {
            Damage();　// ダメージ関数実行
        }
    }

    // ＞追跡開始関数
    // 引数：Collider _Collision
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：プレイヤーが範囲内に入ったら追跡を開始する
    private void OnTriggerEnter(Collider _Collision) //接触判定(追跡範囲)
    {
        if (_Collision.CompareTag("Player")) // プレイヤーが範囲内に入ったら
        {
            m_Target = _Collision.transform; // プレイヤーのTransformを保存
            m_bChasing = true; // 追いかけ開始
        }
    }

    // ＞追跡終了関数
    // 引数：Collider _Collision   
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：プレイヤーが範囲外に出たら追跡を終了する
    private void OnTriggerExit(Collider _Collision) // 接触判定(追跡範囲)
    {
        if (_Collision.CompareTag("Player")) // プレイヤーが範囲外に出たら
        {
            m_bChasing = false;  // 追いかけ終了
            m_Target = null;     // ターゲットを解除
        }
    }
}