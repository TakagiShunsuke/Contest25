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
18:�R�����g�C��:nishibu
=====*/

// ���O��Ԑ錾
using UnityEngine;

// �N���X��`
public class CWaveTimerManager : MonoBehaviour
{
    private float m_fTimer = 0f; // �o�ߎ���


    /// <summary>
	/// -�X�V�֐�
	/// <para>���t���[�����ƂɌo�ߎ��Ԃ����Z���A�w�莞�Ԃ��߂�����EnemySpawner�Ɏ���Wave�J�n��ʒm����</para>
	/// </summary>
    private void Update()
    {
        // �Q�[���J�n�O�܂��͂��ׂĂ�Wave���I�����Ă���ꍇ�͏������Ȃ�
        if (CEnemySpawner.Instance == null || CEnemySpawner.Instance.IsWaveFinished())
            return;

        // ���݂�Wave�̃f�[�^�擾
        CEnemyWaveData _WaveData = CEnemySpawner.Instance.GetCurrentWaveData();
        if (_WaveData == null)
            return;

        // �o�ߎ��Ԃ����Z
        m_fTimer += Time.deltaTime;

        // Wave�̎������Ԃ𒴂����玟��Wave��
        if (m_fTimer >= _WaveData.m_fWaveDuration)
        {
            m_fTimer = 0f;
            CEnemySpawner.Instance.NextWave();
        }
    }
}
