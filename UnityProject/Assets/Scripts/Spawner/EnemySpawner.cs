/*=====
<EnemySpawner.cs>
└作成者：Nishibu

＞内容
敵をスポーンし、Waveごとの敵数制限を管理するクラス

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
5:修正1:nishibu
6:修正2:nishibu
7:修正3、コメント:nishibu
8:修正3:nishibu
=====*/

// 名前空間宣言
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// クラス定義
public class CEnemySpawner : MonoBehaviour
{
    public static CEnemySpawner Instance { get; private set; }

    [Header("Waveデータ")]
    [Tooltip("各Waveの最大敵数とスポーン設定を持つリスト")]
    [SerializeField] private List<CEnemyWaveData> m_WaveDataList;

    private List<CSpawnPoint> m_SpawnPoints = new List<CSpawnPoint>(); // 登録されたすべてのスポーンポイント
    private int m_CurrentWave = 0; // 現在のWave番号


    // シングルトンインスタンス初期化
    private void Awake()
    {
        // まだインスタンスがなければ自分を設定
        if (Instance == null)
            Instance = this;
        else
        // すでに存在する場合は破棄
            Destroy(gameObject);
    }

    // ゲーム開始時にスポーン処理を開始
    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    // スポーンポイントの登録
    public void RegisterSpawnPoint(CSpawnPoint _Point)
    {
        // まだ登録されていない場合のみ追加
        if (!m_SpawnPoints.Contains(_Point))
        {
            m_SpawnPoints.Add(_Point);
        }
    }

    // スポーンポイントの登録解除
    public void UnregisterSpawnPoint(CSpawnPoint _Point)
    {
        m_SpawnPoints.Remove(_Point);
    }

    // Waveを進める処理（CWaveTimerManagerが呼び出す）
    public void NextWave()
    {
        m_CurrentWave++;
    }

    // 敵のスポーンを一定間隔で繰り返す処理
    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            // 全Wave終了していたらコルーチン終了
            if (m_CurrentWave >= m_WaveDataList.Count) yield break;

            // 現在のWaveのデータを取得
            CEnemyWaveData m_WaveData = m_WaveDataList[m_CurrentWave];

            // スポーン間隔分待機
            yield return new WaitForSeconds(m_WaveData.m_SpawnInterval);

            // 敵の最大数未満のときスポーン
            if (CCountEnemy.m_nValInstances < m_WaveData.m_nMaxEnemyCount)
            {
                TrySpawnEnemyForAllPoints(m_WaveData);
            }
        }
    }

    // 全スポーンポイントに対して1体スポーンを試みる処理
    private void TrySpawnEnemyForAllPoints(CEnemyWaveData _WaveData)
    {
        // スポーンポイントが登録されていない場合は処理終了
        if (m_SpawnPoints.Count == 0) return;

        // ランダムな開始インデックスを取得（スポーンの偏りを防ぐ）
        int m_StartIndex = Random.Range(0, m_SpawnPoints.Count);

        // スポーンポイントIDがWaveDataに含まれているか確認
        for (int i = 0; i < m_SpawnPoints.Count; i++)
        {
            int _nIndex = (m_StartIndex + i) % m_SpawnPoints.Count;
            CSpawnPoint m_Point = m_SpawnPoints[_nIndex];

            // PointIDがWaveData内に存在しなければスキップ
            if (m_Point.m_nPointID >= _WaveData.m_SpawnPointDataList.Count) continue;

            // 該当するスポーンポイントの敵プレハブリストを取得
            List<GameObject> m_Prefabs = _WaveData.m_SpawnPointDataList[m_Point.m_nPointID].m_EnemyPrefabs;

            // リストが空ならスキップ
            if (m_Prefabs.Count == 0) continue;

            // ランダムに敵プレハブを1つ選んでスポーン
            GameObject m_Prefab = m_Prefabs[Random.Range(0, m_Prefabs.Count)];
            Instantiate(m_Prefab, m_Point.transform.position, Quaternion.identity);
            break; // 1体スポーンしたら終了
        }
    }

    // 現在のWaveのデータを取得
    public CEnemyWaveData GetCurrentWaveData()
    {
        if (m_CurrentWave < m_WaveDataList.Count)
            return m_WaveDataList[m_CurrentWave];
        return null;
    }

    // 全Wave終了しているかどうか
    public bool IsWaveFinished()
    {
        return m_CurrentWave >= m_WaveDataList.Count;
    }
}



