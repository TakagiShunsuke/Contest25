/*=====
<IndexWithEnum.cs>
└作成者：takagi

＞内容
配列の添え字番号を、シリアライズフィールドで列挙名に上書き表示するもの

＞更新履歴
__Y24
_M06
D
07:プログラム作成:takagi
21:リファクタリング:takagi
__Y25
_M06
19:環境に合わせてリファクタリング:takagi
=====*/

// 名前空間宣言
using System;
using System.Linq;
using UnityEngine;
using UnityEditor;


/// <summary>
/// -配列列挙属性
/// <para>配列の添え字表示を列挙名で上書きする</para>
/// <para><see href="https://qiita.com/ikuzak/items/57e3b333dccfb971e6cc">参考サイト</see></para>
/// </summary>
public class CIndexWithEnum : PropertyAttribute
{
	// 変数宣言
	private string[] m_sEnumNames;	// 列挙の名前一覧

	/// <summary>
	/// コンストラクタ
	/// </summary>
	/// <param name="enumType">対象列挙</param>
	public CIndexWithEnum(Type enumType) => m_sEnumNames = Enum.GetNames(enumType);

#if UNITY_EDITOR	//エディタ使用中
	[CustomPropertyDrawer(typeof(CIndexWithEnum))]
	private class CIndexWithEnumPropertyDrawer : PropertyDrawer
	{
		/// <summary>
		/// GUI表示関数
		/// <para>インスペクタにおけるGUI表示・操作への干渉処理</para>
		/// </summary>
		/// <param name="position">表示位置・サイズ</param>
		/// <param name="property">プロパティデータ</param>
		/// <param name="label">表示ラベル</param>
		public override void OnGUI(Rect position, SerializedProperty _property, GUIContent label)
		{
			// 変数宣言
			var _sNames = ((CIndexWithEnum)attribute).m_sEnumNames;	// 列挙名取得


			// propertyPath returns something like hogehoge.Array.data[0]
			// so get the index from there.
			Debug.Log("a:" + _property.propertyPath);	// プロパティ名を取得："変数名.Array[n]"
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