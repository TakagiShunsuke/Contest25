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
=====*/

// ���O��Ԑ錾
using System.Collections.Generic;
using UnityEngine;

// �N���X��`
[CreateAssetMenu(fileName = "WaveData", menuName = "Enemy/WaveData")]
public class CEnemyWaveData : ScriptableObject
{
    [Header("�G�X�|�[���ő吔")]
    [Tooltip("����Wave�ŏo������G�̍ő吔�i�S�́j")]
    public int m_nMaxEnemyCount;

    [Header("�X�|�i�[�ʂ̓G�ݒ�(��c�X�|�i�[A ���c�X�|�i�[B)")]
    [Tooltip("�e�X�|�[���ꏊ���Ƃ̏o���G�ݒ�")]
    public List<CSpawnPointData> m_SpawnPointDataList = new List<CSpawnPointData>();
}

// �X�|�[���|�C���g�p�̓G�v���n�u���X�g
[System.Serializable]
public class CSpawnPointData
{
    [Header("�G��ސݒ�")]
    [Tooltip("�X�|�i�[���Ƃ̓G�ݒ�")]
    public List<GameObject> m_EnemyPrefabs = new List<GameObject>();
}

