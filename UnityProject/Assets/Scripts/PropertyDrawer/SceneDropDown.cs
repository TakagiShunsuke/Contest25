using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;



// クラス定義

/// <summary>
/// -遷移用シーン情報
/// <para>SceneAssetとその文字列の情報互換を成立させる</para>
/// <para><see href="https://uhiyama-lab.com/blog/gamedev/unity-scene-management-easy-setup/#:~:text=%E3%83%AD%E3%83%BC%E3%83%89%E5%8F%AF%E8%83%BD%E3%81%A7%E3%81%99%EF%BC%89-,%E6%94%B9%E8%89%AF%E7%89%88%EF%BC%9ASceneAsset%E3%81%A8OnValidate%E3%82%92%E4%BD%BF%E3%81%A3%E3%81%9F%E5%AE%9F%E8%A3%85,-%E3%81%9D%E3%82%8C%E3%81%A7%E3%81%AF%E3%80%81%E6%96%87%E5%AD%97%E5%88%97">参考サイト①</see></para>
/// </summary>
[Serializable]
public struct SceneDropDown
{
	// 変数宣言
	[/*HideInInspector, */SerializeField] private string m_SceneName;	// 登録されたシーンアセットの名前格納場所
#if UNITY_EDITOR
	[SerializeField] private SceneAsset m_SceneAsset;	// シーンのインスタンス	※UnityEditor上でしか定義できない(ビルド時損失)
#endif	// !UNITY_EDITOR

	// プロパティ定義

	/// <summary>
	/// シーン名プロパティ
	/// </summary>
	/// <value><see cref="m_SceneName"></value>
	public string SceneName
	{
		get
		{
			return m_SceneName;
		}
	}
}

#if UNITY_EDITOR
/// <summary>
/// -遷移用シーンドロップダウン機構
/// <para>インスペクタ上でシーンを選択すると内部で自動文字列解釈する</para>
/// <para><see href="https://qiita.com/yayoi-exe/items/02feb0fb40da62142a41#%E3%82%B7%E3%83%BC%E3%83%B3%E9%81%B8%E6%8A%9E%E3%81%AE%E3%82%AB%E3%82%B9%E3%82%BF%E3%83%A0%E3%82%AF%E3%83%A9%E3%82%B9-scenefield">参考サイト②</see></para>
/// </summary>
[CustomPropertyDrawer(typeof(SceneDropDown))]
public class CSceneDropDownPropertyDrawer : PropertyDrawer
{
	/// <summary>
	/// GUI表示関数
	/// <para>インスペクタにおけるGUI表示・操作への干渉処理</para>
	/// </summary>
	/// <value><see cref="m_SceneName"></value>
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		// プロパティ取得
		var _SA = property.FindPropertyRelative("m_SceneAsset");	// シーンアセットのプロパティ
		var _SceneNameProperty = property.FindPropertyRelative("m_SceneName");	// シーン名のプロパティ
		
		// インスペクタに表示
		EditorGUI.ObjectField(position, _SA, label);	// 今回はアセットの変更だけで十分(※1行のdrawerと1:1の関連でつながっているため、複数個出そうとするなら他に仲介クラスを設ける必要あり)

		// データ取得
		SceneAsset _SA_Resource = _SA.objectReferenceValue as SceneAsset;	// シーンアセット本体

		// データから文字列をコンバート
		if (_SA_Resource == null)	// ヌルチェック
		{
			_SceneNameProperty.stringValue = string.Empty;	// シーンは失われた
		}
		else
		{
			_SceneNameProperty.stringValue = _SA_Resource.name;	// シーン名の保存
		}
	}
}
#endif // !UNITY_EDITOR
