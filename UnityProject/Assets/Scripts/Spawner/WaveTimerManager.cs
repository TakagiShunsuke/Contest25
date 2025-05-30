/*=====
<WaveTimerManager.cs>
└作成者：Nishibu

＞内容
 Waveのタイマー、進行タイミングを管理するクラス

＞更新履歴
__Y25
_M05
D
5:WaveTimerManager管理クラス生成:nishibu
6:修正:nishibu
7:修正、コメント:nishibu
18:コメント修正:nishibu
21:コメント微修正:takagi
28:経過時間のゲット関数の作成
=====*/

// 名前空間宣言
using UnityEngine;

// クラス定義
public class CWaveTimerManager : MonoBehaviour
{
    // イベント定義
    public delegate void UpdateWaveEvent();  // イベント用の関数定義
    public event UpdateWaveEvent GetWaveCount; // ウェーブ数カウント
    public delegate void FinishedFinalWaveEvent();  // イベント用の関数定義
	public event FinishedFinalWaveEvent OnFinishedFinalWave;	// 最終ウェーブ終了時イベント

    // 変数宣言
    private float m_fTimer = 0f;	// 経過時間


	/// <summary>
	/// -更新関数
	/// <para>毎フレームごとに経過時間を加算し、指定時間を過ぎたらEnemySpawnerに次のWave開始を通知する</para>
	/// </summary>
	private void Update()
	{
		// ゲーム開始前またはすべてのWaveが終了している場合は処理しない
		//if (CEnemySpawner.Instance == null || CEnemySpawner.Instance.IsWaveFinished())
		if (CEnemySpawner.Instance == null)
		{
			return;
		}

		// 最終ウェーブ完了時処理
		if (CEnemySpawner.Instance.IsWaveFinished())
		{
			OnFinishedFinalWave.Invoke();
			return;
		}


		// 現在のWaveのデータ取得
		CEnemyWaveData _WaveData = CEnemySpawner.Instance.GetCurrentWaveData();
		if (_WaveData == null)
			return;

		// 経過時間を加算
		m_fTimer += Time.deltaTime;

		// Waveの持続時間を超えたら次のWaveへ
		if (m_fTimer >= _WaveData.m_fWaveDuration)
		{
			m_fTimer = 0f;
			CEnemySpawner.Instance.NextWave();
            GetWaveCount.Invoke();
        }
	}

    /// <summary>
    /// -GetTimer関数
    /// <para>現在の経過時間の取得</para>
    /// </summary>
    /// <returns>現在の経過時間</returns>
    public float GetTimer()
	{ 
		return m_fTimer; 
	}
}