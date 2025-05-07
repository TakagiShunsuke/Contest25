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
    [SerializeField, Tooltip("�t�F�[�h�Ώۃ}�e���A��")]
    private Material m_OriginalMaterial;    // �}�e���A���{��
    private Material m_InstanceMaterial;    // �����̃}�e���A��

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
        if (m_OriginalMaterial != null)
        {
            // Renderer ���Ȃ��ꍇ�ł��}�e���A�����蓮�ŕ����E�ݒ�
            m_InstanceMaterial = new Material(m_OriginalMaterial);

            // �����A���t�@�ݒ�
            Color color = m_InstanceMaterial.GetColor("_Color");
            color.a = m_fStartAlpha;
            m_InstanceMaterial.SetColor("_Color", color);

            // �z��ɓ���ăt�F�[�h�����Ή�
            m_FadeMaterials = new Material[1];
            m_FadeMaterials[0] = m_InstanceMaterial;
        }
        else
        {
            // �ʏ�ʂ� Renderer ��T������
            Renderer[] _Renderers = GetComponentsInChildren<Renderer>();
            m_FadeMaterials = new Material[_Renderers.Length];

            for (int i = 0; i < _Renderers.Length; i++)
            {
                Material _NewMaterial = new Material(_Renderers[i].sharedMaterial);
                _Renderers[i].material = _NewMaterial;
                m_FadeMaterials[i] = _NewMaterial;

                Color color = _NewMaterial.GetColor("_Color");
                color.a = m_fStartAlpha;
                _NewMaterial.SetColor("_Color", color);
            }
        }

        // �����_���[�擾
        EffectRenderer = GetComponent<CEffectRenderer>();
        if (EffectRenderer == null)
        {
            Debug.LogWarning("[DeathEffect] EffectRenderer ���A�^�b�`����Ă��܂���B�G�t�F�N�g�̏I�������̓X�L�b�v����܂��B");
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
        if (m_InstanceMaterial != null)
        {
            Destroy(m_InstanceMaterial);
        }

        Destroy(gameObject);
    }
}
