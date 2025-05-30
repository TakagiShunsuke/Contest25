using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearManage : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKey(KeyCode.Return))
        {
                SceneManager.LoadScene("TitleScene");
            
        }
    }
 
}
