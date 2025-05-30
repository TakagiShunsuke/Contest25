/*=====
<CountEnemy.cs>
└作成者：Nishibu

＞内容
// 敵の総数をカウントするためのクラス

＞更新履歴
__Y25 
_M05
D
5:CountEnemyクラス生成:nishibu
6:修正:nishibu
7:修正、コメント:nishibu
21:リファクタリング:takagi
=====*/

// 名前空間宣言
using UnityEngine;

// クラス定義
public class CCountEnemy : MonoBehaviour
{
	// 変数宣言
	public static uint m_nValInstances { get; private set; } = 0;	// 現在ステージ上に存在している敵の数

	
	/// <summary>
	/// -生成時関数
	/// <para>敵のインスタンス数を1つ増やす</para>
	/// </summary>
	protected virtual void Start()
	{
		m_nValInstances++;	// 敵の数を1増やす
	}

	/// <summary>
	/// -死亡時関数
	/// <para>敵のインスタンス数を1つ減らす</para>
	/// </summary>
	protected virtual void OnDestroy()
	{
		m_nValInstances--; // 敵の数を1減らす
	}
}