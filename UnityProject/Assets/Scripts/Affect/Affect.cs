/*=====
<Affect.cs>
└作成者：takagi

＞内容
効果要素を実装

＞注意事項
・インスペクタ上でのパラメータの変更は「試用限定機能であり、本実装では無効化される」こととなりました！
・試用段階はLDが[CreateAsset()]で仮仕様として制作・取りつけ。
	→本実装が決まったらそのスクリプト内で new クラス名() で作成・パラメータをプロパティから変更し使用の流れ。
	これによってScriptableが膨大になることを避けつつ、イベントごとに効果の取り付け・試用が可能に。

＞更新履歴
__Y25
_M05
D
12:プログラム仮作成:takagi
16:リファクタリング:takagi
_M06
13:継承元を MonoBehavior→ScriptableObject に変更:takagi
=====*/

// 名前空間宣言
using UnityEngine;

// クラス定義
//[CreateAssetMenu(menuName = AFFECT_MENU_TAB_NAME + "AffectName", fileName = "AffectName")]	と子クラスは記述
public abstract class CAffect : ScriptableObject
{
	// 定数定義
	public const string AFFECT_MENU_TAB_NAME = "Affect/";	// 共通メニュータブ名	※MenuItemの定義場所的にpublicでないと厳しい


	/// <summary>
	/// -効果関数
	/// <para>各効果の呼び出し共通化のための抽象関数</para>
	/// </summary>
	/// <param name="_Oneself">効果の発動者</param>
	/// <param name="_Opponent">効果の受動者</param>
	public abstract void Affect(GameObject _Oneself, GameObject _Opponent);
}