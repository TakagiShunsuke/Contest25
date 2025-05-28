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
23�F�G�t�F�N�g�N���A�����s��C���Ftei

=====*/

// ���O��Ԑ錾
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Unity.VisualScripting;

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

    // === �U��΂鋅�̂̏�ԊǗ� ===
    // ����1���̃f�[�^���i�[����N���X�i�ʒu�E���x�E�����X�P�[���Ȃǁj
    private class CSphereData
    {
        public Transform tf;          // ���̂�Transform
        public Vector3 velocity;      // �����U���̑��x
        public Vector3 initialScale;  // �ŏ��̑傫���i�k���Ɏg���j
    }

    [Header("�v���n�u�����������_�������p�ϐ�")]
    [SerializeField, Tooltip("�G�t�F�N�g�v���n�u")] private GameObject m_SpherePrefab;
    [SerializeField, Tooltip("���̍ŏ���")] private int m_nMinSpheres = 5;
    [SerializeField, Tooltip("���̍ő吔")] private int m_nMaxSpheres = 10;
    [SerializeField, Tooltip("���ރX�s�[�h")] private float m_fFallSpeed = 0.5f;
    [SerializeField, Tooltip("�k���䗦")] private float m_fShrinkAmount = 0.3f;
    [SerializeField, Tooltip("�����x�̑傫��")] private float m_fInitialSpeed = 1.5f;
    [SerializeField, Tooltip("�����̋���")] private float m_fDrag = 2.0f;
    [SerializeField, Tooltip("�G�t�F�N�g��������")] private float m_fLifeTime = 5.0f;
    [SerializeField, Tooltip("�����������̂̑傫��(���a)")] private float m_SpawnRadius = 0.5f;
    [SerializeField, Tooltip("�����������̂̑傫���̃����_���X�P�[���ŏ��l")] private float m_MinScale = 0.2f;
    [SerializeField, Tooltip("�����������̂̑傫���̃����_���X�P�[���ő�l")] private float m_MaxScale = 0.6f;

    private List<CSphereData> m_SphereList = new();   // ���̃g�����X�t�H�[���i�[���X�g
    private float m_fElapsedTime = 0.0f;            // �o�ߎ���


    // ���������֐�
    // �����F�Ȃ�
    // ��
    // �ߒl�F�Ȃ�
    // ��
    // �T�v�F����������
    private void Start()
    {
        if (m_SpherePrefab == null)
        {
            Debug.LogError("SpherePrefab ���ݒ肳��Ă��܂���I");
            return;
        }

        if (m_SpherePrefab.GetComponent<CEffectRenderer>() != null)
        {
            Debug.LogError("SpherePrefab �� CEffectRenderer ���܂܂�Ă���A�������[�v�ɂȂ�܂��I");
            return;
        }

        //�v���n�u���̐���������
        int count = Random.Range(m_nMinSpheres, m_nMaxSpheres + 1);

        for (int i = 0; i < count; i++)
        {
            // �v���n�u���狅�̐����i�e�͎����j
            GameObject sphere = Instantiate(m_SpherePrefab, transform);

            // �� �U��΂�����Ƀ����_���z�u�i���a�͈͓��j
            Vector3 offset = Random.insideUnitSphere * m_SpawnRadius;
            sphere.transform.localPosition = offset;

            // �� �傫�������_��
            float scale = Random.Range(m_MinScale, m_MaxScale);
            sphere.transform.localScale = Vector3.one * scale;

            // �� �����_�������x�iXZ���S + ����Y�����j
            Vector3 velocity = (Random.insideUnitSphere + Vector3.up * 0.5f).normalized * m_fInitialSpeed;

            // �f�[�^�ۑ�
            m_SphereList.Add(new CSphereData
            {
                tf = sphere.transform,
                velocity = velocity,
                initialScale = sphere.transform.localScale
            });
        }

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

    // ���X�V�֐�
    // �����F�Ȃ�
    // ��
    // �ߒl�F�Ȃ�
    // ��
    // �T�v�F�X�V����
    private void Update()
    {
        m_fElapsedTime += Time.deltaTime;

        // ���Ԃɉ����ďk���ʂ��v�Z�i0�`1�j
        float shrinkT = Mathf.Clamp01(m_fElapsedTime / m_fLifeTime);

        foreach (var data in m_SphereList)
        {
            Transform tf = data.tf;

            // === �@ �U��΂铮���i�����x����j===
            if (data.velocity.magnitude > 0.01f)
            {
                // ���x�Ɋ�Â��Ĉړ�
                tf.localPosition += data.velocity * Time.deltaTime;

                // ���C�i�����j
                data.velocity = Vector3.Lerp(data.velocity, Vector3.zero, m_fDrag * Time.deltaTime);
            }
            else
            {
                // === �A ��~��ɒ��ޏ��� ===
                Vector3 pos = tf.localPosition;
                pos.y -= m_fFallSpeed * Time.deltaTime;
                tf.localPosition = pos;
            }

            // === �B ���Ԃŏ������k�� ===
            float scaleFactor = Mathf.Lerp(1f, 1f - m_fShrinkAmount, shrinkT);
            tf.localScale = data.initialScale * scaleFactor;
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
    }
}
