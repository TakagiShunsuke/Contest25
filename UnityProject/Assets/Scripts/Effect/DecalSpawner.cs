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
        Debug.Log($"[DecalSpawner] �Փˌ��o�I �� ����: {collision.gameObject.name}");

        //if (Time.time - m_LastSpawnTime < m_SpawnCooldown) return;
        //m_LastSpawnTime = Time.time;

        if (m_HasSpawned) return; // ��x��������
        m_HasSpawned = true;

        ContactPoint contact = collision.contacts[0];
        Vector3 pos = contact.point + contact.normal * 0.01f; // �n�ʂ��班����������
        Quaternion rot = Quaternion.Euler(90, 0, 0);
        //Quaternion rot = Quaternion.LookRotation(-contact.normal); // �@���ɍ��킹�ē\��I

        GameObject decal = Instantiate(m_DecalPrefab, pos, rot);

        var projector = decal.GetComponent<DecalProjector>();
        if (projector != null)
        {
            var block = new MaterialPropertyBlock();
            block.SetColor("_BaseColor", m_DecalColor); // �������ŐF��ύX
            projector.material.SetColor("_BaseColor", m_DecalColor); // ����̂��ߗ������
        }

        Debug.Log($"�f�J�[�������I {decal.name} at {pos}");
        // �I�v�V�����F���b��Ɏ����폜
        Destroy(decal, 10f);
    }
}
