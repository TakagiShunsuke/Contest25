/*=====
<EnemySpawner.cs>
└作成者：Nishibu

＞内容
ゲーム時間に応じてWaveを管理し、EnemyManagerに通知するクラス

＞更新履歴
__Y25 
_M04
D
18:スポナーを仮作成:nishibu
25:スポナー作成(α版):nishibu
29:コメント追加、修正:nishibu
30:仕様作成、修正1:nishibu
_M05
D
1:仕様作成、修正2:nishibu
3:仕様作成、修正3:nishibu
4:コメント追加:nishibu
=====*/

// 名前空間宣言
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// クラス定義
public class CEnemySpawner : MonoBehaviour
{
    [Header("Wave全体の敵設定(上からWave1,Wave2...)")]
    [Tooltip("Wave全体の敵設定")]
    public List<CEnemyWaveData> m_WaveList = new List<CEnemyWaveData>(); // 全Waveの設定データ

    [Header("ゲーム時間(分)")]
    [Tooltip("ゲーム時間")]
    public float m_fTotalGameTimeInMinutes = 10.0f;                         // 全Waveの進行時間（分）

    private float m_fWaveDuration;    // 1Waveあたりの継続時間（秒）
    private float m_fTimer;           // 現在のWave経過時間
    private int m_nCurrentWaveIndex = 0; // 現在のWave番号（0始まり）
    private CEnemyManager m_EnemyManager; // EnemyManagerへの参照

    [HideInInspector]
    public bool m_bIsAllWaveEnd = false; // 最終ウェーブが終了したか判定

    public int m_nCurrentwaveIndex => m_nCurrentWaveIndex; // 現在のWave番号を公開


    // ＞初期化関数
    // 引数：なし   
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要: Waveの継続時間を設定し、EnemyManagerの初期スポーンを許可
    private void Start()
    {
        m_fWaveDuration = (m_fTotalGameTimeInMinutes / m_WaveList.Count) * 60f; // Wave時間を算出（秒換算）
        m_EnemyManager = GetComponent<CEnemyManager>();

        if (m_EnemyManager != null)
        {
            m_EnemyManager.ResetInitialSpawnFlag(); // ゲーム開始時に初期スポーン許可
        }
    }

    // ＞更新関数
    // 引数：なし   
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要: Waveの経過時間をチェックし、次のWaveへ切り替える処理を行う
    private void Update()
    {
        m_fTimer += Time.deltaTime; // タイマースタート

        // Waveの切り替え判定（現在のWave時間を超えたら次のWaveへ）
        if (m_fTimer >= m_fWaveDuration && m_nCurrentWaveIndex < m_WaveList.Count - 1)
        {
            m_nCurrentWaveIndex++; // Waveを進める
            m_fTimer = 0.0f; // タイマーリセット

            if (m_EnemyManager != null)
            {
                m_EnemyManager.ResetInitialSpawnFlag(); // 新しいWave開始時に初回スポーンを許可
            }
        }
        else if (m_fTimer >= m_fWaveDuration && m_nCurrentWaveIndex >= m_WaveList.Count - 1)
        {
            // 最終Waveが終わたらTrue
            m_bIsAllWaveEnd = true;
        }
    }
}

