/*=====
<CountEnemy.cs>
���쐬�ҁFNishibu

�����e
// �G�̑������J�E���g���邽�߂̃N���X

���X�V����
__Y25 
_M05
D
5:CountEnemy�N���X����:nishibu
6:�C��:nishibu
7:�C���A�R�����g:nishibu
21:���t�@�N�^�����O:takagi
=====*/

// ���O��Ԑ錾
using UnityEngine;

// �N���X��`
public class CCountEnemy : MonoBehaviour
{
	// �ϐ��錾
	public static uint m_nValInstances { get; private set; } = 0;	// ���݃X�e�[�W��ɑ��݂��Ă���G�̐�

	
	/// <summary>
	/// -�������֐�
	/// <para>�G�̃C���X�^���X����1���₷</para>
	/// </summary>
	protected virtual void Start()
	{
		m_nValInstances++;	// �G�̐���1���₷
	}

	/// <summary>
	/// -���S���֐�
	/// <para>�G�̃C���X�^���X����1���炷</para>
	/// </summary>
	protected virtual void OnDestroy()
	{
		m_nValInstances--; // �G�̐���1���炷
	}
}