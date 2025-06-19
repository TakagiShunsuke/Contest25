using UnityEngine;

public class BootFade : MonoBehaviour
{
    public FadeManager fadeManager; 
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            fadeManager.StartFadeOut();
        }

    }
}
