/*=====
<CodingRule.cs>	// スクリプト名
└作成者：takagi

＞内容
コーディング規約を記述

＞注意事項	// ないときは省略OK
この規約書に記述のないものは判明次第、適宜追加する

＞更新履歴
__Y24	// '24年
_M04	// 04月
D		// 日
16:プログラム作成:takagi	// 日付:変更内容:施行者
17:あいうえお:takagi

_M05
D
03:いろはにほへと:takagi
04:いくつかの記法追加・誤った表記を修正:takagi
14:インターフェースについて記述:takagi

_M06
D
20:警告文が出る問題を修正・
	インスペクタでの見た目を意識した表記改善・
	メンバでない変数名の記述法変更:takagi

__Y25
_M04
D
03:新チーム用にコードを刷新:takagi
23:enum型の変数宣言を明記:takagi
_M05
D
09:コピペがそのままだったりなどの誤りを訂正、enum変数などの記載法を追加:takagi
14:関数コメントにXMLドキュメントコメントを採用:takagi
15:更新履歴の記述漏れを追加:takagi
=====*/

// 名前空間宣言
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;	// 名前宣言時にはコメントは不要(勝手にずれるため)

// 名前空間定義
namespace Space
{
	// クラス定義
	public static class CSpace
	{
		// 定数定義
		private const uint CONST = 0;	// 定数はコンスタンスケースに従う(ハンガリアン記法には従わない)

		// 変数宣言
		public static readonly double ms_dTemp = 0.0d;	// readonlyな変数も書き方は同じ
	}
}

// インターフェース定義
public interface IInterface	// インターフェースの頭文字にIをつける
{
	// プロパティ定義
	/// <value>？？？</value>
	public double Prop { get; set; }	// 自動実装プロパティはハンガリアン記法を無視してよい

	// プロトタイプ宣言
	public void Signaled();
}


// クラス定義
public class CCodingRule : MonoBehaviour	// クラス型の頭文字にCをつける
{
	// 列挙定義
	public enum E_ENUM	// 列挙は接頭字をE_とする
	{
		A,	// 中身もコンスタンスケース
		B,	// 列挙名を引き継ぐ必要はない
	}

	// 構造体定義
	private struct Struct
	{
		GameObject m_Object;	// クラス型のネーミングはハンガリアン記法に従わない
								// ※m_やs_などの型とは関係ない部分では従う
								// 接頭辞の後ろは大文字から始める
		Ray m_Ray;
	}
	[Serializable]
	public struct SerializeStruct
	{
		[Tooltip("簡易説明")] public GameObject m_Member;	// ※シリアライズできる場合、その変数が何なのかインスペクタからわかるようにする
	}

	// 変数宣言
	[Space]	// 空行を活用し見やすくする
	[Header("初期化")]	// 変数を分類ごとに分けて記述
	[SerializeField, Tooltip("メンバー変数")] private uint m_uMember;	// 属性は記法に影響しない
	private int m_nInt;	// 通常の型はハンガリアン記法に従う[メンバ変数はm_と付ける]
	[SerializeField, Tooltip("構造体")] private SerializeStruct m_Struct;	// その変数が何なのかインスペクタからわかるようにする
	private static string ms_sStr;	// メンバ変数∧静的変数なら融合してms_と記述する
									// string型のハンガリアン記法はsとする
	private E_ENUM m_eEnum;	// 列挙はハンガリアン記法に従いeを接頭辞にする
	private float[] m_fArray;	// 配列は子の型名のみ記述(aとかは不要)
	private List<uint> m_uLists;	// リストも子の型名のみ記述
	private Dictionary<string, float> m_fDictionary;	// 辞書の命名はvalue部の型名に依存

	// プロパティ定義
		// プロパティを書く前に一つ空行を入れる
	/// <summary>
	/// xxプロパティ(プロパティ名)
	/// </summary>
	/// <value>プロパティの取り扱う値について記述</value>
	public double PriProp { get; private set; }	// プロパティはパスカルケースのみ従いハンガリアン記法は無視(関数扱い)

	/// <summary>
	/// メンバープロパティ
	/// </summary>
	/// <value><see cref="m_uMember"/></value>	// 参照しているタイプのプロパティはcrefで示せばOK
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

		// 初回関数定義前に2行空ける	※↓関数コメントはXML形式で
	/// <summary>
	/// -例関数(関数名)
	/// <para>概要を記載</para>
	/// <para>複数項目ある場合は段落(para属性)を分けると効果的</para>
	/// <see href = "https://www.youtube.com/">参考サイト添付</see>
	/// <see cref="m_Struct"/>
	/// </summary>
	/// <typeparam name="Meta">ジェネリック型の説明</typeparam>
	/// <param name="_dDouble">引数1の説明</param>
	/// <param name="_GameObject">引数2の説明</param>
	/// <param name="_MetaData">引数3の説明</param>
	/// <returns>戻り値の説明</returns>
	private int Example<Meta>(double _dDouble, GameObject _GameObject, Meta _MetaData)
	{
		// 変数宣言
		float _fFloat = 0.0f;	// ローカル変数も_から始める
		GameObject _Object = _GameObject;	// 接頭字が無い場合、頭文字を大文字にする

		// 算出
		m_nInt = (int)((float)_dDouble * _fFloat);	// なるべく全処理にコメントをつける

		// 分岐
		switch(m_nInt)
		{
			case 0:	// A
				break;
				// ケース間は空行
			case 1: // B
				m_nInt = 0;  // なるべく全処理にコメントをつける
				break;

			default:	// その他
#if _Debug	//プリプロセッサは左端
				Debug.Log("デフォルト");	// 中身は通常と同じ
#endif
				break;
		}

		if (m_nInt != 0)	// if文は()の前に空白
		{
			m_nInt = 0;
		}
		else	// else自体のコメント位置
		{
			m_nInt *= 0;
		}

		// 提供
		return m_nInt;
	}
		// 二回目以降の関数定義は書く前に一つ空行を入れる
	// ＞xx関数
	// 引数：なし   // 引数がない場合は１を省略してもよい
	// ｘ
	// 戻値：なし
	// ｘ
	// 概要：関数例
	/// <summary>
	/// -xx関数
	/// <para>関数例</para>
	/// </summary>
	// ジェネリック型がない場合は省略
	// 引数がない場合は省略
	// 戻り値がない場合は省略
	[MenuItem("ffff/gggg")]
	public void Function()
	{
	}
}

[CreateAssetMenu(menuName = "ObjectMenu/Menu")]
class CMenuItem : ScriptableObject
{
	/// <summary>
	/// -○○関数
	/// <para>関数例</para>
	/// </summary>
	/// <remarks>タイトル</remarks>
	private void DoSomething()
	{
	}
};