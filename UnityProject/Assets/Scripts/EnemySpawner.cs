/*=====
<EnemySpawner.cs>
���쐬�ҁFNishibu

�����e
�G�𐶐�

���X�V����
__Y25 
_M04
D
18:�X�|�i�[�����쐬:nishibu
25:�X�|�i�[�쐬(����):nishibu
29:�R�����g�ǉ��A�C��:nishibu

=====*/

// ���O��Ԑ錾
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �N���X��`
[System.Serializable]
public class WaveData
{
    [Header("�G�ݒ�(���A���)")]
    [Tooltip("���̃E�F�[�u�ŏo���Gprefab")]
    public GameObject[] m_EnemyPrefabs;
}

// �N���X��`
public class EnemySpawner : MonoBehaviour
{
    // �ϐ��錾	
    [Header("�S�E�F�[�u�G�ݒ�(�ォ��Wave1,Wave2.....)")]
    [SerializeField]
    public List<WaveData> waveList = new List<WaveData>();

    [Header("�Q�[������(��)")]
    [SerializeField, Tooltip("�ŏI�E�F�[�u�܂ł̎���(��)")]
    public float m_fLastWaveTime = 10.0f;

    [Header("�G�X�|�[���Ԋu����(�b)")]
    [SerializeField, Tooltip("Wave�J�n���̓G����������^�C���Ԋu(�b)")]
    public float m_fSpawnTime = 2.0f; 

    [Header("Wave��")]
    [SerializeField, Tooltip("Wave��")]
    public int m_fWave = 5; 

    private bool m_bSpawnflg = false;  // Wave���I�����Ă��邩����
    private float m_fTimer; //�^�C�}�[
    private int m_fWaveCount = 0; // �E�F�[�u�J�E���g
    private float m_fWaveTime = 0; // 1�E�F�[�u�̎���


    private void Start()
    {
        m_fWave = waveList.Count; // Wave���J�E���g
        m_fWaveTime = m_fLastWaveTime / m_fWave * 60f; // 1�E�F�[�u�̎��Ԃ����߂�
    }

    // ���X�V�֐�
    // �����F�Ȃ�   
    // ��
    // �ߒl�F�Ȃ�
    // ��
    // �T�v: �X�|�[������
    private void Update()
    {
        // Wave5���I�������^�C�}�[���~�߂�
        if(!m_bSpawnflg)
        {
            m_fTimer += Time.deltaTime; // �^�C�}�[�X�^�[�g
        }

        // �X�|�[������
        if(m_fTimer >= m_fWaveTime)
        {
            SpawnEnemy(); // �G���X�|�[��
            m_fWaveCount += 1; // Wave�J�E���g

            // Wave5�ɂȂ�����m_bSpawnflg��true
            if (m_fWaveCount >= 6)
            {
                m_bSpawnflg = true;
            }
            m_fTimer = 0.0f; // �^�C�}�[���Z�b�g
        }
    }

    // ���G�l�~�[�X�|�[���֐�
    // �����F�Ȃ�   
    // ��
    // �ߒl�F�Ȃ�
    // ��
    //�T�v : �G�𐶐�
    void SpawnEnemy()
    {
        StartCoroutine(SpawnEnemiesWithDelay());
    }

    // ���G�E�F�[�u�����R���[�`���֐�
    // �����F�Ȃ�
    // ��
    // �ߒl�FIEnumerator
    // �R���[�`�����g����Wave���̓G�����Ԋu�Ő���
    IEnumerator SpawnEnemiesWithDelay()
    {
        // ���݂̃E�F�[�u�ԍ�
        int m_iWaveIndex = m_fWaveCount;

        // �L���ȃE�F�[�u�ԍ����`�F�b�N
        if (m_iWaveIndex >= 0 && m_iWaveIndex < waveList.Count)
        {
            // ���݂̃E�F�[�u�ɑΉ�����G�v���n�u�z����擾
            GameObject[] currentWave = waveList[m_iWaveIndex].m_EnemyPrefabs;
            for (int j = 0; j < currentWave.Length; j++)
            {
                // �G���X�|�[��
                Instantiate(currentWave[j], transform.position, Quaternion.identity);

                // ���̃X�|�[���܂őҋ@
                yield return new WaitForSeconds(m_fSpawnTime);
            }
        }
    }
}
