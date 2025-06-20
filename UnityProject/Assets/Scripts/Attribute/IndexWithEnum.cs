/*=====
<IndexWithEnum.cs>
���쐬�ҁFtakagi

�����e
�z��̓Y�����ԍ����A�V���A���C�Y�t�B�[���h�ŗ񋓖��ɏ㏑���\���������

���X�V����
__Y24
_M06
D
07:�v���O�����쐬:takagi
21:���t�@�N�^�����O:takagi
__Y25
_M06
20:���ɍ��킹�ă��t�@�N�^�����O:takagi
=====*/

// ���O��Ԑ錾
using System;
using UnityEngine;
using UnityEditor;

/// <summary>
/// -�z��񋓑���
/// <para>�z��̓Y�����\����񋓖��ŏ㏑������</para>
/// <para><see href="https://qiita.com/ikuzak/items/57e3b333dccfb971e6cc">�Q�l�T�C�g</see></para>
/// </summary>
public class CIndexWithEnum : PropertyAttribute
{
#if UNITY_EDITOR	//�G�f�B�^�g�p��
	//�N���X��`	
	[CustomPropertyDrawer(typeof(CIndexWithEnum))]
	private class CIndexWithEnumPropertyDrawer : PropertyDrawer
	{
		// �萔��`
		const string UNDEFINED_PROPERTY_NAME = "����`�v�f";	// �񋓖���`�����̖��t���p


		/// <summary>
		/// GUI�\���֐�
		/// <para>�C���X�y�N�^�ɂ�����GUI�\���E����ւ̊�����</para>
		/// </summary>
		/// <param name="position">�\���ʒu�E�T�C�Y</param>
		/// <param name="property">�v���p�e�B�f�[�^</param>
		/// <param name="label">�\�����x��</param>
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			// �ϐ��錾
			var _sNames = ((CIndexWithEnum)attribute).m_sEnumNames; // �񋓖��擾

			// �Y�����擾
			string _sPropertyName = property.propertyPath;// �v���p�e�B�����擾�F"�ϐ���.Array[n]"
			string[] _index_names = _sPropertyName.Split('[', ']');	// �Y�����w��q��؂��蕪���F{�ϐ���.Array, n, \0}
			string _index_name =_index_names[^2];	// ��납��2�ڂ̗v�f���擾	(��1�ڂ�"\0")
			int _nIndex = int.Parse(_index_name);	// String��int
			
			// �\�����ύX
			if (_nIndex < _sNames.Length)	// �z��O�A�N�Z�X�h�~
			{
				label.text = _sNames[_nIndex];	// �Ή�����񋓖��ɕύX
			}
			else
			{
				int _SubIndex = _nIndex - _sNames.Length;	// ����`�����ɂ�����Y�������Z�o
				label.text = UNDEFINED_PROPERTY_NAME + _SubIndex;	// ����`�Ƃ��ĕ\��
			}

			// �v���p�e�B�\��
			EditorGUI.PropertyField(position, property, label, includeChildren: true);	// �v���p�e�B�̕\���f�U�C�����m��
		}

		/// <summary>
		/// �v���p�e�B����p�����񋟊֐�
		/// <para>�q�̔z��\���Ŏ����ďo������鍂���񋟂̏㏑��</para>
		/// </summary>
		/// <param name="property">�v���p�e�B�f�[�^</param>
		/// <param name="label">�\�����x��</param>
		/// <returns>�C���X�y�N�^�ɕ\������v���p�e�B�̏c��</returns>
		public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label)
		{
			return EditorGUI.GetPropertyHeight(_property, _label, includeChildren: true);
		}
	}

	// �ϐ��錾
	private string[] m_sEnumNames;	// �񋓂̖��O�ꗗ
#endif	// !UNITY_EDITOR


	/// <summary>
	/// �R���X�g���N�^
	/// <para>�C���X�^���X�������̏����B�r���h���ɒʂ��悤�ɒ�`�����ςȂ��ɁB</para>
	/// </summary>
	/// <param name="_EnumType">�Ώۗ񋓂̌^</param>
	public CIndexWithEnum(Type _EnumType)
	{
#if UNITY_EDITOR	//�G�f�B�^�g�p��
		m_sEnumNames = Enum.GetNames(_EnumType);	// �񋓖��ꗗ�擾
#endif	// !UNITY_EDITOR
	}
}