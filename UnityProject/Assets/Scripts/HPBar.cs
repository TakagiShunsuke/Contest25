/*=====
<HPBar.cs>
���쐬�ҁFokugami

�����e
Hp�o�[�̐���X�N���v�g

���X�V����
__Y25
_M05
D
21:�쐬�@okugami
=====*/

using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public Slider m_HpBar;
    public CHitPoint m_cHitPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_HpBar.value = m_cHitPoint.HP/100;
    }
}
