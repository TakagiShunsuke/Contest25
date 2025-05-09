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
=====*/

// 名前空間宣言
using UnityEngine;
using System.Collections.Generic;

// クラス定義
public class CSpawnPoint : MonoBehaviour
{
    [Header("スポーンポイントID")]
    [Tooltip("このスポーンポイントのID")]
    public int m_nPointID;


    // ＞初期化関数
    // 引数：なし
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：シーン開始時に自身をスポーンポイントとしてEnemySpawnerに登録する
    private void Start()
    {
        CEnemySpawner.Instance.RegisterSpawnPoint(this);
    }

    // ＞破棄関数
    // 引数：なし
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：シーン終了時などに自身をEnemySpawnerのリストから解除する
    private void OnDestroy()
    {
        CEnemySpawner.Instance.UnregisterSpawnPoint(this);
    }
}
