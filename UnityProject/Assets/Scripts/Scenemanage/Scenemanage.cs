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
    public void changSceneGAMEOVER()
    {
        SceneManager.LoadScene("GAMEOVER");
    }
    public void changSceneGAMECLEAR()
    {
        SceneManager.LoadScene("GAMECLEAR");
    }
    public void ChangSceneTitle()
    {
        SceneManager.LoadScene("Title");
    }
    public void ChangSceneAlpha()
    {
        SceneManager.LoadScene("AlphaScene");
    }
}
