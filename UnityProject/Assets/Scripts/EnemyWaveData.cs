/*=====
<EnemyWaveData.cs>
���쐬�ҁFNishibu

�����e
Wave���Ƃ̓G�̎�ނƍő�o�������Ǘ�����N���X

���X�V����
__Y25 
_M04
D
30:Enemy�Ǘ��N���X�쐬:nishibu
_M05
D
1:�R�����g�ǉ�:nishibu
=====*/

// ���O��Ԑ錾
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CEnemyWaveData
{
    [Header("�G�ݒ�")]
    [Tooltip("����Wave�ŏo��������G�v���n�u�̃��X�g")]
    public List<GameObject> m_EnemyPrefabs;

    [Header("�G�X�|�[���ő吔")]
    [Tooltip("����Wave�ŏ�ɏo��������G�̍ő吔")]
    public int m_nMaxEnemyCount = 5;
}