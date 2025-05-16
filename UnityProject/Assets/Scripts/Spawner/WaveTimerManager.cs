/*=====
<WaveTimerManager.cs>
���쐬�ҁFNishibu

�����e
// Wave�̃^�C�}�[�A�i�s�^�C�~���O���Ǘ�����N���X

���X�V����
__Y25 
_M05
D
5:WaveTimerManager�Ǘ��N���X����:nishibu
6:�C��:nishibu
7:�C���A�R�����g:nishibu
=====*/

// ���O��Ԑ錾
using UnityEngine;

// �N���X��`
public class CWaveTimerManager : MonoBehaviour
{
    private float m_fTimer = 0f; // �o�ߎ���

    // ���X�V�֐�
    // �����F�Ȃ�   
    // ��
    // �ߒl�F�Ȃ�
    // ��
    // �T�v: ���t���[�����ƂɌo�ߎ��Ԃ����Z���A�w�莞�Ԃ��߂�����EnemySpawner�Ɏ���Wave�J�n��ʒm����
    private void Update()
    {
        // �Q�[���J�n�O�܂��͂��ׂĂ�Wave���I�����Ă���ꍇ�͏������Ȃ�
        if (CEnemySpawner.Instance == null || CEnemySpawner.Instance.IsWaveFinished())
            return;

        // ���݂�Wave�̃f�[�^�擾
        CEnemyWaveData waveData = CEnemySpawner.Instance.GetCurrentWaveData();
        if (waveData == null)
            return;

        // �o�ߎ��Ԃ����Z
        m_fTimer += Time.deltaTime;

        // Wave�̎������Ԃ𒴂����玟��Wave��
        if (m_fTimer >= waveData.m_fWaveDuration)
        {
            m_fTimer = 0f;
            CEnemySpawner.Instance.NextWave();
        }
    }
}
