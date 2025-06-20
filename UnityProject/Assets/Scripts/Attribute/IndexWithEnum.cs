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
20:環境に合わせてリファクタリング:takagi
=====*/

// 名前空間宣言
using System;
using UnityEngine;
using UnityEditor;

/// <summary>
/// -配列列挙属性
/// <para>配列の添え字表示を列挙名で上書きする</para>
/// <para><see href="https://qiita.com/ikuzak/items/57e3b333dccfb971e6cc">参考サイト</see></para>
/// </summary>
public class CIndexWithEnum : PropertyAttribute
{
#if UNITY_EDITOR	//エディタ使用中
	//クラス定義	
	[CustomPropertyDrawer(typeof(CIndexWithEnum))]
	private class CIndexWithEnumPropertyDrawer : PropertyDrawer
	{
		// 定数定義
		const string UNDEFINED_PROPERTY_NAME = "未定義要素";	// 列挙未定義部分の名付け用


		/// <summary>
		/// GUI表示関数
		/// <para>インスペクタにおけるGUI表示・操作への干渉処理</para>
		/// </summary>
		/// <param name="position">表示位置・サイズ</param>
		/// <param name="property">プロパティデータ</param>
		/// <param name="label">表示ラベル</param>
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			// 変数宣言
			var _sNames = ((CIndexWithEnum)attribute).m_sEnumNames; // 列挙名取得

			// 添え字取得
			string _sPropertyName = property.propertyPath;// プロパティ名を取得："変数名.Array[n]"
			string[] _index_names = _sPropertyName.Split('[', ']');	// 添え字指定子を切り取り分割：{変数名.Array, n, \0}
			string _index_name =_index_names[^2];	// 後ろから2つ目の要素を取得	(※1つ目は"\0")
			int _nIndex = int.Parse(_index_name);	// String→int
			
			// 表示名変更
			if (_nIndex < _sNames.Length)	// 配列外アクセス防止
			{
				label.text = _sNames[_nIndex];	// 対応する列挙名に変更
			}
			else
			{
				int _SubIndex = _nIndex - _sNames.Length;	// 未定義部分における添え字を算出
				label.text = UNDEFINED_PROPERTY_NAME + _SubIndex;	// 未定義として表示
			}

			// プロパティ表示
			EditorGUI.PropertyField(position, property, label, includeChildren: true);	// プロパティの表示デザインを確定
		}

		/// <summary>
		/// プロパティ制御用高さ提供関数
		/// <para>子の配列表示で自動呼出しされる高さ提供の上書き</para>
		/// </summary>
		/// <param name="property">プロパティデータ</param>
		/// <param name="label">表示ラベル</param>
		/// <returns>インスペクタに表示するプロパティの縦幅</returns>
		public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label)
		{
			return EditorGUI.GetPropertyHeight(_property, _label, includeChildren: true);
		}
	}

	// 変数宣言
	private string[] m_sEnumNames;	// 列挙の名前一覧
#endif	// !UNITY_EDITOR


	/// <summary>
	/// コンストラクタ
	/// <para>インスタンス生成時の処理。ビルド時に通れるように定義しっぱなしに。</para>
	/// </summary>
	/// <param name="_EnumType">対象列挙の型</param>
	public CIndexWithEnum(Type _EnumType)
	{
#if UNITY_EDITOR	//エディタ使用中
		m_sEnumNames = Enum.GetNames(_EnumType);	// 列挙名一覧取得
#endif	// !UNITY_EDITOR
	}
}