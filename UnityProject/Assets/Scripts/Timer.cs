/*=====
<Timer.cs>	// スクリプト名
└作成者：okugami

＞内容
__Y25
_M05
D
23:ウェーブとタイマーのプログラムの基礎を作成:okugami
=====*/

using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public CEnemySpawner spawner;
    private CEnemyWaveData waveData;
    public CWaveTimerManager wtManager;

    public TextMeshProUGUI m_timer;
    public TextMeshProUGUI m_turn;
    float m_limitTime = 0;
    int m_turnCount = 0;

    private void  GetWaveCount()
    {
        m_turnCount = spawner.GetCurrentWaveCount();
        m_turn.text = m_turnCount.ToString("F0");
        waveData = spawner.GetCurrentWaveData();        //現在のウェーブのデータの格納
        m_limitTime = waveData.m_fWaveDuration;         //時間制限の初期化
        m_timer.text = m_limitTime.ToString("F0");      //時間制限をUIに反映
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        wtManager.GetWaveCount+=GetWaveCount;
        m_turnCount = spawner.GetCurrentWaveCount();    //ゲームのウェーブ数を格納
        m_turn.text = m_turnCount.ToString("F0");       //ウェーブ数をUIに反映
        waveData = spawner.GetCurrentWaveData();        //初期ウェーブのデータの格納
        m_limitTime = waveData.m_fWaveDuration;         //時間制限の初期化
        m_timer.text = m_limitTime.ToString("F0");      //時間制限をUIに反映
    }

    // Update is called once per frame
    void Update()
    {
        //時間制限を超えた時の処理
        if (m_limitTime - wtManager.GetTimer() > 0)
        {
            m_timer.text = (m_limitTime - wtManager.GetTimer()).ToString("F0");
        }
    }
}
