using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public string scene;                    //�J�ڐ�̃V�[����
    public float Speed;                     //�t�F�[�h����X�s�[�h
    public bool fadeOnStart = true;         //�Q�[���J�n���Ƀt�F�[�h�C�����邩�ǂ���
    public Image    fadeImage;              //�t�F�[�h�p��Image�R���|�[�l���g
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
            Debug.LogError("�V�[�� " + scene + " ��ǂݍ��߂܂���B���O�~�X or Build Settings���o�^�̉\���A���B");
        }
        else
        {
            Debug.Log("�V�[�� " + scene + " ��ǂݍ��݊J�n");
            SceneManager.LoadScene(scene);
        }

    }

    public void ChangeScene(string scene)
    {
        Debug.Log("Loading scene: " + scene);

        SceneManager.LoadScene(scene, LoadSceneMode.Single);
        Debug.Log("���݂̃A�N�e�B�u�V�[��: " + SceneManager.GetActiveScene().name);
    }
}
