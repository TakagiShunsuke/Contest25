using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManage : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKey(KeyCode.Return))
        {
                SceneManager.LoadScene("AlphaScene");
            
        }
    }
 
}
