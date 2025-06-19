using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class TitleManage : MonoBehaviour
{
    private Image stage1;
    private Image stage2;
    private Image stage3;

    public GameObject stageselect;
    //public bool m_bIsStage1Clear;
    public bool m_bIsStage2Clear = false;
    public bool m_bIsStage3Clear = false;
    public bool m_bIsStageselect = false;
    private int num = 1;

    [SerializeField] private AudioSource decision;
    [SerializeField] private AudioSource cancel;

    void Start()
    {
       // stage1 = GameObject.Find("stage1").GetComponent<Image>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            decision.Play();
               SceneManager.LoadScene("AlphaScene");
            if (stageselect.activeSelf == false)
            {

                stageselect.SetActive(true);
                m_bIsStageselect = true;
                stage1 = GameObject.Find("stage1").GetComponent<Image>();
                stage2 = GameObject.Find("stage2").GetComponent<Image>();
                stage3 = GameObject.Find("stage3").GetComponent<Image>();
                num = 1;
                stage1.sprite = Resources.Load<Sprite>("Stageselect/stage1waku");
                if(m_bIsStage2Clear==true)
                {
                    stage2.sprite = Resources.Load<Sprite>("Stageselect/stage2");
                }
                else
                {
                    stage2.sprite = Resources.Load<Sprite>("Stageselect/stage2rock");
                }
                if (m_bIsStage3Clear == true)
                {
                    stage3.sprite = Resources.Load<Sprite>("Stageselect/stage3");
                }
                else
                {
                    stage3.sprite = Resources.Load<Sprite>("Stageselect/stage3rock");
                }
            }
            else
            {
                switch (num)
                {
                    case 1:
                        SceneManager.LoadScene("AlphaScene");
                        break;
                    case 2:
                        SceneManager.LoadScene("AlphaScene");
                        break;
                    case 3:
                        SceneManager.LoadScene("AlphaScene");
                        break;
                }
            }

        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            cancel.Play();
            stageselect.SetActive(false);
            m_bIsStageselect = false;
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            switch (num)
            {
                case 1:
                    break;
                case 2:
                    stage2.sprite = Resources.Load<Sprite>("Stageselect/stage2");
                    stage1.sprite = Resources.Load<Sprite>("Stageselect/stage1waku");
                    num = 1;
                    break;
                case 3:
                    stage2.sprite = Resources.Load<Sprite>("Stageselect/stage2waku");
                    stage3.sprite = Resources.Load<Sprite>("Stageselect/stage3");
                    num = 2;
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            switch (num)
            {
                case 1:
                    if (m_bIsStage2Clear == true)
                    {
                        stage1.sprite = Resources.Load<Sprite>("Stageselect/stage1");
                        stage2.sprite = Resources.Load<Sprite>("Stageselect/stage2waku");
                        num = 2;
                    }
                    else
                    {
                        //Ç∑ÇƒÅ[Ç∂ÇQÇ≠ÇËÇ†ÇµÇƒÇ»Ç©Ç¡ÇΩÇÁ
                    }
                    //Debug.Log("âÊñ êÿÇËë÷Ç¶");
                    
                    break;
                case 2:
                    if (m_bIsStage3Clear == true)
                    {
                        stage2.sprite = Resources.Load<Sprite>("Stageselect/stage2");
                        stage3.sprite = Resources.Load<Sprite>("Stageselect/stage3waku");
                        num = 3;
                    }
                    else
                    {
                        //Ç∑ÇƒÅ[Ç∂ÇQÇ≠ÇËÇ†ÇµÇƒÇ»Ç©Ç¡ÇΩÇÁ
                    }
                    
                    break;
                case 3:
                    break;
            }
        }
        //debug
        if (Input.GetKey(KeyCode.Alpha2))
        {
            m_bIsStage2Clear = true;
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            m_bIsStage3Clear = true;
        }
    }
}
 

