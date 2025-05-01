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

=====*/
using UnityEngine;

public class CDeathEffect : MonoBehaviour
{
    [Header("�ݒ�")]
    [SerializeField, Tooltip("�\������鎞��")] private float fLifeTime = 5f;
    [SerializeField, Tooltip("�t�F�[�h����")] private float fFadeDuration = 2f;
    [SerializeField, Tooltip("�t�F�[�h�Ώۂ̃}�e���A��")] private Material mFadeMaterial;
    [SerializeField, Tooltip("�����A���t�@")] private float fStartAlpha = 0.9f;

    private float fTimeCount { get; set; } = 0f;    // �G�t�F�N�g�\�����ԃJ�E���^�[
    private float fFadeCount { get; set; } = 0f;    // �G�t�F�N�g�t�F�C�h���ԃJ�E���^�[
    private bool bIsFading { get; set; } = false;   // �t�F�C�h�t���O
    private CEffectRenderer effectRenderer { get; set; }    // �G�t�F�N�g�N���X�Q��      

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
            mFadeMaterial = new Material(renderer.sharedMaterial); // �� sharedMaterial�𕡐��I
            renderer.material = mFadeMaterial;
        }

        // ���l�ݒ�
        if (mFadeMaterial != null)
        {
            Color color = mFadeMaterial.color;
            color.a = fStartAlpha;
            mFadeMaterial.color = color;
        }
        else
        {
            Debug.LogWarning("[CDeadEffect] mFadeMaterial ���ݒ肳��Ă��܂���I");
        }

        // �e�p�����[�^������
        fTimeCount = 0f;
        fFadeDuration = 0f;
        bIsFading = false;
        // �����_���[�擾
        effectRenderer = GetComponent<CEffectRenderer>();
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
        fTimeCount += Time.deltaTime;

        if (fTimeCount >= fLifeTime && !bIsFading)
        {
            bIsFading = true;
        }

        // ���l���v�Z���āA�t�F�C�h�����ɔ��f
        if (bIsFading)
        {
            // ���l�����Ԃɂ���Ĕ����Ȃ�
            fFadeCount += Time.deltaTime;
            float t = Mathf.Clamp01(fFadeCount / fFadeDuration);
            float currentAlpha = Mathf.Lerp(fStartAlpha, 0f, t);

            if (mFadeMaterial != null)
            {
                Color color = mFadeMaterial.color;
                color.a = currentAlpha;
                mFadeMaterial.color = color;
            }
            // ���l��0�ɂȂ�����G�t�F�N�g�̕`��ƃI�u�W�F�N�g���̂��폜
            if (t >= 1f)
            {
                if (effectRenderer != null)
                {
                    effectRenderer.ClearEffect();
                }

                Destroy(gameObject);
            }
        }
    }
}
