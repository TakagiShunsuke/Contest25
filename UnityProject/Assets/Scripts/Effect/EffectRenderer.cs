/*=====
<CEffectRenderer.cs>
���쐬�ҁFtei

�����e
�R�[�f�B���O�K����L�q

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

=====*/using UnityEngine;
using UnityEngine.SceneManagement;

public class CEffectRenderer : MonoBehaviour
{
    [Header("�}�e���A���ݒ�")]
    [SerializeField, Tooltip("�}�e���A��")] private Material RenderMaterial;

    private const int m_nMaxSphereCount = 256; // �����Ɉ�����ő勅���i�V�F�[�_�[�̔z�񐧌��ɍ��킹�āj
    private Vector4[] m_Spheres = new Vector4[m_nMaxSphereCount]; // �V�F�[�_�[�ɓn���u�ʒu�{���a�v�̔z��
    private SphereCollider[] m_Colliders; // �q�I�u�W�F�N�g�ɂ��� SphereCollider ���擾���邽�߂̔z��

     // ���������֐�
    // �����F�Ȃ�
    // ��
    // �ߒl�F�Ȃ�
    // ��
    // �T�v�F����������
    void Start()
    {

        // ���񂾂��R���C�_�[���擾���Ă���
        m_Colliders = GetComponentsInChildren<SphereCollider>();

        // ���̐����}�e���A���ɓn��
        RenderMaterial.SetInt("_nSphereCount", m_Colliders.Length);
    }

    // ���X�V�֐�
    // �����F�Ȃ�
    // ��
    // �ߒl�F�Ȃ�
    // ��
    // �T�v�F�X�V����
    void Update()
    {
        // ���t���[���A�e���̏����X�V
        for (int i = 0; i < m_Colliders.Length; i++)
        {
            var col = m_Colliders[i];
            var t = col.transform;

            // ���S�ʒu�i���[���h���W�j
            Vector3 center = t.position;

            // ���ۂ̔��a�i�X�P�[�����ς���Ă���l���j
            float radius = t.lossyScale.x * col.radius;

            // Vector4 �Ŋi�[�ix,y,z = ���S�ʒu�Aw = ���a�j
            m_Spheres[i] = new Vector4(center.x, center.y, center.z, radius);
        }

        // �V�F�[�_�[�Ɍ��݂̋��̐��Ɣz��𑗐M
        RenderMaterial.SetInt("_nSphereCount", m_Colliders.Length);
        RenderMaterial.SetVectorArray("_fSpheres", m_Spheres);

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
        RenderMaterial.SetInt("_nSphereCount", 0);
        RenderMaterial.SetVectorArray("_fSpheres", m_Spheres);
    }
}
