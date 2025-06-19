using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public string scene;                    //遷移先のシーン名
    public float Speed;                     //フェードするスピード
    public bool fadeOnStart = true;         //ゲーム開始時にフェードインするかどうか
    public Image    fadeImage;              //フェード用のImageコンポーネント
    float           red, green, blue, alfa; 
    Coroutine       fadeCoroutine;

    void Start()
    {
        red = fadeImage.color.r;
        green = fadeImage.color.g;
        blue = fadeImage.color.b;
        alfa = fadeImage.color.a;
    }

    public void StartFadeIn()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeIn());
    }

    public void StartFadeOut()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn()
    {
        fadeImage.enabled = true;
        alfa = 1f;
        while (alfa > 0f)
        {
            alfa -= Speed * Time.deltaTime;
            fadeImage.color = new Color(red, green, blue, alfa);
            yield return null;
        }
        fadeImage.enabled = false;
    }

    IEnumerator FadeOut()
    {
        Debug.Log("Loading scene: " + scene);
        fadeImage.enabled = true;
        alfa = 0f;
        while (alfa < 1f)
        {
            alfa += Speed * Time.deltaTime;
            fadeImage.color = new Color(red, green, blue, Mathf.Clamp01(alfa));
            yield return null;
        }
        if (!Application.CanStreamedLevelBeLoaded(scene))
        {
            Debug.LogError("シーン " + scene + " を読み込めません。名前ミス or Build Settings未登録の可能性アリ。");
        }
        else
        {
            Debug.Log("シーン " + scene + " を読み込み開始");
            SceneManager.LoadScene(scene);
        }

    }

    public void ChangeScene(string scene)
    {
        Debug.Log("Loading scene: " + scene);

        SceneManager.LoadScene(scene, LoadSceneMode.Single);
        Debug.Log("現在のアクティブシーン: " + SceneManager.GetActiveScene().name);
    }
}
