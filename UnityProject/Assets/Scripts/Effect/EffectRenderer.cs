/*=====
<EffectRenderer.cs>
���쐬�ҁFtei

�����e
�G�t�F�N�g�v���n�u�̊e���̂̃f�[�^���Ǘ�

�����ӎ���

���X�V����
__Y25
_M04
D
25�F�v���O�����쐬�Ftei
26�F�������ƃR�����g�ǉ��Ftei

_M05
D
01�F�X�N���v�g���A�ϐ����C���Ftei
04�F�R�[�f�B���O���[���̉����ăR�[�h�C���Ftei
22�F���̎擾��������ǁA���̂̃f�[�^���Ǘ��Ftei

=====*/

// ���O��Ԑ錾
using UnityEngine;
using UnityEngine.SceneManagement;

// �N���X��`
public class CEffectRenderer : MonoBehaviour
{
    // �萔��`
    private const int MAX_SPHERE_COUNT = 256;	// �����Ɉ�����ő勅���i�V�F�[�_�[�̔z�񐧌��ɍ��킹�āj

    // �ϐ��錾
    [Header("�}�e���A���ݒ�")]
    [SerializeField, Tooltip("�}�e���A��")] private Material m_RenderMaterial;

    private Vector4[] m_Spheres = new Vector4[MAX_SPHERE_COUNT]; // �V�F�[�_�[�ɓn���u�ʒu�{���a�v�̔z��
    private SphereCollider[] m_Colliders; // �q�I�u�W�F�N�g�ɂ��� SphereCollider ���擾���邽�߂̔z��


    // ���������֐�
    // �����F�Ȃ�
    // ��
    // �ߒl�F�Ȃ�
    // ��
    // �T�v�F����������
    private void Start()
    {
        // �q�I�u�W�F�N�g��SphereCollider���擾
        m_Colliders = GetComponentsInChildren<SphereCollider>();

        // CDeathEffectManager�Ɏ�����o�^
        if (CDeathEffectManager.Instance != null)
        {
            CDeathEffectManager.Instance.Register(this);
        }
        else
        {
            Debug.LogWarning("[EffectRenderer] DeathEffectManager���V�[����ɑ��݂��܂���B");
        }
    }

    // ���Q�b�g�֐�
    // �����F�Ȃ�
    // ��
    // �ߒl�F�Ȃ�
    // ��
    // �T�v�F���̃f�[�^���擾
    public Vector4[] GetSphereData()
    {
        // null�`�F�b�N�F�R���C�_�[���擾�܂��͋�̏ꍇ�͋�z���Ԃ�
        if (m_Colliders == null || m_Colliders.Length == 0)
        {
            Debug.LogWarning($"[EffectRenderer] SphereCollider �����ݒ�ł��i{gameObject.name}�j");
            return new Vector4[0]; // ��̔z���Ԃ�
        }

        Vector4[] result = new Vector4[m_Colliders.Length];
        
        for (int i = 0; i < m_Colliders.Length; i++)
        {
            var col = m_Colliders[i];
            
            if (col == null) continue; // �ʂ̃R���C�_�[�� null �̏ꍇ���X�L�b�v

            var pos = col.transform.position;
            var rad = col.radius * col.transform.lossyScale.x;
            result[i] = new Vector4(pos.x, pos.y, pos.z, rad);
        }
        return result;
    }

    
    // ���G�t�F�N�g�폜�֐�
    // �����F�Ȃ�
    // ��
    // �ߒl�F�Ȃ�
    // ��
    // �T�v�F�g�p�ς݂̃G�t�F�N�g�`����N���A���č폜���܂�
    public void ClearEffect()
    {
        // �z������
        for (int i = 0; i < m_Spheres.Length; i++)
        {
            m_Spheres[i] = Vector4.zero;
        }

        // Shader �֔��f�i0�ɂ���j
        m_RenderMaterial.SetInt("_nSphereCount", 0);
        m_RenderMaterial.SetVectorArray("_fSpheres", m_Spheres);
    }
}
