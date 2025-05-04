/*=====
<EnemySpawner.cs>
���쐬�ҁFNishibu

�����e
�Q�[�����Ԃɉ�����Wave���Ǘ����AEnemyManager�ɒʒm����N���X

���X�V����
__Y25 
_M04
D
18:�X�|�i�[�����쐬:nishibu
25:�X�|�i�[�쐬(����):nishibu
29:�R�����g�ǉ��A�C��:nishibu
30:�d�l�쐬�A�C��1:nishibu
_M05
D
1:�d�l�쐬�A�C��2:nishibu
3:�d�l�쐬�A�C��3:nishibu
4:�R�����g�ǉ�:nishibu
=====*/

// ���O��Ԑ錾
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �N���X��`
public class CEnemySpawner : MonoBehaviour
{
    [Header("Wave�S�̂̓G�ݒ�(�ォ��Wave1,Wave2...)")]
    [Tooltip("Wave�S�̂̓G�ݒ�")]
    public List<CEnemyWaveData> m_WaveList = new List<CEnemyWaveData>(); // �SWave�̐ݒ�f�[�^

    [Header("�Q�[������(��)")]
    [Tooltip("�Q�[������")]
    public float m_fTotalGameTimeInMinutes = 10.0f;                         // �SWave�̐i�s���ԁi���j

    private float m_fWaveDuration;    // 1Wave������̌p�����ԁi�b�j
    private float m_fTimer;           // ���݂�Wave�o�ߎ���
    private int m_nCurrentWaveIndex = 0; // ���݂�Wave�ԍ��i0�n�܂�j
    private CEnemyManager m_EnemyManager; // EnemyManager�ւ̎Q��

    [HideInInspector]
    public bool m_bIsAllWaveEnd = false; // �ŏI�E�F�[�u���I������������

    public int m_nCurrentwaveIndex => m_nCurrentWaveIndex; // ���݂�Wave�ԍ������J


    // ���������֐�
    // �����F�Ȃ�   
    // ��
    // �ߒl�F�Ȃ�
    // ��
    // �T�v: Wave�̌p�����Ԃ�ݒ肵�AEnemyManager�̏����X�|�[��������
    private void Start()
    {
        m_fWaveDuration = (m_fTotalGameTimeInMinutes / m_WaveList.Count) * 60f; // Wave���Ԃ��Z�o�i�b���Z�j
        m_EnemyManager = GetComponent<CEnemyManager>();

        if (m_EnemyManager != null)
        {
            m_EnemyManager.ResetInitialSpawnFlag(); // �Q�[���J�n���ɏ����X�|�[������
        }
    }

    // ���X�V�֐�
    // �����F�Ȃ�   
    // ��
    // �ߒl�F�Ȃ�
    // ��
    // �T�v: Wave�̌o�ߎ��Ԃ��`�F�b�N���A����Wave�֐؂�ւ��鏈�����s��
    private void Update()
    {
        m_fTimer += Time.deltaTime; // �^�C�}�[�X�^�[�g

        // Wave�̐؂�ւ�����i���݂�Wave���Ԃ𒴂����玟��Wave�ցj
        if (m_fTimer >= m_fWaveDuration && m_nCurrentWaveIndex < m_WaveList.Count - 1)
        {
            m_nCurrentWaveIndex++; // Wave��i�߂�
            m_fTimer = 0.0f; // �^�C�}�[���Z�b�g

            if (m_EnemyManager != null)
            {
                m_EnemyManager.ResetInitialSpawnFlag(); // �V����Wave�J�n���ɏ���X�|�[��������
            }
        }
        else if (m_fTimer >= m_fWaveDuration && m_nCurrentWaveIndex >= m_WaveList.Count - 1)
        {
            // �ŏIWave���I�킽��True
            m_bIsAllWaveEnd = true;
        }
    }
}

