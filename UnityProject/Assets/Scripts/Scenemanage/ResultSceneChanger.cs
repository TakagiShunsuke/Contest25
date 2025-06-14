/*=====
<ClearManage.cs>
���쐬�ҁFtakagi

�����e
�N���A���������

���X�V����
__Y25
_M06
D
13:�v���O�����쐬����:takagi
14:�o�g���f�[�^�̃N���A������ǉ�:takagi
=====*/

// ���O��Ԑ錾
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

// �N���X��`
public class CResultSceneChanger : MonoBehaviour
{
	// �ϐ��錾
	[SerializeField, Tooltip("�J�ڐ�V�[��")] private SceneDropDown m_Scene;
	[SerializeField, Tooltip("�J�ڃL�[")] private KeyCode m_LoadKey = KeyCode.Return;	
	[SerializeField, Tooltip("�I����")] public AudioClip m_DecideSE;
	[SerializeField, Tooltip("�I������")] private float m_DecideSEVolume = 0.05f;
	private AudioSource m_DecideSESource;	// �I��SE�p�̃I�[�f�B�I�\�[�X
	

	/// <summary>
	/// -�������֐�
	/// <para>����������</para>
	/// </summary>
	private void Start()
	{
		// ��������
		m_DecideSESource = gameObject.AddComponent<AudioSource>();	// �I��p�̉����R���|�[�l���g�쐬
		m_DecideSESource.volume = m_DecideSEVolume;	// ���ʂ�ݒ�
	}

	/// <summary>
	/// -�X�V�֐�
	/// <para>�X�V����</para>
	/// </summary>
	void Update()
	{
		// �ۑS
		if (m_Scene.SceneName == string.Empty)	// �k���`�F�b�N
		{
#if UNITY_EDITOR
			Debug.LogError("�J�ڐ�V�[����������܂���");
#endif	// !UNITY_EDITOR
			return;	// �������f
		}

		// �V�[���J��
		if (Input.GetKey(m_LoadKey))	// �V�[���J�ړ��͎�
		{
			// ��p�f�[�^�N���A
			if (CBattleData.Instance != null)	// �k���`�F�b�N
			{
				CBattleData.Instance.Clear();	// ���U���g�p�̃f�[�^�Ȃ̂ŃV�[���J�ڎ��ɂ͏����Ă���
			}

			// ���ʉ��Đ�
			if (!m_DecideSESource.isPlaying)	// �Đ����d�Ȃ�Ȃ�
			{
				m_DecideSESource.PlayOneShot(m_DecideSE);	// �I�����Đ�
			}

			// �J�ڎ��s
			SceneManager.LoadScene(m_Scene.SceneName);	// �ݒ肳��Ă��鎟�V�[���֑J��
		}
	}
}