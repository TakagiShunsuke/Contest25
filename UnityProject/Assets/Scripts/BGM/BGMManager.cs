/*=====
<BGMManager.cs>
└作成者：Nishibu

＞内容
BGMを管理するクラス

＞更新履歴
__Y25 
_M05
D
21:BGMManager作成:nishibu
=====*/

// 名前空間宣言
using UnityEngine;

// クラス定義
public class CBGMManager : MonoBehaviour
{
    [Header("BGM設定")]
    [Tooltip("BGMを設定する")]
    [SerializeField] private AudioClip m_StageBGM;

    private AudioSource m_AudioSource; // BGM用AudioSource


    /// <summary>
	/// -初期化関数
	/// <para>BGM設定・流す</para>
	/// </summary>
    private void Awake()
    {
        m_AudioSource = gameObject.AddComponent<AudioSource>(); // AudioSourceを追加
        m_AudioSource.clip = m_StageBGM; // BGMを設定
        m_AudioSource.loop = true; // ループ再生するよう設定
        m_AudioSource.playOnAwake = true; // 再生開始時に自動で再生するよう設定
        m_AudioSource.volume = 0.05f; // 音量を設定
        m_AudioSource.Play(); // BGMの再生を開始
    }
}
