/*=====
<CLiquidSpread.cs>
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

=====*/

// ���O��Ԑ錾
using UnityEngine;

// �N���X��`
public class CLiquidSpread : MonoBehaviour
{
    // �ϐ��錾

    [Header("�g�U�E�\���E�t�F�[�h�ݒ�")]
    [SerializeField] private float spreadDuration = 1.0f; // �g�U�ɂ����鎞��
    [SerializeField] private float stayDuration = 5.0f;   // �g�U�セ�̂܂ܕ\������鎞��
    [SerializeField] private float fadeDuration = 1.5f;   // �t�F�[�h�A�E�g����
    private float startTime;
    private bool fadeStarted = false;

    [Header("�����_���g�U���ݒ�")]
    private Vector4 randomSpreadPlus;  // x����, y����
    private Vector4 randomSpreadMinus;  // -x����, -y����
    [SerializeField] private float randomSpreadPlusMin = 0.8f;
    [SerializeField] private float randomSpreadPlusMax = 1.2f;
    [SerializeField] private float randomSpreadMinusMin = 0.8f;
    [SerializeField] private float randomSpreadMinusMax = 1.2f;

    [Header("���̃}�e���A���ݒ�")]
    private Material mat;
    private Collider hitbox;

    // �V�F�[�_�[����ϐ������擾
    private static readonly int RandomSpreadPlus_ID = Shader.PropertyToID("_RandomSpreadPlus");
    private static readonly int RandomSpreadMinus_ID = Shader.PropertyToID("_RandomSpreadMinus");

    // �O������ݒ���󂯎�邽��
    private Color liquidColor = Color.green;
    private float externalSpreadSpeed = 1.0f;


    // ���ݒu�֐�
    // �����P�FColor color�F�F
    // �����Q�Ffloat spreadDuration�F���l  // �g�U�ɂ����鎞��
    // �����R�Ffloat stayDuration�F���l    // �t�̎c�鎞��
    // �����S�Ffloat fadeDuration�F���l    // �t�̏����鎞��
    // ��
    // �ߒl�F��
    // ��
    // �T�v�F�g�U�}�e���A���ݒ肷��p
    public void Setup(Color color, float spreadDuration, float stayDuration, float fadeDuration)
    {
        this.liquidColor = color;
        this.externalSpreadSpeed = spreadDuration;
        this.stayDuration = stayDuration;
        this.fadeDuration = fadeDuration;
    }

    // ���������֐�
    // �����F��
    // ��
    // �ߒl�F��
    // ��
    // �T�v�F���s���̏����ݒ�A����
    void Start()
    {
        mat = GetComponent<Renderer>().material;
        startTime = Time.time;

        // �}�e���A���p�����[�^������
        mat.SetFloat("_StartTime", startTime);
        mat.SetFloat("_FadeStartTime", -1.0f);
        mat.SetFloat("_FadeDuration", fadeDuration);

        // �����Ń����_���Ȋg���蕝�����߂�I
        randomSpreadPlus = new Vector4(
            Random.Range(randomSpreadPlusMin, randomSpreadPlusMax), // �E
            Random.Range(randomSpreadPlusMin, randomSpreadPlusMax), // ��
            Random.Range(randomSpreadPlusMin, randomSpreadPlusMax), // ��
            Random.Range(randomSpreadPlusMin, randomSpreadPlusMax)  // ��
        );

        randomSpreadMinus = new Vector4(
            Random.Range(randomSpreadMinusMin, randomSpreadMinusMax), // �E��
            Random.Range(randomSpreadMinusMin, randomSpreadMinusMax), // ����
            Random.Range(randomSpreadMinusMin, randomSpreadMinusMax), // ����
            Random.Range(randomSpreadMinusMin, randomSpreadMinusMax)  // �E��
        );

        mat.SetVector(RandomSpreadPlus_ID, randomSpreadPlus);
        mat.SetVector(RandomSpreadMinus_ID, randomSpreadMinus);
        // �ǉ��F�����F��X�s�[�h���O������ݒ肳��Ă��甽�f����
        mat.SetColor("_BaseColor", liquidColor);
        mat.SetFloat("_SpreadSpeed", externalSpreadSpeed);

        // �����蔻��ݒ�(��)
        // hitbox = gameObject.AddComponent<CircleCollider2D>();
        // ((CircleCollider2D)hitbox).isTrigger = true;
        // ((CircleCollider2D)hitbox).radius = 0.5f;
    }

    // ���X�V�֐�
    // �����F��
    // ��
    // �ߒl�F��
    // ��
    // �T�v�F�t�̂Ɋւ��Ă̍X�V����
    void Update()
    {
        float elapsed = Time.time - startTime;

        // �t�F�[�h�J�n�v�Z
        if (!fadeStarted && elapsed >= spreadDuration + stayDuration)
        {
            mat.SetFloat("_FadeStartTime", Time.time);
            fadeStarted = true;
        }
        // �g�p�ς�(��莞��)�I�u�W�F�N�g�폜
        if (fadeStarted && Time.time - (startTime + spreadDuration + stayDuration) >= fadeDuration)
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
