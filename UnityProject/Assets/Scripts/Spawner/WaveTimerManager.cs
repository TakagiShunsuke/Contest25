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
    [Header("1ウェーブ時間(分)")]
    [Tooltip("ウェーブ時間（分単位）")]
    [SerializeField] private float m_fWaveTime = 2f;

    private float m_fTimer = 0f; // 経過時間

    // ＞更新関数
    // 引数：なし   
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要: 毎フレームごとに経過時間を加算し、指定時間を過ぎたらEnemySpawnerに次のWave開始を通知する
    private void Update()
    {
        m_fTimer += Time.deltaTime; // 毎フレーム加算

        // 指定した時間が経過したら次のWaveへ
        if (m_fTimer >= m_fWaveTime * 60f)
        {
            m_fTimer = 0f;
            CEnemySpawner.Instance.NextWave(); 
        }
    }
}
