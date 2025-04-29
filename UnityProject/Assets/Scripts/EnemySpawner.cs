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
25:スポナー作成(α版):nishibu
29:コメント追加、修正:nishibu

=====*/

// 名前空間宣言
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// クラス定義
[System.Serializable]
public class WaveData
{
    [Header("敵設定(数、種類)")]
    [Tooltip("このウェーブで出す敵prefab")]
    public GameObject[] m_EnemyPrefabs;
}

// クラス定義
public class EnemySpawner : MonoBehaviour
{
    // 変数宣言	
    [Header("全ウェーブ敵設定(上からWave1,Wave2.....)")]
    [SerializeField]
    public List<WaveData> waveList = new List<WaveData>();

    [Header("ゲーム時間(分)")]
    [SerializeField, Tooltip("最終ウェーブまでの時間(分)")]
    public float m_fLastWaveTime = 10.0f;

    [Header("敵スポーン間隔時間(秒)")]
    [SerializeField, Tooltip("Wave開始時の敵を召喚するタイム間隔(秒)")]
    public float m_fSpawnTime = 2.0f; 

    [Header("Wave数")]
    [SerializeField, Tooltip("Wave数")]
    public int m_fWave = 5; 

    private bool m_bSpawnflg = false;  // Waveが終了しているか判定
    private float m_fTimer; //タイマー
    private int m_fWaveCount = 0; // ウェーブカウント
    private float m_fWaveTime = 0; // 1ウェーブの時間


    private void Start()
    {
        m_fWave = waveList.Count; // Wave数カウント
        m_fWaveTime = m_fLastWaveTime / m_fWave * 60f; // 1ウェーブの時間を求める
    }

    // ＞更新関数
    // 引数：なし   
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要: スポーン処理
    private void Update()
    {
        // Wave5が終わったらタイマーを止める
        if(!m_bSpawnflg)
        {
            m_fTimer += Time.deltaTime; // タイマースタート
        }

        // スポーン処理
        if(m_fTimer >= m_fWaveTime)
        {
            SpawnEnemy(); // 敵をスポーン
            m_fWaveCount += 1; // Waveカウント

            // Wave5になったらm_bSpawnflgをtrue
            if (m_fWaveCount >= 6)
            {
                m_bSpawnflg = true;
            }
            m_fTimer = 0.0f; // タイマーリセット
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
        StartCoroutine(SpawnEnemiesWithDelay());
    }

    // ＞敵ウェーブ生成コルーチン関数
    // 引数：なし
    // ｘ
    // 戻値：IEnumerator
    // コルーチンを使ってWave内の敵を一定間隔で生成
    IEnumerator SpawnEnemiesWithDelay()
    {
        // 現在のウェーブ番号
        int m_iWaveIndex = m_fWaveCount;

        // 有効なウェーブ番号かチェック
        if (m_iWaveIndex >= 0 && m_iWaveIndex < waveList.Count)
        {
            // 現在のウェーブに対応する敵プレハブ配列を取得
            GameObject[] currentWave = waveList[m_iWaveIndex].m_EnemyPrefabs;
            for (int j = 0; j < currentWave.Length; j++)
            {
                // 敵をスポーン
                Instantiate(currentWave[j], transform.position, Quaternion.identity);

                // 次のスポーンまで待機
                yield return new WaitForSeconds(m_fSpawnTime);
            }
        }
    }
}
