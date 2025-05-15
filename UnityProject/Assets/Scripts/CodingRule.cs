/*=====
<CodingRule.cs>	// �X�N���v�g��
���쐬�ҁFtakagi

�����e
�R�[�f�B���O�K����L�q

�����ӎ���	// �Ȃ��Ƃ��͏ȗ�OK
���̋K�񏑂ɋL�q�̂Ȃ����͔̂�������A�K�X�ǉ�����

���X�V����
__Y24	// '24�N
_M04	// 04��
D		// ��
16:�v���O�����쐬:takagi	// ���t:�ύX���e:�{�s��
17:����������:takagi

_M05
D
03:����͂ɂقւ�:takagi
04:�������̋L�@�ǉ��E������\�L���C��:takagi
14:�C���^�[�t�F�[�X�ɂ��ċL�q:takagi

_M06
D
20:�x�������o������C���E
	�C���X�y�N�^�ł̌����ڂ��ӎ������\�L���P�E
	�����o�łȂ��ϐ����̋L�q�@�ύX:takagi

__Y25
_M04
D
03:�V�`�[���p�ɃR�[�h�����V:takagi
23:enum�^�̕ϐ��錾�𖾋L:takagi
_M05
D
09:�R�s�y�����̂܂܂�������Ȃǂ̌�������Aenum�ϐ��Ȃǂ̋L�ږ@��ǉ�:takagi
14:�֐��R�����g��XML�h�L�������g�R�����g���̗p:takagi
15:�X�V�����̋L�q�R���ǉ�:takagi
=====*/

// ���O��Ԑ錾
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;	// ���O�錾���ɂ̓R�����g�͕s�v(����ɂ���邽��)

// ���O��Ԓ�`
namespace Space
{
	// �N���X��`
	public static class CSpace
	{
		// �萔��`
		private const uint CONST = 0;	// �萔�̓R���X�^���X�P�[�X�ɏ]��(�n���K���A���L�@�ɂ͏]��Ȃ�)

		// �ϐ��錾
		public static readonly double ms_dTemp = 0.0d;	// readonly�ȕϐ����������͓���
	}
}

// �C���^�[�t�F�[�X��`
public interface IInterface	// �C���^�[�t�F�[�X�̓�������I������
{
	// �v���p�e�B��`
	/// <value>�H�H�H</value>
	public double Prop { get; set; }	// ���������v���p�e�B�̓n���K���A���L�@�𖳎����Ă悢

	// �v���g�^�C�v�錾
	public void Signaled();
}


// �N���X��`
public class CCodingRule : MonoBehaviour	// �N���X�^�̓�������C������
{
	// �񋓒�`
	public enum E_ENUM	// �񋓂͐ړ�����E_�Ƃ���
	{
		A,	// ���g���R���X�^���X�P�[�X
		B,	// �񋓖��������p���K�v�͂Ȃ�
	}

	// �\���̒�`
	private struct Struct
	{
		GameObject m_Object;	// �N���X�^�̃l�[�~���O�̓n���K���A���L�@�ɏ]��Ȃ�
								// ��m_��s_�Ȃǂ̌^�Ƃ͊֌W�Ȃ������ł͏]��
								// �ړ����̌��͑啶������n�߂�
		Ray m_Ray;
	}
	[Serializable]
	public struct SerializeStruct
	{
		[Tooltip("�ȈՐ���")] public GameObject m_Member;	// ���V���A���C�Y�ł���ꍇ�A���̕ϐ������Ȃ̂��C���X�y�N�^����킩��悤�ɂ���
	}

	// �ϐ��錾
	[Space]	// ��s�����p�����₷������
	[Header("������")]	// �ϐ��𕪗ނ��Ƃɕ����ċL�q
	[SerializeField, Tooltip("�����o�[�ϐ�")] private uint m_uMember;	// �����͋L�@�ɉe�����Ȃ�
	private int m_nInt;	// �ʏ�̌^�̓n���K���A���L�@�ɏ]��[�����o�ϐ���m_�ƕt����]
	[SerializeField, Tooltip("�\����")] private SerializeStruct m_Struct;	// ���̕ϐ������Ȃ̂��C���X�y�N�^����킩��悤�ɂ���
	private static string ms_sStr;	// �����o�ϐ��ȐÓI�ϐ��Ȃ�Z������ms_�ƋL�q����
									// string�^�̃n���K���A���L�@��s�Ƃ���
	private E_ENUM m_eEnum;	// �񋓂̓n���K���A���L�@�ɏ]��e��ړ����ɂ���
	private float[] m_fArray;	// �z��͎q�̌^���̂݋L�q(a�Ƃ��͕s�v)
	private List<uint> m_uLists;	// ���X�g���q�̌^���̂݋L�q
	private Dictionary<string, float> m_fDictionary;	// �����̖�����value���̌^���Ɉˑ�

	// �v���p�e�B��`
		// �v���p�e�B�������O�Ɉ��s������
	/// <summary>
	/// xx�v���p�e�B(�v���p�e�B��)
	/// </summary>
	/// <value>�v���p�e�B�̎�舵���l�ɂ��ċL�q</value>
	public double PriProp { get; private set; }	// �v���p�e�B�̓p�X�J���P�[�X�̂ݏ]���n���K���A���L�@�͖���(�֐�����)

	/// <summary>
	/// �����o�[�v���p�e�B
	/// </summary>
	/// <value><see cref="m_uMember"/></value>	// �Q�Ƃ��Ă���^�C�v�̃v���p�e�B��cref�Ŏ�����OK
	public uint Member
	{
		get
		{
			return m_uMember;
		}
		set
		{
			m_uMember = value;
		}
	}

		// ����֐���`�O��2�s�󂯂�	�����֐��R�����g��XML�`����
	/// <summary>
	/// -��֐�(�֐���)
	/// <para>�T�v���L��</para>
	/// <para>�������ڂ���ꍇ�͒i��(para����)�𕪂���ƌ��ʓI</para>
	/// <see href = "https://www.youtube.com/">�Q�l�T�C�g�Y�t</see>
	/// <see cref="m_Struct"/>
	/// </summary>
	/// <typeparam name="Meta">�W�F�l���b�N�^�̐���</typeparam>
	/// <param name="_dDouble">����1�̐���</param>
	/// <param name="_GameObject">����2�̐���</param>
	/// <param name="_MetaData">����3�̐���</param>
	/// <returns>�߂�l�̐���</returns>
	private int Example<Meta>(double _dDouble, GameObject _GameObject, Meta _MetaData)
	{
		// �ϐ��錾
		float _fFloat = 0.0f;	// ���[�J���ϐ���_����n�߂�
		GameObject _Object = _GameObject;	// �ړ����������ꍇ�A��������啶���ɂ���

		// �Z�o
		m_nInt = (int)((float)_dDouble * _fFloat);	// �Ȃ�ׂ��S�����ɃR�����g������

		// ����
		switch(m_nInt)
		{
			case 0:	// A
				break;
				// �P�[�X�Ԃ͋�s
			case 1: // B
				m_nInt = 0;  // �Ȃ�ׂ��S�����ɃR�����g������
				break;

			default:	// ���̑�
#if _Debug	//�v���v���Z�b�T�͍��[
				Debug.Log("�f�t�H���g");	// ���g�͒ʏ�Ɠ���
#endif
				break;
		}

		if (m_nInt != 0)	// if����()�̑O�ɋ�
		{
			m_nInt = 0;
		}
		else	// else���̂̃R�����g�ʒu
		{
			m_nInt *= 0;
		}

		// ��
		return m_nInt;
	}
		// ���ڈȍ~�̊֐���`�͏����O�Ɉ��s������
	// ��xx�֐�
	// �����F�Ȃ�   // �������Ȃ��ꍇ�͂P���ȗ����Ă��悢
	// ��
	// �ߒl�F�Ȃ�
	// ��
	// �T�v�F�֐���
	/// <summary>
	/// -xx�֐�
	/// <para>�֐���</para>
	/// </summary>
	// �W�F�l���b�N�^���Ȃ��ꍇ�͏ȗ�
	// �������Ȃ��ꍇ�͏ȗ�
	// �߂�l���Ȃ��ꍇ�͏ȗ�
	[MenuItem("ffff/gggg")]
	public void Function()
	{
	}
}

[CreateAssetMenu(menuName = "ObjectMenu/Menu")]
class CMenuItem : ScriptableObject
{
	/// <summary>
	/// -�����֐�
	/// <para>�֐���</para>
	/// </summary>
	/// <remarks>�^�C�g��</remarks>
	private void DoSomething()
	{
	}
};