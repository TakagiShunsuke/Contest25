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
14:WaveDataのメタデータの修正1:nishibu
16:WaveDataのメタデータの修正2:nishibu
=====*/

// 名前空間宣言
using System.Collections.Generic;
using UnityEngine;

// 列挙定義
public enum E_SpawnTagType
{
    Ground, // 地上の敵
    Water,  // 水辺の敵
    Normal  // 共通の敵
}

// クラス定義
[CreateAssetMenu(fileName = "WaveData", menuName = "Enemy/WaveData")]
public class CEnemyWaveData : ScriptableObject
{
    [Header("敵スポーン最大数")]
    [Tooltip("このWaveで出現する敵の最大数（全体）")]
    public int m_nMaxEnemyCount;

    [Header("Wave時間（秒）")]
    [Tooltip("このWaveの継続時間（秒）")]
    public float m_fWaveDuration = 120.0f; 

    [Header("スポーン間隔")]
    [Tooltip("敵をスポーンする間隔（秒）")]
    public float m_fSpawnInterval = 3.0f;

    [Header("このWaveの敵設定")]
    [Tooltip("Tagと敵種類を設定")]
    public List<CSpawnTagEnemyList> m_EnemyByTagList = new ();
}

// クラス定義
[System.Serializable]
public class CSpawnTagEnemyList
{
    [Header("タグ設定")]
    [Tooltip("タグ設定")]
    public E_SpawnTagType m_Tag; 

    [Header("敵設定")]
    [Tooltip("敵設定")]
    public GameObject m_EnemyPrefabs; 
}

