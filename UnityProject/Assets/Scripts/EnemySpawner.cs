/*=====
<EnemySpawner.cs>
└作成者：Nishibu

＞内容
敵を生成

＞更新履歴
__Y25 
_M04
D
18:スポナーを仮作成:nishibu

=====*/

// 名前空間宣言
using UnityEngine;

// クラス定義
public class EnemySpawner : MonoBehaviour
{
    // 変数宣言	
    [Header("敵設定")] 
    [SerializeField, Tooltip("敵")]
    public GameObject m_EnemyPrefab; 

    [Header("初回スポーン開始時間")]
    [SerializeField, Tooltip("初回スポーン開始時間")]
    public float m_fSpawn = 5.0f; 

    [Header("２回目以降スポーン間隔")]
    [SerializeField, Tooltip("２回目以降スポーン間隔")]
    public float m_fSpawnInterval = 20.0f; 

    private bool m_bSpawnflg = false;  // 1回目のスポーン判定
    private float m_fTimer; //タイマー


    // ＞更新関数
    // 引数：なし   
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要: スポーン処理
    private void Update()
    {
        // タイマースタート
        m_fTimer += Time.deltaTime;

        // スポーン処理
        if (!m_bSpawnflg)
        {
            // １回目のスポーン
            if (m_fTimer >= m_fSpawn)
            {
                SpawnEnemy(); // エネミースポーン関数呼び出し
                m_fTimer = 0.0f; // タイマーリセット
                m_bSpawnflg = true; //一回目のスポーンが終わったらtrue
            }
        }
        else
        {
            // ２回目以降のスポーン
            if (m_fTimer >= m_fSpawnInterval)
            {
                SpawnEnemy(); // エネミースポーン関数呼び出し
                m_fTimer = 0.0f; // タイマーリセット
            }
        }
    }

    // ＞エネミースポーン関数
    // 引数：なし   
    // ｘ
    // 戻値：なし
    // ｘ
    //概要 : 敵を生成
    void SpawnEnemy()
    {
        // スポナーの位置に敵を出現させる
        Instantiate(m_EnemyPrefab, transform.position, Quaternion.identity);
    }
}
