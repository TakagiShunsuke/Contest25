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
using UnityEngine.AI;

// クラス定義
public class CEnemy : MonoBehaviour
{

    // 構造体定義
    [Serializable]
    public struct Status //敵ステータス
    {
        [SerializeField, Tooltip("HP")] public int m_nHp;                  // HP
        [SerializeField, Tooltip("攻撃力")] public int m_nAttack;       // 攻撃力
        [SerializeField, Tooltip("速度")] public float m_fSpeed;             // 速さ
        [SerializeField, Tooltip("攻撃速度")] public int m_nAttackSpeed;   // 攻撃速度
        [SerializeField, Tooltip("防御力")] public int m_nDefense;         // 防御
        [SerializeField, Tooltip("成長")] public int m_nGrowth;            // 成長
    }

    // 変数宣言

    [Header("ステータス")]
    [SerializeField, Tooltip("ステータス")] private Status m_Status;

    [Header("追跡")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform target;


    // ＞更新関数
    // 引数：なし   
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：更新処理
    private void Update()
    {
        agent.SetDestination(target.position);
    }

    // ＞ダメージ関数
    // 引数：なし   
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：ダメージを与える
    public void Damage(int _nDamage)　
    {
        if(_nDamage <= m_Status.m_nDefense)// 防御が被ダメを上回ったら被ダメを1にする
        {
            _nDamage = 1;
        }
        else
        {
            _nDamage = _nDamage - m_Status.m_nDefense;
        }

        m_Status.m_nHp -= _nDamage;

        if (m_Status.m_nHp <= 0) // HPが0の時
        {
            Destroy(gameObject); // 敵を消す
        }
    }
}