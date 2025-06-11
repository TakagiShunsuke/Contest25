using UnityEngine;

public class InvincibleState : MonoBehaviour
{
    private float m_RemainingTime = 1.0f;

    // ������
    public void Initialize(float duration)
    {
        m_RemainingTime = duration;
    }

    // ���Ԃ������i���݂̎��ԂƔ�r���Ē��������̗p�j
    public void ExtendIfLonger(float newDuration)
    {
        if (newDuration > m_RemainingTime)
        {
            m_RemainingTime = newDuration;
        }
    }

    private void Update()
    {
        m_RemainingTime -= Time.deltaTime;
        if (m_RemainingTime <= 0f)
        {
            Destroy(this);
            Debug.Log("���G��ԉ���");
        }
    }
}
