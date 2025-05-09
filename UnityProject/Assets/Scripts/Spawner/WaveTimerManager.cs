/*=====
<WaveTimerManager.cs>
���쐬�ҁFNishibu

�����e
// Wave�̃^�C�}�[�A�i�s�^�C�~���O���Ǘ�����N���X

���X�V����
__Y25 
_M05
D
5:WaveTimerManager�Ǘ��N���X����:nishibu
6:�C��:nishibu
7:�C���A�R�����g:nishibu
=====*/

// ���O��Ԑ錾
using UnityEngine;

// �N���X��`
public class CWaveTimerManager : MonoBehaviour
{
    [Header("1�E�F�[�u����(��)")]
    [Tooltip("�E�F�[�u���ԁi���P�ʁj")]
    [SerializeField] private float m_fWaveTime = 2f;

    private float m_fTimer = 0f; // �o�ߎ���

    // ���X�V�֐�
    // �����F�Ȃ�   
    // ��
    // �ߒl�F�Ȃ�
    // ��
    // �T�v: ���t���[�����ƂɌo�ߎ��Ԃ����Z���A�w�莞�Ԃ��߂�����EnemySpawner�Ɏ���Wave�J�n��ʒm����
    private void Update()
    {
        m_fTimer += Time.deltaTime; // ���t���[�����Z

        // �w�肵�����Ԃ��o�߂����玟��Wave��
        if (m_fTimer >= m_fWaveTime * 60f)
        {
            m_fTimer = 0f;
            CEnemySpawner.Instance.NextWave(); 
        }
    }
}
