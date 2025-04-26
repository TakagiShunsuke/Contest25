/*=====
<LiquidSpread.cs>
���쐬�ҁFtei

�����e
�t�̊g�U�ݒ�A����

�����ӎ���
�@�E�v�����i�[���}�e���A����������ꍇ�u�g�U�E�\���E�t�F�[�h�ݒ�v�Ɓu�����_���g�U���ݒ�v
�@�̕ϐ����l�𒲐�����΂ł��܂��B
�@�E�G�t�F�N�g�̐����ɂ��āu�ݒu�֐��v�g�p����΂ł��܂��B��F�G���S��Setup���Ăяo���B
�@�E�����蔻��̏������Ə����͉��œ���܂������A�ǉ��̏����������Ȃ��Ɠ����蔻��Ƃ��͂��܂���B

���X�V����
�@�@__Y25
�@_M04
D
18�F�v���O�����쐬�Ftei
19�F�����������A�R�����g�ǉ��Ftei
21�F�g�U�T�C�Y�Ɗg�U���x�̕ϐ����ʁX�œn���A
�@�@SpreadSpeed��SpreadDuration�̌�p�C���i��̕ϐ��ɕύX�j�Ftei
22�F�n���K���A���L�@�Ŗ����C���A�����_���g�U�p�ϐ����C���A�������d�l�ɋ߂��������֕ύX�A�e�ϐ��R�����g�ǉ��A
�@�@Start�AUpdate�֐��v���C�x�[�g���Ftei
23�F�R�[�f�B���O�K��ɏ]���C���ASetUp�֐����g��Ȃ����߃R�����g�A�E�g

=====*/

// ���O��Ԑ錾
using UnityEngine;

// �N���X��`
public class CLiquidSpread : MonoBehaviour
{
	// �ϐ��錾

	[Header("�g�U�E�\���E�t�F�[�h�ݒ�")]
	[SerializeField, Tooltip("�g�U�ɂ����鎞��")] private float m_fSpreadDuration = 1.0f;
	[SerializeField, Tooltip("�g�U��ɂ��̂܂ܕ\������鎞��")] private float m_fStayDuration = 10.0f;
	[SerializeField, Tooltip("�t�F�[�h�A�E�g����")] private float m_fFadeDuration = 1.5f;
	[SerializeField, Tooltip("�g�U�T�C�Y")] private float m_fMaxSpread = 0.4f;

	private Material matMaterial;   // �}�e���A��
	//private Collider cldHitbox;

	// �V�F�[�_�[�̃v���p�e�BID�擾
	private static readonly int m_nRandomSpreadBottomLeftToTopRight_ID = Shader.PropertyToID("_RandomSpreadBottomLeftToTopRight");    // �����̊eID�ϐ��R�}���h��SH_LiquidSpread.shader�ɎQ��
	private static readonly int m_nRandomSpreadTopLeftToBottomRight_ID = Shader.PropertyToID("_RandomSpreadTopLeftToBottomRight");
	private static readonly int m_nStartTime_ID = Shader.PropertyToID("_StartTime");
	private static readonly int m_nFadeStartTime_ID = Shader.PropertyToID("_FadeStartTime");
	private static readonly int m_nFadeDuration_ID = Shader.PropertyToID("_FadeDuration");
	private static readonly int m_nSpreadDuration_ID = Shader.PropertyToID("_SpreadDuration");
	private static readonly int m_nMaxSpread_ID = Shader.PropertyToID("_MaxSpread");
	private static readonly int m_nBaseColor_ID = Shader.PropertyToID("_BaseColor");

	// �v���p�e�B��`
	private float m_fStartTime { get; set; }	// �g�U�J�n����
	private bool m_bFadeStarted { get; set; } = false;	// �g�U�J�n�t���O
	public Vector4 m_RandomSpreadBottomLeftToTopRight { private get; set; }	// ���� x = y �����̊g�U
	public Vector4 m_RandomSpreadTopLeftToBottomRight { private get; set; }	// ���� -x = y ����
	private float m_fRandomSpreadBase { get; set; } = 1.0f;	// �g�U�O�̃x�[�X�T�C�Y
	public float m_fRandomSpreadAdjust { private get; set; } = 0.2f;	// �g�U�傫���̒����l(20%)
	public Color m_ClrLiquidColor { private get; set; } = Color.green;	// �}�e���A���F�ݒ�
	public float m_fSetUpSpreadDuration { private get; set; } = -1;	// �g�U���ԃZ�b�g


	// ���������֐�
	// �����F��
	// ��
	// �ߒl�F��
	// ��
	// �T�v�F���s���̏����ݒ�A����
	private void Start()
	{
		// �}�e���A���Q�b�g
		matMaterial = GetComponent<Renderer>().material;

		// �X�^�[�g���Ԑݒ�
		m_fStartTime = Time.time;

		// �}�e���A���p�����[�^������
		matMaterial.SetFloat(m_nStartTime_ID, m_fStartTime);
		matMaterial.SetFloat(m_nFadeStartTime_ID, -1.0f);
		matMaterial.SetFloat(m_nFadeDuration_ID, m_fFadeDuration);

		// �����_���Ȋg���蕝�����߂�
		m_RandomSpreadBottomLeftToTopRight = new Vector4(
			Random.Range(m_fRandomSpreadBase - m_fRandomSpreadAdjust, m_fRandomSpreadBase + m_fRandomSpreadAdjust),	// �E
			Random.Range(m_fRandomSpreadBase - m_fRandomSpreadAdjust, m_fRandomSpreadBase + m_fRandomSpreadAdjust),	// ��
			Random.Range(m_fRandomSpreadBase - m_fRandomSpreadAdjust, m_fRandomSpreadBase + m_fRandomSpreadAdjust),	// ��
			Random.Range(m_fRandomSpreadBase - m_fRandomSpreadAdjust, m_fRandomSpreadBase + m_fRandomSpreadAdjust)	// ��
		);

		// �����_���Ȋg���蕝�����߂�
		m_RandomSpreadTopLeftToBottomRight = new Vector4(
			Random.Range(m_fRandomSpreadBase - m_fRandomSpreadAdjust, m_fRandomSpreadBase + m_fRandomSpreadAdjust),	// �E��
			Random.Range(m_fRandomSpreadBase - m_fRandomSpreadAdjust, m_fRandomSpreadBase + m_fRandomSpreadAdjust),	// ����
			Random.Range(m_fRandomSpreadBase - m_fRandomSpreadAdjust, m_fRandomSpreadBase + m_fRandomSpreadAdjust),	// ����
			Random.Range(m_fRandomSpreadBase - m_fRandomSpreadAdjust, m_fRandomSpreadBase + m_fRandomSpreadAdjust)	// �E��
		);

		matMaterial.SetVector(m_nRandomSpreadBottomLeftToTopRight_ID, m_RandomSpreadBottomLeftToTopRight);
		matMaterial.SetVector(m_nRandomSpreadTopLeftToBottomRight_ID, m_RandomSpreadTopLeftToBottomRight);
		
		// �����F��X�s�[�h���O������ݒ肳��Ă��甽�f����
		matMaterial.SetColor(m_nBaseColor_ID, m_ClrLiquidColor);
		matMaterial.SetFloat(m_nSpreadDuration_ID, m_fSetUpSpreadDuration > 0 ? m_fSetUpSpreadDuration : m_fSpreadDuration);
		matMaterial.SetFloat(m_nMaxSpread_ID, m_fMaxSpread);

	  
		// �����蔻��ݒ�(��)
		// cldHitbox = gameObject.AddComponent<CircleCollider2D>();
		// ((CircleCollider2D)hitbox).isTrigger = true;
		// ((CircleCollider2D)hitbox).radius = 0.5f;
	}

	// ���X�V�֐�
	// �����F��
	// ��
	// �ߒl�F��
	// ��
	// �T�v�F�t�̂Ɋւ��Ă̍X�V����
	private void Update()
	{
		// �o�ߎ��Ԍv�Z
		float fElapsed = Time.time - m_fStartTime;

		// �t�F�[�h�J�n�v�Z
		if (!m_bFadeStarted && fElapsed >= m_fSpreadDuration + m_fStayDuration)
		{
			matMaterial.SetFloat(m_nFadeStartTime_ID, Time.time);
			m_bFadeStarted = true;
		}
		// �g�p�ς�(��莞��)�I�u�W�F�N�g�폜
		if (m_bFadeStarted && Time.time - (m_fStartTime + m_fSpreadDuration + m_fStayDuration) >= m_fFadeDuration)
		{
			Destroy(gameObject);
		}
	}

	// �����蔻�菈��->�v���C���[���t�̂ɐG�ꂽ�甽��(��)
	// private void OnTriggerEnter2D(Collider2D other)
	// {
	//     if (other.CompareTag("Player"))
	//     {
	//         Debug.Log("�v���C���[���t�̂ɐG�ꂽ�I");
	//         
	//         // TODO:�v���C���[���t�̂�G�����珈����ǉ�
	//     }
	// }
}
