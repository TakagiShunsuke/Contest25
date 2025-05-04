/*=====
<EffectRenderer.cs>
���쐬�ҁFtei

�����e
�G�t�F�N�g�v���n�u�̊e���̂̃v���p�e�B���擾���āA�V�F�[�_�[�ɓn��

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
        // ���񂾂��R���C�_�[���擾���Ă���
        m_Colliders = GetComponentsInChildren<SphereCollider>();

        // ���̐����}�e���A���ɓn��
        m_RenderMaterial.SetInt("_nSphereCount", m_Colliders.Length);
    }

    // ���X�V�֐�
    // �����F�Ȃ�
    // ��
    // �ߒl�F�Ȃ�
    // ��
    // �T�v�F�X�V����
    private void Update()
    {
        // ���t���[���A�e���̏����X�V
        for (int i = 0; i < m_Colliders.Length; i++)
        {
            var _Col = m_Colliders[i];
            var _T = _Col.transform;

            // ���S�ʒu�i���[���h���W�j
            Vector3 _Center = _T.position;

            // ���ۂ̔��a�i�X�P�[�����ς���Ă���l���j
            float _Radius = _T.lossyScale.x * _Col.radius;

            // Vector4 �Ŋi�[�ix,y,z = ���S�ʒu�Aw = ���a�j
            m_Spheres[i] = new Vector4(_Center.x, _Center.y, _Center.z, _Radius);
        }

        // �V�F�[�_�[�Ɍ��݂̋��̐��Ɣz��𑗐M
        m_RenderMaterial.SetInt("_nSphereCount", m_Colliders.Length);
        m_RenderMaterial.SetVectorArray("_fSpheres", m_Spheres);
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
