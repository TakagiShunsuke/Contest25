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

=====*/

// ���O��Ԑ錾
using UnityEngine;

// �N���X��`
public class EnemySpawner : MonoBehaviour
{
    // �ϐ��錾	
    [Header("�G�ݒ�")] 
    [SerializeField, Tooltip("�G")]
    public GameObject m_EnemyPrefab; 

    [Header("����X�|�[���J�n����")]
    [SerializeField, Tooltip("����X�|�[���J�n����")]
    public float m_fSpawn = 5.0f; 

    [Header("�Q��ڈȍ~�X�|�[���Ԋu")]
    [SerializeField, Tooltip("�Q��ڈȍ~�X�|�[���Ԋu")]
    public float m_fSpawnInterval = 20.0f; 

    private bool m_bSpawnflg = false;  // 1��ڂ̃X�|�[������
    private float m_fTimer; //�^�C�}�[


    // ���X�V�֐�
    // �����F�Ȃ�   
    // ��
    // �ߒl�F�Ȃ�
    // ��
    // �T�v: �X�|�[������
    private void Update()
    {
        // �^�C�}�[�X�^�[�g
        m_fTimer += Time.deltaTime;

        // �X�|�[������
        if (!m_bSpawnflg)
        {
            // �P��ڂ̃X�|�[��
            if (m_fTimer >= m_fSpawn)
            {
                SpawnEnemy(); // �G�l�~�[�X�|�[���֐��Ăяo��
                m_fTimer = 0.0f; // �^�C�}�[���Z�b�g
                m_bSpawnflg = true; //���ڂ̃X�|�[�����I�������true
            }
        }
        else
        {
            // �Q��ڈȍ~�̃X�|�[��
            if (m_fTimer >= m_fSpawnInterval)
            {
                SpawnEnemy(); // �G�l�~�[�X�|�[���֐��Ăяo��
                m_fTimer = 0.0f; // �^�C�}�[���Z�b�g
            }
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
        // �X�|�i�[�̈ʒu�ɓG���o��������
        Instantiate(m_EnemyPrefab, transform.position, Quaternion.identity);
    }
}
