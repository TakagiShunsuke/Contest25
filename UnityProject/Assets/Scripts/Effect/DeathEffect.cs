/*=====
<CDeathEffect.cs>
���쐬�ҁFtei

�����e
���S�G�t�F�N�g�̐ݒ�A����

�����ӎ���

���X�V����
__Y25
_M04
D
25�F�v���O�����쐬�Ftei
26�F�������A�R�����g�ǉ��Ftei
30�F�G�t�F�N�g�Đ����C���Ftei

_M05
D
01�F�X�N���v�g���A�ϐ����C���Ftei

=====*/
using UnityEngine;

public class CDeathEffect : MonoBehaviour
{
    [Header("�ݒ�")]
    [SerializeField, Tooltip("�\������鎞��")] private float m_fLifeTime = 5f;
    [SerializeField, Tooltip("�t�F�[�h����")] private float m_fFadeDuration = 2f;
    [SerializeField, Tooltip("�����A���t�@")] private float m_fStartAlpha = 0.9f;
    [SerializeField, Tooltip("�t�F�[�h�Ώۂ̃}�e���A��")] private Material FadeMaterial;

    private float m_fTimecount { get; set; } = 0f;    // �G�t�F�N�g�\�����ԃJ�E���^�[
    private float m_fFadeCount { get; set; } = 0f;    // �G�t�F�N�g�t�F�C�h���ԃJ�E���^�[
    private bool m_bIsFading { get; set; } = false;   // �t�F�C�h�t���O
    private CEffectRenderer EffectRenderer { get; set; }    // �G�t�F�N�g�N���X�Q��      

    // ���������֐�
    // �����F�Ȃ�
    // ��
    // �ߒl�F�Ȃ�
    // ��
    // �T�v�F����������

    void Start()
    {
        // Material���C���X�^���X�����ēƗ�������
        var renderer = GetComponentInChildren<Renderer>();

        if (renderer != null)
        {
            FadeMaterial = new Material(renderer.sharedMaterial); // �� sharedMaterial�𕡐��I
            renderer.material = FadeMaterial;
        }

        // ���l�ݒ�
        if (FadeMaterial != null)
        {
            Color color = FadeMaterial.color;
            color.a = m_fStartAlpha;
            FadeMaterial.color = color;
        }
        else
        {
            Debug.LogWarning("[CDeadEffect] FadeMaterial ���ݒ肳��Ă��܂���I");
        }

        // �e�p�����[�^������
        m_fTimecount = 0f;
        m_fFadeDuration = 0f;
        m_bIsFading = false;
        // �����_���[�擾
        EffectRenderer = GetComponent<CEffectRenderer>();
    }

    // ���X�V�֐�
    // �����F�Ȃ�
    // ��
    // �ߒl�F�Ȃ�
    // ��
    // �T�v�F�X�V����

    void Update()
    {
        // �t�F�C�h�v�Z
        m_fTimecount += Time.deltaTime;

        if (m_fTimecount >= m_fLifeTime && !m_bIsFading)
        {
            m_bIsFading = true;
        }

        // ���l���v�Z���āA�t�F�C�h�����ɔ��f
        if (m_bIsFading)
        {
            // ���l�����Ԃɂ���Ĕ����Ȃ�
            m_fFadeCount += Time.deltaTime;
            float t = Mathf.Clamp01(m_fFadeCount / m_fFadeDuration);
            float currentAlpha = Mathf.Lerp(m_fStartAlpha, 0f, t);

            if (FadeMaterial != null)
            {
                Color color = FadeMaterial.color;
                color.a = currentAlpha;
                FadeMaterial.color = color;
            }
            // ���l��0�ɂȂ�����G�t�F�N�g�̕`��ƃI�u�W�F�N�g���̂��폜
            if (t >= 1f)
            {
                if (EffectRenderer != null)
                {
                    EffectRenderer.ClearEffect();
                }

                Destroy(gameObject);
            }
        }
    }
}
