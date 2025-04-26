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

=====*/using UnityEngine;
using UnityEngine.SceneManagement;

public class CEffectRenderer : MonoBehaviour
{
    [Header("�}�e���A���ݒ�")]
    [SerializeField, Tooltip("�}�e���A��")] private Material material;

    private const int m_nMaxSphereCount = 256; // �����Ɉ�����ő勅���i�V�F�[�_�[�̔z�񐧌��ɍ��킹�āj
    private Vector4[] Spheres = new Vector4[m_nMaxSphereCount]; // �V�F�[�_�[�ɓn���u�ʒu�{���a�v�̔z��
    private SphereCollider[] Colliders; // �q�I�u�W�F�N�g�ɂ��� SphereCollider ���擾���邽�߂̔z��

     // ���������֐�
    // �����F�Ȃ�
    // ��
    // �ߒl�F�Ȃ�
    // ��
    // �T�v�F����������
    void Start()
    {

        // ���񂾂��R���C�_�[���擾���Ă���
        Colliders = GetComponentsInChildren<SphereCollider>();

        // ���̐����}�e���A���ɓn��
        material.SetInt("_nSphereCount", Colliders.Length);
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
        for (int i = 0; i < Colliders.Length; i++)
        {
            var col = Colliders[i];
            var t = col.transform;

            // ���S�ʒu�i���[���h���W�j
            Vector3 center = t.position;

            // ���ۂ̔��a�i�X�P�[�����ς���Ă���l���j
            float radius = t.lossyScale.x * col.radius;

            // Vector4 �Ŋi�[�ix,y,z = ���S�ʒu�Aw = ���a�j
            Spheres[i] = new Vector4(center.x, center.y, center.z, radius);
        }

        // �V�F�[�_�[�Ɍ��݂̋��̐��Ɣz��𑗐M
        material.SetInt("_nSphereCount", Colliders.Length);
        material.SetVectorArray("_fSpheres", Spheres);

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
        for (int i = 0; i < Spheres.Length; i++)
        {
            Spheres[i] = Vector4.zero;
        }

        // Shader �֔��f�i0�ɂ���j
        material.SetInt("_nSphereCount", 0);
        material.SetVectorArray("_fSpheres", Spheres);
    }
}
