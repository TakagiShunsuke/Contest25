using UnityEngine;
using UnityEngine.SceneManagement;
public class Scenemanage : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void changSceneResult()
    {
        SceneManager.LoadScene("Result");
    }
    public void ChangSceneTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
