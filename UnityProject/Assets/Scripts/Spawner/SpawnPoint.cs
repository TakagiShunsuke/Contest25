/*=====
<SpawnPoint.cs>
└作成者：Nishibu

＞内容
敵のスポーン地点を管理するクラス

＞更新履歴
__Y25 
_M05
D
5:Spawner管理クラス生成:nishibu
6:修正:nishibu
7:修正、コメント:nishibu
8:EnemyWaveDataと連携できるように変更:nishibu
14:WaveDataのメタデータの修正に伴い修正1:nishibu
16:WaveDataのメタデータの修正に伴い修正2:nishibu
=====*/

// 名前空間宣言
using UnityEngine;
using System.Collections.Generic;

// クラス定義  
public class CSpawnPoint : MonoBehaviour
{
    [Header("このスポーン地点が対応するタグ（複数可）")]
    [Tooltip("スポーン地点が対応するタグを設定する")]
    public List<E_SpawnTagType> m_eMetaTags = new();


    /// <summary>
	/// -初期化関数
	/// <para>スポーンポイントを追加</para>
	/// </summary>
    private void Start()
    {
        if (CEnemySpawner.Instance != null)
            CEnemySpawner.Instance.RegisterSpawnPoint(this);
    }

    /// <summary>
	/// -破棄関数
	/// <para>スポーンポイントを削除</para>
	/// </summary>
    private void OnDestroy()
    {
        if (CEnemySpawner.Instance != null)
            CEnemySpawner.Instance.UnregisterSpawnPoint(this);
    }
}
