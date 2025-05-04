/*=====
<EnemyWaveData.cs>
└作成者：Nishibu

＞内容
Waveごとの敵の種類と最大出現数を管理するクラス

＞更新履歴
__Y25 
_M04
D
30:Enemy管理クラス作成:nishibu
_M05
D
1:コメント追加:nishibu
=====*/

// 名前空間宣言
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CEnemyWaveData
{
    [Header("敵設定")]
    [Tooltip("このWaveで出現させる敵プレハブのリスト")]
    public List<GameObject> m_EnemyPrefabs;

    [Header("敵スポーン最大数")]
    [Tooltip("このWaveで常に出現させる敵の最大数")]
    public int m_nMaxEnemyCount = 5;
}