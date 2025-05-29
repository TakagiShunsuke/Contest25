using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CDecalSpawner : MonoBehaviour
{
    [SerializeField] private GameObject m_DecalPrefab;
    //[SerializeField] private float m_SpawnCooldown = 0.3f;
    //private float m_LastSpawnTime = -10f;
    [SerializeField] private Color m_DecalColor = Color.green;

    private bool m_HasSpawned = false;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"[DecalSpawner] 衝突検出！ → 相手: {collision.gameObject.name}");

        //if (Time.time - m_LastSpawnTime < m_SpawnCooldown) return;
        //m_LastSpawnTime = Time.time;

        if (m_HasSpawned) return; // 一度だけ生成
        m_HasSpawned = true;

        ContactPoint contact = collision.contacts[0];
        Vector3 pos = contact.point + contact.normal * 0.01f; // 地面から少し浮かせる
        Quaternion rot = Quaternion.Euler(90, 0, 0);
        //Quaternion rot = Quaternion.LookRotation(-contact.normal); // 法線に合わせて貼る！

        GameObject decal = Instantiate(m_DecalPrefab, pos, rot);

        var projector = decal.GetComponent<DecalProjector>();
        if (projector != null)
        {
            var block = new MaterialPropertyBlock();
            block.SetColor("_BaseColor", m_DecalColor); // ★ここで色を変更
            projector.material.SetColor("_BaseColor", m_DecalColor); // 安定のため両方やる
        }

        Debug.Log($"デカール生成！ {decal.name} at {pos}");
        // オプション：数秒後に自動削除
        Destroy(decal, 10f);
    }
}
