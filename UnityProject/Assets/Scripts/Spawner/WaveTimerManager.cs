/*=====
<WaveTimerManager.cs>
└作成者：Nishibu

＞内容
// Waveのタイマー、進行タイミングを管理するクラス

＞更新履歴
__Y25 
_M05
D
5:WaveTimerManager管理クラス生成:nishibu
6:修正:nishibu
7:修正、コメント:nishibu
=====*/

// 名前空間宣言
using UnityEngine;

// クラス定義
public class CWaveTimerManager : MonoBehaviour
{
    private float m_fTimer = 0f; // 経過時間

    // ＞更新関数
    // 引数：なし   
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要: 毎フレームごとに経過時間を加算し、指定時間を過ぎたらEnemySpawnerに次のWave開始を通知する
    private void Update()
    {
        // ゲーム開始前またはすべてのWaveが終了している場合は処理しない
        if (CEnemySpawner.Instance == null || CEnemySpawner.Instance.IsWaveFinished())
            return;

        // 現在のWaveのデータ取得
        CEnemyWaveData waveData = CEnemySpawner.Instance.GetCurrentWaveData();
        if (waveData == null)
            return;

        // 経過時間を加算
        m_fTimer += Time.deltaTime;

        // Waveの持続時間を超えたら次のWaveへ
        if (m_fTimer >= waveData.m_fWaveDuration)
        {
            m_fTimer = 0f;
            CEnemySpawner.Instance.NextWave();
        }
    }
}
