/*=====
<SpawnPoint.cs>
���쐬�ҁFNishibu

�����e
�G�̃X�|�[���n�_���Ǘ�����N���X

���X�V����
__Y25 
_M05
D
5:Spawner�Ǘ��N���X����:nishibu
6:�C��:nishibu
7:�C���A�R�����g:nishibu
8:EnemyWaveData�ƘA�g�ł���悤�ɕύX:nishibu
=====*/

// ���O��Ԑ錾
using UnityEngine;
using System.Collections.Generic;

// �N���X��`
public class CSpawnPoint : MonoBehaviour
{
    [Header("�X�|�[���|�C���gID")]
    [Tooltip("���̃X�|�[���|�C���g��ID")]
    public int m_nPointID;


    // ���������֐�
    // �����F�Ȃ�
    // ��
    // �ߒl�F�Ȃ�
    // ��
    // �T�v�F�V�[���J�n���Ɏ��g���X�|�[���|�C���g�Ƃ���EnemySpawner�ɓo�^����
    private void Start()
    {
        CEnemySpawner.Instance.RegisterSpawnPoint(this);
    }

    // ���j���֐�
    // �����F�Ȃ�
    // ��
    // �ߒl�F�Ȃ�
    // ��
    // �T�v�F�V�[���I�����ȂǂɎ��g��EnemySpawner�̃��X�g�����������
    private void OnDestroy()
    {
        CEnemySpawner.Instance.UnregisterSpawnPoint(this);
    }
}
