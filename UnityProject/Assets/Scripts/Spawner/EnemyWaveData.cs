/*=====
<EnemyWaveData.cs>
└作成者：Nishibu

＞内容
Waveごとに敵の最大出現数を管理するクラス

＞更新履歴
__Y25 
_M04
D
30:Enemy管理クラス作成:nishibu
_M05
D
1:コメント追加:nishibu
5:修正1:nishibu
6:修正2;nishibu
7:修正3、コメント:nishibu
8:WaveDataで敵の最大数、スポナー別の敵種類設定できるように変更:nishibu
=====*/

// 名前空間宣言
using System.Collections.Generic;
using UnityEngine;

// クラス定義
[CreateAssetMenu(fileName = "WaveData", menuName = "Enemy/WaveData")]
public class CEnemyWaveData : ScriptableObject
{
    [Header("敵スポーン最大数")]
    [Tooltip("このWaveで出現する敵の最大数（全体）")]
    public int m_nMaxEnemyCount;

    [Header("スポナー別の敵設定(上…スポナーA 下…スポナーB)")]
    [Tooltip("各スポーン場所ごとの出現敵設定")]
    public List<CSpawnPointData> m_SpawnPointDataList = new List<CSpawnPointData>();
}

// スポーンポイント用の敵プレハブリスト
[System.Serializable]
public class CSpawnPointData
{
    [Header("敵種類設定")]
    [Tooltip("スポナーごとの敵設定")]
    public List<GameObject> m_EnemyPrefabs = new List<GameObject>();
}

