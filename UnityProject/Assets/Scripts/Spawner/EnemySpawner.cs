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
// 修正箇所: 'FirstOrDefault' を使用するために 'System.Linq' をインポート
using System.Linq;

// 既存のコードに変更は不要です。'System.Linq' をインポートすることで、'FirstOrDefault' が使用可能になります。
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
16:WaveDataのメタデータの修正に伴い修正1:nishibu
16:WaveDataのメタデータの修正に伴い修正2:nishibu
=====*/

// 名前空間宣言
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// クラス定義
public class CEnemySpawner : MonoBehaviour
{
    public static CEnemySpawner Instance { get; private set; }  // インスタンス取得用プロパティ

    [Header("WaveDataリスト")]
    [Tooltip("WaveDataを設定")]
    [SerializeField] private List<CEnemyWaveData> m_WaveDataList;

    private List<CSpawnPoint> m_SpawnPoints = new();  // 登録済みスポーンポイント
    private int m_nCurrentWave = 0; // 現在のWave番号


    /// <summary>
	/// -Awake関数
	/// <para>シングルトン初期化</para>
	/// </summary>
    private void Awake()
    {
        // シングルトン生成チェック
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
	/// -Start関数
	/// <para>スポナーループ開始</para>
	/// </summary>
    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    /// <summary>
    /// -RegisterSpawnPoint関数
    /// <para>スポーンポイント登録</para>
    /// </summary>
    /// <param name="_Point">登録するスポーンポイント</param>
    public void RegisterSpawnPoint(CSpawnPoint _Point)
    {
        if (!m_SpawnPoints.Contains(_Point))
            m_SpawnPoints.Add(_Point);
    }

    /// <summary>
    /// -UnregisterSpawnPoint関数
    /// <para>スポーンポイント解除</para>
    /// </summary>
    /// <param name="_Point">解除するスポーンポイント</param>
    public void UnregisterSpawnPoint(CSpawnPoint _Point)
    {
        m_SpawnPoints.Remove(_Point);
    }

    /// <summary>
    /// -NextWave関数
    /// <para>次のWaveへ</para>
    /// </summary>
    public void NextWave()
    {
        m_nCurrentWave++;
    }

    /// <summary>
    /// -SpawnLoop関数
    /// <para>Waveごとの敵スポーンループ</para>
    /// </summary>
    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            if (m_nCurrentWave >= m_WaveDataList.Count) 
                yield break;

            var _WaveData = m_WaveDataList[m_nCurrentWave];

            yield return new WaitForSeconds(_WaveData.m_fSpawnInterval);

            if (CCountEnemy.m_nValInstances < _WaveData.m_nMaxEnemyCount)
            {
                TrySpawnEnemyForAllPoints(_WaveData);
            }
        }
    }

    /// <summary>
    /// -TrySpawnEnemyForAllPoints関数
    /// <para>全スポーンポイントを巡回し、条件に合う敵を1体スポーン</para>
    /// </summary>
    /// <param name="_WaveData">現在Waveのデータ</param>
    private void TrySpawnEnemyForAllPoints(CEnemyWaveData _WaveData)
    {
        if (m_SpawnPoints.Count == 0) 
            return; // ポイント無しなら中断

        int _nStartIndex = Random.Range(0, m_SpawnPoints.Count); // ランダム開始インデックス

        // 全ポイントを一周探索
        for (int i = 0; i < m_SpawnPoints.Count; i++)
        {
            int nIndex = (_nStartIndex + i) % m_SpawnPoints.Count; // ランダムに開始したインデックスから順に探索
            var _Point = m_SpawnPoints[nIndex]; // スポーンポイント取得

            // スポーンポイントのメタタグに基づいて敵をスポーン
            List<GameObject> _MatchList = new();
            foreach (var tag in _Point.m_eMetaTags)
            {
                // WaveDataのメタタグに基づいて敵をスポーン
                var match = _WaveData.m_EnemyByTagList.FirstOrDefault(e => e.m_Tag == tag);
                if (match != null)
                {
                    _MatchList.Add(match.m_EnemyPrefabs);
                }
            }

            // スポーンする敵が無いなら次のポイントへ
            if (_MatchList.Count == 0) 
                continue; 

            // スポーンする敵が見つかったらランダムに1体スポーン
            GameObject _Selected = _MatchList[Random.Range(0, _MatchList.Count)];
            Instantiate(_Selected, _Point.transform.position, Quaternion.identity);
            break; // 1体スポーンしたら抜ける
        }
    }

    /// <summary>
    /// -GetCurrentWaveData関数
    /// <para>現在Waveのデータ取得</para>
    /// </summary>
    /// <returns>現在Waveデータ</returns>
    public CEnemyWaveData GetCurrentWaveData()
    {
        // 現在Waveのデータを取得
        if (m_nCurrentWave < m_WaveDataList.Count)
            return m_WaveDataList[m_nCurrentWave];
        return null;
    }

    /// <summary>
    /// -IsWaveFinished関数
    /// <para>全Wave終了判定</para>
    /// </summary>
    /// <returns>true = 終了</returns>
    public bool IsWaveFinished()
    {
        return m_nCurrentWave >= m_WaveDataList.Count;
    }
}



