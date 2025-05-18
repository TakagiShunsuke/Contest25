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
14:WaveData�̃��^�f�[�^�̏C���ɔ����C��1:nishibu
16:WaveData�̃��^�f�[�^�̏C���ɔ����C��2:nishibu
=====*/

// ���O��Ԑ錾
using UnityEngine;
using System.Collections.Generic;

// �N���X��`  
public class CSpawnPoint : MonoBehaviour
{
    [Header("���̃X�|�[���n�_���Ή�����^�O�i�����j")]
    [Tooltip("�X�|�[���n�_���Ή�����^�O��ݒ肷��")]
    public List<E_SpawnTagType> m_eMetaTags = new();


    /// <summary>
	/// -�������֐�
	/// <para>�X�|�[���|�C���g��ǉ�</para>
	/// </summary>
    private void Start()
    {
        if (CEnemySpawner.Instance != null)
            CEnemySpawner.Instance.RegisterSpawnPoint(this);
    }

    /// <summary>
	/// -�j���֐�
	/// <para>�X�|�[���|�C���g���폜</para>
	/// </summary>
    private void OnDestroy()
    {
        if (CEnemySpawner.Instance != null)
            CEnemySpawner.Instance.UnregisterSpawnPoint(this);
    }
}
