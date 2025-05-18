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
// �C���ӏ�: 'FirstOrDefault' ���g�p���邽�߂� 'System.Linq' ���C���|�[�g
using System.Linq;

// �����̃R�[�h�ɕύX�͕s�v�ł��B'System.Linq' ���C���|�[�g���邱�ƂŁA'FirstOrDefault' ���g�p�\�ɂȂ�܂��B
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
16:WaveData�̃��^�f�[�^�̏C���ɔ����C��1:nishibu
16:WaveData�̃��^�f�[�^�̏C���ɔ����C��2:nishibu
=====*/

// ���O��Ԑ錾
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// �N���X��`
public class CEnemySpawner : MonoBehaviour
{
    public static CEnemySpawner Instance { get; private set; }  // �C���X�^���X�擾�p�v���p�e�B

    [Header("WaveData���X�g")]
    [Tooltip("WaveData��ݒ�")]
    [SerializeField] private List<CEnemyWaveData> m_WaveDataList;

    private List<CSpawnPoint> m_SpawnPoints = new();  // �o�^�ς݃X�|�[���|�C���g
    private int m_nCurrentWave = 0; // ���݂�Wave�ԍ�


    /// <summary>
	/// -Awake�֐�
	/// <para>�V���O���g��������</para>
	/// </summary>
    private void Awake()
    {
        // �V���O���g�������`�F�b�N
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
	/// -Start�֐�
	/// <para>�X�|�i�[���[�v�J�n</para>
	/// </summary>
    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    /// <summary>
    /// -RegisterSpawnPoint�֐�
    /// <para>�X�|�[���|�C���g�o�^</para>
    /// </summary>
    /// <param name="_Point">�o�^����X�|�[���|�C���g</param>
    public void RegisterSpawnPoint(CSpawnPoint _Point)
    {
        if (!m_SpawnPoints.Contains(_Point))
            m_SpawnPoints.Add(_Point);
    }

    /// <summary>
    /// -UnregisterSpawnPoint�֐�
    /// <para>�X�|�[���|�C���g����</para>
    /// </summary>
    /// <param name="_Point">��������X�|�[���|�C���g</param>
    public void UnregisterSpawnPoint(CSpawnPoint _Point)
    {
        m_SpawnPoints.Remove(_Point);
    }

    /// <summary>
    /// -NextWave�֐�
    /// <para>����Wave��</para>
    /// </summary>
    public void NextWave()
    {
        m_nCurrentWave++;
    }

    /// <summary>
    /// -SpawnLoop�֐�
    /// <para>Wave���Ƃ̓G�X�|�[�����[�v</para>
    /// </summary>
    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            if (m_nCurrentWave >= m_WaveDataList.Count) 
                yield break;

            var _WaveData = m_WaveDataList[m_nCurrentWave];

            yield return new WaitForSeconds(_WaveData.m_fSpawnInterval);

            if (CCountEnemy.m_nValInstances < _WaveData.m_nMaxEnemyCount)
            {
                TrySpawnEnemyForAllPoints(_WaveData);
            }
        }
    }

    /// <summary>
    /// -TrySpawnEnemyForAllPoints�֐�
    /// <para>�S�X�|�[���|�C���g�����񂵁A�����ɍ����G��1�̃X�|�[��</para>
    /// </summary>
    /// <param name="_WaveData">����Wave�̃f�[�^</param>
    private void TrySpawnEnemyForAllPoints(CEnemyWaveData _WaveData)
    {
        if (m_SpawnPoints.Count == 0) 
            return; // �|�C���g�����Ȃ璆�f

        int _nStartIndex = Random.Range(0, m_SpawnPoints.Count); // �����_���J�n�C���f�b�N�X

        // �S�|�C���g������T��
        for (int i = 0; i < m_SpawnPoints.Count; i++)
        {
            int nIndex = (_nStartIndex + i) % m_SpawnPoints.Count; // �����_���ɊJ�n�����C���f�b�N�X���珇�ɒT��
            var _Point = m_SpawnPoints[nIndex]; // �X�|�[���|�C���g�擾

            // �X�|�[���|�C���g�̃��^�^�O�Ɋ�Â��ēG���X�|�[��
            List<GameObject> _MatchList = new();
            foreach (var tag in _Point.m_eMetaTags)
            {
                // WaveData�̃��^�^�O�Ɋ�Â��ēG���X�|�[��
                var match = _WaveData.m_EnemyByTagList.FirstOrDefault(e => e.m_Tag == tag);
                if (match != null)
                {
                    _MatchList.Add(match.m_EnemyPrefabs);
                }
            }

            // �X�|�[������G�������Ȃ玟�̃|�C���g��
            if (_MatchList.Count == 0) 
                continue; 

            // �X�|�[������G�����������烉���_����1�̃X�|�[��
            GameObject _Selected = _MatchList[Random.Range(0, _MatchList.Count)];
            Instantiate(_Selected, _Point.transform.position, Quaternion.identity);
            break; // 1�̃X�|�[�������甲����
        }
    }

    /// <summary>
    /// -GetCurrentWaveData�֐�
    /// <para>����Wave�̃f�[�^�擾</para>
    /// </summary>
    /// <returns>����Wave�f�[�^</returns>
    public CEnemyWaveData GetCurrentWaveData()
    {
        // ����Wave�̃f�[�^���擾
        if (m_nCurrentWave < m_WaveDataList.Count)
            return m_WaveDataList[m_nCurrentWave];
        return null;
    }

    /// <summary>
    /// -IsWaveFinished�֐�
    /// <para>�SWave�I������</para>
    /// </summary>
    /// <returns>true = �I��</returns>
    public bool IsWaveFinished()
    {
        return m_nCurrentWave >= m_WaveDataList.Count;
    }
}



