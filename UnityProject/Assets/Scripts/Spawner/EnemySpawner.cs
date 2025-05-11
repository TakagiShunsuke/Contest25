/*=====
<EnemySpawner.cs>
���쐬�ҁFNishibu

�����e
�G���X�|�[�����AWave���Ƃ̓G���������Ǘ�����N���X

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
5:�C��1:nishibu
6:�C��2:nishibu
7:�C��3�A�R�����g:nishibu
8:�C��3:nishibu
=====*/

// ���O��Ԑ錾
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �N���X��`
public class CEnemySpawner : MonoBehaviour
{
    public static CEnemySpawner Instance { get; private set; }

    [Header("Wave�f�[�^")]
    [Tooltip("�eWave�̍ő�G���ƃX�|�[���ݒ�������X�g")]
    [SerializeField] private List<CEnemyWaveData> m_WaveDataList;

    private List<CSpawnPoint> m_SpawnPoints = new List<CSpawnPoint>(); // �o�^���ꂽ���ׂẴX�|�[���|�C���g
    private int m_CurrentWave = 0; // ���݂�Wave�ԍ�


    // �V���O���g���C���X�^���X������
    private void Awake()
    {
        // �܂��C���X�^���X���Ȃ���Ύ�����ݒ�
        if (Instance == null)
            Instance = this;
        else
        // ���łɑ��݂���ꍇ�͔j��
            Destroy(gameObject);
    }

    // �Q�[���J�n���ɃX�|�[���������J�n
    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    // �X�|�[���|�C���g�̓o�^
    public void RegisterSpawnPoint(CSpawnPoint _Point)
    {
        // �܂��o�^����Ă��Ȃ��ꍇ�̂ݒǉ�
        if (!m_SpawnPoints.Contains(_Point))
        {
            m_SpawnPoints.Add(_Point);
        }
    }

    // �X�|�[���|�C���g�̓o�^����
    public void UnregisterSpawnPoint(CSpawnPoint _Point)
    {
        m_SpawnPoints.Remove(_Point);
    }

    // Wave��i�߂鏈���iCWaveTimerManager���Ăяo���j
    public void NextWave()
    {
        m_CurrentWave++;
    }

    // �G�̃X�|�[�������Ԋu�ŌJ��Ԃ�����
    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            // �SWave�I�����Ă�����R���[�`���I��
            if (m_CurrentWave >= m_WaveDataList.Count) yield break;

            // ���݂�Wave�̃f�[�^���擾
            CEnemyWaveData m_WaveData = m_WaveDataList[m_CurrentWave];

            // �X�|�[���Ԋu���ҋ@
            yield return new WaitForSeconds(m_WaveData.m_SpawnInterval);

            // �G�̍ő吔�����̂Ƃ��X�|�[��
            if (CCountEnemy.m_nValInstances < m_WaveData.m_nMaxEnemyCount)
            {
                TrySpawnEnemyForAllPoints(m_WaveData);
            }
        }
    }

    // �S�X�|�[���|�C���g�ɑ΂���1�̃X�|�[�������݂鏈��
    private void TrySpawnEnemyForAllPoints(CEnemyWaveData _WaveData)
    {
        // �X�|�[���|�C���g���o�^����Ă��Ȃ��ꍇ�͏����I��
        if (m_SpawnPoints.Count == 0) return;

        // �����_���ȊJ�n�C���f�b�N�X���擾�i�X�|�[���̕΂��h���j
        int m_StartIndex = Random.Range(0, m_SpawnPoints.Count);

        // �X�|�[���|�C���gID��WaveData�Ɋ܂܂�Ă��邩�m�F
        for (int i = 0; i < m_SpawnPoints.Count; i++)
        {
            int _nIndex = (m_StartIndex + i) % m_SpawnPoints.Count;
            CSpawnPoint m_Point = m_SpawnPoints[_nIndex];

            // PointID��WaveData���ɑ��݂��Ȃ���΃X�L�b�v
            if (m_Point.m_nPointID >= _WaveData.m_SpawnPointDataList.Count) continue;

            // �Y������X�|�[���|�C���g�̓G�v���n�u���X�g���擾
            List<GameObject> m_Prefabs = _WaveData.m_SpawnPointDataList[m_Point.m_nPointID].m_EnemyPrefabs;

            // ���X�g����Ȃ�X�L�b�v
            if (m_Prefabs.Count == 0) continue;

            // �����_���ɓG�v���n�u��1�I��ŃX�|�[��
            GameObject m_Prefab = m_Prefabs[Random.Range(0, m_Prefabs.Count)];
            Instantiate(m_Prefab, m_Point.transform.position, Quaternion.identity);
            break; // 1�̃X�|�[��������I��
        }
    }

    // ���݂�Wave�̃f�[�^���擾
    public CEnemyWaveData GetCurrentWaveData()
    {
        if (m_CurrentWave < m_WaveDataList.Count)
            return m_WaveDataList[m_CurrentWave];
        return null;
    }

    // �SWave�I�����Ă��邩�ǂ���
    public bool IsWaveFinished()
    {
        return m_CurrentWave >= m_WaveDataList.Count;
    }
}



