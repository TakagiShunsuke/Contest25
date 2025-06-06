using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static CDecalSpawner;

public class CDecalSpawner : MonoBehaviour
{
    // 列挙定義
    public enum E_EffectType
    {
        None = 0,
        Poison = 1,
        Asid = 2,
        Heal = 3,
        // 追加が簡単！
    }
    // 変数宣言
    [SerializeField, Tooltip("生成するデカールプレハブ")] private GameObject m_DecalPrefab;
    [SerializeField, Tooltip("生成の色設定")] private Color m_DecalColor = Color.red;
    [SerializeField, Tooltip("ビジュアル用シェーダ")] private Shader m_VisualShader;
    [SerializeField, Tooltip("効果データ格納用シェーダ")] private Shader m_DataShader;
    private Shader test;

    private float fEffectLevel = 1.0f;   // 液体の効果段階
    private bool bHasSpawned = false;  // 生成フラグ
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

        // ビジュアル用RenderTexture
        RenderTexture rtVisual = new RenderTexture(256, 256, 0, RenderTextureFormat.ARGB32);
        rtVisual.Create();

        // データ情報用RenderTexture
        RenderTexture rtEffect = new RenderTexture(256, 256, 0, RenderTextureFormat.ARGB32);
        rtEffect.Create();

        // 見た目用：色つき
        Material matVisual = new Material(m_VisualShader);
        matVisual.SetColor("_BaseColor", m_DecalColor);
        Graphics.Blit(null, rtVisual, matVisual);

        // 情報用：効果値
        Material matEffect = new Material(m_DataShader);
        matEffect.SetFloat("_EffectType", (float)eEffectType); // 例：1=毒
        matEffect.SetFloat("_EffectLevel", fEffectLevel); // 例：0.5
        Graphics.Blit(null, rtEffect, matEffect);

        // マネージャに登録
        if (CDeathEffectManager.Instance != null)
        {
            CDeathEffectManager.Instance.RegisterEffectTexture(rtEffect, contact.point);
        }

        // デカール生成
        GameObject decal = Instantiate(m_DecalPrefab, pos, rot);
        var projector = decal.GetComponent<DecalProjector>();

        if (projector != null)
        {
            Material mat = new Material(projector.material); // インスタンス化
            mat.SetTexture("_BaseMap", rtVisual);
            mat.SetColor("_BaseColor", m_DecalColor); // 明度調整にも使える
            projector.material = mat;

            Debug.Log("b"+m_DecalColor);
        }
        
        Destroy(decal, 10f);

        // データ用RTも解放したいなら：
        StartCoroutine(RemoveEffectTextureAfter(rtEffect, 10f));

    }
        
}
