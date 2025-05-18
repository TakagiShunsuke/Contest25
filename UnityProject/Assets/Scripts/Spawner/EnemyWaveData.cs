/*=====
<EnemyWaveData.cs>
���쐬�ҁFNishibu

�����e
Wave���ƂɓG�̍ő�o�������Ǘ�����N���X

���X�V����
__Y25 
_M04
D
30:Enemy�Ǘ��N���X�쐬:nishibu
_M05
D
1:�R�����g�ǉ�:nishibu
5:�C��1:nishibu
6:�C��2;nishibu
7:�C��3�A�R�����g:nishibu
8:WaveData�œG�̍ő吔�A�X�|�i�[�ʂ̓G��ސݒ�ł���悤�ɕύX:nishibu
14:WaveData�̃��^�f�[�^�̏C��1:nishibu
16:WaveData�̃��^�f�[�^�̏C��2:nishibu
=====*/

// ���O��Ԑ錾
using System.Collections.Generic;
using UnityEngine;

// �񋓒�`
public enum E_SpawnTagType
{
    Ground, // �n��̓G
    Water,  // ���ӂ̓G
    Normal  // ���ʂ̓G
}

// �N���X��`
[CreateAssetMenu(fileName = "WaveData", menuName = "Enemy/WaveData")]
public class CEnemyWaveData : ScriptableObject
{
    [Header("�G�X�|�[���ő吔")]
    [Tooltip("����Wave�ŏo������G�̍ő吔�i�S�́j")]
    public int m_nMaxEnemyCount;

    [Header("Wave���ԁi�b�j")]
    [Tooltip("����Wave�̌p�����ԁi�b�j")]
    public float m_fWaveDuration = 120.0f; 

    [Header("�X�|�[���Ԋu")]
    [Tooltip("�G���X�|�[������Ԋu�i�b�j")]
    public float m_fSpawnInterval = 3.0f;

    [Header("����Wave�̓G�ݒ�")]
    [Tooltip("Tag�ƓG��ނ�ݒ�")]
    public List<CSpawnTagEnemyList> m_EnemyByTagList = new ();
}

// �N���X��`
[System.Serializable]
public class CSpawnTagEnemyList
{
    [Header("�^�O�ݒ�")]
    [Tooltip("�^�O�ݒ�")]
    public E_SpawnTagType m_Tag; 

    [Header("�G�ݒ�")]
    [Tooltip("�G�ݒ�")]
    public GameObject m_EnemyPrefabs; 
}

