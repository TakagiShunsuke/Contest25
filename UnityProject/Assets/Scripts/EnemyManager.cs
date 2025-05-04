/*=====
<EnemyManager.cs>
���쐬�ҁFNishibu

�����e
���݂�Wave�ɉ����ēG�𐶐��A��[���鏈�����s���N���X

���X�V����
__Y25
_04
D
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

public class CEnemyManager : MonoBehaviour
{
    private CEnemySpawner m_Spawner;                        // Wave�̐i�s���Ǘ�����N���X�ւ̎Q��
    private List<GameObject> m_ActiveEnemies = new List<GameObject>();       // ���݃A�N�e�B�u�ȓG�̃��X�g
    private bool m_bHasSpawnedInitial = false;               // ����X�|�[�����s�������ǂ���
    private bool m_bIsSpawning = false;                      // �R���[�`���ɂ��X�|�[�����������ǂ���

    [Header("�X�|�[���Ԋu�i�b�j")]
    [Tooltip("�G�̃X�|�[���Ԋu(�b)")]
    [SerializeField]
    private float m_fSpawnDelay = 3.0f;                       // �G��1�̂��o���Ԋu


    // ���������֐�
    // �����F�Ȃ�   
    // ��
    // �ߒl�F�Ȃ�
    // ��
    // �T�v: EnemySpawner�N���X���擾����
    private void Start()
    {
        m_Spawner = GetComponent<CEnemySpawner>();          // ����GameObject����Spawner���擾
    }

    // ���X�V�֐�
    // �����F�Ȃ�   
    // ��
    // �ߒl�F�Ȃ�
    // ��
    // �T�v: ���݂�Wave�ɉ����ēG�����񐶐��E��[�X�|�[��������
    private void Update()
    {
        // �O��`�F�b�N�Fspawner�����݂��Ȃ��A�܂���Wave�f�[�^����Ȃ�X�L�b�v
        if (m_Spawner == null || m_Spawner.m_WaveList.Count == 0)
            return;

        // �SWave���I����Ă�����X�|�[�������𒆒f
        if (m_Spawner.m_bIsAllWaveEnd)
            return;

        // ���݂�WaveIndex���擾
        int _nWaveIndex = m_Spawner.m_nCurrentwaveIndex;

        // Index�������Ȕ͈͂�������X�L�b�v
        if (_nWaveIndex < 0 || _nWaveIndex >= m_Spawner.m_WaveList.Count)
            return;

        // ���݂�Wave�f�[�^���擾
        CEnemyWaveData m_WaveData = m_Spawner.m_WaveList[_nWaveIndex];

        // ���񂾓G(null)�����X�g����폜
        m_ActiveEnemies.RemoveAll(e => e == null);

        // ����X�|�[�������i1Wave���Ƃ�1�񂾂��j
        if (!m_bHasSpawnedInitial)
        {
            StartCoroutine(SpawnEnemiesWithDelay(m_WaveData, m_WaveData.m_nMaxEnemyCount));
            m_bHasSpawnedInitial = true;
        }

        // ����Ȃ��G���[�i����ȍ~�A�G���������Ƃ��j
        int _nToSpawn = m_WaveData.m_nMaxEnemyCount - m_ActiveEnemies.Count;
        if (_nToSpawn > 0 && !m_bIsSpawning)
        {
            StartCoroutine(SpawnEnemiesWithDelay(m_WaveData, _nToSpawn));
        }
    }

    // �������̂̃G�l�~�[�X�|�[���R���[�`���֐�
    // ����1�F_WaveData�F���݂�Wave�̓G�f�[�^
    // ����2�F_nCount�F�X�|�[������G�̐�
    // ��
    // �ߒl�FIEnumerator
    // ��
    // �T�v:�R���[�`�����g���A���Ԋu���ƂɎw�萔�̓G���X�|�[������
    private IEnumerator SpawnEnemiesWithDelay(CEnemyWaveData _WaveData, int _nCount)
    {
        m_bIsSpawning = true; // �X�|�[�����t���O��True
        for (int i = 0; i < _nCount; i++)
        {
            SpawnEnemy(_WaveData); // 1�̂��X�|�[��
            yield return new WaitForSeconds(m_fSpawnDelay); // �w��Ԋu�����ҋ@
        }
        m_bIsSpawning = false; // �X�|�[���I��
    }

    // ��1�̃G�l�~�[�X�|�[���֐�
    // ����1�F_WaveData�F���݂�Wave�̓G�f�[�^   
    // ��
    // �ߒl�F�Ȃ�
    // ��
    // �T�v: Wave�f�[�^���烉���_���ɓG��1�̃X�|�[������
    private void SpawnEnemy(CEnemyWaveData _WaveData)
    {
        // �v���n�u���ݒ肳��Ă��Ȃ��ꍇ�̓X�L�b�v
        if (_WaveData.m_EnemyPrefabs.Count == 0) return;

        // �����_����1�̑I��
        int _Rand = Random.Range(0, _WaveData.m_EnemyPrefabs.Count);
        GameObject prefab = _WaveData.m_EnemyPrefabs[_Rand];

        // �X�|�[���ʒu
        GameObject _Enemy = Instantiate(prefab, transform.position, Quaternion.identity);
        m_ActiveEnemies.Add(_Enemy); // ���X�g�ɒǉ�
    }

    // ���X�|�[���t���O���Z�b�g�֐�
    // �����F�Ȃ�   
    // ��
    // �ߒl�F�Ȃ�
    // ��
    // �T�v: �����X�|�[���t���O�����Z�b�g���A�ăX�|�[�����\�ɂ���
    public void ResetInitialSpawnFlag()
    {
        m_bHasSpawnedInitial = false;
    }
}
