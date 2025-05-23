/*=====
<DeathEffect.cs>
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
04�F�����G�̏ꍇ�G�t�F�N�g�����ʂ��������C���A�R�[�f�B���O���[���̉����ăR�[�h�C���Ftei

=====*/

// ���O��Ԑ錾
using UnityEngine;

// �N���X��`
public class CDeathEffect : MonoBehaviour
{
    // �ϐ��錾
    [Header("�ݒ�")]
    [SerializeField, Tooltip("�\������鎞��")] private float m_fLifeTime = 5f;
    [SerializeField, Tooltip("�t�F�[�h����")] private float m_fFadeDuration = 2f;
    [SerializeField, Tooltip("�����A���t�@")] private float m_fStartAlpha = 0.9f;
    [SerializeField, Tooltip("�t�F�[�h�Ώۂ̃}�e���A���z��")] private Material[] m_FadeMaterials;

    // �v���p�e�B��`
    private float TimeCount { get; set; } = 0f;    // �G�t�F�N�g�\�����ԃJ�E���^�[
    private float FadeCount { get; set; } = 0f;    // �G�t�F�N�g�t�F�C�h���ԃJ�E���^�[
    private bool IsFading { get; set; } = false;   // �t�F�C�h�t���O
    private CEffectRenderer EffectRenderer { get; set; }    // �G�t�F�N�g�N���X�Q��      


    // ���������֐�
    // �����F�Ȃ�
    // ��
    // �ߒl�F�Ȃ�
    // ��
    // �T�v�F����������

    private void Start()
    {
        
        // �����_���[�擾�i�Ȃ��Ă��x���j
        EffectRenderer = GetComponent<CEffectRenderer>();
        if (EffectRenderer == null)
        {
            Debug.LogWarning("[DeathEffect] EffectRenderer ���A�^�b�`����Ă��܂���B");
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
        // �t�F�C�h�v�Z
        TimeCount += Time.deltaTime;

        // �\�����Ԃ��߂�����t�F�[�h�J�n
        if (TimeCount >= m_fLifeTime && !IsFading)
        {
            IsFading = true;
            FadeCount = 0f;
        }

        // �t�F�[�h������
        if (IsFading)
        {
            // �t�F�[�h���Ԃ��J�E���g
            FadeCount += Time.deltaTime;

            // �i�s�����i0�`1�j���v�Z
            float _T = Mathf.Clamp01(FadeCount / m_fFadeDuration);
            float _CurrentAlpha = Mathf.Lerp(m_fStartAlpha, 0f, _T);

            // �S�}�e���A���ɑ΂��ăA���t�@�l��K�p
            foreach (Material mat in m_FadeMaterials)
            {
                if (mat != null)
                {
                    Color color = mat.GetColor("_Color");
                    color.a = _CurrentAlpha;
                    mat.SetColor("_Color", color);
                }
            }

            // �t�F�[�h������̏���
            if (_T >= 1f)
            {
                StopEffect();
            }
        }
    }

    // ���G�t�F�N�g��~�֐�
    // �����F�Ȃ�
    // ��
    // �ߒl�F�Ȃ�
    // ��
    // �T�v�F�G�t�F�N�g���~���A�֘A���������s���č폜����

    private void StopEffect()
    {
        if (EffectRenderer != null)
        {
            EffectRenderer.ClearEffect();
        }

        // ����GameObject�i�G�t�F�N�g�j���폜
        Destroy(gameObject);
    }
}
