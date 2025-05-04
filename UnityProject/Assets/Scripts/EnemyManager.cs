/*=====
<EnemyManager.cs>
└作成者：Nishibu

＞内容
現在のWaveに応じて敵を生成、補充する処理を行うクラス

＞更新履歴
__Y25
_04
D
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

public class CEnemyManager : MonoBehaviour
{
    private CEnemySpawner m_Spawner;                        // Waveの進行を管理するクラスへの参照
    private List<GameObject> m_ActiveEnemies = new List<GameObject>();       // 現在アクティブな敵のリスト
    private bool m_bHasSpawnedInitial = false;               // 初回スポーンを行ったかどうか
    private bool m_bIsSpawning = false;                      // コルーチンによるスポーン処理中かどうか

    [Header("スポーン間隔（秒）")]
    [Tooltip("敵のスポーン間隔(秒)")]
    [SerializeField]
    private float m_fSpawnDelay = 3.0f;                       // 敵を1体ずつ出す間隔


    // ＞初期化関数
    // 引数：なし   
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要: EnemySpawnerクラスを取得する
    private void Start()
    {
        m_Spawner = GetComponent<CEnemySpawner>();          // 同じGameObjectからSpawnerを取得
    }

    // ＞更新関数
    // 引数：なし   
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要: 現在のWaveに応じて敵を初回生成・補充スポーンをする
    private void Update()
    {
        // 前提チェック：spawnerが存在しない、またはWaveデータが空ならスキップ
        if (m_Spawner == null || m_Spawner.m_WaveList.Count == 0)
            return;

        // 全Waveが終わっていたらスポーン処理を中断
        if (m_Spawner.m_bIsAllWaveEnd)
            return;

        // 現在のWaveIndexを取得
        int _nWaveIndex = m_Spawner.m_nCurrentwaveIndex;

        // Indexが無効な範囲だったらスキップ
        if (_nWaveIndex < 0 || _nWaveIndex >= m_Spawner.m_WaveList.Count)
            return;

        // 現在のWaveデータを取得
        CEnemyWaveData m_WaveData = m_Spawner.m_WaveList[_nWaveIndex];

        // 死んだ敵(null)をリストから削除
        m_ActiveEnemies.RemoveAll(e => e == null);

        // 初回スポーン処理（1Waveごとに1回だけ）
        if (!m_bHasSpawnedInitial)
        {
            StartCoroutine(SpawnEnemiesWithDelay(m_WaveData, m_WaveData.m_nMaxEnemyCount));
            m_bHasSpawnedInitial = true;
        }

        // 足りない敵を補充（初回以降、敵が減ったとき）
        int _nToSpawn = m_WaveData.m_nMaxEnemyCount - m_ActiveEnemies.Count;
        if (_nToSpawn > 0 && !m_bIsSpawning)
        {
            StartCoroutine(SpawnEnemiesWithDelay(m_WaveData, _nToSpawn));
        }
    }

    // ＞複数体のエネミースポーンコルーチン関数
    // 引数1：_WaveData：現在のWaveの敵データ
    // 引数2：_nCount：スポーンする敵の数
    // ｘ
    // 戻値：IEnumerator
    // ｘ
    // 概要:コルーチンを使い、一定間隔ごとに指定数の敵をスポーンする
    private IEnumerator SpawnEnemiesWithDelay(CEnemyWaveData _WaveData, int _nCount)
    {
        m_bIsSpawning = true; // スポーン中フラグをTrue
        for (int i = 0; i < _nCount; i++)
        {
            SpawnEnemy(_WaveData); // 1体ずつスポーン
            yield return new WaitForSeconds(m_fSpawnDelay); // 指定間隔だけ待機
        }
        m_bIsSpawning = false; // スポーン終了
    }

    // ＞1体エネミースポーン関数
    // 引数1：_WaveData：現在のWaveの敵データ   
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要: Waveデータからランダムに敵を1体スポーンする
    private void SpawnEnemy(CEnemyWaveData _WaveData)
    {
        // プレハブが設定されていない場合はスキップ
        if (_WaveData.m_EnemyPrefabs.Count == 0) return;

        // ランダムに1体選択
        int _Rand = Random.Range(0, _WaveData.m_EnemyPrefabs.Count);
        GameObject prefab = _WaveData.m_EnemyPrefabs[_Rand];

        // スポーン位置
        GameObject _Enemy = Instantiate(prefab, transform.position, Quaternion.identity);
        m_ActiveEnemies.Add(_Enemy); // リストに追加
    }

    // ＞スポーンフラグリセット関数
    // 引数：なし   
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要: 初期スポーンフラグをリセットし、再スポーンを可能にする
    public void ResetInitialSpawnFlag()
    {
        m_bHasSpawnedInitial = false;
    }
}
