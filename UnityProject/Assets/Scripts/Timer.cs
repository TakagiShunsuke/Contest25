/*=====
<Timer.cs>	// �X�N���v�g��
���쐬�ҁFokugami

�����e
__Y25
_M05
D
23:�E�F�[�u�ƃ^�C�}�[�̃v���O�����̊�b���쐬:okugami
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
        waveData = spawner.GetCurrentWaveData();        //���݂̃E�F�[�u�̃f�[�^�̊i�[
        m_limitTime = waveData.m_fWaveDuration;         //���Ԑ����̏�����
        m_timer.text = m_limitTime.ToString("F0");      //���Ԑ�����UI�ɔ��f
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        wtManager.GetWaveCount+=GetWaveCount;
        m_turnCount = spawner.GetCurrentWaveCount();    //�Q�[���̃E�F�[�u�����i�[
        m_turn.text = m_turnCount.ToString("F0");       //�E�F�[�u����UI�ɔ��f
        waveData = spawner.GetCurrentWaveData();        //�����E�F�[�u�̃f�[�^�̊i�[
        m_limitTime = waveData.m_fWaveDuration;         //���Ԑ����̏�����
        m_timer.text = m_limitTime.ToString("F0");      //���Ԑ�����UI�ɔ��f
    }

    // Update is called once per frame
    void Update()
    {
        //���Ԑ����𒴂������̏���
        if (m_limitTime - wtManager.GetTimer() > 0)
        {
            m_timer.text = (m_limitTime - wtManager.GetTimer()).ToString("F0");
        }
    }
}
