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
19:���ɍ��킹�ă��t�@�N�^�����O:takagi
=====*/

// ���O��Ԑ錾
using System;
using System.Linq;
using UnityEngine;
using UnityEditor;


/// <summary>
/// -�z��񋓑���
/// <para>�z��̓Y�����\����񋓖��ŏ㏑������</para>
/// <para><see href="https://qiita.com/ikuzak/items/57e3b333dccfb971e6cc">�Q�l�T�C�g</see></para>
/// </summary>
public class CIndexWithEnum : PropertyAttribute
{
	// �ϐ��錾
	private string[] m_sEnumNames;	// �񋓂̖��O�ꗗ

	/// <summary>
	/// �R���X�g���N�^
	/// </summary>
	/// <param name="enumType">�Ώۗ�</param>
	public CIndexWithEnum(Type enumType) => m_sEnumNames = Enum.GetNames(enumType);

#if UNITY_EDITOR	//�G�f�B�^�g�p��
	[CustomPropertyDrawer(typeof(CIndexWithEnum))]
	private class CIndexWithEnumPropertyDrawer : PropertyDrawer
	{
		/// <summary>
		/// GUI�\���֐�
		/// <para>�C���X�y�N�^�ɂ�����GUI�\���E����ւ̊�����</para>
		/// </summary>
		/// <param name="position">�\���ʒu�E�T�C�Y</param>
		/// <param name="property">�v���p�e�B�f�[�^</param>
		/// <param name="label">�\�����x��</param>
		public override void OnGUI(Rect position, SerializedProperty _property, GUIContent label)
		{
			// �ϐ��錾
			var _sNames = ((CIndexWithEnum)attribute).m_sEnumNames;	// �񋓖��擾


			// propertyPath returns something like hogehoge.Array.data[0]
			// so get the index from there.
			Debug.Log("a:" + _property.propertyPath);	// �v���p�e�B�����擾�F"�ϐ���.Array[n]"
			Debug.Log("b:" + _property.propertyPath.Split('[', ']'));
			Debug.Log("c:" + _property.propertyPath.Split('[', ']').Where(c => !string.IsNullOrEmpty(c)).Last());

			var _nIndex = int.Parse(_property.propertyPath.Split('[', ']').Where(c => !string.IsNullOrEmpty(c)).Last());
			if (_nIndex < _sNames.Length) label.text = _sNames[_nIndex];
			EditorGUI.PropertyField(position, _property, label, includeChildren: true);
		}

		public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label)
		{
			return EditorGUI.GetPropertyHeight(_property, _label, includeChildren: true);
		}
	}
#endif
}