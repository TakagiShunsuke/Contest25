using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static CDecalSpawner;

public class CDecalSpawner : MonoBehaviour
{
    // �񋓒�`
    public enum E_EffectType
    {
        None = 0,
        Poison = 1,
        Asid = 2,
        Heal = 3,
        // �ǉ����ȒP�I
    }
    // �ϐ��錾
    [SerializeField, Tooltip("��������f�J�[���v���n�u")] private GameObject m_DecalPrefab;
    [SerializeField, Tooltip("�����̐F�ݒ�")] private Color m_DecalColor = Color.red;
    [SerializeField, Tooltip("�r�W���A���p�V�F�[�_")] private Shader m_VisualShader;
    [SerializeField, Tooltip("���ʃf�[�^�i�[�p�V�F�[�_")] private Shader m_DataShader;
    private Shader test;

    private float fEffectLevel = 1.0f;   // �t�̂̌��ʒi�K
    private bool bHasSpawned = false;  // �����t���O
    private void Start()
    {
        //m_DecalColor = Color.red;
        Debug.Log(m_DecalColor);
    }

    private E_EffectType eEffectType = E_EffectType.None;
    public void SetEffectType(E_EffectType type) => eEffectType = type;
    public void SetEffectLevel(float level) => fEffectLevel = level;

    public void SetDecalColor(Color color) => m_DecalColor = color;

    private System.Collections.IEnumerator RemoveEffectTextureAfter(RenderTexture rt, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (CDeathEffectManager.Instance != null)
            CDeathEffectManager.Instance.UnregisterEffectTexture(rt);

        rt.Release();
        Destroy(rt);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("a"+m_DecalColor);

        if (bHasSpawned) return;
        bHasSpawned = true;

        ContactPoint contact = collision.contacts[0];
        Vector3 pos = contact.point + contact.normal * 0.01f;
        Quaternion rot = Quaternion.LookRotation(-contact.normal);

        // �r�W���A���pRenderTexture
        RenderTexture rtVisual = new RenderTexture(256, 256, 0, RenderTextureFormat.ARGB32);
        rtVisual.Create();

        // �f�[�^���pRenderTexture
        RenderTexture rtEffect = new RenderTexture(256, 256, 0, RenderTextureFormat.ARGB32);
        rtEffect.Create();

        // �����ڗp�F�F��
        Material matVisual = new Material(m_VisualShader);
        matVisual.SetColor("_BaseColor", m_DecalColor);
        Graphics.Blit(null, rtVisual, matVisual);

        // ���p�F���ʒl
        Material matEffect = new Material(m_DataShader);
        matEffect.SetFloat("_EffectType", (float)eEffectType); // ��F1=��
        matEffect.SetFloat("_EffectLevel", fEffectLevel); // ��F0.5
        Graphics.Blit(null, rtEffect, matEffect);

        // �}�l�[�W���ɓo�^
        if (CDeathEffectManager.Instance != null)
        {
            CDeathEffectManager.Instance.RegisterEffectTexture(rtEffect, contact.point);
        }

        // �f�J�[������
        GameObject decal = Instantiate(m_DecalPrefab, pos, rot);
        var projector = decal.GetComponent<DecalProjector>();

        if (projector != null)
        {
            Material mat = new Material(projector.material); // �C���X�^���X��
            mat.SetTexture("_BaseMap", rtVisual);
            mat.SetColor("_BaseColor", m_DecalColor); // ���x�����ɂ��g����
            projector.material = mat;

            Debug.Log("b"+m_DecalColor);
        }
        
        Destroy(decal, 10f);

        // �f�[�^�pRT������������Ȃ�F
        StartCoroutine(RemoveEffectTextureAfter(rtEffect, 10f));

    }
        
}
