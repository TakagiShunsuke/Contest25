/*=====
<BGMManager.cs>
���쐬�ҁFNishibu

�����e
BGM���Ǘ�����N���X

���X�V����
__Y25 
_M05
D
21:BGMManager�쐬:nishibu
=====*/

// ���O��Ԑ錾
using UnityEngine;

// �N���X��`
public class CBGMManager : MonoBehaviour
{
    [Header("BGM�ݒ�")]
    [Tooltip("BGM��ݒ肷��")]
    [SerializeField] private AudioClip m_StageBGM;

    private AudioSource m_AudioSource; // BGM�pAudioSource


    /// <summary>
	/// -�������֐�
	/// <para>BGM�ݒ�E����</para>
	/// </summary>
    private void Awake()
    {
        m_AudioSource = gameObject.AddComponent<AudioSource>(); // AudioSource��ǉ�
        m_AudioSource.clip = m_StageBGM; // BGM��ݒ�
        m_AudioSource.loop = true; // ���[�v�Đ�����悤�ݒ�
        m_AudioSource.playOnAwake = true; // �Đ��J�n���Ɏ����ōĐ�����悤�ݒ�
        m_AudioSource.volume = 0.05f; // ���ʂ�ݒ�
        m_AudioSource.Play(); // BGM�̍Đ����J�n
    }
}
