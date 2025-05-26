/*=====
<HPBar.cs>
└作成者：okugami

＞内容
Hpバーの制御スクリプト

＞更新履歴
__Y25
_M05
D
21:作成　okugami
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
