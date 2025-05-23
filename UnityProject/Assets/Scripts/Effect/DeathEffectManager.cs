/*=====
<DeathEffectManager.cs>
���쐬�ҁFtei

�����e
EffectRenderer.cs���狅�f�[�^���܂Ƃ߂āA�V�F�[�_�[�ɑ��M

�����ӎ���
���̃f�[�^�̏����256�A����������ꍇ�͒����������؂�̂Ă���̂ŁA�\�����o�O��Ǝv���܂��B

���X�V����
__Y25
_M05
D
22�F�v���O�����쐬�Ftei

=====*/

// ���O��Ԑ錾
using System.Collections.Generic;
using UnityEngine;

// �N���X��`
public class CDeathEffectManager : MonoBehaviour
{
    // �萔��`
    public static CDeathEffectManager Instance { get; private set; }    // �V���O���g���p�^�[���i�V�[����1�������݁j

    // �ϐ��錾
    [Header("���ʃ}�e���A���i�S�G�t�F�N�g�ɓK�p�j")]
    [SerializeField, Tooltip("SH_DeathEffect�p�̃}�e���A�����w��")] private Material m_SharedMaterial;

    private List<CEffectRenderer> EffectRenderers = new List<CEffectRenderer>();    // �o�^���ꂽ�S�Ă� EffectRenderer ��ێ�

    private Vector4[] SphereBuffer = new Vector4[256];  // �S�G�t�F�N�g����W�߂��X�t�B�A�����ꎞ�I�ɕۑ�����z��


    // ���������֐�
    // �����F�Ȃ�
    // ��
    // �ߒl�F�Ȃ�
    // ��
    // �T�v�F����������
    private void Awake()
    {
        // �V���O���g���C���X�^���X�̐ݒ�
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // ���o�^�֐�
    // �����FCEffectRenderer renderer�F�X�N���v�g
    // ��
    // �ߒl�F�Ȃ�
    // ��
    // �T�v�FEffectRenderer����̃f�[�^��o�^
    public void Register(CEffectRenderer renderer)
    {
        if (!EffectRenderers.Contains(renderer))
        {
            EffectRenderers.Add(renderer);
        }
    }

    // ���X�V�֐�
    // �����F�Ȃ�
    // ��
    // �ߒl�F�Ȃ�
    // ��
    // �T�v�F�X�V����
    private void Update()
    {
        int _Index = 0;

        // �o�^���ꂽ���ׂĂ�EffectRenderer����X�t�B�A�������W
        foreach (var renderer in EffectRenderers)
        {
            var spheres = renderer.GetSphereData();
            foreach (var sphere in spheres)
            {
                if (_Index >= 256) break; // 256�𒴂���ꍇ�͐؂�̂�
                SphereBuffer[_Index++] = sphere;
            }
        }

        // �}�e���A���Ɍ��݂̃X�t�B�A���Ɣz����V�F�[�_�[�ɓn��
        if (m_SharedMaterial != null)
        {
            m_SharedMaterial.SetInt("_nSphereCount", _Index);
            m_SharedMaterial.SetVectorArray("_fSpheres", SphereBuffer);
        }
    }
}
